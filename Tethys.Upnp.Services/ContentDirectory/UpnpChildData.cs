// ---------------------------------------------------------------------------
// <copyright file="UpnpChildData.cs" company="Tethys">
//   Copyright (C) 2017 T. Graf
// </copyright>
//
// Licensed under the Apache License, Version 2.0.
// Unless required by applicable law or agreed to in writing, 
// software distributed under the License is distributed on an
// "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND,
// either express or implied. 
// ---------------------------------------------------------------------------

namespace Tethys.Upnp.Services.ContentDirectory
{
    using System;

    /// <summary>
    /// Sub element of <see cref="BrowseChildDataResult"/>.
    /// Encapsulates information about a specific child item.
    /// </summary>
    public class UpnpChildData
    {
        #region PUBLIC PROPERTIES
        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the parent identifier.
        /// </summary>
        public string ParentId { get; set; }

        /// <summary>
        /// Gets or sets the class.
        /// </summary>
        public string Class { get; set; }

        /// <summary>
        /// Gets a value indicating whether this item is a container.
        /// </summary>
        public bool IsContainer
        {
            get
            {
                return this.Class.StartsWith("object.container", StringComparison.OrdinalIgnoreCase);
            }
        }
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
            return $"{this.Title}, {this.Id}, {this.ParentId}, {this.Class}";
        } // ToString()
        #endregion // PUBLIC METHODS
    } // UpnpChildData
}
