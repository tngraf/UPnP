// ---------------------------------------------------------------------------
// <copyright file="DeviceSchema.cs" company="Tethys">
//   Copyright (C) 2017 T. Graf
// </copyright>
//
// Licensed under the Apache License, Version 2.0.
// Unless required by applicable law or agreed to in writing, 
// software distributed under the License is distributed on an
// "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND,
// either express or implied. 
// ---------------------------------------------------------------------------

namespace Tethys.Upnp.Core
{
    using System.Collections.Generic;

    /// <summary>
    /// Implement a <c>UPnP</c> device schema.
    /// </summary>
    public class DeviceSchema
    {
        #region PRIVATE PROPERTIES        
        /// <summary>
        /// The icons.
        /// </summary>
        private readonly List<DeviceIcon> icons;

        /// <summary>
        /// The services.
        /// </summary>
        private readonly List<UpnpService> services;

        /// <summary>
        /// The sub devices.
        /// </summary>
        private readonly List<DeviceSchema> subDevices;
        #endregion // PRIVATE PROPERTIES

        //// ---------------------------------------------------------------------

        #region PUBLIC PROPERTIES            
        /// <summary>
        /// Gets or sets the type of the device.
        /// </summary>
        /// <example>
        /// <c>urn:schemas-upnp-org:device:MediaServer:2</c>
        /// </example>
        public string DeviceType { get; set; }

        /// <summary>
        /// Gets or sets the friendly name.
        /// </summary>
        /// <example>
        /// <c>DIGA im Wohnzimmer</c>
        /// </example>
        public string FriendlyName { get; set; }

        /// <summary>
        /// Gets or sets the manufacturer.
        /// </summary>
        /// <example>
        /// <c>Panasonic</c>
        /// </example>
        public string Manufacturer { get; set; }

        /// <summary>
        /// Gets or sets the manufacturer URL.
        /// </summary>
        /// <example>
        /// <c>http://www.dlink.com</c>
        /// </example>
        /// <remarks>Optional property</remarks>
        public string ManufacturerUrl { get; set; }

        /// <summary>
        /// Gets or sets the name of the model.
        /// </summary>
        /// <example>
        /// <c>BD/DVD Recorder</c>
        /// </example>
        public string ModelName { get; set; }

        /// <summary>
        /// Gets or sets the model number.
        /// </summary>
        /// <example>
        /// <c>DMR-BCT720/721</c>
        /// </example>
        /// <remarks>Optional property</remarks>
        public string ModelNumber { get; set; }

        /// <summary>
        /// Gets or sets the model description.
        /// </summary>
        /// <example>
        /// <c>Panasonic BD/DVD Recorder</c>
        /// </example>
        /// <remarks>Optional property</remarks>
        public string ModelDescription { get; set; }

        /// <summary>
        /// Gets or sets the model URL.
        /// </summary>
        /// <example>
        /// <c>http://support.dlink.com</c>
        /// </example>
        /// <remarks>Optional property</remarks>
        public string ModelUrl { get; set; }

        /// <summary>
        /// Gets or sets the serial number.
        /// </summary>
        /// <example>
        /// <c>none</c>
        /// </example>
        /// <remarks>Optional property</remarks>
        public string SerialNumber { get; set; }

        /// <summary>
        /// Gets or sets the (UDN).
        /// </summary>
        /// <example>
        /// <c>uuid:4D454930-0100-1000-8000-8CC121820D8E</c>
        /// </example>
        public string UDN { get; set; }

        /// <summary>
        /// Gets or sets the universal product code (UPC).
        /// </summary>
        /// <example>
        /// <c>0001</c>
        /// </example>
        /// <remarks>Optional property</remarks>
        public string UPC { get; set; }

        /// <summary>
        /// Gets the icons.
        /// </summary>
        public IReadOnlyList<DeviceIcon> Icons
        {
            get { return this.icons; }
        }

        /// <summary>
        /// Gets the services.
        /// </summary>
        public IReadOnlyList<UpnpService> Services
        {
            get { return this.services; }
        }

        /// <summary>
        /// Gets the sub devices.
        /// </summary>
        public IReadOnlyList<DeviceSchema> SubDevices
        {
            get { return this.subDevices; }
        }

        /// <summary>
        /// Gets or sets the presentation URL.
        /// </summary>
        public string PresentationUrl { get; set; }
        #endregion // PUBLIC PROPERTIES

        //// ---------------------------------------------------------------------

        #region CONSTRUCTION
        /// <summary>
        /// Initializes a new instance of the <see cref="DeviceSchema"/> class.
        /// </summary>
        public DeviceSchema()
        {
            this.icons = new List<DeviceIcon>();
            this.services = new List<UpnpService>();
            this.subDevices = new List<DeviceSchema>();
        } // DeviceSchema()
        #endregion // CONSTRUCTION

        //// ---------------------------------------------------------------------

        #region PUBLIC METHODS            
        /// <summary>
        /// Adds the icon.
        /// </summary>
        /// <param name="icon">The icon.</param>
        public void AddIcon(DeviceIcon icon)
        {
            this.icons.Add(icon);
        } // AddIcon()

        /// <summary>
        /// Adds the service.
        /// </summary>
        /// <param name="service">The service.</param>
        public void AddService(UpnpService service)
        {
            this.services.Add(service);
        } // AddService()

        /// <summary>
        /// Adds the sub device.
        /// </summary>
        /// <param name="device">The device.</param>
        public void AddSubDevice(DeviceSchema device)
        {
            this.subDevices.Add(device);
        } // AddSubDevice()

        /// <summary>
        /// Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="string" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return $"{this.FriendlyName}, {this.DeviceType}, {this.Manufacturer}, #{this.subDevices.Count}"
                + $"subdevices, #{this.services.Count} services, #{this.icons.Count} icons";
        } // ToString()
        #endregion // PUBLIC METHODS
    } // DeviceSchema
}
