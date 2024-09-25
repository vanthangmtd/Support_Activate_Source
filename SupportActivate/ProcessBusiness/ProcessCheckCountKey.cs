using SupportActivate.Common;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Linq;

namespace SupportActivate.ProcessBusiness
{
    public class ProcessCheckCountKey
    {
        // Key for HMAC/SHA256 signature.
        private static readonly byte[] MacKey = new byte[64] {
            254,  49, 152, 117, 251,  72, 132, 134,
            156, 243, 241, 206, 153, 168, 144, 100,
            171,  87,  31, 202,  71,   4,  80,  88,
            48,   36, 226,  20,  98, 135, 121, 160,
            0,     0,   0,   0,   0,   0,   0,   0,
            0,     0,   0,   0,   0,   0,   0,   0,
            0,     0,   0,   0,   0,   0,   0,   0,
            0,     0,   0,   0,   0,   0,   0,   0
        };

        private const string Action = "http://www.microsoft.com/BatchActivationService/BatchActivate";

        private static readonly Uri Uri = new Uri("https://activation.sls.microsoft.com/BatchActivation/BatchActivation.asmx");

        private static readonly XNamespace SoapSchemaNs = "http://schemas.xmlsoap.org/soap/envelope/";
        private static readonly XNamespace XmlSchemaInstanceNs = "http://www.w3.org/2001/XMLSchema-instance";
        private static readonly XNamespace XmlSchemaNs = "http://www.w3.org/2001/XMLSchema";
        private static readonly XNamespace BatchActivationServiceNs = "http://www.microsoft.com/BatchActivationService";
        private static readonly XNamespace BatchActivationRequestNs = "http://www.microsoft.com/DRM/SL/BatchActivationRequest/1.0";
        private static readonly XNamespace BatchActivationResponseNs = "http://www.microsoft.com/DRM/SL/BatchActivationResponse/1.0";

        public string CallWebService(int requestType, string installationId, string extendedProductId)
        {
            XDocument soapRequest = CreateSoapRequest(requestType, installationId, extendedProductId);
            HttpWebRequest webRequest = CreateWebRequest(soapRequest);
            XDocument soapResponse = new XDocument();

            try
            {
                IAsyncResult asyncResult = webRequest.BeginGetResponse(null, null);
                asyncResult.AsyncWaitHandle.WaitOne();

                // Read data from the response stream.
                using (WebResponse webResponse = webRequest.EndGetResponse(asyncResult))
                using (StreamReader streamReader = new StreamReader(webResponse.GetResponseStream()))
                {
                    soapResponse = XDocument.Parse(streamReader.ReadToEnd());
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return ParseSoapResponse(soapResponse, installationId);
        }

        private static XDocument CreateSoapRequest(int requestType, string installationId, string extendedProductId)
        {
            // Create an activation request.           
            XElement activationRequest = new XElement(BatchActivationRequestNs + "ActivationRequest",
                new XElement(BatchActivationRequestNs + "VersionNumber", "2.0"),
                new XElement(BatchActivationRequestNs + "RequestType", requestType),
                new XElement(BatchActivationRequestNs + "Requests",
                    new XElement(BatchActivationRequestNs + "Request",
                        new XElement(BatchActivationRequestNs + "PID", extendedProductId),
                        requestType == 1 ? new XElement(BatchActivationRequestNs + "IID", installationId) : null)
                )
            );

            // Get the unicode byte array of activationRequest and convert it to Base64.
            byte[] bytes = Encoding.Unicode.GetBytes(activationRequest.ToString());
            string requestXml = Convert.ToBase64String(bytes);

            XDocument soapRequest = new XDocument();

            using (HMACSHA256 hMACSHA = new HMACSHA256(MacKey))
            {
                // Convert the HMAC hashed data to Base64.
                string digest = Convert.ToBase64String(hMACSHA.ComputeHash(bytes));

                soapRequest = new XDocument(
                new XDeclaration("1.0", "UTF-8", "no"),
                new XElement(SoapSchemaNs + "Envelope",
                    new XAttribute(XNamespace.Xmlns + "soap", SoapSchemaNs),
                    new XAttribute(XNamespace.Xmlns + "xsi", XmlSchemaInstanceNs),
                    new XAttribute(XNamespace.Xmlns + "xsd", XmlSchemaNs),
                    new XElement(SoapSchemaNs + "Body",
                        new XElement(BatchActivationServiceNs + "BatchActivate",
                            new XElement(BatchActivationServiceNs + "request",
                                new XElement(BatchActivationServiceNs + "Digest", digest),
                                new XElement(BatchActivationServiceNs + "RequestXml", requestXml)
                            )
                        )
                    )
                ));

            }

            return soapRequest;
        }

        private static HttpWebRequest CreateWebRequest(XDocument soapRequest)
        {
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(Uri);
            webRequest.Accept = "text/xml";
            webRequest.ContentType = "text/xml; charset=\"utf-8\"";
            webRequest.Headers.Add("SOAPAction", Action);
            webRequest.Method = "POST";

            try
            {
                // Insert SOAP envelope
                using (Stream stream = webRequest.GetRequestStream())
                {
                    soapRequest.Save(stream);
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return webRequest;
        }

        private static string ParseSoapResponse(XDocument soapResponse, string iid)
        {
            try
            {
                if (soapResponse == null)
                {
                    return ContantResource.serverBusy;
                    //throw new ArgumentNullException("The remote server returned an unexpected response.");
                }

                XDocument responseXml = XDocument.Parse(soapResponse.Descendants(BatchActivationServiceNs + "ResponseXml").First().Value);

                if (responseXml.Descendants(BatchActivationResponseNs + "ErrorCode").Any())
                {
                    string errorCode = responseXml.Descendants(BatchActivationResponseNs + "ErrorCode").First().Value;
                    if (errorCode == "0x7F") return ContantResource.Need_to_call;
                    else if (errorCode == "0x67") return ContantResource.KeyBlock;
                    else if (errorCode == "0x8D") return ContantResource.Exceeded_IID;
                    else if (errorCode == "0x90") return ContantResource.Wrong_IID;
                    else if (errorCode == "0x68") return ContantResource.Not_legimate_key;
                    else if (errorCode == "0xD5" || errorCode == "0xD6") return ContantResource.Blocked_IID;
                    else if (errorCode == "0x86") return "-1";
                    else if (errorCode == "0x71" || errorCode == "0x8E") return "0x71";
                    else if (errorCode == "0xC004C017" || errorCode == "0x80131509") return "0xC004C017";
                    else
                    {
                        return "Please try again or try another server. " + errorCode;
                    }
                }
                else if (responseXml.Descendants(BatchActivationResponseNs + "ResponseType").Any())
                {
                    int responseType = Convert.ToInt32(responseXml.Descendants(BatchActivationResponseNs + "ResponseType").First().Value);
                    if (responseType == 1) return responseXml.Descendants(BatchActivationResponseNs + "CID").First().Value;
                    if (responseType == 2) return responseXml.Descendants(BatchActivationResponseNs + "ActivationRemaining").First().Value;
                    else
                    {
                        return "Please try again or try another server. responseType: " + responseType;
                    }
                }
                else
                {
                    return ContantResource.error;
                    //throw new Exception("The remote server returned an unrecognized response.");
                }

            }
            catch
            {
                return ContantResource.error;
            }
        }
    }
}
