// ---------------------------------------------------------------------------
// <copyright file="ServiceInfoControl.cs" company="Tethys">
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
    using Tethys.Upnp.Services.ContentDirectory;

    /// <summary>
    /// A control to work with <c>UPnP</c> services.
    /// </summary>
    /// <seealso cref="System.Windows.Forms.UserControl" />
    /// <seealso cref="IContentControl" />
    public partial class ServiceInfoControl : UserControl, IContentControl
    {
        #region PRIVATE PROPERTIES        
        /// <summary>
        /// The page with the service implementation.
        /// </summary>
        private TabPage pageServiceImpl;
        #endregion // PRIVATE PROPERTIES

        //// ---------------------------------------------------------------------

        #region PUBLIC PROPERTIES            
        /// <summary>
        /// Gets or sets the service.
        /// </summary>
        public UpnpService Service { get; set; }
        #endregion // PUBLIC PROPERTIES

        //// ---------------------------------------------------------------------

        #region CONSTRUCTION            
        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceInfoControl"/> class.
        /// </summary>
        public ServiceInfoControl()
        {
            this.InitializeComponent();
        } // ServiceInfoControl()
        #endregion // CONSTRUCTION

        //// ---------------------------------------------------------------------

        #region UI HANDLING            
        /// <summary>
        /// Handles the Load event of the DeviceInfoControl control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing
        /// the event data.</param>
        private void ServiceInfoControlLoad(object sender, EventArgs e)
        {
            this.InitUI();
            this.DisplayServiceInfo();
            this.CheckForServiceImplementation();
        } // ServiceInfoControlLoad()
        #endregion // UI HANDLING

        //// ---------------------------------------------------------------------

        #region PUBLIC METHODS
        /// <summary>
        /// Checks for an available service implementation.
        /// </summary>
        public void CheckForServiceImplementation()
        {
            var showServicePage = false;

            // we use functionality of ContentDirectory:1 that is also
            // fully compatible with ContentDirectory:2
            if (this.Service.Type.Contains("ContentDirectory:"))
            {
                this.AddContentDirectory2Service();
                showServicePage = true;
            }
            else
            {
                this.RemoveContentDirectory2Service();
            } // if

            if ((!showServicePage) && (this.pageServiceImpl != null))
            {
                this.tabControl.TabPages.Remove(this.pageServiceImpl);
                this.pageServiceImpl = null;
            } // if
        } // CheckForServiceImplementation()
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
            this.DisplayServiceInfo();
        } // RefreshDisplay()
        #endregion // ICONTENTCONTROL IMPLEMENTATION

        //// ---------------------------------------------------------------------

        #region PRIVATE METHODS            
        /// <summary>
        /// Removes the content directory2 service.
        /// </summary>
        private void RemoveContentDirectory2Service()
        {
            if ((this.pageServiceImpl != null) && (this.pageServiceImpl.Controls.Count > 0))
            {
                this.pageServiceImpl.Controls.Clear();
            } // if
        } // RemoveContentDirectory2Service()

        /// <summary>
        /// Adds the content directory2 service.
        /// </summary>
        private void AddContentDirectory2Service()
        {
            if ((this.pageServiceImpl == null) || (this.pageServiceImpl.Controls.Count == 0))
            {
                var ctl = new ContentBrowserControl();
                ctl.Service = new ContentDirectoryService(this.Service);
                ctl.Dock = DockStyle.Fill;

                this.pageServiceImpl = new TabPage();
                this.pageServiceImpl.Controls.Add(ctl);
                this.pageServiceImpl.Location = new System.Drawing.Point(4, 22);
                this.pageServiceImpl.Dock = DockStyle.Fill;
                this.pageServiceImpl.Name = "tabServiceImpl";
                this.pageServiceImpl.Padding = new Padding(3);
                this.pageServiceImpl.Size = new System.Drawing.Size(512, 390);
                this.pageServiceImpl.TabIndex = 0;
                this.pageServiceImpl.Text = "Content Directory 2 Service";
                this.pageServiceImpl.UseVisualStyleBackColor = true;

                this.tabControl.TabPages.Add(this.pageServiceImpl);
            }
            else
            {
                var ctl = this.pageServiceImpl.Controls[0] as ContentBrowserControl;
                ctl?.Reset();
            } // if
        } // AddContentDirectory2Service()

        /// <summary>
        /// Displays the service information.
        /// </summary>
        private void DisplayServiceInfo()
        {
            if (this.Service == null)
            {
                return;
            } // if

            this.listViewProperties.Items.Clear();

            this.AddNewPropertyValuePair("Type", this.Service.Type);
            this.AddNewPropertyValuePair("ControlUrl", this.Service.ControlUrl);
            this.AddNewPropertyValuePair("EventSubURL", this.Service.EventSubURL);
            this.AddNewPropertyValuePair("Id", this.Service.Id);
            this.AddNewPropertyValuePair("ScpdUrl", this.Service.ScpdUrl);
            this.AddNewPropertyValuePair("#Actions", this.Service.Actions.Count.ToString());
            this.AddNewPropertyValuePair("#StateVariables", this.Service.StateVariables.Count.ToString());

            this.AddNewPropertyValuePair(string.Empty, string.Empty);
            foreach (var action in this.Service.Actions)
            {
                this.AddNewPropertyValuePair("Action", action.ToString());
            } // foeach

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
    } // ServiceInfoControl
}
