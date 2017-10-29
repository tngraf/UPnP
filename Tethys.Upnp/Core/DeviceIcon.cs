// ---------------------------------------------------------------------------
// <copyright file="DeviceIcon.cs" company="Tethys">
//   Copyright (C) 2017 T. Graf
// </copyright>
//
// Licensed under the Apache License, Version 2.0.
// Unless required by applicable law or agreed to in writing, 
// software distributed under the License is distributed on an
// "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND,
// either express or implied. 
// ---------------------------------------------------------------------------

namespace Tethys.Upnp.Core
{
    /// <summary>
    /// Represents a <c>UPnP</c> device icon.
    /// </summary>
    public class DeviceIcon
    {
        #region PUBLIC PROPERTIES            
        /// <summary>
        /// Gets or sets the MIME type.
        /// </summary>
        /// <example>
        /// <c>image/png</c>
        /// </example>
        public string MimeType { get; set; }

        /// <summary>
        /// Gets or sets the width.
        /// </summary>
        /// <example><c>48</c></example>
        public int Width { get; set; }

        /// <summary>
        /// Gets or sets the height.
        /// </summary>
        /// <example><c>48</c></example>
        public int Height { get; set; }

        /// <summary>
        /// Gets or sets the depth.
        /// </summary>
        /// <example><c>24</c></example>
        public int Depth { get; set; }

        /// <summary>
        /// Gets or sets the URL.
        /// </summary>
        /// <remarks>
        /// We can't be sure to get a real URL.
        /// </remarks>
        /// <example>
        /// <c>
        /// http://192.168.0.108:60020/rui/dmsicon/sm_icon.png
        /// icon_logo4wmc_48x48.png
        /// </c>
        /// </example>
        public string URL { get; set; }
        #endregion // PUBLIC PROPERTIES

        //// ---------------------------------------------------------------------

        #region PUBLIC METHODS            
        /// <summary>
        /// Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="string" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return $"{this.Width}x{this.Height}, {this.Depth}bpp, {this.URL}";
        } // ToString()
        #endregion // PUBLIC METHODS
    } // DeviceIcon
}
