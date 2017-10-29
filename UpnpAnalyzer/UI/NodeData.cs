// ---------------------------------------------------------------------------
// <copyright file="NodeData.cs" company="Tethys">
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
    /// Additional data to be assigned to a tree node.
    /// </summary>
    internal class NodeData
    {
        /// <summary>
        /// Gets the type.
        /// </summary>
        public NodeType Type { get; }

        /// <summary>
        /// Gets the payload.
        /// </summary>
        public object PayLoad { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="NodeData"/> class.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="payLoad">The pay load.</param>
        public NodeData(NodeType type, object payLoad)
        {
            this.Type = type;
            this.PayLoad = payLoad;
        } // NodeData()
    } // NodeData
}
