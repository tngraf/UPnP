// ---------------------------------------------------------------------------
// <copyright file="Datagram.cs" company="Tethys">
//   Copyright (C) 2017 T. Graf
// </copyright>
//
// Licensed under the Apache License, Version 2.0.
// Unless required by applicable law or agreed to in writing, 
// software distributed under the License is distributed on an
// "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND,
// either express or implied. 
// 
// Based on code written by Nils Maier, see 
// https://github.com/nmaier/simpleDLNA
// Licensed under as BSD-2-Clause license.
// 
// ---------------------------------------------------------------------------

namespace Tethys.Upnp.Core
{
    using System;
    using System.Net;
    using System.Net.Sockets;
    using System.Text;
    using Logging;

    /// <summary>
    /// Helper class for UPD datagrams.
    /// </summary>
    internal sealed class Datagram
    {
        #region PRIVATE PROPERTIES
        /// <summary>
        /// The logger for this class.
        /// </summary>
        private static readonly ILog Log = LogManager.GetLogger(typeof(Datagram));

        /// <summary>
        /// The send client.
        /// </summary>
        private readonly UdpClient sendClient;
        #endregion // PRIVATE PROPERTIES

        //// ---------------------------------------------------------------------

        #region PUBLIC PROPERTIES            
        /// <summary>
        /// Gets the remote end point.
        /// </summary>
        public IPEndPoint RemoteEndPoint { get; }

        /// <summary>
        /// Gets the local end point.
        /// </summary>
        public IPEndPoint LocalEndPoint { get; }

        /// <summary>
        /// Gets the message.
        /// </summary>
        public string Message { get; }

        /// <summary>
        /// Gets a value indicating whether this <see cref="Datagram"/> is sticky.
        /// </summary>
        public bool Sticky { get; }

        /// <summary>
        /// Gets the send count.
        /// </summary>
        public uint SendCount { get; private set; }
        #endregion // PUBLIC PROPERTIES

        //// ---------------------------------------------------------------------

        #region CONSTRUCTION            
        /// <summary>
        /// Initializes a new instance of the <see cref="Datagram"/> class.
        /// </summary>
        /// <param name="remoteEndPoint">The end point.</param>
        /// <param name="localAddress">The local address.</param>
        /// <param name="message">The message.</param>
        /// <param name="sticky">if set to <c>true</c> [sticky].</param>
        public Datagram(IPEndPoint remoteEndPoint, IPAddress localAddress,
            string message, bool sticky)
        {
            this.RemoteEndPoint = remoteEndPoint;
            this.LocalEndPoint = new IPEndPoint(localAddress, 0);
            this.Message = message;
            this.Sticky = sticky;
            this.SendCount = 0;
        } // Datagram()

        /// <summary>
        /// Initializes a new instance of the <see cref="Datagram" /> class.
        /// </summary>
        /// <param name="remoteEndPoint">The end point.</param>
        /// <param name="localEndPoint">The local end point.</param>
        /// <param name="message">The message.</param>
        /// <param name="sticky">if set to <c>true</c> [sticky].</param>
        public Datagram(IPEndPoint remoteEndPoint, IPEndPoint localEndPoint,
            string message, bool sticky)
        {
            this.RemoteEndPoint = remoteEndPoint;
            this.LocalEndPoint = localEndPoint;
            this.Message = message;
            this.Sticky = sticky;
            this.SendCount = 0;
        } // Datagram()

        /// <summary>
        /// Initializes a new instance of the <see cref="Datagram" /> class.
        /// </summary>
        /// <param name="remoteEndPoint">The end point.</param>
        /// <param name="sendClient">The send client.</param>
        /// <param name="message">The message.</param>
        /// <param name="sticky">if set to <c>true</c> [sticky].</param>
        public Datagram(IPEndPoint remoteEndPoint, UdpClient sendClient,
            string message, bool sticky)
        {
            this.RemoteEndPoint = remoteEndPoint;
            this.sendClient = sendClient;
            this.Message = message;
            this.Sticky = sticky;
            this.SendCount = 0;
        } // Datagram()
        #endregion // CONSTRUCTION

        //// ---------------------------------------------------------------------

        #region PUBLIC METHODS            
        /// <summary>
        /// Sends this instance.
        /// </summary>
        public void Send()
        {
            var msg = Encoding.ASCII.GetBytes(this.Message);
            try
            {
                UdpClient client;
                if (this.sendClient != null)
                {
                    client = this.sendClient;
                }
                else
                {
                    client = new UdpClient();
                    client.Client.Bind(this.LocalEndPoint);
                } // if
                
                client.Ttl = 10;
                client.Client.SetSocketOption(SocketOptionLevel.IP, 
                    SocketOptionName.MulticastTimeToLive, 10);
                client.BeginSend(msg, msg.Length, this.RemoteEndPoint, result =>
                {
                    try
                    {
                        client.EndSend(result);
                    }
                    catch (Exception ex)
                    {
                        Log.Debug(ex);
                    }
                    finally
                    {
                        try
                        {
                            if (this.sendClient == null)
                            {
                                client.Close();
                            } // if
                        }
                        catch (Exception)
                        {
                            // IGNORE
                        } // catch
                    } // finally
                }, null);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            } // catch

            ++this.SendCount;
        } // Send
        #endregion // PUBLIC METHODS
    } // Datagram
}
