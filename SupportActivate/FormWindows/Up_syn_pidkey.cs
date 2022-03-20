using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SupportActivate.Common;
using SupportActivate.ProcessSQL;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace SupportActivate.FormWindows
{
    public partial class Up_syn_pidkey : Form
    {
        string KeyOffice = "KEYOFFICE";
        string KeyWindows = "KEYWINDOWS";
        string KeyServer = "KEYSERVER";
        DateTime dateTimeCheckKey;
        ServerKey serverKey;
        private log4net.ILog logger = log4net.LogManager.GetLogger(typeof(Up_syn_pidkey));
        public Up_syn_pidkey()
        {
            InitializeComponent();
            serverKey = new ServerKey();
        }

        private List<Pidkey_Up_syn_pidkey> getListKey(string table)
        {
            List<Pidkey_Up_syn_pidkey> list_key = new List<Pidkey_Up_syn_pidkey>();
            using (SQLiteConnection SqlConn = new SQLiteConnection(serverKey.connectionString))
            {
                string query = "SELECT * FROM " + table + " ORDER BY LicenseType DESC, Description ASC";
                SQLiteCommand cmd = new SQLiteCommand(query, SqlConn);
                SqlConn.Open();
                SQLiteDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Pidkey_Up_syn_pidkey keyoffice = new Pidkey_Up_syn_pidkey()
                    {
                        Key = reader.GetString(1),
                        Description = reader.GetString(2),
                        SubType = reader.GetString(3),
                        LicenseType = reader.GetString(4),
                        MAKCount = reader.GetString(5),
                        Errorcode = reader.GetString(6)
                    };
                    list_key.Add(keyoffice);
                }
                SqlConn.Close();
            }
            return list_key;
        }

        public void up_SynPid()
        {
            try
            {
                List<Pidkey_Up_syn_pidkey> list_key = new List<Pidkey_Up_syn_pidkey>();
                var listKeyOffice = getListKey(KeyOffice);
                var listKeyWindows = getListKey(KeyWindows);
                var listKeyServer = getListKey(KeyServer);
                foreach (var value in listKeyOffice)
                    list_key.Add(value);
                foreach (var value in listKeyWindows)
                    list_key.Add(value);
                foreach (var value in listKeyServer)
                    list_key.Add(value);

                var limitItem = 160;
                var totalCount = list_key.Count;
                var sendCount = totalCount / limitItem;
                var bienDem = 1;
                var viTriDauTien = 0;
                while (bienDem <= sendCount)
                {
                    data data = new data();
                    data.list_pidkey.Clear();
                    for (int i = viTriDauTien; i <= (limitItem * bienDem) - 1; i++)
                    {
                        data.list_pidkey.Add(new Pidkey_Up_syn_pidkey()
                        {
                            Key = list_key[i].Key,
                            Description = list_key[i].Description,
                            SubType = list_key[i].SubType,
                            LicenseType = list_key[i].LicenseType,
                            MAKCount = list_key[i].MAKCount,
                            Errorcode = list_key[i].Errorcode
                        });
                    }

                    viTriDauTien = bienDem * limitItem;
                    bienDem += 1;
                    // tien hanh dong bo key len server
                    bool resultUpLoadKey = processUpKeyToServer(data);
                    if (!resultUpLoadKey)
                        break;
                }

                var soDu = list_key.Count % limitItem;
                if (soDu > 0)
                {
                    data data1 = new data();
                    data1.list_pidkey.Clear();
                    for (int i = sendCount * limitItem; i <= ((sendCount * limitItem) + soDu) - 1; i++)
                    {
                        data1.list_pidkey.Add(new Pidkey_Up_syn_pidkey()
                        {
                            Key = list_key[i].Key,
                            Description = list_key[i].Description,
                            SubType = list_key[i].SubType,
                            LicenseType = list_key[i].LicenseType,
                            MAKCount = list_key[i].MAKCount,
                            Errorcode = list_key[i].Errorcode
                        });
                    }
                    processUpKeyToServer(data1);
                    MessageBox.Show("Successfully added " + totalCount + " keys.", Messages.success, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (soDu == 0)
                {
                    timer1.Stop();
                    MessageBox.Show("Successfully added " + totalCount + " keys.", Messages.success, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                timer1.Stop();
                logger.Error(ex);
                MessageBox.Show("Synchronized error1.", Messages.error, MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private bool processUpKeyToServer(data data)
        {
            data.token = Regex.Replace(tbx_Token.Text.ToLower(), "[^0-9a-z]+", "");
            string jsonString = JsonConvert.SerializeObject(data);
            var results = upload_syn(jsonString).ToString();
            if (string.IsNullOrEmpty(results))
            {
                timer1.Stop();
                MessageBox.Show("Synchronized error.", Messages.error, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else
            {
                var dataJson = JObject.Parse(results);
                string status = (string)dataJson.SelectToken("status");
                string res = (string)dataJson.SelectToken("message");
                if (status == "error")
                {
                    timer1.Stop();
                    MessageBox.Show(res, Messages.error, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                else
                    return true;
            }
        }


        private string upload_syn(string data)
        {
            HttpWebResponse response = null/* TODO Change to default(_) if this is not a reference type */;
            StreamReader reader;
            Uri address;
            string results = "";
            address = new Uri("https://getcid.info/api-up-syn-pidkey");
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072 | SecurityProtocolType.Ssl3 | (SecurityProtocolType)768 | SecurityProtocolType.Tls;
            ServicePointManager.DefaultConnectionLimit = 50;
            ServicePointManager.ServerCertificateValidationCallback = AcceptAllCertifications;
            WebRequest request;
            request = WebRequest.Create(address);
            byte[] data2 = Encoding.UTF8.GetBytes(data);
            request.Method = "POST";
            request.ContentType = "application/json;charset=UTF-8";
            request.ContentLength = data2.Length;
            Stream stream2 = request.GetRequestStream();
            stream2.Write(data2, 0, data2.Length);
            stream2.Close();
            try
            {
                // Get response  
                response = (HttpWebResponse)request.GetResponse();
                // Get the response stream into a reader  
                reader = new StreamReader(response.GetResponseStream());
                results = reader.ReadToEnd();
            }
            catch (Exception e)
            {
                logger.Error(e);
                results = "";
            }
            finally
            {
                if (response != null)
                    response.Close();
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
            Thread SynPid = new Thread(up_SynPid);
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
