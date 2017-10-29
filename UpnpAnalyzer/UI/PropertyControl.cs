// ---------------------------------------------------------------------------
// <copyright file="PropertyControl.cs" company="Tethys">
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
    /// Implements a control to show properties, or better
    /// name/value pairs.
    /// </summary>
    /// <seealso cref="System.Windows.Forms.UserControl" />
    public partial class PropertyControl : UserControl
    {
        #region PRIVATE PROPERTIES        
        /// <summary>
        /// The cached items.
        /// </summary>
        private readonly List<ListViewItem> chachedItems;
        #endregion // PRIVATE PROPERTIES

        //// ---------------------------------------------------------------------

        #region PUBLIC PROPERTIES            
        /// <summary>
        /// Gets or sets a value indicating whether to automatic resize the columns.
        /// </summary>
        public bool AutoResize { get; set; }
        #endregion // PUBLIC PROPERTIES

        //// ---------------------------------------------------------------------

        #region CONSTRUCTION            
        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyControl"/> class.
        /// </summary>
        public PropertyControl()
        {
            this.InitializeComponent();
            this.chachedItems = new List<ListViewItem>();
        } // PropertyControl()
        #endregion // CONSTRUCTION

        //// ---------------------------------------------------------------------

        #region PUBLIC METHODS
        /// <summary>
        /// Clears this instance.
        /// </summary>
        public void Clear()
        {
            this.listViewProperties.Items.Clear();
        } // Clear()

        /// <summary>
        /// Automatically resizes the columns to their content width.
        /// </summary>
        public void AutoResizeColumns()
        {
            this.listViewProperties.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
        } // AutoResizeColumns()

        /// <summary>
        /// Adds the new property value pair.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="value">The value.</param>
        public void AddNewPropertyValuePair(string propertyName, string value)
        {
            var lvi = new ListViewItem();
            lvi.Text = propertyName;
            lvi.SubItems.Add(value);

            if (this.IsHandleCreated)
            {
                this.listViewProperties.Items.Add(lvi);
            }
            else
            {
                this.chachedItems.Add(lvi);
            } // if
        } // AddNewPropertyValuePair()

        /// <summary>
        /// Adds the properties.
        /// </summary>
        /// <param name="dictionary">The dictionary.</param>
        public void AddProperties(Dictionary<string, string> dictionary)
        {
            foreach (var entry in dictionary)
            {
                this.AddNewPropertyValuePair(entry.Key, entry.Value);
            } // foreach
        } // AddProperties()
        #endregion // PUBLIC METHODS

        //// ---------------------------------------------------------------------

        #region UI METHODS            
        /// <summary>
        /// Handles the Load event of the PropertyControl control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void PropertyControlLoad(object sender, EventArgs e)
        {
            this.InitUI();
            this.ProcessChachedItems();
            if (this.AutoResize)
            {
                this.AutoResizeColumns();
            } // if
        } // PropertyControlLoad()
        #endregion // UI METHODS

        //// ---------------------------------------------------------------------

        #region PRIVATE METHODS            
        /// <summary>
        /// Processes the cached items.
        /// </summary>
        private void ProcessChachedItems()
        {
            foreach (var item in this.chachedItems)
            {
                this.listViewProperties.Items.Add(item);
            } // foreach

            this.chachedItems.Clear();
        } // ProcessChachedItems()

        /// <summary>
        /// Initializes the UI.
        /// </summary>
        private void InitUI()
        {
            this.listViewProperties.Clear();

            var col = new ColumnHeader();
            col.Text = "Name";
            this.listViewProperties.Columns.Add(col);

            col = new ColumnHeader();
            col.Text = "Value";
            this.listViewProperties.Columns.Add(col);
        } // InitUI()
        #endregion // PRIVATE METHODS
    } // PropertyControl
}
