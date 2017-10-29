// ---------------------------------------------------------------------------
// <copyright file="UpnpDevice.cs" company="Tethys">
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
    using System.Net;

    /// <summary>
    /// Represents a <c>UPnP</c> device or service discovered during a
    /// search.
    /// </summary>
    public class UpnpDevice
    {
        #region PUBLIC PROPERTIES            
        /// <summary>
        /// Gets the address.
        /// </summary>
        public IPAddress Address { get; }

        /// <summary>
        /// Gets the type.
        /// </summary>
        /// <example>
        /// <c>upnp:rootdevice</c>
        /// </example>
        public string Type { get; }

        /// <summary>
        /// Gets the server.
        /// </summary>
        /// <example>
        /// <c>Synology/DSM/192.168.0.16</c>
        /// </example>
        public string Server { get; }

        /// <summary>
        /// Gets the location.
        /// </summary>
        /// <example>
        /// <c>http://192.168.0.16:5000/ssdp/desc-DSM-eth0.xml</c>
        /// </example>
        public string Location { get; }

        /// <summary>
        /// Gets the Unique Service Name (USN).
        /// </summary>
        /// <example>
        /// <c>
        /// uuid:73796E6F-6473-6D00-0000-0011323bd327::upnp:rootdevice
        /// urn:schemas-upnp-org:device:WANConnectionDevice:
        /// urn:schemas-upnp-org:service:ConnectionManager:
        /// urn:schemas-wifialliance-org:device:WFADevice:
        /// uuid:4D454930-0100-1000-8000-8CC121820D8E
        /// urn:schemas-upnp-org:service:ContentDirectory:2
        /// </c>
        /// </example>
        public string USN { get; }

        /// <summary>
        /// Gets or sets the device description.
        /// </summary>
        public DeviceSchema DeviceDescription { get; set; }
        #endregion // PUBLIC PROPERTIES

        //// ---------------------------------------------------------------------

        #region CONSTRUCTION            
        /// <summary>
        /// Initializes a new instance of the <see cref="UpnpDevice" /> class.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="server">The server.</param>
        /// <param name="location">The location.</param>
        /// <param name="usn">The unique service name (USN).</param>
        /// <param name="address">The address.</param>
        public UpnpDevice(string type, string server, string location,
            string usn, IPAddress address)
        {
            this.Type = type;
            this.Server = server;
            this.Location = location;
            this.USN = usn;
            this.Address = address;
        } // UpnpDevice()
        #endregion // CONSTRUCTION

        //// ---------------------------------------------------------------------

        #region PUBLIC METHODS            
        /// <summary>
        /// Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="string" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return $"{this.Type}, {this.Server}, {this.Location}, {this.Address}";
        } // ToString()

        /// <summary>
        /// Determines whether the specified <see cref="object" />, is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            var device = obj as UpnpDevice;
            if (device == null)
            {
                return false;
            } // if

            // ONLY compare USN
            if (!device.USN.Equals(this.USN))
            {
                return false;
            } // if

            return true;
        } // Equals()

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return $"{this.Type}, {this.Server}, {this.Location}, {this.Address}".GetHashCode();
        } // GetHashCode()
        #endregion // PUBLIC METHODS
    } // UpnpDevice
}
