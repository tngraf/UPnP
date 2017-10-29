// ---------------------------------------------------------------------------
// <copyright file="BrowseResult.cs" company="Tethys">
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
    /// <summary>
    /// Abstract base class for browse results returned by
    /// <see cref="ContentDirectoryService.BrowseMetaData"/> or
    /// <see cref="ContentDirectoryService.BrowseDirectChildren"/>.
    /// </summary>
    public abstract class BrowseResult
    {
        #region PUBLIC PROPERTIES        
        /// <summary>
        /// Gets or sets the number of items returned.
        /// </summary>
        public int NumberReturned { get; set; }

        /// <summary>
        /// Gets or sets the number of total matches.
        /// </summary>
        public int TotalMatches { get; set; }

        /// <summary>
        /// Gets or sets the update identifier.
        /// </summary>
        public int UpdateId { get; set; }
        #endregion // PUBLIC PROPERTIES

        //// ---------------------------------------------------------------------

        #region CONSTRUCTION
        #endregion // CONSTRUCTION

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
            return $"#{this.NumberReturned} returned, {this.TotalMatches} matches, update id={this.UpdateId}";
        } // ToString()
        #endregion // PUBLIC METHODS
    }
}
