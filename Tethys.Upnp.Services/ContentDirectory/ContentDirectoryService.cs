// ---------------------------------------------------------------------------
// <copyright file="ContentDirectoryService.cs" company="Tethys">
//   Copyright (C) 2017 T. Graf
// </copyright>
//
// Licensed under the Apache License, Version 2.0.
// Unless required by applicable law or agreed to in writing, 
// software distributed under the License is distributed on an
// "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND,
// either express or implied. 
// ---------------------------------------------------------------------------

namespace Tethys.Upnp.Services.ContentDirectory
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Xml.Linq;
    using Core;
    using Logging;
    using Services;
    using Xml;

    /// <summary>
    /// Implementation of the <c>UPnP</c> directory service.
    /// </summary>
    public class ContentDirectoryService
    {
        #region PRIVATE PROPERTIES
        /// <summary>
        /// The logger for this class.
        /// </summary>
        private static readonly ILog Log = LogManager.GetLogger(typeof(ContentDirectoryService));
        #endregion // PRIVATE PROPERTIES

        //// ---------------------------------------------------------------------

        #region PUBLIC PROPERTIES
        /// <summary>
        /// Gets the service.
        /// </summary>
        public UpnpService Service { get; }

        /// <summary>
        /// Gets or sets the status display interface.
        /// </summary>
        public IDisplayStatusText StatusDisplay { get; set; }
        #endregion // PUBLIC PROPERTIES

        //// ---------------------------------------------------------------------

        #region CONSTRUCTION
        /// <summary>
        /// Initializes a new instance of the <see cref="ContentDirectoryService" /> class.
        /// </summary>
        /// <param name="service">The service.</param>
        public ContentDirectoryService(UpnpService service)
        {
            this.Service = service;
        } // ContentDirectory2Service()
        #endregion // CONSTRUCTION

        //// ---------------------------------------------------------------------

        #region PUBLIC METHODS
        /// <summary>
        /// Gets the search capabilities.
        /// </summary>
        /// <returns>A string with the device search capabilities.</returns>
        public async Task<string> GetSearchCapabilities()
        {
            var action = this.Service.Actions.FirstOrDefault(x => x.Name == "GetSearchCapabilities");
            if (action == null)
            {
                Log.Error("Action 'SearchCapabilities' not found!");
                return string.Empty;
            } // if

            var result = await InvokeAction(action, new object[1]);
            if (!result.Success)
            {
                Log.Error("Error invoking action!");
                this.DisplayStatusText($"Error executing action: {result.ErrorMessage} (code {result.ErrorCode})",
                    Color.Red);
                return null;
            } // if

            if ((result.Output == null) || (result.Output.Length < 1))
            {
                Log.Error("Invalid number of return arguments!");
                return null;
            } // if

            this.DisplayStatusText("Action successfully executed.", Color.Black);
            return (string)result.Output[0];
        } // GetSearchCapabilities()

        /// <summary>
        /// Gets the sort capabilities.
        /// </summary>
        /// <returns>A string with the device sort capabilities.</returns>
        public async Task<string> GetSortCapabilities()
        {
            var action = this.Service.Actions.FirstOrDefault(x => x.Name == "GetSortCapabilities");
            if (action == null)
            {
                Log.Error("Action 'GetSortCapabilities' not found!");
                return string.Empty;
            } // if

            var result = await InvokeAction(action, new object[1]);
            if (!result.Success)
            {
                Log.Error("Error invoking action!");
                this.DisplayStatusText($"Error executing action: {result.ErrorMessage} (code {result.ErrorCode})",
                    Color.Red);
                return null;
            } // if

            if ((result.Output == null) || (result.Output.Length < 1))
            {
                Log.Error("Invalid number of return arguments!");
                return null;
            } // if

            this.DisplayStatusText("Action successfully executed.", Color.Black);
            return (string)result.Output[0];
        } // GetSortCapabilities()

        /// <summary>
        /// Gets the feature list.
        /// </summary>
        /// <returns>A list of features.</returns>
        public async Task<List<Feature>> GetFeatureList()
        {
            var action = this.Service.Actions.FirstOrDefault(x => x.Name == "GetFeatureList");
            if (action == null)
            {
                Log.Error("Action 'GetFeatureList' not found!");
                return null;
            } // if

            var result = await InvokeAction(action, new object[1]);
            if (!result.Success)
            {
                Log.Error("Error invoking action!");
                this.DisplayStatusText($"Error executing action: {result.ErrorMessage} (code {result.ErrorCode})",
                    Color.Red);
                return null;
            } // if

            if ((result.Output == null) || (result.Output.Length < 1))
            {
                Log.Error("Invalid number of return arguments!");
                return null;
            } // if

            this.DisplayStatusText("Action successfully executed.", Color.Black);
            return ParseFeatureList((string)result.Output[0]);
        } // GetFeatureList        

        /// <summary>
        /// Gets the system update ID.
        /// </summary>
        /// <returns>A string with the system update ID.</returns>
        public async Task<string> GetSystemUpdateId()
        {
            var action = this.Service.Actions.FirstOrDefault(x => x.Name == "GetSystemUpdateID");
            if (action == null)
            {
                Log.Error("Action 'GetSystemUpdateID' not found!");
                return string.Empty;
            } // if

            var result = await InvokeAction(action, new object[1]);
            if (!result.Success)
            {
                Log.Error("Error invoking action!");
                this.DisplayStatusText($"Error executing action: {result.ErrorMessage} (code {result.ErrorCode})",
                    Color.Red);
                return null;
            } // if

            if ((result.Output == null) || (result.Output.Length < 1))
            {
                Log.Error("Invalid number of return arguments!");
                return null;
            } // if

            this.DisplayStatusText("Action successfully executed.", Color.Black);
            return (string)result.Output[0];
        } // GetSystemUpdateId()

        /// <summary>
        /// Gets the service reset token.
        /// </summary>
        /// <returns>A string with the service reset token.</returns>
        public async Task<string> GetServiceResetToken()
        {
            var action = this.Service.Actions.FirstOrDefault(x => x.Name == "GetServiceResetToken");
            if (action == null)
            {
                Log.Error("Action 'GetServiceResetToken' not found!");
                return string.Empty;
            } // if

            var result = await InvokeAction(action, new object[1]);
            if (!result.Success)
            {
                Log.Error("Error invoking action!");
                this.DisplayStatusText($"Error executing action: {result.ErrorMessage} (code {result.ErrorCode})",
                    Color.Red);
                return null;
            } // if

            if ((result.Output == null) || (result.Output.Length < 1))
            {
                Log.Error("Invalid number of return arguments!");
                return null;
            } // if

            this.DisplayStatusText("Action successfully executed.", Color.Black);
            return (string)result.Output[0];
        } // GetServiceResetToken()

        /// <summary>
        /// Browses the meta data.
        /// </summary>
        /// <param name="objectId">The object identifier.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="requestCount">The request count.</param>
        /// <param name="sortCriteria">The sort criteria.</param>
        /// <returns>
        /// A <see cref="BrowseMetaDataResult" /> object.
        /// </returns>
        /// <remarks>
        /// Only with the filter "*" all relevant child properties are returned!
        /// </remarks>
        public async Task<BrowseMetaDataResult> BrowseMetaData(string objectId = "",
            string filter = "", int requestCount = 0,  string sortCriteria = "")
        {
            var action = this.Service.Actions.FirstOrDefault(x => x.Name == "Browse");
            if (action == null)
            {
                Log.Error("Action 'Browse' not found!");
                return null;
            } // if

            var input = new List<object>();
            input.Add(objectId); // ObjectID (0, AV_ALL or id like AV-0-268435456-0-0-268435723-332267400)
            input.Add("BrowseMetadata"); // BrowseFlag (BrowseDirectChildren or BrowseMetadata)
            input.Add(filter); // Filter "" or "*"
            input.Add("0"); // StartingIndex (always 0 for meatdata)
            input.Add(requestCount); // RequestedCount
            input.Add(sortCriteria); // SortCriteria

            var result = await InvokeAction(action, input);
            if (!result.Success)
            {
                Log.Error("Error invoking action!");
                this.DisplayStatusText($"Error executing action: {result.ErrorMessage} (code {result.ErrorCode})", 
                    Color.Red);
                return null;
            }  // if

            if ((result.Output == null) || (result.Output.Length < 4))
            {
                this.DisplayStatusText("Invalid number of return arguments!", Color.Red);
                Log.Error("Invalid number of return arguments!");
                return null;
            } // if

            var metadata = this.ParseDidlMetaData((string)result.Output[0]);
            metadata.NumberReturned = int.Parse((string)result.Output[1]);
            metadata.TotalMatches = int.Parse((string)result.Output[2]);
            metadata.UpdateId = int.Parse((string)result.Output[3]);

            this.DisplayStatusText("Action successfully executed.", Color.Black);
            return metadata;
        } // BrowseMetaData()

        /// <summary>
        /// Browses the direct children.
        /// </summary>
        /// <param name="objectId">The object identifier.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="startIndex">The start index.</param>
        /// <param name="requestCount">The request count.</param>
        /// <param name="sortCriteria">The sort criteria.</param>
        /// <returns>
        /// A <see cref="BrowseChildDataResult" /> object.
        /// </returns>
        public async Task<BrowseChildDataResult> BrowseDirectChildren(string objectId = "", 
            string filter = "", int startIndex = 0, int requestCount = 0, string sortCriteria = "")
        {
            var action = this.Service.Actions.FirstOrDefault(x => x.Name == "Browse");
            if (action == null)
            {
                Log.Error("Action 'Browse' not found!");
                return null;
            } // if

            var input = new List<object>();
            input.Add(objectId); // ObjectID (0, AV_ALL or id like AV-0-268435456-0-0-268435723-332267400)
            input.Add("BrowseDirectChildren");
            input.Add(filter); // Filter "" or "*"
            input.Add(startIndex);
            input.Add(requestCount);
            input.Add(sortCriteria);

            var result = await InvokeAction(action, input);
            if (!result.Success)
            {
                Log.Error("Error invoking action!");
                this.DisplayStatusText($"Error executing action: {result.ErrorMessage} (code {result.ErrorCode})",
                    Color.Red);
                return null;
            }  // if

            if ((result.Output == null) || (result.Output.Length < 4))
            {
                this.DisplayStatusText("Invalid number of return arguments!", Color.Red);
                Log.Error("Invalid number of return arguments!");
                return null;
            } // if

            var metadata = ParseDidlChildData((string)result.Output[0]);
            metadata.NumberReturned = int.Parse((string)result.Output[1]);
            metadata.TotalMatches = int.Parse((string)result.Output[2]);
            metadata.UpdateId = int.Parse((string)result.Output[3]);

            var total = metadata.TotalMatches;
            var count = metadata.NumberReturned;
            while (count < total)
            {
                var input2 = new List<object>();
                input2.Add(objectId);
                input2.Add("BrowseDirectChildren");
                input2.Add(filter); // Filter "" or "*"
                input2.Add(count);
                input2.Add(requestCount);
                input2.Add(sortCriteria);

                result = await InvokeAction(action, input2);
                if (!result.Success)
                {
                    Log.Error("Error invoking action!");
                    this.DisplayStatusText($"Error executing action: {result.ErrorMessage} (code {result.ErrorCode})",
                        Color.Red);
                    return metadata;
                }  // if

                if ((result.Output == null) || (result.Output.Length < 4))
                {
                    this.DisplayStatusText("Invalid number of return arguments!", Color.Red);
                    Log.Error("Invalid number of return arguments!");
                    return metadata;
                } // if

                var metadata2 = ParseDidlChildData((string)result.Output[0]);
                metadata2.NumberReturned = int.Parse((string)result.Output[1]);

                foreach (var child in metadata2.Children)
                {
                    metadata.AddChildItem(child);
                } // foreach

                count += metadata2.NumberReturned;
            } // while

            this.DisplayStatusText("Action successfully executed.", Color.Black);
            return metadata;
        } // BrowseDirectChildren()
        #endregion // PUBLIC METHODS

        //// ---------------------------------------------------------------------

        #region PRIVATE METHODS
        /// <summary>
        /// Display a status text.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="color">The color.</param>
        private void DisplayStatusText(string text, Color color)
        {
            this.StatusDisplay?.DisplayStatusText(text, color);
        } // DisplayStatusText()

        /// <summary>
        /// Parses a feature list.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns>A  list of <see cref="Feature"/> objects.</returns>
        private static List<Feature> ParseFeatureList(string text)
        {
            var result = new List<Feature>();

            var document = XDocument.Parse(text);
            var featureRoot = XmlSupport.GetFirstSubNode(document, "Features");
            var itemlist = featureRoot.Elements().Where(e => e.Name.LocalName == "Feature");
            foreach (var item in itemlist)
            {
                var feature = ParseFeature(item);
                result.Add(feature);
            } // foreach

            return result;
        } // ParseFeatureList()

        /// <summary>
        /// Parses a feature.
        /// </summary>
        /// <param name="parent">The parent.</param>
        /// <returns>A <see cref="Feature"/> object.</returns>
        private static Feature ParseFeature(XElement parent)
        {
            var feature = new Feature();
            feature.Name = XmlSupport.GetAttributeValue(parent, "name");
            feature.Version = XmlSupport.GetAttributeValue(parent, "version");
            var itemlist = parent.Elements().Where(e => e.Name.LocalName == "ObjectIDs");
            foreach (var item in itemlist)
            {
                feature.AddObjectId(item.Value);
            } // foreach

            return feature;
        } // ParseFeature()

        /// <summary>
        /// Parses the <c>DIDL</c> child data.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns>A <see cref="BrowseChildDataResult"/> object.</returns>
        private static BrowseChildDataResult ParseDidlChildData(string text)
        {
            var result = new BrowseChildDataResult();

            var document = XDocument.Parse(text);
            var didl = XmlSupport.GetFirstSubNode(document, "DIDL-Lite");

            // first lokk for containers
            var itemlist = didl.Elements().Where(e => e.Name.LocalName == "container");
            foreach (var item in itemlist)
            {
                result.AddChildItem(ParseChildData(item));
            } // foreach

            itemlist = didl.Elements().Where(e => e.Name.LocalName == "item");
            foreach (var item in itemlist)
            {
                result.AddChildItem(ParseChildData(item));
            } // foreach

            return result;
        } // ParseDidlChildData()

        /// <summary>
        /// Parses <c>UPnP</c> child data.
        /// </summary>
        /// <param name="parent">The parent.</param>
        /// <returns>A <see cref="UpnpChildData"/> object.</returns>
        private static UpnpChildData ParseChildData(XElement parent)
        {
            var result = new UpnpChildData();

            result.Id = XmlSupport.GetAttributeValue(parent, "id");
            result.ParentId = XmlSupport.GetAttributeValue(parent, "parentID");

            result.Title = XmlSupport.GetFirstSubNodeValue(parent, "title");
            ////result.WriteStatus = XmlSupport.XmlSupport.GetFirstSubNodeValue(xcontainer, "writeStatus", false);
            ////result.IsRecordable = XmlSupport.XmlSupport.ParseUpnpBoolean(XmlSupport.XmlSupport.GetFirstSubNodeValue(xcontainer, "recordable"));
            result.Class = XmlSupport.GetFirstSubNodeValue(parent, "class");

            return result;
        } // ParseChildData()

        /// <summary>
        /// Parses the DIDL meta data.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns>A <see cref="BrowseMetaDataResult"/> object.</returns>
        private BrowseMetaDataResult ParseDidlMetaData(string text)
        {
            var result = new BrowseMetaDataResult();

            var document = XDocument.Parse(text);
            var didl = XmlSupport.GetFirstSubNode(document, "DIDL-Lite");
            var container = XmlSupport.GetFirstSubNode(didl, "container", false);
            if (container != null)
            {
                return ParseDidlContainerMetaData(container);
            } // if

            var item = XmlSupport.GetFirstSubNode(didl, "item");
            if (item != null)
            {
                return ParseDidlItemMetaData(item);
            } // if

            return result;
        } // ParseDidlMetaData()

        /// <summary>
        /// Parses DIDL container meta data.
        /// </summary>
        /// <param name="xcontainer">The parent.</param>
        /// <returns>A <see cref="BrowseMetaDataResult"/> object.</returns>
        private static BrowseMetaDataResult ParseDidlContainerMetaData(XElement xcontainer)
        {
            /*
             * <DIDL-Lite xmlns="urn:schemas-upnp-org:metadata-1-0/DIDL-Lite/" 
             *   xmlns:dc="http://purl.org/dc/elements/1.1/" 
             *   xmlns:upnp="urn:schemas-upnp-org:metadata-1-0/upnp/" 
             *   xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" 
             *   xsi:schemaLocation="urn:schemas-upnp-org:metadata-1-0/DIDL-Lite/ http://www.upnp.org/schemas/av/didl-lite-v2-20060531.xsd urn:schemas-upnp-org:metadata-1-0/upnp/ http://www.upnp.org/schemas/av/upnp-v2-20060531.xsd">
             *   <container id="AV_ALL" parentID="AV" restricted="0">
             *     <dc:title>Alle</dc:title>
             *     <upnp:writeStatus>NOT_WRITABLE</upnp:writeStatus>
             *     <upnp:recordable>0</upnp:recordable>
             *     <upnp:class name="container">object.container</upnp:class>
             *   </container>
             * </DIDL-Lite>
             */

            var result = new BrowseMetaDataResult();

            result.Id = XmlSupport.GetAttributeValue(xcontainer, "id");
            result.ParentId = XmlSupport.GetAttributeValue(xcontainer, "parentID");

            result.Title = XmlSupport.GetFirstSubNodeValue(xcontainer, "title");
            result.WriteStatus = XmlSupport.GetFirstSubNodeValue(xcontainer, "writeStatus", false);
            ////result.IsRecordable = XmlSupport.XmlSupport.ParseUpnpBoolean(XmlSupport.XmlSupport.GetFirstSubNodeValue(xcontainer, "recordable"));
            result.Class = XmlSupport.GetFirstSubNodeValue(xcontainer, "class");

            return result;
        } // ParseDidlContainerMetaData

        /// <summary>
        /// Parses DIDL item meta data.
        /// </summary>
        /// <param name="xitem">The parent.</param>
        /// <returns>A <see cref="BrowseMetaDataResult"/> object.</returns>
        private static BrowseMetaDataResult ParseDidlItemMetaData(XElement xitem)
        {
            var result = new BrowseMetaDataResult();

            result.Id = XmlSupport.GetAttributeValue(xitem, "id");
            result.ParentId = XmlSupport.GetAttributeValue(xitem, "parentID");
            var help = XmlSupport.GetAttributeValue(xitem, "restricted");
            if (!string.IsNullOrEmpty(help))
            {
                result.Restricted = UPNP.ParseUpnpBoolean(help);
            } // if
            
            result.Title = XmlSupport.GetFirstSubNodeValue(xitem, "title");
            result.Class = XmlSupport.GetFirstSubNodeValue(xitem, "class");
            result.Date = XmlSupport.GetFirstSubNodeValue(xitem, "date", false);

#if false
            result.Title = XmlSupport.XmlSupport.GetFirstSubNodeValue(xitem, "channelName", false);
            result.Title = XmlSupport.XmlSupport.GetFirstSubNodeValue(xitem, "date", false); // iso8601
            result.Title = XmlSupport.XmlSupport.GetFirstSubNodeValue(xitem, "objectType", false);
            result.Title = XmlSupport.XmlSupport.GetFirstSubNodeValue(xitem, "groupID", false);
            result.Title = XmlSupport.XmlSupport.GetFirstSubNodeValue(xitem, "storageMedium", false);
#endif

            var res = XmlSupport.GetFirstSubNode(xitem, "res", false);
            if (res != null)
            {
                result.ProtocolInfo = XmlSupport.GetAttributeValue(res, "protocolInfo", false);
                result.Resolution = XmlSupport.GetAttributeValue(res, "resolution", false);
                help = XmlSupport.GetAttributeValue(res, "size", false);
                if (!string.IsNullOrEmpty(help))
                {
                    result.Size = long.Parse(help);
                } // if

                help = XmlSupport.GetAttributeValue(res, "bitrate", false);
                if (!string.IsNullOrEmpty(help))
                {
                    result.Bitrate = int.Parse(help);
                } // if

                result.Duration = XmlSupport.GetAttributeValue(xitem, "duration", false);
                help = XmlSupport.GetAttributeValue(res, "nrAudioChannels", false);
                if (!string.IsNullOrEmpty(help))
                {
                    result.NumAudioChannels = int.Parse(help);
                } // if

                help = XmlSupport.GetAttributeValue(res, "sampleFrequency", false);
                if (!string.IsNullOrEmpty(help))
                {
                    result.SampleFrequency = int.Parse(help);
                } // if

                result.Resource = res.Value;
            } // if

            return result;
        } // ParseDidlItemMetaData()

        /*
         * <item id="33$@155" parentID="33$466" restricted="1" xmlns="urn:schemas-upnp-org:metadata-1-0/DIDL-Lite/">
  <dc:title xmlns:dc="http://purl.org/dc/elements/1.1/">DTH-Unplugged</dc:title>
  <upnp:class xmlns:upnp="urn:schemas-upnp-org:metadata-1-0/upnp/">object.item.videoItem</upnp:class>
  <dc:date xmlns:dc="http://purl.org/dc/elements/1.1/">2015-02-08T09:54:25</dc:date>
  <res protocolInfo="http-get:*:video/avi:DLNA.ORG_OP=01;DLNA.ORG_FLAGS=01700000000000000000000000000000" resolution="512x384" size="698892352" bitrate="196265" duration="0:59:20.000" nrAudioChannels="2" sampleFrequency="44100">http://192.168.0.16:50002/v/NDLNA/155.avi</res>
</item>
         */

        /// <summary>
        /// Invokes an action.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="input">The input.</param>
        /// <returns>An <see cref="InvokeActionResult"/> object.</returns>
        private static async Task<InvokeActionResult> InvokeAction(UpnpServiceAction action, IEnumerable<object> input)
        {
            var soap = new SOAP();
            var result = new InvokeActionResult();

            try
            {
                var res = await soap.Invoke(action.Service.ControlUrl,
                    action.Service.Type, action, input.ToArray());
                result = res;
            }
            catch (Exception ex)
            {
                Log.Error("Error executing action", ex);
                result.Success = false;
            } // catch

            return result;
        } // InvokeAction()
#endregion // PRIVATE METHODS
    } // ContentDirectory2Service
}
