// ---------------------------------------------------------------------------
// <copyright file="IContentControl.cs" company="Tethys">
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
    /// <summary>
    /// Interface for content controls.
    /// </summary>
    internal interface IContentControl
    {
        /// <summary>
        /// Gets a value indicating whether the contents of this view
        /// has been changed by the user.
        /// </summary>
        bool IsDirty { get; }

        /// <summary>
        /// Gets or sets a value indicating whether a refresh of the view is needed.
        /// </summary>
        bool ForceRefresh { get; set; }

        /// <summary>
        /// Refreshes the display.
        /// </summary>
        void RefreshDisplay();
    } // IContentControl
}
