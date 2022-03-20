using Newtonsoft.Json.Linq;
using SupportActivate.Common;
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
    public partial class Register_token : Form
    {
        private log4net.ILog logger = log4net.LogManager.GetLogger(typeof(Register_token));
        private DateTime dateTimeCheckKey;

        public Register_token()
        {
            InitializeComponent();
        }

        private void btn_Reg_Click(object sender, EventArgs e)
        {
            string token = Regex.Replace(tbx_Token.Text.ToLower(), "[^0-9a-z]+", "");
            string email = tbx_Email.Text;
            if (token.Length < 10)
                MessageBox.Show("Token has 10 characters, please enter 10 full characters.", Messages.warning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else if (email == "")
                MessageBox.Show("Please enter your email address.", Messages.warning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else
            {
                lb_Timer.Text = "00:00:00:000";
                dateTimeCheckKey = DateTime.Now;
                timer1.Start();
                Thread registerToken = new Thread(register);
                registerToken.Start();
                tbx_Email.Enabled = false;
                tbx_Token.Enabled = false;
                btn_Reg.Enabled = false;
                btn_Cancel.Enabled = false;
            }
        }

        private bool AcceptAllCertifications(object sender, System.Security.Cryptography.X509Certificates.X509Certificate certification, System.Security.Cryptography.X509Certificates.X509Chain chain, System.Net.Security.SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

        private void register()
        {
            HttpWebRequest request;
            HttpWebResponse response = null/* TODO Change to default(_) if this is not a reference type */;
            StreamReader reader;
            Uri address;
            StringBuilder data;
            byte[] byteData;
            Stream postStream = null;
            try
            {
                address = new Uri("https://getcid.info/api-registry-token-pidkey");
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
                data.Append("&email=" + HttpUtility.UrlEncode(tbx_Email.Text));

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
                    JToken results = JObject.Parse(reader.ReadToEnd());
                    string status = (string)results.SelectToken("status");
                    string res = (string)results.SelectToken("res");
                    if (status == "error")
                    {
                        timer1.Stop();
                        MessageBox.Show(res, Messages.error, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        timer1.Stop();
                        MessageBox.Show(res, Messages.success, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                finally
                {
                    if (response != null)
                        response.Close();
                }
            }
            catch (Exception ex)
            {
                timer1.Stop();
                logger.Error(ex);
                MessageBox.Show("Error", Messages.error, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            timer1.Stop();
            btn_Reg.Invoke(new Action(() =>
            {
                tbx_Email.Enabled = true;
                tbx_Token.Enabled = true;
                btn_Reg.Enabled = true;
                btn_Cancel.Enabled = true;
                ActiveForm.Close();
            }));
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
