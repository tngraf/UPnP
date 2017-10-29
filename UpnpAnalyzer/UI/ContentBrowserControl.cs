// ---------------------------------------------------------------------------
// <copyright file="ContentBrowserControl.cs" company="Tethys">
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
    using System.Net;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows.Forms;
    using Tethys.Logging;
    using Tethys.Upnp.Services.ContentDirectory;
    using UpnpAnalyzer;

    /// <summary>
    /// Implement a browser for the content of a media server that
    /// implements the content directory service.
    /// </summary>
    /// <seealso cref="UserControl" />
    public partial class ContentBrowserControl : UserControl
    {
        #region PRIVATE PROPERTIES
        /// <summary>
        /// The dummy identifier.
        /// </summary>
        private const string DummyId = "Dummy!$%";

        /// <summary>
        /// The logger for this class.
        /// </summary>
        private static readonly ILog Log = LogManager.GetLogger(typeof(ContentBrowserControl));

        /// <summary>
        /// The progress form.
        /// </summary>
        private DownloadProgressForm progressForm;
        #endregion // PRIVATE PROPERTIES

        //// ---------------------------------------------------------------------

        #region PUBLIC PROPERTIES            
        /// <summary>
        /// Gets or sets the service.
        /// </summary>
        public ContentDirectoryService Service { get; set; }
        #endregion // PUBLIC PROPERTIES

        //// ---------------------------------------------------------------------

        #region CONSTRUCTION            
        /// <summary>
        /// Initializes a new instance of the <see cref="ContentBrowserControl"/> class.
        /// </summary>
        public ContentBrowserControl()
        {
            this.InitializeComponent();
        } // ContentBrowserControl()
        #endregion // CONSTRUCTION

        //// ---------------------------------------------------------------------

        #region UI METHODS
        /// <summary>
        /// Handles the Load event of the ContentBrowserControl control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the 
        /// event data.</param>
        private void ContentBrowserControlLoad(object sender, EventArgs e)
        {
            this.InitializeTree();
        } // ContentBrowserControlLoad()

        /// <summary>
        /// Handles the AfterExpand event of the treeView control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="TreeViewEventArgs"/> instance containing the event data.</param>
        private async void TreeViewAfterExpand(object sender, TreeViewEventArgs e)
        {
            var tag = e.Node.Nodes[0].Tag;
            if (tag != null && ((e.Node.Nodes.Count == 1) && ((string)tag == DummyId)))
            {
                var count = 0;
                try
                {
                    count = await this.PopulateChildren(e.Node);
                }
                catch (Exception ex)
                {
                    Log.Error("Error getting child nodes", ex);
                } // catch

                if (count == 0)
                {
                    e.Node.Nodes.Clear();
                } // if
            } // if
        } // TreeViewAfterExpand()

        /// <summary>
        /// Handles the Click event of the 'show details' context menu item.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private async void CtxShowDetailsClick(object sender, EventArgs e)
        {
            try
            {
                var node = this.treeView.SelectedNode;
                if (node == null)
                {
                    return;
                } // if

                await this.ShowDetails(node);
            }
            catch (Exception ex)
            {
                Log.Error("Error geting details file", ex);
            } // catch
        } // CtxShowDetailsClick()

        /// <summary>
        /// Handles the Click event of the 'Download Item' context menu item.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private async void CtxDownloadItemClick(object sender, EventArgs e)
        {
            try
            {
                var node = this.treeView.SelectedNode;
                if (node == null)
                {
                    return;
                } // if

                await this.DownloadItem(node);
            }
            catch (Exception ex)
            {
                Log.Error("Error downloading file", ex);
            } // catch
        } // CtxDownloadItemClick()
        #endregion // UI METHODS

        //// ---------------------------------------------------------------------

        #region PUBLIC METHODS            
        /// <summary>
        /// Resets this instance.
        /// </summary>
        public void Reset()
        {
            var root = new TreeNode("Root");
            root.Tag = "0";
            root.ImageKey = "drive";
            root.SelectedImageKey = root.ImageKey;
            AddDummyNode(root);

            this.treeView.Nodes.Clear();
            this.treeView.Nodes.Add(root);
        } // Reset()
        #endregion // PUBLIC METHODS

        //// ---------------------------------------------------------------------

        #region PRIVATE METHODS
        /// <summary>
        /// Downloads the item.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <returns><c>true</c>if the item has been successfully downloaded;
        /// otherwise <c>false</c>.</returns>
        private async Task<bool> DownloadItem(TreeNode node)
        {
            var id = node?.Tag as string;
            if (id == null)
            {
                return false;
            } // if

            var metadata = await this.Service.BrowseMetaData(id, "*");
            if (metadata == null)
            {
                return false;
            } // if

            var address = metadata.Resource;

            var filename = metadata.Title;
            var ext = GetFileExtensionFromProtocolInfo(metadata.ProtocolInfo);
            if (string.IsNullOrEmpty(ext))
            {
                ext = ".dat";
            } // if

            if (!filename.EndsWith(ext))
            {
                filename = filename + ext;
            } // if

            if (!string.IsNullOrEmpty(address))
            {
                var dlg = new SaveFileDialog();
                dlg.CheckPathExists = true;
                dlg.FileName = filename;
                dlg.DefaultExt = ext;
                if (dlg.ShowDialog(this) != DialogResult.OK)
                {
                    return true;
                } // if

                if (metadata.Size < 1000000)
                {
                    return await GetRemoteFileAsync(address, dlg.FileName);
                } // if
                
                this.progressForm = new DownloadProgressForm();
                this.progressForm.FileText = filename;
                this.progressForm.SynchronizationContext = SynchronizationContext.Current;
                this.progressForm.Show(this);
                this.GetRemoteFile(address, dlg.FileName, metadata.Size);
                return true;
            } // if

            Log.Warn("No resource address found!");

            return false;
        } // DownloadItem()

        /// <summary>
        /// Gets the file extension from protocol information.
        /// </summary>
        /// <param name="protocolInfo">The protocol information.</param>
        /// <returns>A file extension string.</returns>
        private static string GetFileExtensionFromProtocolInfo(string protocolInfo)
        {
            if (string.IsNullOrEmpty(protocolInfo))
            {
                return string.Empty;
            } // if

            protocolInfo = protocolInfo.ToLowerInvariant();

            if (protocolInfo.Contains("mpeg-tts"))
            {
                return ".TTS";
            } // if

            if (protocolInfo.Contains("mpeg"))
            {
                return ".mpeg";
            } // if

            if (protocolInfo.Contains("jpeg"))
            {
                return ".jpeg";
            } // if

            if (protocolInfo.Contains("flac"))
            {
                return ".flac";
            } // if

            if (protocolInfo.Contains("mp4"))
            {
                return ".mp4";
            } // if

            if (protocolInfo.Contains("wma"))
            {
                return ".wma";
            } // if

            if (protocolInfo.Contains("wmv"))
            {
                return ".wmv";
            } // if

            if (protocolInfo.Contains("avi"))
            {
                return ".avi";
            } // if

            return string.Empty;
        } // GetFileExtensionFromProtocolInfo()

        /// <summary>
        /// Gets a text file from the given address via HTTP.
        /// </summary>
        /// <param name="address">The address.</param>
        /// <param name="destination">The destination.</param>
        /// <returns><c>true</c>if the item has been successfully downloaded;
        /// otherwise <c>false</c>.</returns>
        private static async Task<bool> GetRemoteFileAsync(string address, string destination)
        {
            using (var client = new WebClient())
            {
                await client.DownloadFileTaskAsync(address, destination);
            } // using
                
            return true;
        } // GetRemoteFile()

        /// <summary>
        /// Gets a text file from the given address via HTTP.
        /// </summary>
        /// <param name="address">The address.</param>
        /// <param name="destination">The destination.</param>
        /// <param name="filesize">The file size.</param>
        private void GetRemoteFile(string address, string destination, long filesize)
        {
            using (var client = new WebClient())
            {
                Log.Info($"Starting to download {destination}..");
                client.DownloadProgressChanged += this.OnDownloadProgressChanged;
                client.DownloadFileCompleted += this.OnDownloadFileCompleted;
                client.DownloadFileAsync(new Uri(address), destination, filesize);
            } // using
        } // GetRemoteFile()

        /// <summary>
        /// Handles the DownloadFileCompleted event of the Client control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.ComponentModel.AsyncCompletedEventArgs"/>
        /// instance containing the event data.</param>
        private void OnDownloadFileCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            if (this.progressForm != null)
            {
                Log.Info($"Download finished.");
                this.progressForm.Close();
                this.progressForm = null;
            } // if
        } // OnDownloadFileCompleted()

        /// <summary>
        /// Handles the DownloadProgressChanged event of the Client control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="DownloadProgressChangedEventArgs"/> instance containing the event data.</param>
        private void OnDownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            var totalBytesToReceive = e.TotalBytesToReceive;
            var progressPercentage = e.ProgressPercentage;
            if (e.TotalBytesToReceive <= 0)
            {
                totalBytesToReceive = (long)e.UserState;
                progressPercentage = Convert.ToInt32((e.BytesReceived * 100) / totalBytesToReceive);
            } // if

            this.progressForm?.SetProgress(progressPercentage, e.BytesReceived, totalBytesToReceive);
        } // OnDownloadProgressChanged()

        /// <summary>
        /// Shows the details.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <returns><c>true</c> if the detail data could get successfully
        /// retrieved; otherwise <c>false</c>.</returns>
        private async Task<bool> ShowDetails(TreeNode node)
        {
            var id = node?.Tag as string;
            if (id == null)
            {
                return false;
            } // if

            var metadata = await this.Service.BrowseMetaData(id, "*");
            if (metadata == null)
            {
                return false;
            } // if

            var dlg = new PropertyForm();
            dlg.AddNewPropertyValuePair("Id", metadata.Id);
            dlg.AddNewPropertyValuePair("Title", metadata.Title);
            dlg.AddNewPropertyValuePair("ParentId", metadata.ParentId);
            dlg.AddNewPropertyValuePair("Class", metadata.Class);
            dlg.AddNewPropertyValuePair("Restricted", metadata.Restricted.ToString());
            if (!string.IsNullOrEmpty(metadata.Date))
            {
                dlg.AddNewPropertyValuePair("Date", metadata.Date);
            } // if

            if (!string.IsNullOrEmpty(metadata.WriteStatus))
            {
                dlg.AddNewPropertyValuePair("WriteStatus", metadata.WriteStatus);
            } // if

            if (!string.IsNullOrEmpty(metadata.SearchClass))
            {
                dlg.AddNewPropertyValuePair("SearchClass", metadata.SearchClass);
            } // if

            if (!string.IsNullOrEmpty(metadata.Resource))
            {
                dlg.AddNewPropertyValuePair("Resource", metadata.Resource);
            } // if

            if (!string.IsNullOrEmpty(metadata.ProtocolInfo))
            {
                dlg.AddNewPropertyValuePair("ProtocolInfo", metadata.ProtocolInfo);
            } // if

            if (!string.IsNullOrEmpty(metadata.Resolution))
            {
                dlg.AddNewPropertyValuePair("Resolution", metadata.Resolution);
            } // if

            dlg.AddNewPropertyValuePair("Size", Support.ToFileSize(metadata.Size));
            dlg.AddNewPropertyValuePair("Bitrate", metadata.Bitrate.ToString());
            dlg.AddNewPropertyValuePair("NumAudioChannels", metadata.NumAudioChannels.ToString());
            dlg.AddNewPropertyValuePair("SampleFrequency", metadata.SampleFrequency.ToString());

            if (!string.IsNullOrEmpty(metadata.Duration))
            {
                dlg.AddNewPropertyValuePair("Duration", metadata.Duration);
            } // if

            dlg.AutoResize = true;
            dlg.ShowDialog(this);

            return true;
        } // ShowDetails()

        /// <summary>
        /// Populates theo node with its children.
        /// </summary>
        /// <param name="parent">The parent.</param>
        /// <returns>The number of children found.</returns>
        private async Task<int> PopulateChildren(TreeNode parent)
        {
            var id = parent?.Tag as string;
            if (id == null)
            {
                return 0;
            } // if

            parent.Nodes.Clear();
            parent.Nodes.Add(new TreeNode("(loading)"));

            var childdata = await this.Service.BrowseDirectChildren(id, "*");
            if (childdata == null)
            {
                return 0;
            } // if

            parent.Nodes.Clear();
            foreach (var child in childdata.Children)
            {
                var node = new TreeNode(child.Title);
                node.Tag = child.Id;
                node.ImageKey = GetItemIconKey(child);
                node.SelectedImageKey = node.ImageKey;
                if (child.IsContainer)
                {
                    AddDummyNode(node);
                } // if

                parent.Nodes.Add(node);
            } // foreach

            return childdata.Children.Count;
        } // PopulateChildren()

        /// <summary>
        /// Gets the item icon key.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns>A string icon key.</returns>
        private static string GetItemIconKey(UpnpChildData data)
        {
            if (data.IsContainer)
            {
                return "folder";
            } // if

            var cls = data.Class.ToUpperInvariant();
            if (cls.Contains("AUDIOITEM"))
            {
                return "music";
            } // if

            if (cls.Contains("PHOTO"))
            {
                return "picture";
            } // if

            if (cls.Contains("VIDEOITEM"))
            {
                return "film";
            } // if

            return string.Empty;
        } // GetItemIconKey()

        /// <summary>
        /// Adds the dummy node that we use for delay loading the child data.
        /// </summary>
        /// <param name="node">The node.</param>
        private static void AddDummyNode(TreeNode node)
        {
            var dummy = new TreeNode(string.Empty);
            dummy.Tag = DummyId;

            node.Nodes.Clear();
            node.Nodes.Add(dummy);
        } // AddDummyNode()

        /// <summary>
        /// Initializes the tree.
        /// </summary>
        private void InitializeTree()
        {
            this.Reset();
        } // InitializeTree()
        #endregion // PRIVATE METHODS
    } // ContentBrowserControl
}
