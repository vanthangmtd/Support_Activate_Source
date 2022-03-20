using Newtonsoft.Json.Linq;
using SupportActivate.Common;
using SupportActivate.ProcessSQL;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.Windows.Forms;

namespace SupportActivate.FormWindows
{
    public partial class Down_syn_pidkey : Form
    {
        DateTime dateTimeCheckKey;
        ServerKey serverKey;
        private log4net.ILog logger = log4net.LogManager.GetLogger(typeof(Down_syn_pidkey));

        public Down_syn_pidkey()
        {
            InitializeComponent();
            serverKey = new ServerKey();
        }

        private void processData()
        {
            try
            {
                var jsonData = getData();
                if (!string.IsNullOrEmpty(jsonData))
                {
                    JToken results = JObject.Parse(jsonData);
                    string status = (string)results.SelectToken("status");
                    if (status == "error")
                    {
                        timer1.Stop();
                        MessageBox.Show((string)results.SelectToken("res"), Messages.error, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        JArray list = (JArray)results.SelectToken("res");
                        foreach (JObject item in list)
                        {
                            var Description = Convert.ToString(item["Description"]);
                            pid pid = new pid();
                            pid.Key = string.IsNullOrEmpty(Convert.ToString(item["Key"])) ? string.Empty : Convert.ToString(item["Key"]);
                            pid.Description = string.IsNullOrEmpty(Convert.ToString(item["Description"])) ? string.Empty : Convert.ToString(item["Description"]);
                            pid.SubType = string.IsNullOrEmpty(Convert.ToString(item["SubType"])) ? string.Empty : Convert.ToString(item["SubType"]);
                            pid.LicenseType = string.IsNullOrEmpty(Convert.ToString(item["LicenseType"])) ? string.Empty : Convert.ToString(item["LicenseType"]);
                            pid.MAKCount = string.IsNullOrEmpty(Convert.ToString(item["MAKCount"])) ? string.Empty : Convert.ToString(item["MAKCount"]);
                            pid.ErrorCode = string.IsNullOrEmpty(Convert.ToString(item["Errorcode"])) ? string.Empty : Convert.ToString(item["Errorcode"]);
                            pid.KeyGetWeb = string.Empty;
                            serverKey.CreateDataKey(true, pid, string.Empty);
                        }
                        timer1.Stop();
                        MessageBox.Show("Successful data synchronization.", Messages.success, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    timer1.Stop();
                    MessageBox.Show("No data returned.", Messages.warning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                timer1.Stop();
                logger.Error(ex);
                MessageBox.Show("There is an error synchronizing data.", Messages.error, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            timer1.Stop();
            btn_Syn.Invoke(new Action(() =>
            {
                btn_Syn.Enabled = true;
                btn_Cancel.Enabled = true;
                tbx_Token.Enabled = true;
                ActiveForm.Close();
            }));
        }

        private string getData()
        {
            HttpWebRequest request;
            HttpWebResponse response = null/* TODO Change to default(_) if this is not a reference type */;
            string results = "";
            StreamReader reader;
            Uri address;
            StringBuilder data;
            byte[] byteData;
            Stream postStream = null;
            try
            {
                address = new Uri("https://getcid.info/api-down-syn-pidkey");
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072 | SecurityProtocolType.Ssl3 | (SecurityProtocolType)768 | SecurityProtocolType.Tls;
                ServicePointManager.DefaultConnectionLimit = 50;
                ServicePointManager.ServerCertificateValidationCallback = AcceptAllCertifications;
                // Create the web request  
                request = (HttpWebRequest)WebRequest.Create(address);

                // Set type to POST  
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";

                data = new StringBuilder();
                data.Append("token=" + HttpUtility.UrlEncode(Regex.Replace(tbx_Token.Text.ToLower(), "[^0-9a-z]+", "")));

                // Create a byte array of the data we want to send  
                byteData = UTF8Encoding.UTF8.GetBytes(data.ToString());

                // Set the content length in the request headers  
                request.ContentLength = byteData.Length;

                // Write data  
                try
                {
                    postStream = request.GetRequestStream();
                    postStream.Write(byteData, 0, byteData.Length);
                }
                finally
                {
                    if (postStream != null)
                        postStream.Close();
                }

                try
                {
                    // Get response  
                    response = (HttpWebResponse)request.GetResponse();
                    // Get the response stream into a reader  
                    reader = new StreamReader(response.GetResponseStream());
                    // Console application output  
                    results = reader.ReadToEnd();
                }
                finally
                {
                    if (response != null)
                        response.Close();
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
            return results;
        }

        private bool AcceptAllCertifications(object sender, System.Security.Cryptography.X509Certificates.X509Certificate certification, System.Security.Cryptography.X509Certificates.X509Chain chain, System.Net.Security.SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

        private void btn_Syn_Click(object sender, EventArgs e)
        {
            lb_Timer.Text = "00:00:00:000";
            dateTimeCheckKey = DateTime.Now;
            timer1.Start();
            btn_Syn.Enabled = false;
            btn_Cancel.Enabled = false;
            tbx_Token.Enabled = false;
            Thread SynPid = new Thread(processData);
            SynPid.Start();
        }

        private void btn_Cancel_Click(object sender, EventArgs e)
        {
            ActiveForm.Close();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            TimeSpan diff = DateTime.Now.Subtract(dateTimeCheckKey);
            lb_Timer.Text = diff.ToString(@"hh\:mm\:ss\:fff");
        }
    }
}
