using FormWindows.SupportActivate;
using SupportActivate.Common;
using SupportActivate.ProcessSQL;
using System;
using System.IO;
using System.Net;
using System.Threading;
using System.Windows.Forms;

namespace SupportActivate.ProcessTabControl
{
    public class ProcessTabSetting
    {
        FormSupportActivate formMain = FormSupportActivate.formMain;
        ServerSetting serverSetting;
        ServerKey serverKey;
        private log4net.ILog logger = log4net.LogManager.GetLogger(typeof(ProcessTabCheckKeySame));

        public ProcessTabSetting()
        {
            serverSetting = new ServerSetting();
        }

        public void cb_EnablePidKeyAdv_CheckedChanged(object sender, EventArgs e)
        {
            if (formMain.cb_EnablePidKeyAdv.Checked)
                formMain.tbx_ValidKey.Text = Messages.AdvancedCheckPidkey;
            else
            {
                formMain.btn_CheckAdv.Enabled = false;
                formMain.tbx_ValidKey.Clear();
            }
        }

        public void cb_EnableMultiThread_CheckedChanged(object sender, EventArgs e)
        {
            if (formMain.cb_EnableMultiThread.Checked)
            {
                var confirm = MessageBox.Show(Messages.MultythreadNoSupportWin7Off10, Messages.warning, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (confirm == DialogResult.Yes)
                    formMain.cbb_MultyThread.Enabled = true;
                else
                {
                    formMain.cbb_MultyThread.Enabled = false;
                    formMain.cb_EnableMultiThread.Checked = false;
                }
            }
            else
                formMain.cbb_MultyThread.Enabled = false;
        }

        public void btnSaveSetting_Click(object sender, EventArgs e)
        {
            if (formMain.cb_EnablePidKeyAdv.Checked)
                formMain.btn_CheckAdv.Enabled = true;
            else
                formMain.btn_CheckAdv.Enabled = false;
            try
            {
                serverSetting.UpdateSetting(formMain.cb_EnablePidKeyAdv.Checked, formMain.cb_LoadInDataKey.Checked, formMain.cb_QuestionClose.Checked);
                MessageBox.Show(Messages.SaveComplete, Messages.success, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(Messages.SaveError, Messages.error, MessageBoxButtons.OK, MessageBoxIcon.Error);
                serverSetting.CreateDataBase();
                serverKey = new ServerKey();
                serverKey.createDataBase();
                MessageBox.Show(Messages.OperationAgain, Messages.error, MessageBoxButtons.OK, MessageBoxIcon.Error);
                logger.Error(ex);
            }
        }

        public FileInfo ToFileFromUri(Uri url)
        {
            String filename = url.PathAndQuery.Replace('/', Path.DirectorySeparatorChar);
            return new FileInfo(filename);
        }

        public void update()
        {
            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls;
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
                string addressUpdate = "https://gitlab.com/vanthangmtd/support-activate/-/raw/main/update.txt";
                WebClient clientUpdate = new WebClient();
                StreamReader readerUpdate = new StreamReader(clientUpdate.OpenRead(addressUpdate));
                string valueUpdate = readerUpdate.ReadLine();
                var breakVersion = valueUpdate.IndexOf("version:");
                var valueUpdateNew = valueUpdate.Substring(breakVersion + 8);
                readerUpdate.Close();
                double version = formMain.version;
                if (valueUpdateNew == version.ToString())
                {
                    formMain.lb_CheckUpdate.Invoke(new Action(() =>
                    {
                        formMain.lb_CheckUpdate.ForeColor = System.Drawing.SystemColors.ControlText;
                        formMain.lb_NotiUpdate.ForeColor = System.Drawing.SystemColors.ControlText;
                        formMain.lb_CheckUpdate.Text = "There are no new updates";
                        formMain.lb_NotiUpdate.Text = "";
                    }));
                }
                else if (valueUpdateNew != version.ToString())
                {
                    formMain.lb_CheckUpdate.Invoke(new Action(() =>
                    {
                        formMain.lb_CheckUpdate.ForeColor = System.Drawing.Color.Red;
                        formMain.lb_CheckUpdate.Text = "Please update to a newer version of Support Activate!";
                        formMain.lb_NotiUpdate.ForeColor = System.Drawing.Color.Red;
                        formMain.lb_NotiUpdate.Text = "Please update to a newer version of Support Activate!";
                    }));
                    var ud = MessageBox.Show(Messages.PleaseUpdateNewerVersion, Messages.warning, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (ud == DialogResult.Yes)
                    {
                        ServicePointManager.Expect100Continue = true;
                        ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls;
                        ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
                        string link = "https://gitlab.com/vanthangmtd/support-activate/-/raw/main/update.txt";
                        WebClient client = new WebClient();
                        StreamReader reader = new StreamReader(client.OpenRead(link));
                        string part = Application.StartupPath;
                        string partUrlSaveFile;
                        string linkUrlSaveFile;
                        string fileName, extention;
                        string fullPart;
                        ServicePointManager.Expect100Continue = true;
                        ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls;
                        ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;

                        while (!reader.EndOfStream)
                        {
                            string linkUrl = reader.ReadLine();
                            var breakFileUrl = linkUrl.IndexOf("=>");
                            if (breakFileUrl != -1)
                            {
                                // neu co dinh dang:  \config=>https://raw.githubusercontent.com/vanthangmtd/Support-activate/master/README.md
                                int lengthvalue = Microsoft.VisualBasic.Strings.InStr(linkUrl, "=>");
                                partUrlSaveFile = linkUrl.Substring(0, lengthvalue - 1);
                                linkUrlSaveFile = linkUrl.Substring(lengthvalue + 1);
                                fileName = linkUrlSaveFile.Replace("%20", " ");
                                fullPart = part + partUrlSaveFile + @"\" + fileName.Replace("%20", " ");

                                if (File.Exists(fullPart))
                                {
                                    var fi = ToFileFromUri(new Uri(fileName));
                                    extention = fi.Extension;
                                    var fileNa = fi.Name.Replace(".exe", "").Replace("%20", " "); ;
                                    fullPart = part + partUrlSaveFile + @"\" + fileNa + "(1)" + extention;
                                    using (WebClient webClient = new WebClient())
                                    {
                                        formMain.lb_CheckUpdate.Invoke(new Action(() =>
                                        {
                                            formMain.lb_CheckUpdate.ForeColor = System.Drawing.Color.Red;
                                            formMain.lb_CheckUpdate.Text = "Download: " + fileNa;
                                            formMain.probDownload.Value = 10;
                                        }));
                                        webClient.DownloadFile(linkUrlSaveFile, fullPart);
                                        formMain.lb_CheckUpdate.Invoke(new Action(() =>
                                        {
                                            formMain.probDownload.Value = 100;
                                        }));
                                    }
                                }
                                else
                                {
                                    var fi = ToFileFromUri(new Uri(fileName));
                                    var fileNa = fi.Name.Replace("%20", " ");
                                    fullPart = part + partUrlSaveFile + @"\" + fileNa;

                                    using (WebClient webClient = new WebClient())
                                    {
                                        formMain.lb_CheckUpdate.Invoke(new Action(() =>
                                        {
                                            formMain.lb_CheckUpdate.ForeColor = System.Drawing.Color.Red;
                                            formMain.lb_CheckUpdate.Text = "Download: " + fileNa;
                                            formMain.probDownload.Value = 10;
                                        }));
                                        webClient.DownloadFile(linkUrlSaveFile, fullPart);
                                        formMain.lb_CheckUpdate.Invoke(new Action(() =>
                                        {
                                            formMain.probDownload.Value = 100;
                                        }));
                                    }
                                }
                            }
                        }
                        reader.Close();
                        formMain.lb_CheckUpdate.Invoke(new Action(() =>
                        {
                            formMain.lb_CheckUpdate.ForeColor = System.Drawing.SystemColors.ControlText;
                            formMain.lb_CheckUpdate.Text = "Download update success";
                            formMain.lb_NotiUpdate.ForeColor = System.Drawing.SystemColors.ControlText;
                            formMain.lb_NotiUpdate.Text = "";
                        }));
                        MessageBox.Show("Download update success", Messages.success, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                    formMain.lb_CheckUpdate.Invoke(new Action(() =>
                    {
                        formMain.lb_CheckUpdate.ForeColor = System.Drawing.SystemColors.ControlText;
                        formMain.lb_CheckUpdate.Text = "There are no new updates";
                        formMain.lb_NotiUpdate.ForeColor = System.Drawing.SystemColors.ControlText;
                        formMain.lb_NotiUpdate.Text = "";
                    }));
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                MessageBox.Show("Download update error", Messages.error, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        public void btn_CheckUpdate_Click(object sender, EventArgs e)
        {
            Thread updateOTA = new Thread(update);
            updateOTA.TrySetApartmentState(ApartmentState.STA);
            updateOTA.Start();
        }
    }
}
