// ---------------------------------------------------------------------------
// <copyright file="UpnpService.cs" company="Tethys">
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
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// The <c>UPnP</c> service description.
    /// </summary>
    public class UpnpService
    {
        #region PRIVATE PROPERTIES        
        /// <summary>
        /// The actions.
        /// </summary>
        private readonly List<UpnpServiceAction> actions;

        /// <summary>
        /// The state variables.
        /// </summary>
        private readonly List<UpnpStateVariable> stateVariables;
        #endregion // PRIVATE PROPERTIES

        //// ---------------------------------------------------------------------

        #region PUBLIC PROPERTIES            
        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <example>
        /// <c>urn:schemas-upnp-org:service:ContentDirectory:2</c>
        /// </example>
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <example>
        /// <c>urn:upnp-org:serviceId:ContentDirectory</c>
        /// </example>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the SCPD URL.
        /// </summary>
        /// <remarks>
        /// We can't be sure to get a real URL.
        /// </remarks>
        /// <example>
        /// <c>http://192.168.0.108:60606/Server0/CDS_SCPD</c>
        /// </example>
        public string ScpdUrl { get; set; }

        /// <summary>
        /// Gets or sets the control URL.
        /// </summary>
        /// <remarks>
        /// We can't be sure to get a real URL.
        /// </remarks>
        /// <example>
        /// <c>
        /// http://192.168.0.108:60606/Server0/CDS_control
        /// /ctl/L3F
        /// /dummy
        /// </c>
        /// </example>
        public string ControlUrl { get; set; }

        /// <summary>
        /// Gets or sets the event sub URL.
        /// </summary>
        /// <remarks>
        /// We can't be sure to get a real URL.
        /// </remarks>
        /// <example>
        /// <c>http://192.168.0.108:60606/Server0/CDS_event</c>
        /// </example>
        public string EventSubURL { get; set; }

        //// ===== Service Details from Service Coontrol Point Description =====

        /// <summary>
        /// Gets the actions.
        /// </summary>
        public IReadOnlyList<UpnpServiceAction> Actions
        {
            get { return this.actions; }
        }

        /// <summary>
        /// Gets the state variables
        /// </summary>
        public IReadOnlyList<UpnpStateVariable> StateVariables
        {
            get
            {
                return this.stateVariables;
            }
        }
        #endregion // PUBLIC PROPERTIES

        //// ---------------------------------------------------------------------

        #region CONSTRUCTION            
        /// <summary>
        /// Initializes a new instance of the <see cref="UpnpService"/> class.
        /// </summary>
        public UpnpService()
        {
            this.actions = new List<UpnpServiceAction>();
            this.stateVariables = new List<UpnpStateVariable>();
        } // UpnpService()
        #endregion // CONSTRUCTION

        //// ---------------------------------------------------------------------

        #region PUBLIC METHODS            
        /// <summary>
        /// Gets the information about the given variable.
        /// </summary>
        /// <param name="argument">The argument.</param>
        /// <returns>A <see cref="UpnpStateVariable"/> object.</returns>
        public UpnpStateVariable GetVariableInfo(UpnpArgument argument)
        {
            return this.stateVariables.FirstOrDefault(
                variable => argument.RelatedStateVariable.Equals(variable.Name, StringComparison.OrdinalIgnoreCase));
        } // GetVariableInfo()

        /// <summary>
        /// Adds the action.
        /// </summary>
        /// <param name="action">The action.</param>
        public void AddAction(UpnpServiceAction action)
        {
            this.actions.Add(action);
        } // AddAction()

        /// <summary>
        /// Adds the state variable.
        /// </summary>
        /// <param name="variable">The variable.</param>
        public void AddStateVariable(UpnpStateVariable variable)
        {
            this.stateVariables.Add(variable);
        } // AddStateVariable()

        /// <summary>
        /// Updates the properties on this service with the information
        /// of the given service.
        /// </summary>
        /// <param name="service">The service.</param>
        public void UpdateServiceInfo(UpnpService service)
        {
            foreach (var action in service.Actions)
            {
                this.actions.Add(action);
            } // foreach

            foreach (var variable in service.StateVariables)
            {
                this.stateVariables.Add(variable);
            } // foreach
        } // UpdateServiceInfo()

        /// <summary>
        /// Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="string" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return $"{this.Type}, {this.actions.Count} actions, {this.stateVariables.Count} state variables";
        } // ToString()
        #endregion // PUBLIC METHODS
    } // UpnpService
}
