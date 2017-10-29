// ---------------------------------------------------------------------------
// <copyright file="DownloadProgressForm.cs" company="Tethys">
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
    using System.Threading;
    using System.Windows.Forms;
    using UpnpAnalyzer;

    /// <summary>
    /// Form to display a file download progress.
    /// </summary>
    /// <seealso cref="System.Windows.Forms.Form" />
    public partial class DownloadProgressForm : Form
    {
        #region PUBLIC PROPERTIES            
        /// <summary>
        /// Gets or sets the file text.
        /// </summary>
        public string FileText
        {
            get { return this.lblFile.Text; }
            set { this.lblFile.Text = value; }
        }

        /// <summary>
        /// Gets or sets the synchronization context.
        /// </summary>
        public SynchronizationContext SynchronizationContext { get; set; }
        #endregion // PUBLIC PROPERTIES

        //// ---------------------------------------------------------------------

        #region CONSTRUCTION            
        /// <summary>
        /// Initializes a new instance of the <see cref="DownloadProgressForm"/> class.
        /// </summary>
        public DownloadProgressForm()
        {
            this.InitializeComponent();
            this.progressBar.Minimum = 0;
            this.progressBar.Maximum = 100;
        } // DownloadProgressForm()
        #endregion // CONSTRUCTION

        //// ---------------------------------------------------------------------

        #region PUBLIC METHODS

        /// <summary>
        /// Sets the progress.
        /// </summary>
        /// <param name="progress">The progress.</param>
        /// <param name="current">The current.</param>
        /// <param name="total">The total.</param>
        public void SetProgress(int progress, long current, long total)
        {
            if (this.InvokeRequired)
            {
                this.SynchronizationContext?.Post((o) => this.SetProgress(progress, current, total), null);
            }
            else
            {
                this.progressBar.Value = progress;
                this.lblStatus.Text = $"{Support.ToFileSize(current)} of {Support.ToFileSize(total)} total.";
            } // if
        } // SetProgress()
        #endregion // PUBLIC METHODS
    } // DownloadProgressForm
}
