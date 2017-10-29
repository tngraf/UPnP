namespace UpnpAnalyzer.UI
{
    partial class MainForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.splitContainerHorizontal = new System.Windows.Forms.SplitContainer();
            this.treeViewDevices = new System.Windows.Forms.TreeView();
            this.emptyViewControl = new EmptyViewControl();
            this.btnDiscoverDevices = new System.Windows.Forms.Button();
            this.rtfLogView = new Tethys.Logging.Controls.RtfLogView();
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerHorizontal)).BeginInit();
            this.splitContainerHorizontal.Panel1.SuspendLayout();
            this.splitContainerHorizontal.Panel2.SuspendLayout();
            this.splitContainerHorizontal.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer
            // 
            this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer.Location = new System.Drawing.Point(0, 0);
            this.splitContainer.Name = "splitContainer";
            this.splitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.Controls.Add(this.splitContainerHorizontal);
            this.splitContainer.Panel1.Controls.Add(this.btnDiscoverDevices);
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.Controls.Add(this.rtfLogView);
            this.splitContainer.Size = new System.Drawing.Size(816, 634);
            this.splitContainer.SplitterDistance = 455;
            this.splitContainer.TabIndex = 2;
            // 
            // splitContainerHorizontal
            // 
            this.splitContainerHorizontal.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainerHorizontal.Location = new System.Drawing.Point(12, 12);
            this.splitContainerHorizontal.Name = "splitContainerHorizontal";
            // 
            // splitContainerHorizontal.Panel1
            // 
            this.splitContainerHorizontal.Panel1.Controls.Add(this.treeViewDevices);
            // 
            // splitContainerHorizontal.Panel2
            // 
            this.splitContainerHorizontal.Panel2.Controls.Add(this.emptyViewControl);
            this.splitContainerHorizontal.Size = new System.Drawing.Size(792, 411);
            this.splitContainerHorizontal.SplitterDistance = 369;
            this.splitContainerHorizontal.TabIndex = 4;
            // 
            // treeViewDevices
            // 
            this.treeViewDevices.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.treeViewDevices.ItemHeight = 18;
            this.treeViewDevices.Location = new System.Drawing.Point(0, 0);
            this.treeViewDevices.Name = "treeViewDevices";
            this.treeViewDevices.Size = new System.Drawing.Size(366, 411);
            this.treeViewDevices.TabIndex = 1;
            this.treeViewDevices.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.TreeViewDevicesAfterSelect);
            // 
            // emptyViewControl
            // 
            this.emptyViewControl.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.emptyViewControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.emptyViewControl.ForceRefresh = false;
            this.emptyViewControl.Location = new System.Drawing.Point(0, 0);
            this.emptyViewControl.Name = "emptyViewControl";
            this.emptyViewControl.Size = new System.Drawing.Size(419, 411);
            this.emptyViewControl.TabIndex = 0;
            // 
            // btnDiscoverDevices
            // 
            this.btnDiscoverDevices.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDiscoverDevices.Location = new System.Drawing.Point(12, 427);
            this.btnDiscoverDevices.Name = "btnDiscoverDevices";
            this.btnDiscoverDevices.Size = new System.Drawing.Size(129, 23);
            this.btnDiscoverDevices.TabIndex = 0;
            this.btnDiscoverDevices.Text = "Discover Devices";
            this.btnDiscoverDevices.UseVisualStyleBackColor = true;
            this.btnDiscoverDevices.Click += new System.EventHandler(this.BtnDiscoverDevicesClick);
            // 
            // rtfLogView
            // 
            this.rtfLogView.AddAtTail = true;
            this.rtfLogView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rtfLogView.LabelText = "Status:";
            this.rtfLogView.Location = new System.Drawing.Point(3, 3);
            this.rtfLogView.MaxLogLength = -1;
            this.rtfLogView.Name = "rtfLogView";
            this.rtfLogView.ShowDebugEvents = false;
            this.rtfLogView.Size = new System.Drawing.Size(810, 169);
            this.rtfLogView.TabIndex = 0;
            this.rtfLogView.TextColor = System.Drawing.Color.Black;
            // 
            // imageList
            // 
            this.imageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.imageList.ImageSize = new System.Drawing.Size(16, 16);
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(816, 634);
            this.Controls.Add(this.splitContainer);
            this.Name = "MainForm";
            this.Text = "UPnP Analyzer";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainFormFormClosing);
            this.Load += new System.EventHandler(this.MainFormLoad);
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
            this.splitContainer.ResumeLayout(false);
            this.splitContainerHorizontal.Panel1.ResumeLayout(false);
            this.splitContainerHorizontal.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerHorizontal)).EndInit();
            this.splitContainerHorizontal.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer;
        private Tethys.Logging.Controls.RtfLogView rtfLogView;
    private System.Windows.Forms.Button btnDiscoverDevices;
        private System.Windows.Forms.SplitContainer splitContainerHorizontal;
        private System.Windows.Forms.TreeView treeViewDevices;
        private System.Windows.Forms.ImageList imageList;
        private EmptyViewControl emptyViewControl;
    }
}

