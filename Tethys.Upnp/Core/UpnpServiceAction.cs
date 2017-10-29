// ---------------------------------------------------------------------------
// <copyright file="UpnpServiceAction.cs" company="Tethys">
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
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Implements a <c>UPnP</c> (service) action.
    /// </summary>
    public class UpnpServiceAction
    {
        #region PRIVATE PROPERTIES        
        /// <summary>
        /// The arguments.
        /// </summary>
        private readonly List<UpnpArgument> arguments;
        #endregion // PRIVATE PROPERTIES

        //// ---------------------------------------------------------------------

        #region PUBLIC PROPERTIES
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets the arguments.
        /// </summary>
        public IReadOnlyList<UpnpArgument> Arguments
        {
            get { return this.arguments; }
        }

        /// <summary>
        /// Gets the input arguments.
        /// </summary>
        public IReadOnlyList<UpnpArgument> ArgumentsIn
        {
            get
            {
                return this.arguments.Where(
                    argument => argument.Direction == UpnpArgument.InDirection)
                    .ToList();
            }
        }

        /// <summary>
        /// Gets the output arguments.
        /// </summary>
        public IReadOnlyList<UpnpArgument> ArgumentsOut
        {
            get
            {
                return this.arguments.Where(
                        argument => argument.Direction == UpnpArgument.OutDirection)
                    .ToList();
            }
        }

        /// <summary>
        /// Gets or sets the service this action belongs to.
        /// </summary>
        public UpnpService Service { get; set; }
        #endregion // PUBLIC PROPERTIES

        //// ---------------------------------------------------------------------

        #region CONSTRUCTION            
        /// <summary>
        /// Initializes a new instance of the <see cref="UpnpServiceAction"/> class.
        /// </summary>
        public UpnpServiceAction()
        {
            this.arguments = new List<UpnpArgument>();
        } // UpnpServiceAction()
        #endregion // CONSTRUCTION

        //// ---------------------------------------------------------------------

        #region PUBLIC METHODS            
        /// <summary>
        /// Adds the argument.
        /// </summary>
        /// <param name="argument">The argument.</param>
        public void AddArgument(UpnpArgument argument)
        {
            this.arguments.Add(argument);
        } // AddArgument()

        /// <summary>
        /// Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="string" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            var sb = new StringBuilder(200);
            sb.Append(this.Name);
            sb.Append("(");
            for (var i = 0; i < this.arguments.Count; i++)
            {
                sb.Append($"[{this.arguments[i].Direction}] ");
                sb.Append(this.arguments[i].Name);
                if (i < this.arguments.Count - 1)
                {
                    sb.Append(",");
                } // if
            } // for

            sb.Append(")");

            return sb.ToString();
        } // ToString()
        #endregion // PUBLIC METHODS
    } // UpnpServiceAction
}
