// ---------------------------------------------------------------------------
// <copyright file="ActionInfoControl.cs" company="Tethys">
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
    using System.Drawing;
    using System.Windows.Forms;
    using Tethys.Logging;
    using Tethys.Upnp.Core;
    using Tethys.Upnp.Services;
    using Tethys.Upnp.Services.ContentDirectory;
    using UpnpAnalyzer;

    /// <summary>
    /// A control to work with <c>UPnP</c> service actions.
    /// </summary>
    /// <seealso cref="System.Windows.Forms.UserControl" />
    /// <seealso cref="IContentControl" />
    /// <seealso cref="Tethys.Upnp.Services.IDisplayStatusText" />
    public partial class ActionInfoControl : UserControl, IContentControl, IDisplayStatusText
    {
        #region PRIVATE PROPERTIES
        /// <summary>
        /// The logger for this class.
        /// </summary>
        private static readonly ILog Log = LogManager.GetLogger(typeof(ActionInfoControl));
        #endregion // PRIVATE PROPERTIES

        //// ---------------------------------------------------------------------

        #region PUBLIC PROPERTIES            
        /// <summary>
        /// Gets or sets the action.
        /// </summary>
        public UpnpServiceAction Action { get; set; }
        #endregion // PUBLIC PROPERTIES

        //// ---------------------------------------------------------------------

        #region CONSTRUCTION            
        /// <summary>
        /// Initializes a new instance of the <see cref="ActionInfoControl"/> class.
        /// </summary>
        public ActionInfoControl()
        {
            this.InitializeComponent();
        } // ActionInfoControl()
        #endregion // CONSTRUCTION

        //// ---------------------------------------------------------------------

        #region UI METHODS            
        /// <summary>
        /// Handles the Click event of the <c>btnInvokeAction</c> control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private async void BtnInvokeActionClick(object sender, EventArgs e)
        {
            this.rtfStatus.Clear();
#if true
            if (this.Action.Service.Type.Contains("ContentDirectory:2"))
            {
                var svc2 = new ContentDirectoryService(this.Action.Service);
                svc2.StatusDisplay = this;
                var x = await svc2.GetSearchCapabilities();
                ////var x2 = await svc2.BrowseMetaData("0");
                ////var x3 = await svc2.BrowseDirectChildren("0");
                ////var x3 = await svc2.BrowseDirectChildren("AV_ALL");
                var x4 = await svc2.GetFeatureList();
            }
#endif
            var soap = new SOAP();

            try
            {
                var input = new List<object>();
                for (var i = 0; i < this.Action.ArgumentsIn.Count; i++)
                {
                    var value = this.dataGridInputs.Rows[i].Cells["colValue"].Value;
                    var variableInfo = this.Action.Service.GetVariableInfo(this.Action.ArgumentsIn[i]);
                    if ((value == null) && (variableInfo.Type == "i4"))
                    {
                        value = "0";
                    } // if

                    input.Add(value);
                } // foreach

                var result = await soap.Invoke(this.Action.Service.ControlUrl,
                    this.Action.Service.Type, this.Action, input.ToArray());

                if (result?.Output != null)
                {
                    Log.Debug("Result:");
                    foreach (var obj in result.Output)
                    {
                        Log.Debug($"  {obj}");
                    } // foreach
                    
                    for (var i = 0; i < this.Action.ArgumentsOut.Count; i++)
                    {
                        if (result.Output.Length < i)
                        {
                            continue;
                        } // if

                        this.dataGridOutputs.Rows[i].Cells[2].Value = result.Output[i];
                    } // foreach

                    this.DisplayStatusText("Action successfully executed.");
                } // if

                if ((result != null) && (!result.Success))
                {
                    this.DisplayStatusText($"Error executing action: {result.ErrorMessage} (code {result.ErrorCode})",
                        Color.Red);
                } // if
            }
            catch (Exception ex)
            {
                Log.Error("Error executing action", ex);
                this.DisplayStatusText($"Error executing action: {ex.Message}", Color.Red);
            } // catch
        } // BtnInvokeActionClick()
        #endregion // UI METHODS

        //// ---------------------------------------------------------------------

        #region ICONTENTCONTROL IMPLEMENTATION
        /// <summary>
        /// Gets a value indicating whether the contents of this view
        /// has been changed by the user.
        /// </summary>
        public bool IsDirty { get; }

        /// <summary>
        /// Gets or sets a value indicating whether a refresh of the view is needed.
        /// </summary>
        public bool ForceRefresh { get; set; }

        /// <summary>
        /// Refreshes the display.
        /// </summary>
        public void RefreshDisplay()
        {
            this.DisplayActionInfo();
        } // RefreshDisplay()

        /// <summary>
        /// Displays the status text.
        /// </summary>
        /// <param name="text">The text.</param>
        public void DisplayStatusText(string text)
        {
            this.DisplayStatusText(text, Color.Black);
        } // DisplayStatusText()
        #endregion // ICONTENTCONTROL IMPLEMENTATION

        #region IDISPLAYSTATUSTEXT IMPLEMENTATION
        /// <summary>
        /// Displays the status text.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="color">The color.</param>
        public void DisplayStatusText(string text, Color color)
        {
            this.rtfStatus.SelectionStart = 2000000000;
            this.rtfStatus.SelectionColor = color;
            this.rtfStatus.SelectedText = text + "\r\n";

            this.rtfStatus.SelectionColor = Color.Black;
            this.rtfStatus.Select(this.rtfStatus.TextLength, 0);
            this.rtfStatus.ScrollToCaret();
        } // DisplayStatusText()
        #endregion // IDISPLAYSTATUSTEXT IMPLEMENTATION
        //// ---------------------------------------------------------------------

        #region PRIVATE METHODS
        /// <summary>
        /// Displays the action information.
        /// </summary>
        private void DisplayActionInfo()
        {
            if (this.Action == null)
            {
                return;
            } // if

            this.dataGridInputs.Rows.Clear();
            foreach (var argument in this.Action.ArgumentsIn)
            {
                var variableInfo = this.Action.Service.GetVariableInfo(argument);
                var allowed = Support.ListToCommaSeparatedList(variableInfo.AllowedValueList);
                if (string.IsNullOrEmpty(allowed))
                {
                    allowed = "(any)";
                } // if

                this.dataGridInputs.Rows.Add(argument.Name,
                    variableInfo.Type, variableInfo.DefaultValue, allowed);
            } // foreach

            this.dataGridOutputs.Rows.Clear();
            foreach (var argument in this.Action.ArgumentsOut)
            {
                var variableInfo = this.Action.Service.GetVariableInfo(argument);
                this.dataGridOutputs.Rows.Add(argument.Name,
                    variableInfo.Type, argument.ReturnValue);
            } // foreach
        } // DisplayActionInfo()
        #endregion // PRIVATE METHODS
    } // ActionInfoControl
}
