// ---------------------------------------------------------------------------
// <copyright file="Feature.cs" company="Tethys">
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
    /// Implements a feature as returned by the directory service
    /// action <see cref="ContentDirectoryService.GetFeatureList"/>.
    /// </summary>
    public class Feature
    {
        #region PRIVATE PROPERTIES        
        /// <summary>
        /// The object ids.
        /// </summary>
        private readonly List<string> objectIds;
        #endregion // PRIVATE PROPERTIES

        //// ---------------------------------------------------------------------

        #region PUBLIC PROPERTIES
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// Gets the object ids.
        /// </summary>
        public IReadOnlyList<string> ObjectIds => this.objectIds;
        #endregion // PUBLIC PROPERTIES

        //// ---------------------------------------------------------------------

        #region CONSTRUCTION            
        /// <summary>
        /// Initializes a new instance of the <see cref="Feature"/> class.
        /// </summary>
        public Feature()
        {
            this.objectIds = new List<string>();
        } // Feature
        #endregion // CONSTRUCTION

        //// ---------------------------------------------------------------------

        #region PUBLIC METHODS            
        /// <summary>
        /// Adds the object identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        public void AddObjectId(string id)
        {
            this.objectIds.Add(id);
        } // AddObjectId()

        /// <summary>
        /// Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="string" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            var ids = "(none)";
            if (this.objectIds.Count > 0)
            {
                ids = this.objectIds[0];
            } // if

            return $"{this.Name}: {ids}";
        } // ToString()
        #endregion // PUBLIC METHODS
    } // Feature
}
