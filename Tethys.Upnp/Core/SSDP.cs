// ---------------------------------------------------------------------------
// <copyright file="SSDP.cs" company="Tethys">
//   Copyright (C) 2017 T. Graf
// </copyright>
//
// Licensed under the Apache License, Version 2.0.
// Unless required by applicable law or agreed to in writing, 
// software distributed under the License is distributed on an
// "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND,
// either express or implied. 
// 
// ---------------------------------------------------------------------------

//#define LOG_MORE

namespace Tethys.Upnp.Core
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net;
    using System.Net.Sockets;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using HttpSupport;
    using Logging;

    /// <summary>
    /// SSPD (Simple Service Discovery Protocol) support.
    /// </summary>
    /// <seealso cref="IDisposable" />
    public class SSDP : IDisposable
    {
        #region PRIVATE PROPERTIES
        /// <summary>
        /// The SSPD address.
        /// </summary>
        private const string SspdAddress = "239.255.255.250";

        /// <summary>
        /// The SSPD port.
        /// </summary>
        private const int SspdPort = 1900;

        /// <summary>
        /// The HTTP server signature default value.
        /// </summary>
        private const string HttpServerSignatureDefault = "UPnPDummy";

        /// <summary>
        /// The time in seconds that our search lasts.
        /// </summary>
        private const int SecondsForSearch = 10;

        /// <summary>
        /// SSDP query for root devices.
        /// </summary>
        private const string QueryDevices = "upnp:rootdevice";

        /// <summary>
        /// The Notification Sub Type (NTS) for alive notifications.
        /// </summary>
        private const string NtsAlive = "ssdp:alive";

        /// <summary>
        /// The Notification Sub Type (NTS) for device gone notifications.
        /// </summary>
        private const string NtsBye = "ssdp:byebye";

        /// <summary>
        /// The Notification Sub Type (NTS) for device update notifications.
        /// </summary>
        private const string NtsUpdate = "ssdp:update";

        /// <summary>
        /// The logger for this class.
        /// </summary>
        private static readonly ILog Log = LogManager.GetLogger(typeof(SSDP));

        /// <summary>
        /// The SSDP endpoint.
        /// </summary>
        private static readonly IPEndPoint SspdEndpoint =
            new IPEndPoint(IPAddress.Parse(SspdAddress), SspdPort);

        /// <summary>
        /// The broadcast endpoint.
        /// </summary>
        private static readonly IPEndPoint BroadcastEndpoint =
            new IPEndPoint(IPAddress.Parse("255.255.255.255"), SspdPort);

        /// <summary>
        /// The SSPD IP address.
        /// </summary>
        private static readonly IPAddress SspdIpAddress =
            IPAddress.Parse(SspdAddress);

        /// <summary>
        /// The list of devices found.
        /// </summary>
        private readonly List<UpnpDevice> devicesFound;

        /// <summary>
        /// The lock object.
        /// </summary>
        private readonly object lockObject = new object();

        /// <summary>
        /// The thread for the device discovery via notifications.
        /// </summary>
        private Thread discoveryThread;

        /// <summary>
        /// The search client IP4.
        /// </summary>
        private UdpClient searchClientIp4;
        #endregion // PRIVATE PROPERTIES

        //// ---------------------------------------------------------------------

        #region PUBLIC PROPERTIES
        /// <summary>
        /// Gets a value indicating whether we're running in server mode.
        /// </summary>
        public bool IsServerMode { get; private set; }

        /// <summary>
        /// Gets or sets the HTTP server signature.
        /// </summary>
        public string HttpServerSignature { get; set; }

        /// <summary>
        /// Delegate for device state change callbacks.
        /// </summary>
        /// <param name="device">The device.</param>
        public delegate void DeviceStateNotification(UpnpDevice device);

        /// <summary>
        /// Gets or sets the device found callback.
        /// </summary>
        public DeviceStateNotification DeviceFoundCallback { get; set; }

        /// <summary>
        /// Gets or sets the device removed callback.
        /// </summary>
        public DeviceStateNotification DeviceRemovedCallback { get; set; }

        /// <summary>
        /// Gets or sets the synchronization context for callbacks.
        /// </summary>
        public SynchronizationContext SynchronizationContext { get; set; }
        #endregion // PUBLIC PROPERTIES

        //// ---------------------------------------------------------------------

        #region CONSTRUCTION
        /// <summary>
        /// Initializes a new instance of the <see cref="SSDP"/> class.
        /// </summary>
        public SSDP()
        {
            this.HttpServerSignature = HttpServerSignatureDefault;
            this.devicesFound = new List<UpnpDevice>();
            this.discoveryThread = null;
        } // SSDP()
        #endregion // CONSTRUCTION

        //// ---------------------------------------------------------------------

        #region PUBLIC METHODS
        /// <summary>
        /// Starts the discovery service.
        /// </summary>
        public void StartDiscoveryService()
        {
            var broadcast = new IPEndPoint(IPAddress.Any, SspdPort);

            this.searchClientIp4 = new UdpClient();
            this.searchClientIp4.Client.SetSocketOption(
                SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            this.searchClientIp4.ExclusiveAddressUse = false;
            this.searchClientIp4.Client.Bind(broadcast);
            
            // important: join multicast group
            this.searchClientIp4.JoinMulticastGroup(SspdIpAddress, 10);

            this.discoveryThread = new Thread(this.ReceiveNotificationsLoop);
            this.discoveryThread.Start(this.searchClientIp4);

            Log.Debug("SSDP device discovery service started...");
        } // StartDiscoveryService()

        /// <summary>
        /// Stops the discovery service.
        /// </summary>
        public void StopDiscoveryService()
        {
            Log.Debug("Stopping SSDP device discovery service...");

            if ((this.discoveryThread != null) && (this.discoveryThread.IsAlive))
            {
                this.discoveryThread.Join(50);
                if (this.discoveryThread.IsAlive)
                {
                    this.discoveryThread.Abort();
                } // if
            } // if

            Log.Debug("SSDP device discovery service stopped.");
        } // StopDiscoveryService()

        /// <summary>
        /// Searches for devices.
        /// </summary>
        /// <returns>A list of <see cref="UpnpDevice"/> objects.</returns>
        public async Task<List<UpnpDevice>> SearchForDevices()
        {
            // ReSharper disable once InconsistentlySynchronizedField
            this.IsServerMode = false;

            var host = Dns.GetHostEntry(Dns.GetHostName());
            Log.Debug($"Found {host.AddressList.Length} IP addresses:");
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    var myIp = ip;
                    var localEndPoint = new IPEndPoint(myIp, 12346);
                    var searchClient = new UdpClient();
                    searchClient.Client.UseOnlyOverlappedIO = true;
                    searchClient.Client.SetSocketOption(
                        SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
                    searchClient.ExclusiveAddressUse = false;
                    searchClient.EnableBroadcast = true;
                    searchClient.Client.Bind(localEndPoint);

                    Log.Debug($"  Searching for devices IP={localEndPoint.Address}, port={localEndPoint.Port}...");

                    var receiveThread = new Thread(this.ReceiveLoop);
                    receiveThread.Start(searchClient);

                    SendSearchRequest(SspdEndpoint, searchClient);

                    // asynchronously wait for some time to receive answers
                    await Task.Delay(new TimeSpan(0, 0, SecondsForSearch));

                    if (receiveThread.IsAlive)
                    {
                        receiveThread.Join(50);
                        if (receiveThread.IsAlive)
                        {
                            receiveThread.Abort();
                        } // if
                    } // if
                } // if
            } // foreach

            return this.devicesFound;
        } // SearchForDevices()
        #endregion // PUBLIC METHODS

        //// ---------------------------------------------------------------------

        #region PRIVATE METHODS            
        /// <summary>
        /// Loop to receive <c>UPnP</c> notification messages.
        /// </summary>
        /// <param name="data">The data.</param>
        private async void ReceiveNotificationsLoop(object data)
        {
            var searchClient = data as UdpClient;
            if (searchClient == null)
            {
                return;
            } // if

            try
            {
                while (true)
                {
                    try
                    {
                        var udpReceiveResult = await searchClient.ReceiveAsync();
                        var method = EvaluateDatagram(udpReceiveResult.Buffer, out Headers headers);
                        if (method == "NOTIFY") 
                        {
#if LOG_MORE
                            Log.Debug($"ReceiveNotificationsLoop got: {method}");
#endif
                            this.HandleDeviceNotification(headers, 
                                udpReceiveResult.RemoteEndPoint.Address);
                        } // if
                    }
                    catch (Exception e)
                    {
                        Log.Error("Error receiving SSDP notifications", e);
                    } // catch
                } // while
            }
            catch (ThreadAbortException)
            {
                // IGNORE
            }
            catch (Exception ex)
            {
                Log.Error("Error receiving SSDP notifications", ex);
            } // catch
        } // ReceiveNotificationsLoop()

        /// <summary>
        /// Handles the device notification.
        /// </summary>
        /// <param name="headers">The headers.</param>
        /// <param name="address">The address.</param>
        private void HandleDeviceNotification(Headers headers, IPAddress address)
        {
#if LOG_MORE
            Log.Debug(headers);
#endif
            var deviceType = "(unknown)";
            if (headers.ContainsKey("ST"))
            {
                deviceType = headers["ST"];
            } // if

            var dev = new UpnpDevice(deviceType, headers["SERVER"],
                headers["LOCATION"], headers["USN"], address);

            // NTS is required only for NOTIFY messages and not for
            // M-SEARCH answers
            var nts = NtsAlive;
            if (headers.ContainsKey("NTS"))
            {
                nts = headers["NTS"];
            } // if
            
            if (nts == NtsBye)
            {
                this.RemoveSearchResult(dev);
            }
            else if ((nts == NtsAlive) || (nts == NtsUpdate))
            {
                // NT is required only for NOTIFY messages and not for
                // M-SEARCH answers
                if ((!headers.ContainsKey("NT")) || (headers["NT"] == "upnp:rootdevice"))
                {
                    this.AddSearchResult(dev);
                } // if
            } // if
        } // HandleDeviceNotification()

        /// <summary>
        /// Loop to receive answers from the search.
        /// </summary>
        /// <param name="data">The data.</param>
        private async void ReceiveLoop(object data)
        {
            var searchClient = data as UdpClient;
            if (searchClient == null)
            {
                return;
            } // if

            try
            {
                while (true)
                {
                    try
                    {
                        var udpReceiveResult = await searchClient.ReceiveAsync();
                        var method = EvaluateDatagram(udpReceiveResult.Buffer, out Headers headers);
                        if ((method == "NOTIFY") || (method == "HTTP/1.1"))
                        {
#if LOG_MORE
                            Log.Debug($"ReceiveLoop got: {method}");
#endif
                            this.HandleDeviceNotification(headers,
                                udpReceiveResult.RemoteEndPoint.Address);
                        } // if
                    }
                    catch (SocketException sex)
                    {
                        var errorcode = (uint)sex.ErrorCode;
                        if (errorcode != 0xc0000120)
                        {
                            Log.Error("Error receiving SSDP answers", sex);
                        } // if
                    }
                    catch (Exception e)
                    {
                        Log.Error("Error receiving SSDP answers", e);
                    } // catch
                } // while
            }
            catch (ThreadAbortException)
            {
                // IGNORE
            }
            catch (Exception ex)
            {
                Log.Error("Error receiving SSDP answers", ex);
            } // catch
        } // ReceiveThread()

        /// <summary>
        /// Adds the search result.
        /// </summary>
        /// <param name="device">The device.</param>
        private void AddSearchResult(UpnpDevice device)
        {
            lock (this.lockObject)
            {
                foreach (var dev in this.devicesFound)
                {
                    if (dev.Equals(device))
                    {
                        return;
                    } // if
                } // foreach

                this.devicesFound.Add(device);
                if (this.DeviceFoundCallback != null)
                {
                    this.SynchronizationContext.Post(x => this.DeviceFoundCallback(device), null);
                } // if
            } // lock
        } // AddSearchResult()

        /// <summary>
        /// Removes the search result.
        /// </summary>
        /// <param name="device">The device.</param>
        private void RemoveSearchResult(UpnpDevice device)
        {
            lock (this.lockObject)
            {
                foreach (var dev in this.devicesFound)
                {
                    if (dev.Equals(device))
                    {
                        this.devicesFound.Remove(dev);
                        if (this.DeviceRemovedCallback != null)
                        {
                            this.SynchronizationContext.Post(x => this.DeviceRemovedCallback(device), null);
                        } // if

                        break;
                    } // if
                } // foreach
            } // lock
        } // RemoveSearchResult()

        /// <summary>
        /// Sends the search response.
        /// </summary>
        /// <param name="remoteEndpoint">The remote endpoint.</param>
        /// <param name="sendClient">The send client.</param>
        private static void SendSearchRequest(IPEndPoint remoteEndpoint, UdpClient sendClient)
        {
            var headers = new RawHeaders();
            headers.Add("HOST", "239.255.255.250:1900");
            ////headers.Add("ST", QueryAll);
            headers.Add("ST", QueryDevices);
            headers.Add("MAN", "\"ssdp:discover\"");
            headers.Add("MX", SecondsForSearch.ToString());

            var dgram = new Datagram(remoteEndpoint, sendClient,
                $"M-SEARCH * HTTP/1.1\r\n{headers.HeaderBlock}\r\n",
                false);
            dgram.Send();
        } // SendSearchRequest()

        /// <summary>
        /// Evaluates the datagram data received..
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="headers">The headers.</param>
        /// <returns>A string containing the SSDP method.</returns>
        private static string EvaluateDatagram(byte[] data, out Headers headers)
        {
            string method;
            headers = new Headers();
            using (var reader = new StreamReader(
                new MemoryStream(data), Encoding.ASCII))
            {
                var proto = reader.ReadLine();
                proto = proto?.Trim() ?? throw new IOException("Couldn't read protocol line");
                if (string.IsNullOrEmpty(proto))
                {
                    throw new IOException("Invalid protocol line");
                } // if

                method = proto.Split(new[] { ' ' }, 2)[0];
                for (var line = reader.ReadLine(); line != null;
                    line = reader.ReadLine())
                {
                    line = line.Trim();
                    if (string.IsNullOrEmpty(line))
                    {
                        break;
                    } // if

                    var parts = line.Split(new[] { ':' }, 2);
                    headers[parts[0]] = parts[1].Trim();
                } // for
            } // using

            return method;
        } // EvaluateDatagram()

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        public void Dispose()
        {
            Log.Debug("Disposing SSDP");
        } // Dispose()
        #endregion // PRIVATE METHODS
    } // SSPD
}
