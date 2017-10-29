// ---------------------------------------------------------------------------
// <copyright file="InvokeActionResult.cs" company="Tethys">
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
    /// Container for results of a <c>UPnP</c> action.
    /// </summary>
    public class InvokeActionResult
    {
        #region PUBLIC PROPERTIES            
        /// <summary>
        /// Gets or sets a value indicating whether this action invocation was successful.
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Gets or sets the output value.
        /// </summary>
        /// <remarks>
        /// Output[0] may contain a SOAP fault code (if Success is false).
        /// Output[1] may contain a SOAP fault string (if Success is false).
        /// </remarks>
        public object[] Output { get; set; }

        /// <summary>
        /// Gets or sets the <c>UPnP</c> error code (if Success is false).
        /// </summary>
        public int ErrorCode { get; set; }

        /// <summary>
        /// Gets or sets the <c>UPnP</c> error message (if Success is false).
        /// </summary>
        public string ErrorMessage { get; set; }
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
            if (this.Success)
            {
                return "Success";
            } // if

            return $"Error: {this.ErrorCode}: {this.ErrorMessage}";
        } // ToString()
        #endregion // PUBLIC METHODS
    } // InvokeActionResult
}
