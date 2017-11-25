// ---------------------------------------------------------------------------
// <copyright file="MainForm.cs" company="Tethys">
//   Copyright (C) 2017 T. Graf
// </copyright>
//
// Licensed under the Apache License, Version 2.0.
// Unless required by applicable law or agreed to in writing, 
// software distributed under the License is distributed on an
// "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND,
// either express or implied. 
// ---------------------------------------------------------------------------

////#define SHOW_SEARCH_RESULTS

namespace UpnpAnalyzer.UI
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Threading;
    using System.Windows.Forms;
    using Tethys.Logging;
    using Tethys.Upnp.Core;

    /// <summary>
    /// Main form of the application.
    /// </summary>
    /// <seealso cref="Form" />
    public partial class MainForm : Form
    {
        #region PRIVATE PROPERTIES
        /// <summary>
        /// The logger for this class.
        /// </summary>
        private static ILog log;

        /// <summary>
        /// The SSDP protocol support.
        /// </summary>
        private readonly SSDP ssdp;

        /// <summary>
        /// The root node.
        /// </summary>
        private TreeNode root;

        /// <summary>
        /// The current view.
        /// </summary>
        private IContentControl currentView;

        /// <summary>
        /// The device view.
        /// </summary>
        private DeviceInfoControl deviceView;

        /// <summary>
        /// The service view.
        /// </summary>
        private ServiceInfoControl serviceView;

        /// <summary>
        /// The action view.
        /// </summary>
        private ActionInfoControl actionView;
        #endregion // PRIVATE PROPERTIES

        //// ---------------------------------------------------------------------

        #region CONSTRUCTION
        /// <summary>
        /// Initializes a new instance of the <see cref="MainForm"/> class.
        /// </summary>
        public MainForm()
        {
            this.InitializeComponent();
            this.ConfigureLogging();

            this.ssdp = new SSDP();
            this.ssdp.SynchronizationContext = SynchronizationContext.Current;
            this.ssdp.DeviceFoundCallback = this.OnDeviceFound;
            this.ssdp.DeviceRemovedCallback = this.OnDeviceRemoved;
        } // MainForm()
        #endregion // CONSTRUCTION

        //// ---------------------------------------------------------------------

        #region UI HANDLING
        /// <summary>
        /// Handles the Load event of the MainForm control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing
        /// the event data.</param>
        private void MainFormLoad(object sender, EventArgs e)
        {
            this.InitUI();

            try
            {
                this.ssdp.StartDiscoveryService();
            }
            catch (Exception ex)
            {
                log.Error("Error starting discovery service", ex);
            } // catch

            log.Info("Ready.");
        } // MainFormLoad()

        /// <summary>
        /// Handles the FormClosing event of the MainForm control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="FormClosingEventArgs"/> instance
        /// containing the event data.</param>
        private void MainFormFormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                this.ssdp.StopDiscoveryService();
            }
            catch (Exception ex)
            {
                log.Error("Error terminating", ex);
            } // catch
        } // MainFormFormClosing()

        /// <summary>
        /// Handles the Click event of the <c>btnDiscoverDevices</c> control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private async void BtnDiscoverDevicesClick(object sender, EventArgs e)
        {
            log.Info("Starting device search...");
            var result = await this.ssdp.SearchForDevices();
            log.Info($"{result.Count} devices detected.");
            log.Info("Device search finished.");
        } // BtnDiscoverDevicesClick()

        /// <summary>
        /// Handles the AfterSelect event of the treeViewDevices control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="TreeViewEventArgs"/> instance containing the event data.</param>
        private void TreeViewDevicesAfterSelect(object sender, TreeViewEventArgs e)
        {
            var data = e.Node?.Tag as NodeData;
            if (data == null)
            {
                return;
            } // if

            this.SwitchContentPanel(data);
        } // TreeViewDevicesAfterSelect()
        #endregion // UI HANDLING
        //// ---------------------------------------------------------------------

        #region PRIVATE METHODS
        /// <summary>
        /// Called when a device has been found.
        /// </summary>
        /// <param name="upnpDevice">The <c>UPnP</c> device.</param>
        private async void OnDeviceFound(UpnpDevice upnpDevice)
        {
            try
            {
                var text = await UPNP.GetRemoteTextFile(upnpDevice.Location);
                upnpDevice.DeviceDescription = UPNP.ParseDeviceSchema(text);

                Image image = null;
                var icon = upnpDevice.DeviceDescription.Icons.FirstOrDefault(x => x.Height == 48);
                if (icon != null)
                {
                    var url = UPNP.BuildUrl(upnpDevice, icon.URL);
                    image = await UPNP.GetRemoteImageFile(url);
                } // if
                
                var node = this.AddDevice(upnpDevice, this.root, image);
                this.AddSubDevices(upnpDevice, node, image);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
            } // catch
        } // OnDeviceFound()

        /// <summary>
        /// Adds the sub devices.
        /// </summary>
        /// <param name="upnpDevice">The <c>UPnP</c> device.</param>
        /// <param name="parent">The parent.</param>
        /// <param name="image">The image.</param>
        private void AddSubDevices(UpnpDevice upnpDevice, TreeNode parent, Image image)
        {
            foreach (var subDeviceDescription in upnpDevice.DeviceDescription.SubDevices)
            {
                var subDevice = new UpnpDevice(upnpDevice.Type, upnpDevice.Server,
                    upnpDevice.Location, upnpDevice.USN, upnpDevice.Address);
                subDevice.DeviceDescription = subDeviceDescription;
                var node = this.AddDevice(subDevice, parent, image);
                this.AddSubDevices(subDevice, node, image);
            } // foreach
        } // AddSubDevices()

        /// <summary>
        /// Adds a new (sub) device.
        /// </summary>
        /// <param name="upnpDevice">The <c>UPnP</c> device.</param>
        /// <param name="parent">The parent.</param>
        /// <param name="image">The image.</param>
        /// <returns>The resulting <see cref="TreeNode"/>.</returns>
        private TreeNode AddDevice(UpnpDevice upnpDevice, TreeNode parent, Image image)
        {
            var tn = this.AddDeviceNode(parent, upnpDevice, image);
            tn.EnsureVisible();
            tn.Expand();

            ////log.Info($"    {upnpDevice.DeviceDescription.FriendlyName}");
            foreach (var service in upnpDevice.DeviceDescription.Services)
            {
                this.OnServiceFound(tn, upnpDevice, service);
            } // foreach

            return tn;
        } // AddDevice()

        /// <summary>
        /// Called when a device has been removed.
        /// </summary>
        /// <param name="upnpDevice">The <c>UPnP</c> device.</param>
        private void OnDeviceRemoved(UpnpDevice upnpDevice)
        {
            var node = this.FindDeviceTreeNode(upnpDevice);
            if (node != null)
            {
                log.Debug($"Device '{upnpDevice.USN}' removed.");
                this.root.Nodes.Remove(node);
            } // if
        } // OnDeviceRemoved()

        /// <summary>
        /// Finds the device tree node.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <returns>A <see cref="TreeNode"/> or null.</returns>
        private TreeNode FindDeviceTreeNode(UpnpDevice device)
        {
            foreach (TreeNode node in this.root.Nodes)
            {
                // node.Tag = new NodeData(NodeType.Device, device);
                var nd = node.Tag as NodeData;
                var dev = nd?.PayLoad as UpnpDevice;
                if (dev == null)
                {
                    continue;
                } // if

                if (dev.USN == device.USN)
                {
                    return node;
                } // if
            } // foreach

            return null;
        } // FindTreeNode()

        /// <summary>
        /// Called when a service has been found.
        /// </summary>
        /// <param name="nodeDevice">The node device.</param>
        /// <param name="upnpDevice">The <c>UPnP</c> device.</param>
        /// <param name="service">The service.</param>
        private async void OnServiceFound(TreeNode nodeDevice, UpnpDevice upnpDevice, UpnpService service)
        {
            try
            {
                var url = UPNP.BuildUrl(upnpDevice, service.ScpdUrl);
                var text = await UPNP.GetRemoteTextFile(url);
                var serviceDescription = UPNP.ParseServiceSchema(text);
                service.UpdateServiceInfo(serviceDescription);
                service.ScpdUrl = url;
                service.ControlUrl = UPNP.BuildUrl(upnpDevice, service.ControlUrl);
                service.EventSubURL = UPNP.BuildUrl(upnpDevice, service.EventSubURL);

#if SHOW_SEARCH_RESULTS
                log.Info($"        {service.Type}");
#endif

                if ((service.Actions.Count == 0) && (service.StateVariables.Count == 0))
                {
                    // seems to be a dummy service
                    return;
                } // if

                var tn = AddServiceNode(nodeDevice, service);
                foreach (var action in service.Actions)
                {
                    action.Service = service;
                    this.OnActionFound(tn, action);
                } // foreach
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
            } // catch
        } // OnServiceFound()

        /// <summary>
        /// Called when an action has been found.
        /// </summary>
        /// <param name="nodeService">The node service.</param>
        /// <param name="action">The action.</param>
        private void OnActionFound(TreeNode nodeService, UpnpServiceAction action)
        {
#if SHOW_SEARCH_RESULTS
            log.Info($"            {action}");
#endif
            AddActionNode(nodeService, action);
        } // OnActionFound()

        /// <summary>
        /// Switches the content panel.
        /// </summary>
        /// <param name="nodeData">The node data.</param>
        private void SwitchContentPanel(NodeData nodeData)
        {
            switch (nodeData.Type)
            {
                case NodeType.Home:
                    this.SwitchToEmptyView();
                    break;
                case NodeType.Device:
                    this.SwitchToDeviceView(nodeData.PayLoad as UpnpDevice);
                    break;
                case NodeType.Service:
                    this.SwitchToServiceView(nodeData.PayLoad as UpnpService);
                    break;
                case NodeType.Action:
                    this.SwitchToActionView(nodeData.PayLoad as UpnpServiceAction);
                    break;
                default:
                    this.SwitchToEmptyView();
                    break;
            } // switch

            this.currentView.ForceRefresh = true;
            this.currentView.RefreshDisplay();
        } // SwitchContentPanel()

        /// <summary>
        /// Switches to the action view.
        /// </summary>
        /// <param name="action">The action.</param>
        private void SwitchToActionView(UpnpServiceAction action)
        {
            if (this.actionView == null)
            {
                this.actionView = new ActionInfoControl();
            } // if

            this.SetCurrentViewControl(this.actionView);
            this.currentView = this.actionView;
            this.actionView.Action = action;
        } // SwitchToActionView()

        /// <summary>
        /// Switches to the service view.
        /// </summary>
        /// <param name="service">The service.</param>
        private void SwitchToServiceView(UpnpService service)
        {
            if (this.serviceView == null)
            {
                this.serviceView = new ServiceInfoControl();
            } // if

            this.serviceView.Service = service;
            this.serviceView.CheckForServiceImplementation();
            this.SetCurrentViewControl(this.serviceView);
            this.currentView = this.serviceView;
        } // SwitchToServiceView()

        /// <summary>
        /// Switches to the device view.
        /// </summary>
        /// <param name="device">The device.</param>
        private void SwitchToDeviceView(UpnpDevice device)
        {
            if (this.deviceView == null)
            {
                this.deviceView = new DeviceInfoControl();
            } // if

            this.SetCurrentViewControl(this.deviceView);
            this.currentView = this.deviceView;
            this.deviceView.Device = device;
        } // SwitchToDeviceView()

        /// <summary>
        /// Switches to the empty view.
        /// </summary>
        private void SwitchToEmptyView()
        {
            if (this.emptyViewControl == null)
            {
                this.emptyViewControl = new EmptyViewControl();
            } // if

            this.SetCurrentViewControl(this.emptyViewControl);
            this.currentView = this.emptyViewControl;
        } // SwitchToEmptyView()

        /// <summary>
        /// Sets the current view control.
        /// </summary>
        /// <param name="control">The control.</param>
        private void SetCurrentViewControl(Control control)
        {
            this.splitContainerHorizontal.Panel2.Controls.Clear();
            this.splitContainerHorizontal.Panel2.Controls.Add(control);
            control.Dock = DockStyle.Fill;
        } // SetCurrentViewControl()

        /// <summary>
        /// Adds a device node.
        /// </summary>
        /// <param name="parent">The parent.</param>
        /// <param name="device">The device.</param>
        /// <param name="image">The (optional) image.</param>
        /// <returns>A <see cref="TreeNode"/>.</returns>
        private TreeNode AddDeviceNode(TreeNode parent, UpnpDevice device, Image image = null)
        {
            var node = new TreeNode(device.DeviceDescription.FriendlyName);
            if (image == null)
            {
                node.ImageKey = @"Device";
            }
            else
            {
                this.imageList.Images.Add(device.USN, image);
                node.ImageKey = device.USN;
            } // if

            node.Tag = new NodeData(NodeType.Device, device);
            node.SelectedImageKey = node.ImageKey;
            parent.Nodes.Add(node);

            return node;
        } // AddDeviceNode()

        /// <summary>
        /// Adds a service node.
        /// </summary>
        /// <param name="deviceNode">The device node.</param>
        /// <param name="service">The service.</param>
        /// <returns>A <see cref="TreeNode"/>.</returns>
        private static TreeNode AddServiceNode(TreeNode deviceNode, UpnpService service)
        {
            const string ServicePrefix = "urn:schemas-upnp-org:service:";

            var type = service.Type;
            if (type.StartsWith(ServicePrefix))
            {
                type = type.Substring(ServicePrefix.Length);
            } // if

            var node = new TreeNode(type);
            node.ImageKey = @"Service";
            node.SelectedImageKey = @"Service";
            node.Tag = new NodeData(NodeType.Service, service);

            deviceNode.Nodes.Add(node);

            return node;
        } // AddServiceNode()

        /// <summary>
        /// Adds the action node.
        /// </summary>
        /// <param name="serviceNode">The service node.</param>
        /// <param name="action">The action.</param>
        private static void AddActionNode(TreeNode serviceNode, UpnpServiceAction action)
        {
            var node = new TreeNode(action.Name);
            node.ImageKey = @"Action";
            node.SelectedImageKey = @"Action";
            node.Tag = new NodeData(NodeType.Action, action);

            serviceNode.Nodes.Add(node);
        } // AddActionNode()

        /// <summary>
        /// Initializes the image list.
        /// </summary>
        private void InitializeImageList()
        {
            var ch = CreateCharacterIcon('H', Color.DarkGray);
            var cd = CreateCharacterIcon('D', Color.Black);
            var cs = CreateCharacterIcon('S', Color.Green);
            var ca = CreateCharacterIcon('A', Color.Blue);
            var cv = CreateCharacterIcon('V', Color.Orange);

            this.imageList.Images.Add("Home", ch);
            this.imageList.Images.Add("Device", cd);
            this.imageList.Images.Add("Service", cs);
            this.imageList.Images.Add("Action", ca);
            this.imageList.Images.Add("Variable", cv);
        } // InitializeImageList()

        /// <summary>
        /// Initializes the UI.
        /// </summary>
        private void InitUI()
        {
            this.root = new TreeNode("This PC");
            this.root.ImageKey = @"Home";
            this.root.SelectedImageKey = @"Home";
            this.root.Tag = new NodeData(NodeType.Home, null);

            this.treeViewDevices.Nodes.Add(this.root);
            this.treeViewDevices.ImageList = this.imageList;

            this.InitializeImageList();
            this.root.Expand();
        } // InitUI()

        /// <summary>
        /// Creates a character icon.
        /// </summary>
        /// <param name="character">The character to draw in the icon.</param>
        /// <param name="color">The color of the char in the icon.</param>
        /// <returns>A 16x16 32 bit per pixel ARGB bitmap for the icon.</returns>
        protected static Bitmap CreateCharacterIcon(char character, Color color)
        {
            // create the bitmap
            var bitmap = new Bitmap(16, 16, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            // create the graphics
            using (var graphics = Graphics.FromImage(bitmap))
            {
                using (var font = new Font("Arial", 14, FontStyle.Bold, GraphicsUnit.Pixel))
                {
                    using (Brush brush = new SolidBrush(color))
                    {
                        // no anti-aliasing - transparency only (no alpha)
                        graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SingleBitPerPixelGridFit;

                        // Setup a centered string format (horizontally and vertically)
                        var lsfFormat = new StringFormat();
                        lsfFormat.LineAlignment = StringAlignment.Center;
                        lsfFormat.Alignment = StringAlignment.Center;

                        // Draw the char on the bitmap
                        graphics.DrawString(character.ToString(), font, brush,
                            new Point(7, 9), lsfFormat);
                    } // using
                } // using
            } // using

            return bitmap;
        } // CreateCharacterIcon()

        /// <summary>
        /// Configures the logging.
        /// </summary>
        private void ConfigureLogging()
        {
            this.rtfLogView.AddAtTail = true;
#if !DEBUG
            this.rtfLogView.MaxLogLength = 10000;
#endif
            this.rtfLogView.ShowDebugEvents = true;

            var settings = new Dictionary<string, string>();
            ////settings.Add("AddTime", "false");
            settings.Add("AddLevel", "false");
            LogManager.Adapter = new LogViewFactoryAdapter(this.rtfLogView, settings);
            log = LogManager.GetLogger(typeof(MainForm));
        } // ConfigureLogging()
        #endregion // PRIVATE METHODS
    } // MainForm
}
