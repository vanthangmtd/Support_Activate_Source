using SupportActivate.Common;
using System;
using System.IO;
using System.Net;
using System.Text;

namespace SupportActivate.ProcessBusiness
{
    public class ProcessGetcid
    {
        ProcessGetRemainingActivationsOrCID processGetRemainingActivationsOrCID;
        private log4net.ILog logger = log4net.LogManager.GetLogger(typeof(ProcessGetcid));

        public ProcessGetcid()
        {
            processGetRemainingActivationsOrCID = new ProcessGetRemainingActivationsOrCID();
        }

        public string GetConfirmationID(string iid, string tokenGetcid)
        {
            string cid = processGetRemainingActivationsOrCID.GetRemainingActivationsOrCID(1, iid, "55041-03413-188-239517-00-1033-9200.0000-2692020");
            if (ContantResource.KeyBlock == cid) return ContantResource.Exceeded_IID;
            if (cid.IndexOf("Please") == -1 && cid != ContantResource.serverBusy && cid != "0x71" && cid != "0xC004C017" && cid != ContantResource.Need_to_call)
                return cid;
            return GetcidWithApi(iid, tokenGetcid);
        }

        private string GetcidWithApi(string iid, string token)
        {
            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls;
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
                string api = "https://getcid.info/api/" + iid + "/" + token;
                UTF8Encoding encoding = new UTF8Encoding();
                HttpWebRequest postreq = (HttpWebRequest)HttpWebRequest.Create(api);
                postreq.ContentType = "application/x-www-form-urlencoded";
                postreq.Method = "GET";
                postreq.UserAgent = "Request from Support Activate";
                postreq.Timeout = 90000;
                postreq.ReadWriteTimeout = 90000;
                StreamReader postreqreader = new StreamReader(postreq.GetResponse().GetResponseStream());
                string thepage = postreqreader.ReadToEnd();
                postreqreader.Close();
                return thepage.Replace(@"""", "").ToString();
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                return ContantResource.serverBusy;
            }
        }
    }
}
