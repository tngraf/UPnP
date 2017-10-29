// ---------------------------------------------------------------------------
// <copyright file="DeviceInfoControl.cs" company="Tethys">
//   Copyright (C) 2017 T. Graf
// </copyright>
//
// Licensed under the Apache License, Version 2.0.
// Unless required by applicable law or agreed to in writing, 
// software distributed under the License is distributed on an
// "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND,
// either express or implied. 
// ---------------------------------------------------------------------------

namespace UpnpAnalyzer.UI
{
    using System;
    using System.Windows.Forms;
    using Tethys.Upnp.Core;

    /// <summary>
    /// Control to display device information.
    /// </summary>
    /// <seealso cref="System.Windows.Forms.UserControl" />
    /// <seealso cref="IContentControl" />
    public partial class DeviceInfoControl : UserControl, IContentControl
    {
        #region PRIVATE PROPERTIES
        #endregion // PRIVATE PROPERTIES

        //// ---------------------------------------------------------------------

        #region PUBLIC PROPERTIES            
        /// <summary>
        /// Gets or sets the device.
        /// </summary>
        public UpnpDevice Device { get; set; }
        #endregion // PUBLIC PROPERTIES

        //// ---------------------------------------------------------------------

        #region CONSTRUCTION
        /// <summary>
        /// Initializes a new instance of the <see cref="DeviceInfoControl"/> class.
        /// </summary>
        public DeviceInfoControl()
        {
            this.InitializeComponent();
        } // DeviceInfoControl()
        #endregion // CONSTRUCTION

        //// ---------------------------------------------------------------------

        #region UI HANDLING            
        /// <summary>
        /// Handles the Load event of the DeviceInfoControl control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing
        /// the event data.</param>
        private void DeviceInfoControlLoad(object sender, EventArgs e)
        {
            this.InitUI();
            this.DisplayDeviceInfo();
        } // DeviceInfoControlLoad()
        #endregion // UI HANDLING

        //// ---------------------------------------------------------------------

        #region PUBLIC METHODS
        #endregion // PUBLIC METHODS

        //// ---------------------------------------------------------------------

        #region ICONTENTCONTROL IMPLEMENTATION

        /// <summary>
        /// Gets a value indicating whether the contents of this view
        /// has been changed by the user.
        /// </summary>
        public bool IsDirty
        {
            get { return false; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether a refresh of the view is needed.
        /// </summary>
        public bool ForceRefresh { get; set; }

        /// <summary>
        /// Refreshes the display.
        /// </summary>
        public void RefreshDisplay()
        {
            this.DisplayDeviceInfo();
        } // RefreshDisplay()
        #endregion // ICONTENTCONTROL IMPLEMENTATION

        //// ---------------------------------------------------------------------

        #region PRIVATE METHODS
        /// <summary>
        /// Displays the device information.
        /// </summary>
        private void DisplayDeviceInfo()
        {
            if (this.Device == null)
            {
                return;
            } // if

            this.listViewProperties.Items.Clear();

            this.AddNewPropertyValuePair("FriendlyName", this.Device.DeviceDescription.FriendlyName);
            this.AddNewPropertyValuePair("DeviceType", this.Device.DeviceDescription.DeviceType);
            this.AddNewPropertyValuePair("Manufacturer", this.Device.DeviceDescription.Manufacturer);
            this.AddNewPropertyValuePair("ManufacturerUrl", this.Device.DeviceDescription.ManufacturerUrl);
            this.AddNewPropertyValuePair("ModelName", this.Device.DeviceDescription.ModelName);
            this.AddNewPropertyValuePair("ModelDescription", this.Device.DeviceDescription.ModelDescription);
            this.AddNewPropertyValuePair("ModelNumber", this.Device.DeviceDescription.ModelNumber);
            this.AddNewPropertyValuePair("ModelUrl", this.Device.DeviceDescription.ModelUrl);
            this.AddNewPropertyValuePair("SerialNumber", this.Device.DeviceDescription.SerialNumber);
            this.AddNewPropertyValuePair("PresentationUrl", this.Device.DeviceDescription.PresentationUrl);
            this.AddNewPropertyValuePair("UDN", this.Device.DeviceDescription.UDN);
            this.AddNewPropertyValuePair("UPC", this.Device.DeviceDescription.UPC);

            this.AddNewPropertyValuePair("#Icons", this.Device.DeviceDescription.Icons.Count.ToString());
            this.AddNewPropertyValuePair("#Services", this.Device.DeviceDescription.Services.Count.ToString());
            this.AddNewPropertyValuePair("#SubDevices", this.Device.DeviceDescription.SubDevices.Count.ToString());

            this.AddNewPropertyValuePair("Type", this.Device.Type);
            this.AddNewPropertyValuePair("Location", this.Device.Location);
            this.AddNewPropertyValuePair("Server", this.Device.Server);
            this.AddNewPropertyValuePair("USN", this.Device.USN);

            this.AddNewPropertyValuePair("Address", this.Device.Address.ToString());

            this.listViewProperties.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
        } // DisplayDeviceInfo()

        /// <summary>
        /// Adds the new property value pair.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="value">The value.</param>
        private void AddNewPropertyValuePair(string propertyName, string value)
        {
            var lvi = new ListViewItem();
            lvi.Text = propertyName;
            lvi.SubItems.Add(value);
            this.listViewProperties.Items.Add(lvi);
        } // AddNewPropertyValuePair()

        /// <summary>
        /// Initializes the UI.
        /// </summary>
        private void InitUI()
        {
            this.listViewProperties.Clear();

            var col = new ColumnHeader();
            col.Text = "Name";
            this.listViewProperties.Columns.Add(col);

            col = new ColumnHeader();
            col.Text = "Value";
            this.listViewProperties.Columns.Add(col);
        } // InitUI()
        #endregion // PRIVATE METHODS
    }
}
