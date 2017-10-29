// ---------------------------------------------------------------------------
// <copyright file="UpnpStateVariable.cs" company="Tethys">
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
    using System.Collections.Generic;

    /// <summary>
    /// Implement a <c>UPnP</c> state variable.
    /// </summary>
    public class UpnpStateVariable
    {
        #region PRIVATE PROPERTIES
        /// <summary>
        /// The allowed value list.
        /// </summary>
        private readonly List<string> allowedValueList;
        #endregion // PRIVATE PROPERTIES

        //// ---------------------------------------------------------------------

        #region PUBLIC PROPERTIES
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the variable type.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the default value.
        /// </summary>
        public string DefaultValue { get; set; }

        /// <summary>
        /// Gets the list of allowed values.
        /// </summary>
        public IReadOnlyList<string> AllowedValueList
        {
            get { return this.allowedValueList; }
        }
        #endregion // PUBLIC PROPERTIES

        //// ---------------------------------------------------------------------

        #region CONSTRUCTION
        /// <summary>
        /// Initializes a new instance of the <see cref="UpnpStateVariable"/> class.
        /// </summary>
        public UpnpStateVariable()
        {
            this.allowedValueList = new List<string>();
        } // UpnpStateVariable()
        #endregion // CONSTRUCTION

        //// ---------------------------------------------------------------------

        #region PUBLIC METHODS            
        /// <summary>
        /// Adds an allowed value.
        /// </summary>
        /// <param name="value">The value.</param>
        public void AddAllowedValue(string value)
        {
            this.allowedValueList.Add(value);
        } // AddAllowedValue()

        /// <summary>
        /// Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="string" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return $"{this.Type} {this.Name}";
        } // ToString()
        #endregion // PUBLIC METHODS
    }
}
