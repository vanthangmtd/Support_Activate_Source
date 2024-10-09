using FormWindows.SupportActivate;
using SupportActivate.Common;
using SupportActivate.ProcessBusiness;
using SupportActivate.ProcessSQL;
using SupportActivate.Setting;
using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace SupportActivate.ProcessTabControl
{
    public class ProcessTabSupportActivate
    {
        FormSupportActivate formMain = FormSupportActivate.formMain;
        Validate validate;
        ConfigSetting setting;
        SourceData sourceData;
        ServerSetting serverSetting;
        ProcessGetcid processGetcid;
        string key, optionCbx;
        DateTime dateTimeGetCID;

        public ProcessTabSupportActivate()
        {
            validate = new Validate();
            setting = new ConfigSetting();
            sourceData = new SourceData();
            serverSetting = new ServerSetting();
        }

        //Script Activate Windows/Office
        public void tbx_Key_Click(object sender, EventArgs e)
        {
            StringBuilder keyInput = new StringBuilder();
            key = keyInput.Append(Clipboard.GetText().Trim().ToUpper()).ToString();
            formMain.tbx_Key.Text = key.ToString();
        }

        public void tbx_Key_TextChanged(object sender, EventArgs e)
        {
            optionCbx = formMain.cbb_VersionActivate.Text;
            StringBuilder keyInput = new StringBuilder();
            key = keyInput.Append(Clipboard.GetText().Trim().ToUpper()).ToString();
            if (validate.ValidateKey(key))
            {
                formMain.tbx_Key.Text = key;
                formMain.cbb_VersionActivate.Enabled = true;
                if (optionCbx == setting.optionSelect)
                    formMain.btn_OKActivate.Enabled = false;
                else
                    formMain.btn_OKActivate.Enabled = true;
            }
            else
            {
                formMain.tbx_Key.Clear();
                formMain.cbb_VersionActivate.Enabled = false;
                formMain.btn_OKActivate.Enabled = false;
            }
        }

        public void cb_Decode_CheckedChanged(object sender, EventArgs e)
        {
            if (formMain.cb_Decode.Checked)
            {
                formMain.tbx_Key.PasswordChar = '\u0000';
                formMain.tbx_Log.PasswordChar = '\u0000';
                formMain.tbx_TokenGetcid.PasswordChar = '\u0000';
            }
            else
            {
                formMain.tbx_Key.PasswordChar = char.Parse("*");
                formMain.tbx_Log.PasswordChar = char.Parse("*");
                formMain.tbx_TokenGetcid.PasswordChar = char.Parse("*");
            }
        }

        public void cbb_VersionActivate(object sender, EventArgs e, string function_name)
        {
            StringBuilder keyInput = new StringBuilder();
            key = keyInput.Append(formMain.tbx_Key.Text.Trim().ToUpper()).ToString();
            optionCbx = formMain.cbb_VersionActivate.Text;
            formMain.lb_TitleToken.Hide();
            formMain.tbx_TokenActivate.Hide();
            formMain.tbx_TokenActivate.Clear();
            formMain.cbb_VersionVL.Hide();
            formMain.cbb_VersionVL.Items.Clear();
            if (optionCbx == setting.optionSelect)
            {
                formMain.tbx_Log.Clear();
                formMain.btn_OKActivate.Enabled = false;
            }
            else if (optionCbx != setting.optionSelect)
            {
                cb_Decode_CheckedChanged(sender, e);
                var listSourceData = sourceData.OptionCbxWindowsOffice();
                if (listSourceData[4] == optionCbx || listSourceData[7] == optionCbx)
                {
                    Clipboard.Clear();
                    formMain.lb_TitleToken.Show();
                    formMain.tbx_TokenActivate.Show();
                    formMain.tbx_TokenActivate.Clear();
                    formMain.tbx_TokenActivate.Text = Regex.Replace(serverSetting.GetKEYAPI().ToLower(), "[^a-z0-9]", "");
                    formMain.tbx_Log.Clear();
                    if (function_name == "cbb_VersionActivate_SelectedIndexChanged")
                        formMain.btn_OKActivate.Enabled = true;
                }
                else if (listSourceData[11] == optionCbx || listSourceData[12] == optionCbx || listSourceData[13] == optionCbx)
                {
                    formMain.tbx_Log.Clear();
                    formMain.btn_OKActivate.Enabled = false;
                    formMain.cbb_VersionVL.Show();
                    string year = Regex.Replace(optionCbx, @"\D", "");
                    var listOfficeVL = sourceData.OptionOfficeVL(year);
                    foreach (var item in listOfficeVL)
                    {
                        formMain.cbb_VersionVL.Items.Add(item);
                    }
                }
                else
                {
                    formMain.btn_OKActivate.Enabled = true;
                    for (int i = 0; i < listSourceData.Count; i++)
                    {
                        if (listSourceData[i] == optionCbx)
                        {
                            StringBuilder data = new StringBuilder();
                            string tokenGetcid = Regex.Replace(formMain.tbx_TokenActivate.Text.ToLower(), "[^a-z0-9]", "");
                            data.Append(sourceData.ScriptWindowsOffice(i, key, tokenGetcid, string.Empty));
                            formMain.tbx_Log.Text = data.ToString();
                            Clipboard.SetText(data.ToString());
                        }
                    }
                }
            }
        }

        public void cbb_VersionVL(object sender, EventArgs e)
        {
            StringBuilder keyInput = new StringBuilder();
            key = keyInput.Append(formMain.tbx_Key.Text.Trim().ToUpper()).ToString();
            optionCbx = formMain.cbb_VersionActivate.Text;
            string typeOfficeVL = formMain.cbb_VersionVL.Text;
            var listSourceData = sourceData.OptionCbxWindowsOffice();
            if (optionCbx == setting.optionSelect)
            {
                formMain.tbx_Log.Clear();
                formMain.btn_OKActivate.Enabled = false;
            }
            else if (optionCbx != setting.optionSelect && (listSourceData[11] == optionCbx || listSourceData[12] == optionCbx || listSourceData[13] == optionCbx))
            {
                cb_Decode_CheckedChanged(sender, e);
                if (!string.IsNullOrEmpty(typeOfficeVL))
                {
                    formMain.btn_OKActivate.Enabled = true;
                    StringBuilder data = new StringBuilder();
                    data.Append(sourceData.ScriptWindowsOffice(11, key, string.Empty, typeOfficeVL));
                    formMain.tbx_Log.Text = data.ToString();
                    Clipboard.SetText(data.ToString());
                }
            }
        }

        public void btn_OKActivate_Click(object sender, EventArgs e)
        {
            StringBuilder keyInput = new StringBuilder();
            key = keyInput.Append(formMain.tbx_Key.Text.Trim().ToUpper()).ToString();
            optionCbx = formMain.cbb_VersionActivate.Text;

            cb_Decode_CheckedChanged(sender, e);
            var listSourceData = sourceData.OptionCbxWindowsOffice();
            if (listSourceData[4] == optionCbx || listSourceData[7] == optionCbx)
            {
                string tokenGetcid = Regex.Replace(formMain.tbx_TokenActivate.Text.ToLower(), "[^a-z0-9]", "");
                if (string.IsNullOrEmpty(tokenGetcid))
                    MessageBox.Show(MessagesResource.TokenApiCannotEmpty, MessagesResource.warning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                else
                {
                    formMain.btn_OKActivate.Enabled = true;
                    for (int i = 0; i < listSourceData.Count; i++)
                    {
                        if (listSourceData[i] == optionCbx)
                        {
                            StringBuilder data = new StringBuilder();
                            data.Append(sourceData.ScriptWindowsOffice(i, key, tokenGetcid, string.Empty));
                            formMain.tbx_Log.Text = data.ToString();
                            Clipboard.SetText(data.ToString());
                        }
                    }
                    string keyAPIFile = Regex.Replace(serverSetting.GetKEYAPI().ToLower(), "[^a-z0-9]", "");
                    if (keyAPIFile != tokenGetcid && tokenGetcid.Length >= 6)
                    {
                        var result = MessageBox.Show(MessagesResource.UpdateTokenGetcid, MessagesResource.warning, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (result == DialogResult.Yes)
                            serverSetting.UpdateKEYAPI(Regex.Replace(tokenGetcid.ToLower(), "[^a-z0-9]", ""));
                    }
                }
            }
            else if (optionCbx != setting.optionSelect && (listSourceData[11] == optionCbx || listSourceData[12] == optionCbx || listSourceData[13] == optionCbx))
            {
                cbb_VersionVL(sender, e);
            }
            else
            {
                for (int i = 0; i < listSourceData.Count; i++)
                {
                    if (listSourceData[i] == optionCbx)
                    {
                        StringBuilder data = new StringBuilder();
                        string tokenGetcid = Regex.Replace(formMain.tbx_TokenActivate.Text.ToLower(), "[^a-z0-9]", "");
                        data.Append(sourceData.ScriptWindowsOffice(i, key, tokenGetcid, string.Empty));
                        formMain.tbx_Log.Text = data.ToString();
                        Clipboard.SetText(data.ToString());
                    }
                }
            }
        }


        //Get ConfirmationID
        public void tbx_IID_TextChanged(object sender, EventArgs e)
        {
            string iid = Regex.Replace(formMain.tbx_IID.Text, @"\D", "");
            if (iid.Length == 54 || iid.Length == 63)
            {
                formMain.tbx_TokenGetcid.Enabled = true;
                formMain.tbx_TokenGetcid.Text = serverSetting.GetKEYAPI();
            }
            else
            {
                formMain.tbx_TokenGetcid.Enabled = false;
                formMain.tbx_IID.Clear();
                formMain.tbx_TokenGetcid.Clear();
            }
        }

        public void tbx_TokenGetcid_TextChanged(object sender, EventArgs e)
        {
            string tokenGetcid = Regex.Replace(formMain.tbx_TokenGetcid.Text.ToLower(), "[^a-z0-9]", "");
            if (tokenGetcid.Length >= 6)
                formMain.btn_Getcid.Enabled = true;
            else
                formMain.btn_Getcid.Enabled = false;
        }

        public void btn_Getcid_Click(object sender, EventArgs e)
        {
            string tokenGetcid = Regex.Replace(formMain.tbx_TokenGetcid.Text.ToLower(), "[^a-z0-9]", "");
            if (string.IsNullOrEmpty(tokenGetcid))
            {
                MessageBox.Show(MessagesResource.TokenApiCannotEmpty, MessagesResource.warning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            formMain.lb_TimerGetcid.Text = "00:00:00:000";
            dateTimeGetCID = DateTime.Now;
            if (tokenGetcid.Length >= 6 && tokenGetcid != serverSetting.GetKEYAPI())
            {
                var result = MessageBox.Show(MessagesResource.UpdateTokenGetcid, MessagesResource.warning, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                    serverSetting.UpdateKEYAPI(Regex.Replace(tokenGetcid.ToLower(), "[^a-z0-9]", ""));
            }
            formMain.tbx_IID.Enabled = false;
            formMain.btn_Getcid.Enabled = false;
            formMain.tbx_TokenGetcid.Enabled = false;
            formMain.tbx_CID.Enabled = false;
            formMain.cbb_VersionGetcid.Enabled = false;
            formMain.btn_CopyCID.Enabled = false;

            formMain.timerGetcid.Start();
            new Thread(() =>
            {
                string iid = string.Empty, confirmationID = string.Empty;
                formMain.tbx_CID.Invoke(new Action(() =>
                {
                    formMain.tbx_CID.Clear();
                    iid = formMain.tbx_IID.Text;
                }));

                processGetcid = new ProcessGetcid();
                confirmationID = processGetcid.GetConfirmationID(iid, tokenGetcid);

                formMain.tbx_CID.Invoke(new Action(() =>
                {
                    formMain.tbx_CID.Text = confirmationID;
                    formMain.tbx_Log.Clear();
                }));
                formMain.timerGetcid.Stop();
                formMain.tbx_CID.Invoke(new Action(() =>
                {
                    formMain.tbx_IID.Enabled = true;
                    formMain.btn_Getcid.Enabled = true;
                    formMain.tbx_TokenGetcid.Enabled = true;
                    formMain.btn_CopyCID.Enabled = true;
                }));

            })
            { IsBackground = true }.Start();
        }

        public void timerGetcid_Tick(object sender, EventArgs e)
        {
            TimeSpan diff = DateTime.Now.Subtract(dateTimeGetCID);
            formMain.lb_TimerGetcid.Text = diff.ToString(@"hh\:mm\:ss\:fff");
        }

        public void tbx_CID_TextChanged(object sender, EventArgs e)
        {
            string cid = formMain.tbx_CID.Text.Replace(" ", "").Trim();
            if (validate.ValidateCID(cid))
            {
                formMain.cbb_VersionGetcid.Enabled = true;
                if (formMain.cbb_VersionGetcid.Text == setting.optionSelect)
                    formMain.btn_OKGetcid.Enabled = false;
                else
                    formMain.btn_OKGetcid.Enabled = true;
            }
            else
            {
                formMain.cbb_VersionGetcid.Enabled = false;
                formMain.btn_OKGetcid.Enabled = false;
            }
        }

        public void cbb_VersionGetcid(object sender, EventArgs e, string function_name)
        {
            formMain.tbx_Log.PasswordChar = '\u0000';
            string optionCbx = formMain.cbb_VersionGetcid.Text;
            string cid = formMain.tbx_CID.Text.Replace(" ", "").Replace("-", "").Trim();
            if (string.IsNullOrEmpty(cid))
            {
                formMain.btn_OKGetcid.Enabled = false;
                formMain.cbb_VersionGetcid.SelectedIndex = 0;
                if (function_name == "cbb_VersionGetcid_Click")
                    MessageBox.Show(MessagesResource.NoCID, MessagesResource.warning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (!validate.ValidateCID(cid))
            {
                formMain.btn_OKGetcid.Enabled = false;
                formMain.cbb_VersionGetcid.SelectedIndex = 0;
                MessageBox.Show(MessagesResource.CIDWrong, MessagesResource.warning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (optionCbx == setting.optionSelect)
                formMain.btn_OKGetcid.Enabled = false;
            else
            {
                formMain.btn_OKGetcid.Enabled = true;
                var optionCbxCID = sourceData.OptionCbxCID();
                for (int i = 0; i < optionCbxCID.Count; i++)
                {
                    if (optionCbxCID[i] == optionCbx)
                    {
                        string data = sourceData.ScriptCID(i, cid);
                        formMain.tbx_Log.Text = data;
                        Clipboard.SetText(data);
                    }
                }
            }
        }

        public void btn_OKGetcid_Click(object sender, EventArgs e)
        {
            formMain.tbx_Log.PasswordChar = '\u0000';
            string optionCbx = formMain.cbb_VersionGetcid.Text;
            string cid = formMain.tbx_CID.Text.Replace(" ", "").Replace("-", "").Trim();
            var optionCbxCID = sourceData.OptionCbxCID();
            for (int i = 0; i < optionCbxCID.Count; i++)
            {
                if (optionCbxCID[i] == optionCbx)
                {
                    string data = sourceData.ScriptCID(i, cid);
                    formMain.tbx_Log.Text = data;
                    Clipboard.SetText(data);
                }
            }
        }

        //Check/Remove License
        public void cbb_VersionCheckRemoveLincense_Click(object sender, EventArgs e)
        {
            formMain.tbx_Log.PasswordChar = '\u0000';
            string optionCbx = formMain.cbb_VersionCheckRemoveLincense.Text;
            if (optionCbx == "Check/Remove")
                formMain.btn_OKCheckRemoveLincense.Enabled = false;
            else
            {
                formMain.btn_OKCheckRemoveLincense.Enabled = true;
                var optionCbxCheckRemove = sourceData.OptionCbxCheckRemove();
                for (int i = 0; i < optionCbxCheckRemove.Count; i++)
                {
                    if (optionCbxCheckRemove[i] == optionCbx)
                    {
                        string data = sourceData.ScriptCheckRemove(i);
                        formMain.tbx_Log.Text = data;
                        Clipboard.SetText(data);
                    }
                }
            }
        }

        public void btn_OKCheckRemoveLincense_Click(object sender, EventArgs e)
        {
            formMain.tbx_Log.PasswordChar = '\u0000';
            string optionCbx = formMain.cbb_VersionCheckRemoveLincense.Text;
            if (optionCbx == "Check/Remove")
                MessageBox.Show(MessagesResource.NoSelectCommand, MessagesResource.warning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else
            {
                var optionCbxCheckRemove = sourceData.OptionCbxCheckRemove();
                for (int i = 0; i < optionCbxCheckRemove.Count; i++)
                {
                    if (optionCbxCheckRemove[i] == optionCbx)
                    {
                        string data = sourceData.ScriptCheckRemove(i);
                        formMain.tbx_Log.Text = data;
                        Clipboard.SetText(data);
                    }
                }
            }
        }

    }
}
