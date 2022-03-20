using SupportActivate.Common;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace SupportActivate.ProcessBusiness
{
    public class ProcessGetRemainingActivationsOrCID
    {
        private log4net.ILog logger = log4net.LogManager.GetLogger(typeof(ProcessGetRemainingActivationsOrCID));
        /// <summary>
        /// GetRemainingActivationsOrCID
        /// GETCID: type = 1
        /// PIDKEY: type = 2
        /// </summary>
        /// <param name="type"></param>
        /// <param name="installationId"></param>
        /// <param name="extendedProductId"></param>
        /// <returns></returns>
        public string GetRemainingActivationsOrCID(int type, string installationId, string extendedProductId)
        {
            try
            {
                // Microsoft's PRIVATE KEY for HMAC-SHA256 encoding
                byte[] bPrivateKey = new byte[]
                {
                0xfe, 0x31, 0x98, 0x75, 0xfb, 0x48, 0x84, 0x86, 0x9c, 0xf3, 0xf1, 0xce, 0x99, 0xa8, 0x90, 0x64,
                0xab, 0x57, 0x1f, 0xca, 0x47, 0x04, 0x50, 0x58, 0x30, 0x24, 0xe2, 0x14, 0x62, 0x87, 0x79, 0xa0
                };

                // XML Namespace
                const string uri = "http://www.microsoft.com/DRM/SL/BatchActivationRequest/1.0";
                XNamespace BatchActivationServiceNs = "http://www.microsoft.com/BatchActivationService";
                XNamespace BatchActivationResponseNs = "http://www.microsoft.com/DRM/SL/BatchActivationResponse/1.0";

                // Create new XML Document
                XmlDocument xmlDoc = new XmlDocument();

                // Create Root Element
                XmlElement rootElement = xmlDoc.CreateElement("ActivationRequest", uri);
                xmlDoc.AppendChild(rootElement);

                // Create VersionNumber Element
                XmlElement versionNumber = xmlDoc.CreateElement("VersionNumber", rootElement.NamespaceURI);
                versionNumber.InnerText = "2.0";
                rootElement.AppendChild(versionNumber);

                // Create RequestType Element
                XmlElement requestType = xmlDoc.CreateElement("RequestType", rootElement.NamespaceURI);
                requestType.InnerText = type.ToString();
                rootElement.AppendChild(requestType);

                // Create Requests Group Element
                XmlElement requestsGroupElement = xmlDoc.CreateElement("Requests", rootElement.NamespaceURI);

                // Create Request Element
                XmlElement requestElement = xmlDoc.CreateElement("Request", requestsGroupElement.NamespaceURI);

                // Add PID as Request Element
                XmlElement pidEntry = xmlDoc.CreateElement("PID", requestElement.NamespaceURI);
                pidEntry.InnerText = extendedProductId.Replace("XXXXX", "55041");
                requestElement.AppendChild(pidEntry);

                if (!string.IsNullOrEmpty(installationId))
                {
                    XmlElement iidEntry = xmlDoc.CreateElement("IID", requestElement.NamespaceURI);
                    iidEntry.InnerText = installationId;
                    requestElement.AppendChild(iidEntry);
                }

                // Add Request Element to Requests Group Element
                requestsGroupElement.AppendChild(requestElement);

                // Add Requests and Request to XML Document
                rootElement.AppendChild(requestsGroupElement);

                // Get Unicode Byte Array of XML Document
                byte[] byteXml = Encoding.Unicode.GetBytes(xmlDoc.InnerXml);

                // Convert Byte Array to Base64
                string base64Xml = Convert.ToBase64String(byteXml);

                // Compute Digest of the Base 64 XML Bytes
                string digest;
                using (HMACSHA256 hmacsha256 = new HMACSHA256 { Key = bPrivateKey })
                {
                    digest = Convert.ToBase64String(hmacsha256.ComputeHash(byteXml));
                }

                // Create SOAP Envelope for Web Request
                string form = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                    "<soap:Envelope xmlns:soap=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">" +
                    "<soap:Body>" +
                    "<BatchActivate xmlns=\"" + BatchActivationServiceNs + "\">" +
                    "<request>" +
                    "<Digest>REPLACEME1</Digest>" +
                    "<RequestXml>REPLACEME2</RequestXml>" +
                    "</request>" +
                    "</BatchActivate>" +
                    "</soap:Body>" +
                    "</soap:Envelope>";
                form = form.Replace("REPLACEME1", digest);      // Put your Digest value (BASE64 encoded)
                form = form.Replace("REPLACEME2", base64Xml);   // Put your Base64 XML value (BASE64 encoded)
                XmlDocument soapEnvelopeXml = new XmlDocument();
                soapEnvelopeXml.LoadXml(form);

                // Create Web Request
                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create("https://activation.sls.microsoft.com/BatchActivation/BatchActivation.asmx");
                webRequest.Method = "POST";
                webRequest.ContentType = "text/xml; charset=\"utf-8\"";
                webRequest.Headers.Add("SOAPAction", "http://www.microsoft.com/BatchActivationService/BatchActivate");

                // Insert SOAP Envelope into Web Request
                using (Stream stream = webRequest.GetRequestStream())
                {
                    soapEnvelopeXml.Save(stream);
                }

                // Begin Async call to Web Request
                IAsyncResult asyncResult = webRequest.BeginGetResponse(null, null);

                // Suspend Thread until call is complete
                asyncResult.AsyncWaitHandle.WaitOne();

                // Get the Response from the completed Web Request
                XDocument soapResponse = new XDocument();
                using (WebResponse webResponse = webRequest.EndGetResponse(asyncResult))

                // ReSharper disable AssignNullToNotNullAttribute
                using (StreamReader rd = new StreamReader(webResponse.GetResponseStream()))
                // ReSharper restore AssignNullToNotNullAttribute
                {
                    soapResponse = XDocument.Parse(rd.ReadToEnd());
                }

                XDocument responseXml = XDocument.Parse(soapResponse.Descendants(BatchActivationServiceNs + "ResponseXml").First().Value);
                if (responseXml.Descendants(BatchActivationResponseNs + "ErrorCode").Any())
                {
                    string errorCode = responseXml.Descendants(BatchActivationResponseNs + "ErrorCode").First().Value;
                    if (errorCode == "0x7F") return Constant.Need_to_call;
                    else if (errorCode == "0x67") return Constant.KeyBlock;
                    else if (errorCode == "0x8D") return Constant.Exceeded_IID;
                    else if (errorCode == "0x90") return Constant.Wrong_IID;
                    else if (errorCode == "0x68") return Constant.Not_legimate_key;
                    else if (errorCode == "0xD5" || errorCode == "0xD6") return Constant.Blocked_IID;
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
                    return Constant.error;
                    //throw new Exception("The remote server returned an unrecognized response.");
                }
            }
            catch(Exception ex)
            {
                logger.Error(ex);
                return Constant.error;
            }
        }
    }
}
