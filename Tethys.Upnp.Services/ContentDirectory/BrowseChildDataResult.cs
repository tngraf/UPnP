// ---------------------------------------------------------------------------
// <copyright file="BrowseChildDataResult.cs" company="Tethys">
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
    using System.Collections.Generic;

    /// <summary>
    /// Result value of the <c>UPnP</c> action implementation
    /// <see cref="ContentDirectoryService.BrowseDirectChildren"/>.
    /// </summary>
    /// <seealso cref="Tethys.Upnp.Services.ContentDirectory.BrowseResult" />
    public class BrowseChildDataResult : BrowseResult
    {
        #region PRIVATE PROPERTIES        
        /// <summary>
        /// The children.
        /// </summary>
        private readonly List<UpnpChildData> children;
        #endregion // PRIVATE PROPERTIES

        //// ---------------------------------------------------------------------

        #region PUBLIC PROPERTIES            
        /// <summary>
        /// Gets the children.
        /// </summary>
        public IReadOnlyList<UpnpChildData> Children => this.children;
        #endregion // PUBLIC PROPERTIES

        //// ---------------------------------------------------------------------

        #region CONSTRUCTION
        /// <summary>
        /// Initializes a new instance of the <see cref="BrowseChildDataResult"/> class.
        /// </summary>
        public BrowseChildDataResult()
        {
            this.children = new List<UpnpChildData>();
        } // BrowseChildDataResult()
        #endregion // CONSTRUCTION

        //// ---------------------------------------------------------------------

        #region PUBLIC METHODS            
        /// <summary>
        /// Adds the child item.
        /// </summary>
        /// <param name="item">The item.</param>
        public void AddChildItem(UpnpChildData item)
        {
            this.children.Add(item);
        } // AddChildItem()

        /// <summary>
        /// Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="string" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return $"{this.children.Count} child items";
        } // ToString()
        #endregion // PUBLIC METHODS
    } // BrowseChildDataResult
}
