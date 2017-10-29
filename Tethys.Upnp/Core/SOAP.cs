// ---------------------------------------------------------------------------
// <copyright file="SOAP.cs" company="Tethys">
//   Copyright (C) 2017 T. Graf
// </copyright>
//
// Licensed under the Apache License, Version 2.0.
// Unless required by applicable law or agreed to in writing, 
// software distributed under the License is distributed on an
// "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND,
// either express or implied. 
// ---------------------------------------------------------------------------

#define ENHANCED_ERROR_OUTPUT

namespace Tethys.Upnp.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading.Tasks;
    using System.Xml;
    using System.Xml.Linq;

    using Logging;
    using Xml;

    /// <summary>
    /// SOAP support.
    /// </summary>
    public class SOAP
    {
        #region PRIVATE PROPERTIES
        /// <summary>
        /// The logger for this class.
        /// </summary>
        private static readonly ILog Log = LogManager.GetLogger(typeof(SOAP));
        #endregion // PRIVATE PROPERTIES

        //// ---------------------------------------------------------------------

        #region PUBLIC METHODS
        /// <summary>
        /// Invokes the specified URL.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="serviceType">Type of the service.</param>
        /// <param name="action">The action.</param>
        /// <param name="input">The input.</param>
        /// <returns>An <see cref="InvokeActionResult"/> object.</returns>
        public async Task<InvokeActionResult> Invoke(string url, string serviceType,
            UpnpServiceAction action, object[] input)
        {
            if (action.ArgumentsIn.Count > input.Length)
            {
                throw new ArgumentException("Not enough input arguments");
            } // if

            var actionText = $"\"{serviceType}#{action.Name}\"";
            var soapEnvelopeXml = CreateSoapEnvelope(action, input, serviceType);
            var soapString = "<?xml version=\"1.0\"?>\r\n" + soapEnvelopeXml.OuterXml;

            var result = new InvokeActionResult();
            var response = await PostXmlRequest(url, soapString, actionText);
            if (response.Content.Headers.Contains("Content-Type"))
            {
                response.Content.Headers.Remove("Content-Type");
            } // if

            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                result.Success = false;
                Log.Error(response.StatusCode);

                try
                {
                    var details = EvaluateResult(content, action);
                    var error = details[2] as UpnpError;
                    if (error != null)
                    {
                        result.ErrorCode = error.ErrorCode;
                        result.ErrorMessage = error.ErrorDescription;
                    } // if
                }
                catch
                {
                    // IGNORE
                } // catch

                return result;
            } // if

            Log.Debug(content);

            result.Success = true;
            result.Output = EvaluateResult(content, action);
            return result;
        } // Invoke()
        #endregion // PUBLIC METHODS

        //// ---------------------------------------------------------------------

        #region PRIVATE METHODS
        /// <summary>
        /// Evaluates the result.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="action">The action.</param>
        /// <returns>An array of objects.</returns>
        /// <exception cref="XmlException">No valid SOAP envelop!</exception>
        private static object[] EvaluateResult(string text, UpnpServiceAction action)
        {
            var output = new List<object>();

            var document = XDocument.Parse(text);
            var xroot = XmlSupport.GetFirstSubNode(document, "Envelope", false);
            if (xroot == null)
            {
                throw new XmlException("No valid SOAP envelop!");
            } // if

            var xbody = XmlSupport.GetFirstSubNode(xroot, "Body");

            var xfault = XmlSupport.GetFirstSubNode(xbody, "Fault", false);
            if (xfault != null)
            {
                return EvaluateFaultResult(xfault);
            } // if

            var xaction = XmlSupport.GetFirstSubNode(xbody, $"{action.Name}Response");
            var resultList =
                from xresult in xaction.Elements()
                select xresult;
            foreach (var xresult in resultList)
            {
                output.Add(xresult.Value);
            } // foreach

            return output.ToArray();
        } // EvaluateResult()

        /// <summary>
        /// Evaluates a fault result.
        /// </summary>
        /// <param name="xfault">The parent element.</param>
        /// <returns>An array of objects.</returns>
        private static object[] EvaluateFaultResult(XContainer xfault)
        {
            Log.Error("Fault answer for SOAP call:");
            var faultcode = XmlSupport.GetFirstSubNodeValue(xfault, "faultcode");
            var faultstring = XmlSupport.GetFirstSubNodeValue(xfault, "faultstring");
            var faultdetail = XmlSupport.GetFirstSubNode(xfault, "detail");

#if ENHANCED_ERROR_OUTPUT
            Log.Error($"Code = {faultcode}");
            Log.Error($"Text = {faultstring}");
            Log.Error($"Details = {faultdetail}");
#endif // ENHANCED_ERROR_OUTPUT

            var upnperror = ParseUpnpError(faultdetail);

#if ENHANCED_ERROR_OUTPUT
            Log.Error($"UPnP error: {upnperror}");
#endif // ENHANCED_ERROR_OUTPUT

            var result = new object[3];
            result[0] = faultcode;
            result[1] = faultstring;
            result[2] = upnperror;

            return result;
        } // EvaluateFaultResult()

        /// <summary>
        /// Parses a <c>UPnP</c> error.
        /// </summary>
        /// <param name="faultdetail">The fault detail.</param>
        /// <returns>
        /// A <see cref="UpnpError" /> object.
        /// </returns>
        private static UpnpError ParseUpnpError(XContainer faultdetail)
        {
            var error = new UpnpError();

            var xupnperror = XmlSupport.GetFirstSubNode(faultdetail, "UPnPError", false);
            if (xupnperror != null)
            {
                error.ErrorCode = int.Parse(XmlSupport.GetFirstSubNodeValue(xupnperror, "errorCode"));
                error.ErrorDescription = XmlSupport.GetFirstSubNodeValue(xupnperror, "errorDescription");
            } // if

            return error;
        } // ParseUpnpError()

        /// <summary>
        /// Posts the XML request.
        /// </summary>
        /// <param name="baseUrl">The base URL.</param>
        /// <param name="xmlString">The XML string.</param>
        /// <param name="action">The action.</param>
        /// <returns>A <see cref="HttpResponseMessage"/> object.</returns>
        private static async Task<HttpResponseMessage> PostXmlRequest(string baseUrl,
            string xmlString, string action)
        {
            using (var httpClient = new HttpClient())
            {
#if true
                httpClient.DefaultRequestHeaders.Add("User-Agent",
                    "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.2; WOW64; Trident / 6.0)");

                // "Microsoft-Windows/10.0 UPnP/1.0"
                // "Linux&2.4.22-1.2115.nptl UPnP/1.0"
#endif

                var request = new HttpRequestMessage(HttpMethod.Post, baseUrl);
                request.Headers.Add("SOAPAction", action);
                request.Content = new StringContent(xmlString, Encoding.UTF8, "text/xml");
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("text/plain"));
                var response = await httpClient.SendAsync(request);
                return response;
            } // using
        } // PostXmlRequest()

        /// <summary>
        /// Creates the SOAP (XML) envelope.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="input">The input.</param>
        /// <param name="serviceType">Type of the service.</param>
        /// <returns>
        /// An <see cref="XmlDocument" />.
        /// </returns>
        private static XmlDocument CreateSoapEnvelope(UpnpServiceAction action, IReadOnlyList<object> input, 
            string serviceType)
        {
            var doc = new XmlDocument();
            var env = doc.CreateElement("SOAP-ENV", "Envelope", "http://schemas.xmlsoap.org/soap/envelope/");
            doc.AppendChild(env);

            var body = doc.CreateElement("SOAP-ENV", "Body", "http://schemas.xmlsoap.org/soap/envelope/");
            env.AppendChild(body);

            var xaction = doc.CreateElement("m", action.Name, serviceType);
            body.AppendChild(xaction);

            for (var i = 0; i < action.ArgumentsIn.Count; i++)
            {
                var xarg = doc.CreateElement(action.ArgumentsIn[i].Name);
                if (input[i] != null)
                {
                    xarg.InnerText = input[i].ToString();
                    xaction.AppendChild(xarg);
                } // if
            } // foreach
            return doc;
        } // CreateSoapEnvelope()
#endregion // PRIVATE METHODS
    } // MySoap
}
