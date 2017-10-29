// ---------------------------------------------------------------------------
// <copyright file="UpnpError.cs" company="Tethys">
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
    /// Implements a container for <c>UPnP</c> error information.
    /// </summary>
    public class UpnpError
    {
        #region PUBLIC PROPERTIES        
        /// <summary>
        /// Gets or sets the error code.
        /// </summary>
        public int ErrorCode { get; set; }

        /// <summary>
        /// Gets or sets the error description.
        /// </summary>
        public string ErrorDescription { get; set; }
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
            return $"{this.ErrorCode}: {this.ErrorDescription}";
        } // ToString()
        #endregion // PUBLIC METHODS
    } // UpnpError
}
