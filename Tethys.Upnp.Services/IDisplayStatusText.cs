// ---------------------------------------------------------------------------
// <copyright file="IDisplayStatusText.cs" company="Tethys">
//   Copyright (C) 2017 T. Graf
// </copyright>
//
// Licensed under the Apache License, Version 2.0.
// Unless required by applicable law or agreed to in writing, 
// software distributed under the License is distributed on an
// "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND,
// either express or implied. 
// ---------------------------------------------------------------------------

namespace Tethys.Upnp.Services
{
    using System.Drawing;

    /// <summary>
    /// Interface for status text output.
    /// </summary>
    public interface IDisplayStatusText
    {
        /// <summary>
        /// Displays the status text.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="color">The color.</param>
        void DisplayStatusText(string text, Color color);
    } // IDisplayStatusText
}
