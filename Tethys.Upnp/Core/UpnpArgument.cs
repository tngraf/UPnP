// ---------------------------------------------------------------------------
// <copyright file="UpnpArgument.cs" company="Tethys">
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
    /// Implements a <c>UPnP</c> service argument
    /// </summary>
    public class UpnpArgument
    {
        #region PUBLIC PROPERTIES
        /// <summary>
        /// The out direction.
        /// </summary>
        public const string OutDirection = "out";

        /// <summary>
        /// The in direction.
        /// </summary>
        public const string InDirection = "in";

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the direction.
        /// </summary>
        public string Direction { get; set; }

        /// <summary>
        /// Gets or sets the related state variable.
        /// </summary>
        public string RelatedStateVariable { get; set; }

        /// <summary>
        /// Gets or sets the return value.
        /// </summary>
        public string ReturnValue { get; set; }
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
            return $"{this.Name}, {this.Direction}, {this.RelatedStateVariable}";
        } // ToString()
        #endregion // PUBLIC METHODS
    }
}
