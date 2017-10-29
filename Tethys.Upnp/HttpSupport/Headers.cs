// ---------------------------------------------------------------------------
// <copyright file="Headers.cs" company="Tethys">
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
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;

    /// <summary>
    /// A dictionary for HTTP headers.
    /// </summary>
    /// <seealso cref="System.Collections.IEnumerable" />
    public class Headers : IEnumerable
    {
        #region PRIVATE PROPERTIES        
        /// <summary>
        /// Flag not to change the casing of the key.
        /// </summary>
        private readonly bool asIs;

        /// <summary>
        /// The dictionary.
        /// </summary>
        private readonly Dictionary<string, string> dict =
          new Dictionary<string, string>();

        /// <summary>
        /// The validator for normalizing.
        /// </summary>
        private static readonly Regex Validator = new Regex(
          @"^[a-z\d][a-z\d_.-]+$",
          RegexOptions.Compiled | RegexOptions.IgnoreCase);
        #endregion // PRIVATE PROPERTIES

        //// ---------------------------------------------------------------------

        #region PUBLIC PROPERTIES            
        /// <summary>
        /// Gets the header count.
        /// </summary>
        public int Count
        {
            get
            {
                return this.dict.Count;
            }
        }

        /// <summary>
        /// Gets the headers as text block.
        /// </summary>
        public string HeaderBlock
        {
            get
            {
                var hb = new StringBuilder();
                foreach (var h in this)
                {
                    hb.AppendFormat("{0}: {1}\r\n", h.Key, h.Value);
                } // foreach

                return hb.ToString();
            }
        }

        /// <summary>
        /// Gets the headers as stream.
        /// </summary>
        public Stream HeaderStream
        {
            get
            {
                return new MemoryStream(Encoding.ASCII.GetBytes(this.HeaderBlock));
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is read only.
        /// </summary>
        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Gets the keys.
        /// </summary>
        public ICollection<string> Keys
        {
            get
            {
                return this.dict.Keys;
            }
        }

        /// <summary>
        /// Gets the values.
        /// </summary>
        public ICollection<string> Values
        {
            get
            {
                return this.dict.Values;
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="string"/> with the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>The assigned value.</returns>
        public string this[string key]
        {
            get
            {
                return this.dict[this.Normalize(key)];
            }

            set
            {
                this.dict[this.Normalize(key)] = value;
            }
        }
        #endregion // PUBLIC PROPERTIES

        //// ---------------------------------------------------------------------

        #region CONSTRUCTION
        /// <summary>
        /// Initializes a new instance of the <see cref="Headers"/> class.
        /// </summary>
        public Headers()
            : this(false)
        {
        } // Headers()

        /// <summary>
        /// Initializes a new instance of the <see cref="Headers"/> class.
        /// </summary>
        /// <param name="asIs">if set to <c>true</c> [as is].</param>
        protected Headers(bool asIs)
        {
            this.asIs = asIs;
        } // Headers()
        #endregion // CONSTRUCTION

        //// ---------------------------------------------------------------------

        #region PUBLIC METHODS
        /// <summary>
        /// Adds the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        public void Add(KeyValuePair<string, string> item)
        {
            this.Add(item.Key, item.Value);
        } // Add()

        /// <summary>
        /// Adds the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public void Add(string key, string value)
        {
            this.dict.Add(this.Normalize(key), value);
        } // Add()

        /// <summary>
        /// Clears this instance.
        /// </summary>
        public void Clear()
        {
            this.dict.Clear();
        } // Clear()

        /// <summary>
        /// Determines whether the specified key value pair exists.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>
        ///   <c>true</c> if the specified item exists; otherwise, <c>false</c>.
        /// </returns>
        public bool Contains(KeyValuePair<string, string> item)
        {
            var p = new KeyValuePair<string, string>(
                this.Normalize(item.Key), item.Value);
            return this.dict.Contains(p);
        } // Contains()

        /// <summary>
        /// Determines whether the specified key exists.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>
        ///   <c>true</c> if the specified key contains key; otherwise, <c>false</c>.
        /// </returns>
        public bool ContainsKey(string key)
        {
            return this.dict.ContainsKey(this.Normalize(key));
        } // ContainsKey()

        /// <summary>
        /// NOT IMPLEMENTED!
        /// </summary>
        /// <param name="array">The array.</param>
        /// <param name="arrayIndex">Index of the array.</param>
        public void CopyTo(KeyValuePair<string, string>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        } // CopyTo()

        /// <summary>
        /// Gets the enumerator.
        /// </summary>
        /// <returns>An <see cref="IEnumerator"/>.</returns>
        public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
        {
            return this.dict.GetEnumerator();
        } // GetEnumerator()

        /// <summary>
        /// Removes the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns><c>true</c> if the key/value pair has been successfully
        /// removed; otherwise <c>false</c>.</returns>
        public bool Remove(string key)
        {
            return this.dict.Remove(this.Normalize(key));
        } // Remove()

        /// <summary>
        /// Removes the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns><c>true</c> if the key/value pair has been successfully
        /// removed; otherwise <c>false</c>.</returns>
        public bool Remove(KeyValuePair<string, string> item)
        {
            return this.Remove(item.Key);
        } // Remove()

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return $"({string.Join(", ", (from x in this.dict select $"{x.Key}={x.Value}"))})";
        } // ToString()

        /// <summary>
        /// Tries to get a specific value.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns><c>true</c> if the key exists; otherwise <c>false</c>.</returns>
        public bool TryGetValue(string key, out string value)
        {
            return this.dict.TryGetValue(this.Normalize(key), out value);
        } // TryGetValue()

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator" /> object that can be
        /// used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.dict.GetEnumerator();
        } // GetEnumerator()
        #endregion // PUBLIC METHODS

        //// ---------------------------------------------------------------------

        #region PRIVATE METHODS            
        /// <summary>
        /// Normalizes the specified header.
        /// </summary>
        /// <param name="header">The header.</param>
        /// <returns>The normalized header (key).</returns>
        /// <exception cref="System.ArgumentException">Invalid header: " + header</exception>
        private string Normalize(string header)
        {
            if (!this.asIs)
            {
                header = header.ToUpperInvariant();
            } // if

            header = header.Trim();
            if (!Validator.IsMatch(header))
            {
                throw new ArgumentException("Invalid header: " + header);
            } // if

            return header;
        } // Normalize()
        #endregion // PRIVATE METHODS
    } // Headers
}
