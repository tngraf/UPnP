// ---------------------------------------------------------------------------
// <copyright file="RawHeaders.cs" company="Tethys">
//   Copyright (C) 2017 T. Graf
// </copyright>
//
// Licensed under the Apache License, Version 2.0.
// Unless required by applicable law or agreed to in writing, 
// software distributed under the License is distributed on an
// "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND,
// either express or implied. 
// 
// Based on code written by Nils Maier, see 
// https://github.com/nmaier/simpleDLNA
// Licensed under as BSD-2-Clause license.
// 
// ---------------------------------------------------------------------------

namespace Tethys.Upnp.HttpSupport
{
    /// <summary>
    /// Version of the <see cref="Headers"/> class, where the casing
    /// of the key is not checked/modified.
    /// </summary>
    /// <seealso cref="Tethys.Upnp.HttpSupport.Headers" />
    public class RawHeaders : Headers
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RawHeaders"/> class.
        /// </summary>
        public RawHeaders()
          : base(true)
        {
        } // RawHeaders()
    } // RawHeaders
}
