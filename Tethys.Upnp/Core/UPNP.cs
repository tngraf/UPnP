// ---------------------------------------------------------------------------
// <copyright file="UPNP.cs" company="Tethys">
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
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Text;
    using System.Threading.Tasks;
    using System.Xml;
    using System.Xml.Linq;
    using Xml;

    /// <summary>
    /// <c>UPnP</c> support.
    /// </summary>
    public class UPNP
    {
        #region PUBLIC PROPERTIES
        /// <summary>
        /// The support <c>UPnP</c> device schema version major.
        /// </summary>
        public const string SupportSpecVersionMajor = "1";

        /// <summary>
        /// The support <c>UPnP</c> device schema version minor.
        /// </summary>
        public const string SupportSpecVersionMinor = "0";
        #endregion // PUBLIC PROPERTIES

        //// ---------------------------------------------------------------------

        #region PUBLIC METHODS            
        /// <summary>
        /// Parses the device schema.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns>A <see cref="DeviceSchema"/> object.</returns>
        public static DeviceSchema ParseDeviceSchema(string text)
        {
            var document = XDocument.Parse(text);
            var nodes = document.Elements().Where(e => e.Name.LocalName == "root");
            var xroot = nodes.FirstOrDefault();
            if (xroot == null)
            {
                throw new XmlException("No node 'root' found!");
            } // if

            CheckSupportedSpecVersion(xroot, 1, 0);
            var xdevice = XmlSupport.GetFirstSubNode(xroot, "device");
            var schema = ParseDeviceSchema(xdevice);

            return schema;
        } // ParseDeviceSchema()

        /// <summary>
        /// Parses the service schema.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns>A <see cref="UpnpService"/> object.</returns>
        public static UpnpService ParseServiceSchema(string text)
        {
            var service = new UpnpService();

            if (string.IsNullOrEmpty(text))
            {
                return service;
            } // if

            var document = XDocument.Parse(text);
            var nodes = document.Elements().Where(e => e.Name.LocalName == "scpd");
            var xroot = nodes.FirstOrDefault();
            if (xroot == null)
            {
                throw new XmlException("No node 'scpd' found!");
            } // if

            CheckSupportedSpecVersion(xroot, 1, 0);
            var xactionList = XmlSupport.GetFirstSubNode(xroot, "actionList", false);
            if (xactionList != null)
            {
                var actionList =
                    from xaction in xactionList.Elements()
                    where (xaction.Name.LocalName == "action")
                    select xaction;
                foreach (var xaction in actionList)
                {
                    var action = ParseAction(xaction);
                    action.Service = service;
                    service.AddAction(action);
                } // foreach
            } // if

            var xserviceStateTable = XmlSupport.GetFirstSubNode(xroot, "serviceStateTable");
            if (xserviceStateTable != null)
            {
                var serviceStateTable =
                    from xvariable in xserviceStateTable.Elements()
                    where (xvariable.Name.LocalName == "stateVariable")
                    select xvariable;
                foreach (var xvariable in serviceStateTable)
                {
                    var variable = ParseStateVariable(xvariable);
                    service.AddStateVariable(variable);
                } // foreach
            } // if

            return service;
        } // ParseServiceSchema()

        /// <summary>
        /// Builds a URL from the given information.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <param name="proposedUrl">The proposed URL.</param>
        /// <returns>A URL.</returns>
        public static string BuildUrl(UpnpDevice device, string proposedUrl)
        {
            if (Uri.IsWellFormedUriString(proposedUrl, UriKind.Absolute))
            {
                return proposedUrl;
            } // if

            var uriDevice = new Uri(device.Location);
            var host = uriDevice.Host;

            var sb = new StringBuilder(200);
            sb.Append("http://");
            sb.Append(host);
            if (uriDevice.Port > 0)
            {
                sb.Append($":{uriDevice.Port}");
            } // if

            if ((proposedUrl.Length > 0) && (proposedUrl[0] != '/'))
            {
                sb.Append('/');
            } // if

            sb.Append(proposedUrl);
            return sb.ToString();
        } // BuildUrl()

        /// <summary>
        /// Gets a text file from the given address via HTTP.
        /// </summary>
        /// <param name="address">The address.</param>
        /// <returns>The requested file as text string.</returns>
        public static async Task<string> GetRemoteTextFile(string address)
        {
            var client = new WebClient();
            return await client.DownloadStringTaskAsync(address);
        } // GetRemoteTextFile()

        /// <summary>
        /// Gets a text file from the given address via HTTP.
        /// </summary>
        /// <param name="address">The address.</param>
        /// <returns>An <see cref="Image"/> object.</returns>
        public static async Task<Image> GetRemoteImageFile(string address)
        {
            var client = new WebClient();
            var imageData = await client.DownloadDataTaskAsync(address);

            Bitmap bitmap;
            using (var ms = new MemoryStream(imageData))
            {
                bitmap = new Bitmap(ms);
            } // using

            return bitmap;
        } // GetRemoteImageFile()

        /// <summary>
        /// Parses a <c>UPnP</c> boolean value.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns>A <see cref="bool"/> value.</returns>
        public static bool ParseUpnpBoolean(string text)
        {
            text = text.ToUpperInvariant();

            if (string.IsNullOrEmpty(text))
            {
                return false;
            } // if

            if ((text == "1") || (text == "TRUE") || (text == "YES"))
            {
                return true;
            } // if

            if ((text == "0") || (text == "FALSE") || (text == "NO"))
            {
                return true;
            } // if

            throw new ArgumentOutOfRangeException(nameof(text), "no vlaid boolean value");
        } // ParseUpnpBoolean
        #endregion // PUBLIC METHODS

        //// ---------------------------------------------------------------------

        #region PRIVATE METHODS
        /// <summary>
        /// Parses the state variable.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns>A <see cref="UpnpStateVariable"/> object.</returns>
        private static UpnpStateVariable ParseStateVariable(XContainer element)
        {
            var variable = new UpnpStateVariable();
            variable.Name = XmlSupport.GetFirstSubNodeValue(element, "name");
            variable.Type = XmlSupport.GetFirstSubNodeValue(element, "dataType");
            variable.DefaultValue = XmlSupport.GetFirstSubNodeValue(element, "defaultValue", false);
            var xallowedValueList = XmlSupport.GetFirstSubNode(element, "allowedValueList", false);
            if (xallowedValueList != null)
            {
                var allowedValueList =
                    from xallowedValue in xallowedValueList.Elements()
                    where (xallowedValue.Name.LocalName == "allowedValue")
                    select xallowedValue;
                foreach (var xargument in allowedValueList)
                {
                    variable.AddAllowedValue(xargument.Value);
                } // foreach
            } // if

            return variable;
        } // ParseStateVariable()

        /// <summary>
        /// Parses the action.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns>A <see cref="UpnpServiceAction"/> object.</returns>
        private static UpnpServiceAction ParseAction(XContainer element)
        {
            var action = new UpnpServiceAction();
            action.Name = XmlSupport.GetFirstSubNodeValue(element, "name");

            var xargumentList = XmlSupport.GetFirstSubNode(element, "argumentList", false);
            if (xargumentList != null)
            {
                var argumentList =
                    from xargument in xargumentList.Elements()
                    where (xargument.Name.LocalName == "argument")
                    select xargument;
                foreach (var xargument in argumentList)
                {
                    var argument = ParseArgument(xargument);
                    action.AddArgument(argument);
                } // foreach
            } // if

            return action;
        } // ParseAction()

        /// <summary>
        /// Parses the argument.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns>A <see cref="UpnpArgument"/> object.</returns>
        private static UpnpArgument ParseArgument(XContainer element)
        {
            var argument = new UpnpArgument();
            argument.Name = XmlSupport.GetFirstSubNodeValue(element, "name");
            argument.Direction = XmlSupport.GetFirstSubNodeValue(element, "direction");
            argument.RelatedStateVariable =
                XmlSupport.GetFirstSubNodeValue(element, "relatedStateVariable");
            argument.ReturnValue =
                XmlSupport.GetFirstSubNodeValue(element, "retVal", false);
            return argument;
        } // ParseArgument()

        /// <summary>
        /// Parses the device schema.
        /// </summary>
        /// <param name="xdevice">The parent element.</param>
        /// <returns>A <see cref="DeviceSchema"/> object.</returns>
        private static DeviceSchema ParseDeviceSchema(XContainer xdevice)
        {
            var schema = new DeviceSchema();
            schema.DeviceType = XmlSupport.GetFirstSubNodeValue(xdevice, "deviceType");
            schema.FriendlyName = XmlSupport.GetFirstSubNodeValue(xdevice, "friendlyName");
            schema.Manufacturer = XmlSupport.GetFirstSubNodeValue(xdevice, "manufacturer");
            schema.ManufacturerUrl = XmlSupport.GetFirstSubNodeValue(xdevice, "manufacturerURL", false);
            schema.ModelDescription = XmlSupport.GetFirstSubNodeValue(xdevice, "modelDescription", false);
            schema.ModelName = XmlSupport.GetFirstSubNodeValue(xdevice, "modelName");
            schema.ModelNumber = XmlSupport.GetFirstSubNodeValue(xdevice, "modelNumber", false);
            schema.ModelUrl = XmlSupport.GetFirstSubNodeValue(xdevice, "modelURL", false);
            schema.SerialNumber = XmlSupport.GetFirstSubNodeValue(xdevice, "serialNumber", false);
            schema.UDN = XmlSupport.GetFirstSubNodeValue(xdevice, "UDN");

            var xiconList = XmlSupport.GetFirstSubNode(xdevice, "iconList", false);
            if (xiconList != null)
            {
                var iconList =
                    from xicon in xiconList.Elements()
                    where (xicon.Name.LocalName == "icon")
                    select xicon;
                foreach (var xicon in iconList)
                {
                    var icon = new DeviceIcon();
                    icon.MimeType = XmlSupport.GetFirstSubNodeValue(xicon, "mimetype");
                    icon.Width = int.Parse(XmlSupport.GetFirstSubNodeValue(xicon, "width"));
                    icon.Height = int.Parse(XmlSupport.GetFirstSubNodeValue(xicon, "height"));
                    icon.Depth = int.Parse(XmlSupport.GetFirstSubNodeValue(xicon, "depth"));
                    icon.URL = XmlSupport.GetFirstSubNodeValue(xicon, "url");
                    schema.AddIcon(icon);
                } // foreach
            } // if

            var xserviceList = XmlSupport.GetFirstSubNode(xdevice, "serviceList");
            var serviceList =
                from xservice in xserviceList.Elements()
                where (xservice.Name.LocalName == "service")
                select xservice;
            foreach (var xservice in serviceList)
            {
                var service = new UpnpService();
                service.Type = XmlSupport.GetFirstSubNodeValue(xservice, "serviceType");
                service.Id = XmlSupport.GetFirstSubNodeValue(xservice, "serviceId");
                service.ControlUrl = XmlSupport.GetFirstSubNodeValue(xservice, "controlURL");
                service.EventSubURL = XmlSupport.GetFirstSubNodeValue(xservice, "eventSubURL");
                service.ScpdUrl = XmlSupport.GetFirstSubNodeValue(xservice, "SCPDURL");
                schema.AddService(service);
            } // foreach

            var xdeviceList = XmlSupport.GetFirstSubNode(xdevice, "deviceList", false);
            if (xdeviceList != null)
            {
                var deviceList =
                    from xsubDevice in xdeviceList.Elements()
                    where (xsubDevice.Name.LocalName == "device")
                    select xsubDevice;
                foreach (var xsubDevice in deviceList)
                {
                    var device = ParseDeviceSchema(xsubDevice);
                    schema.AddSubDevice(device);
                } // foreach
            } // if

            schema.PresentationUrl = XmlSupport.GetFirstSubNodeValue(xdevice, "presentationURL", false);

            return schema;
        } // ParseDeviceSchema()

        /// <summary>
        /// Checks the supported spec version.
        /// </summary>
        /// <param name="parent">The parent.</param>
        /// <param name="majorSupported">The supported major version.</param>
        /// <param name="minorSupported">The supported minor version.</param>
        private static void CheckSupportedSpecVersion(XContainer parent, int majorSupported, int minorSupported)
        {
            var xversion = XmlSupport.GetFirstSubNode(parent, "specVersion");
            var major = int.Parse(XmlSupport.GetFirstSubNodeValue(xversion, "major"));
            var minor = int.Parse(XmlSupport.GetFirstSubNodeValue(xversion, "minor"));
            if (major > majorSupported)
            {
                throw new XmlException("Specversion not supported!");
            } // if

            if (minor > minorSupported)
            {
                throw new XmlException("Specversion not supported!");
            } // if
        } // CheckSupportedSpecVersion()
        #endregion // PRIVATE METHODS
    } // UPNP
}
