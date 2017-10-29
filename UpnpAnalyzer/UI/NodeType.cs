// ---------------------------------------------------------------------------
// <copyright file="NodeType.cs" company="Tethys">
//   Copyright (C) 2017 T. Graf
// </copyright>
//
// Licensed under the Apache License, Version 2.0.
// Unless required by applicable law or agreed to in writing, 
// software distributed under the License is distributed on an
// "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND,
// either express or implied. 
// ---------------------------------------------------------------------------

namespace UpnpAnalyzer.UI
{
    /// <summary>
    /// Tree node types.
    /// </summary>
    internal enum NodeType
    {
        /// <summary>
        /// The unknown node type.
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// The device node.
        /// </summary>
        Device = 1,

        /// <summary>
        /// The service node.
        /// </summary>
        Service = 2,

        /// <summary>
        /// The action node.
        /// </summary>
        Action = 3,

        /// <summary>
        /// The state variable node.
        /// </summary>
        StateVariable = 4,

        /// <summary>
        /// The home node.
        /// </summary>
        Home = 5,
    }
}
