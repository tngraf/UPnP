// ---------------------------------------------------------------------------
// <copyright file="PropertyForm.cs" company="Tethys">
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
    using System;
    using System.Collections.Generic;
    using System.Windows.Forms;

    /// <summary>
    /// A form to display properties.
    /// </summary>
    /// <seealso cref="System.Windows.Forms.Form" />
    public partial class PropertyForm : Form
    {
        #region PUBLIC PROPERTIES            

        /// <summary>
        /// Gets or sets a value indicating whether to automatic resize the columns.
        /// </summary>
        public bool AutoResize
        {
            get { return this.propertyControl.AutoResize; }
            set { this.propertyControl.AutoResize = value; }
        }
        #endregion // PUBLIC PROPERTIES

        //// ---------------------------------------------------------------------

        #region CONSTRUCTION            
        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyForm"/> class.
        /// </summary>
        public PropertyForm()
        {
            this.InitializeComponent();
        } // PropertyForm()
        #endregion // CONSTRUCTION

        //// ---------------------------------------------------------------------

        #region PUBLIC METHODS
        /// <summary>
        /// Clears this instance.
        /// </summary>
        public void Clear()
        {
            this.propertyControl.Clear();
        } // Clear()

        /// <summary>
        /// Automatically resizes the columns to their content width.
        /// </summary>
        public void AutoResizeColumns()
        {
            this.propertyControl.AutoResizeColumns();
        } // AutoResizeColumns()

        /// <summary>
        /// Adds the new property value pair.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="value">The value.</param>
        public void AddNewPropertyValuePair(string propertyName, string value)
        {
            this.propertyControl.AddNewPropertyValuePair(propertyName, value);
        } // AddNewPropertyValuePair()

        /// <summary>
        /// Adds the properties.
        /// </summary>
        /// <param name="dictionary">The dictionary.</param>
        public void AddProperties(Dictionary<string, string> dictionary)
        {
            this.propertyControl.AddProperties(dictionary);
        } // AddProperties()
        #endregion // PUBLIC METHODS

        //// ---------------------------------------------------------------------

        #region UI METHODS            
        /// <summary>
        /// Handles the Load event of the PropertyForm control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void PropertyFormLoad(object sender, EventArgs e)
        {
        } // PropertyFormLoad()
        #endregion // UI METHODS
    } // PropertyForm
}
