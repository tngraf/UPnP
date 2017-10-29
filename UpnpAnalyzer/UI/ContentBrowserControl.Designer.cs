namespace UpnpAnalyzer.UI
{
    partial class ContentBrowserControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ContentBrowserControl));
            this.treeView = new System.Windows.Forms.TreeView();
            this.contextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ctxShowDetails = new System.Windows.Forms.ToolStripMenuItem();
            this.ctxDownloadItem = new System.Windows.Forms.ToolStripMenuItem();
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.contextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // treeView
            // 
            this.treeView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.treeView.ContextMenuStrip = this.contextMenu;
            this.treeView.ImageIndex = 0;
            this.treeView.ImageList = this.imageList;
            this.treeView.ItemHeight = 18;
            this.treeView.Location = new System.Drawing.Point(3, 3);
            this.treeView.Name = "treeView";
            this.treeView.SelectedImageIndex = 0;
            this.treeView.Size = new System.Drawing.Size(420, 357);
            this.treeView.TabIndex = 0;
            this.treeView.AfterExpand += new System.Windows.Forms.TreeViewEventHandler(this.TreeViewAfterExpand);
            // 
            // contextMenu
            // 
            this.contextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ctxShowDetails,
            this.ctxDownloadItem});
            this.contextMenu.Name = "contextMenu";
            this.contextMenu.Size = new System.Drawing.Size(151, 48);
            // 
            // ctxShowDetails
            // 
            this.ctxShowDetails.Name = "ctxShowDetails";
            this.ctxShowDetails.Size = new System.Drawing.Size(150, 22);
            this.ctxShowDetails.Text = "Show Details...";
            this.ctxShowDetails.Click += new System.EventHandler(this.CtxShowDetailsClick);
            // 
            // ctxDownloadItem
            // 
            this.ctxDownloadItem.Name = "ctxDownloadItem";
            this.ctxDownloadItem.Size = new System.Drawing.Size(150, 22);
            this.ctxDownloadItem.Text = "Download...";
            this.ctxDownloadItem.Click += new System.EventHandler(this.CtxDownloadItemClick);
            // 
            // imageList
            // 
            this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList.Images.SetKeyName(0, "document");
            this.imageList.Images.SetKeyName(1, "folder");
            this.imageList.Images.SetKeyName(2, "folder-open");
            this.imageList.Images.SetKeyName(3, "drive");
            this.imageList.Images.SetKeyName(4, "picture");
            this.imageList.Images.SetKeyName(5, "music");
            this.imageList.Images.SetKeyName(6, "film");
            this.imageList.Images.SetKeyName(7, "question");
            // 
            // ContentBrowserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.treeView);
            this.Name = "ContentBrowserControl";
            this.Size = new System.Drawing.Size(426, 363);
            this.Load += new System.EventHandler(this.ContentBrowserControlLoad);
            this.contextMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView treeView;
        private System.Windows.Forms.ContextMenuStrip contextMenu;
        private System.Windows.Forms.ToolStripMenuItem ctxShowDetails;
        private System.Windows.Forms.ToolStripMenuItem ctxDownloadItem;
        private System.Windows.Forms.ImageList imageList;
    }
}
