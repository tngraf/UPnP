// ---------------------------------------------------------------------------
// <copyright file="EmptyViewControl.cs" company="Tethys">
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
    using System.Windows.Forms;

    /// <summary>
    /// An empty view that implements <see cref="IContentControl"/>.
    /// </summary>
    /// <seealso cref="System.Windows.Forms.UserControl" />
    /// <seealso cref="IContentControl" />
    public partial class EmptyViewControl : UserControl, IContentControl
    {
        #region PRIVATE PROPERTIES
        #endregion // PRIVATE PROPERTIES

        //// ---------------------------------------------------------------------

        #region PUBLIC PROPERTIES
        #endregion // PUBLIC PROPERTIES

        //// ---------------------------------------------------------------------

        #region CONSTRUCTION            
        /// <summary>
        /// Initializes a new instance of the <see cref="EmptyViewControl"/> class.
        /// </summary>
        public EmptyViewControl()
        {
            this.InitializeComponent();
        } // EmptyViewControl()
        #endregion // CONSTRUCTION

        //// ---------------------------------------------------------------------

        #region PUBLIC METHODS
        #endregion // PUBLIC METHODS

        //// ---------------------------------------------------------------------

        #region ICONTENTCONTROL IMPLEMENTATION
        /// <summary>
        /// Gets a value indicating whether the contents of this view
        /// has been changed by the user.
        /// </summary>
        public bool IsDirty { get; }

        /// <summary>
        /// Gets or sets a value indicating whether a refresh of the view is needed.
        /// </summary>
        public bool ForceRefresh { get; set; }

        /// <summary>
        /// Refreshes the display.
        /// </summary>
        public void RefreshDisplay()
        {
            // nothing to do
        } // RefreshDisplay()
        #endregion // ICONTENTCONTROL IMPLEMENTATION

        //// ---------------------------------------------------------------------

        #region PRIVATE METHODS
        #endregion // PRIVATE METHODS
    } // EmptyViewControl()
}
