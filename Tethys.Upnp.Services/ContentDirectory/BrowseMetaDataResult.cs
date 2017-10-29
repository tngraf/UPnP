// ---------------------------------------------------------------------------
// <copyright file="BrowseMetaDataResult.cs" company="Tethys">
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
    /// Result value of the <c>UPnP</c> action implementation
    /// <see cref="ContentDirectoryService.BrowseMetaData"/>.
    /// This is the meta information about a specific item.
    /// </summary>
    /// <seealso cref="Tethys.Upnp.Services.ContentDirectory.BrowseResult" />
    public class BrowseMetaDataResult : BrowseResult
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

        // ===== optional properties =====

        /// <summary>
        /// Gets or sets a value indicating whether this item is restricted.
        /// </summary>
        public bool Restricted { get; set; }

        /// <summary>
        /// Gets or sets the date.
        /// </summary>
        public string Date { get; set; }

        /// <summary>
        /// Gets or sets the protocol information.
        /// </summary>
        public string ProtocolInfo { get; set; }

        /// <summary>
        /// Gets or sets the resource.
        /// </summary>
        public string Resource { get; set; }

        /// <summary>
        /// Gets or sets the resolution.
        /// </summary>
        public string Resolution { get; set; }

        /// <summary>
        /// Gets or sets the storage used.
        /// </summary>
        public int StorageUsed { get; set; }

        /// <summary>
        /// Gets or sets the duration.
        /// </summary>
        public string Duration { get; set; }

        /// <summary>
        /// Gets or sets the size.
        /// </summary>
        public long Size { get; set; }

        /// <summary>
        /// Gets or sets the bitrate.
        /// </summary>
        public int Bitrate { get; set; }

        /// <summary>
        /// Gets or sets the sample frequency.
        /// </summary>
        public int SampleFrequency { get; set; }

        /// <summary>
        /// Gets or sets the number of audio channels.
        /// </summary>
        public int NumAudioChannels { get; set; }

        /// <summary>
        /// Gets or sets the write status.
        /// </summary>
        public string WriteStatus { get; set; }

        /// <summary>
        /// Gets or sets the search class.
        /// </summary>
        public string SearchClass { get; set; }
        #endregion // PUBLIC PROPERTIES

        //// ---------------------------------------------------------------------

        #region CONSTRUCTION            
        /// <summary>
        /// Initializes a new instance of the <see cref="BrowseMetaDataResult"/> class.
        /// </summary>
        public BrowseMetaDataResult()
        {
            this.StorageUsed = 0;
        } // BrowseMetaDataResult()
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
            return $"{this.Title}, {this.Class}, {this.Id}, parent={this.ParentId}";
        } // ToString()
        #endregion // PUBLIC METHODS
    } // BrowseMetaDataResult
}
