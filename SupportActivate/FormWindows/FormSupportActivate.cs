using Microsoft.Win32;
using SupportActivate.Common;
using SupportActivate.FormWindows;
using SupportActivate.ProcessBusiness;
using SupportActivate.ProcessSQL;
using SupportActivate.ProcessTabControl;
using SupportActivate.Setting;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Security.Principal;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace FormWindows.SupportActivate
{
    public partial class FormSupportActivate : Form
    {
        public static FormSupportActivate formMain;
        ServerKey serverKey;
        ServerSetting serverSetting;
        ProcessTabSupportActivate processTabSupportActivate;
        ProcessTabPIDKEY processTabPIDKEY;
        ProcessTabCheckKeySame processTabCheckKeySame;
        ProcessTabSetting processTabSetting;
        ProcessTemp processTemp;
        ConfigSetting setting;
        public string version = "3.6.1";
        public string typeApp = " x64";
        private log4net.ILog logger = log4net.LogManager.GetLogger(typeof(FormSupportActivate));

        public FormSupportActivate()
        {
            InitializeComponent();
            formMain = this;
            serverKey = new ServerKey();
            serverSetting = new ServerSetting();
            processTabSupportActivate = new ProcessTabSupportActivate();
            processTabPIDKEY = new ProcessTabPIDKEY();
            processTabCheckKeySame = new ProcessTabCheckKeySame();
            processTabSetting = new ProcessTabSetting();
            processTemp = new ProcessTemp();
            setting = new ConfigSetting();
        }

        private void SupportActivate_Load(object sender, EventArgs e)
        {
            serverKey.createDataBase();
            serverSetting.CreateDataBase();

            Clipboard.GetText();
            Process currentProcess = Process.GetCurrentProcess();
            currentProcess.PriorityClass = ProcessPriorityClass.BelowNormal;

            InfoAppAndCheckStatus();
            setting.ConfigSettingControl();
        }

        private void InfoAppAndCheckStatus()
        {
            this.Name = "Support Activate v." + version + typeApp;
            this.Text = "Support Activate v." + version + typeApp;
            lb_OperatingSystem.Text = (string)Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion").GetValue("ProductName"); ;
        }

        private void tabAction_Resize(object sender, EventArgs e)
        {
            Control control = (Control)sender;
            panelInputPidKey.MaximumSize = new Size(control.Size.Width / 2, control.Size.Height);
            panelInputPidKey.Size = new Size(control.Size.Width / 2, control.Size.Height);

            //panelOutputInvalidKey.MaximumSize = new Size(control.Size.Width / 2 - 10, control.Size.Height / 3);
            panelOutputInvalidKey.Size = new Size(control.Size.Width / 2 - 10, control.Size.Height / 3);

            panelFilterKey1.MaximumSize = new Size(control.Size.Width / 2, control.Size.Height);
            panelFilterKey1.Size = new Size(control.Size.Width / 2, control.Size.Height);

            //panelFilterKeyInput1.MaximumSize = new Size(control.Size.Width / 2, control.Size.Height / 2);
            panelFilterKeyInput1.Size = new Size(control.Size.Width / 2, control.Size.Height / 2);
            //panelFilterKeyInput2.MaximumSize = new Size(control.Size.Width / 2, control.Size.Height / 2-3);
            panelFilterKeyInput2.Size = new Size(control.Size.Width / 2, control.Size.Height / 2 - 3);

            //panelFilterKeyOutput3.MaximumSize = new Size(control.Size.Width / 2 - 10, control.Size.Height / 3 - 10);
            panelFilterKeyOutput3.Size = new Size(control.Size.Width / 2 - 10, control.Size.Height / 3 - 10);
            //panelFilterKeyOutput2.MaximumSize = new Size(control.Size.Width / 2 - 10, control.Size.Height / 3 - 5);
            panelFilterKeyOutput2.Size = new Size(control.Size.Width / 2 - 10, control.Size.Height / 3 - 5);
        }

        public bool IsAdministrator()
        {
            var identity = WindowsIdentity.GetCurrent();
            var principal = new WindowsPrincipal(identity);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }

        private void FormSupportActivate_FormClosing(object sender, FormClosingEventArgs e)
        {
            bool closeApp = serverSetting.GetStatusCLOSEAPP();
            if (closeApp)
            {
                var ask = MessageBox.Show("Do you want to exit the application?", Messages.success, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (ask == DialogResult.Yes)
                {
                    processTemp.DeleteFile();
                    //Application.Exit();
                    Environment.Exit(Environment.ExitCode);
                }
                else
                    e.Cancel = true;
            }
            else
            {
                processTemp.DeleteFile();
                //Application.Exit();
                Environment.Exit(Environment.ExitCode);
            }
        }

        //ProcessTabSupportActivate Start
        //Script Activate Windows/Office
        private void tbx_Key_Click(object sender, EventArgs e)
            => processTabSupportActivate.tbx_Key_Click(sender, e);

        private void tbx_Key_TextChanged(object sender, EventArgs e)
            => processTabSupportActivate.tbx_Key_TextChanged(sender, e);

        private void cb_Decode_CheckedChanged(object sender, EventArgs e)
            => processTabSupportActivate.cb_Decode_CheckedChanged(sender, e);

        private void cbb_VersionActivate_Click(object sender, EventArgs e)
        => processTabSupportActivate.cbb_VersionActivate_Click(sender, e);

        private void cbb_VersionActivate_SelectedIndexChanged(object sender, EventArgs e)
            => processTabSupportActivate.cbb_VersionActivate_SelectedIndexChanged(sender, e);

        private void btn_OKActivate_Click(object sender, EventArgs e)
            => processTabSupportActivate.btn_OKActivate_Click(sender, e);


        //Get ConfirmationID
        private void tbx_IID_Click(object sender, EventArgs e)
        {
            tbx_IID.Text = Clipboard.GetText();
        }

        private void tbx_IID_TextChanged(object sender, EventArgs e)
            => processTabSupportActivate.tbx_IID_TextChanged(sender, e);

        private void tbx_TokenGetcid_Click(object sender, EventArgs e)
        {
            string iid = Regex.Replace(Clipboard.GetText().Replace(" ", "").Trim(), @"\D", "");
            if (iid.Length >= 54)
                tbx_TokenGetcid.Text = iid;
        }

        private void tbx_TokenGetcid_TextChanged(object sender, EventArgs e)
            => processTabSupportActivate.tbx_TokenGetcid_TextChanged(sender, e);

        private void btn_Getcid_Click(object sender, EventArgs e)
            => processTabSupportActivate.btn_Getcid_Click(sender, e);

        private void timerGetcid_Tick(object sender, EventArgs e)
            => processTabSupportActivate.timerGetcid_Tick(sender, e);

        private void tbx_CID_Click(object sender, EventArgs e)
        {
            if (tbx_CID.Text.Length == 0)
                tbx_CID.Text = Clipboard.GetText().Replace(" ", "").Trim();
        }

        private void tbx_CID_TextChanged(object sender, EventArgs e)
            => processTabSupportActivate.tbx_CID_TextChanged(sender, e);

        private void cb_EnableCID_CheckedChanged(object sender, EventArgs e)
            => tbx_CID.Enabled = cb_EnableCID.Checked;

        private void btn_CopyCID_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tbx_CID.Text))
                MessageBox.Show(Messages.CIDWrong, Messages.error, MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
                Clipboard.SetText(tbx_CID.Text);
        }

        private void cbb_VersionGetcid_Click(object sender, EventArgs e)
            => processTabSupportActivate.cbb_VersionGetcid_Click(sender, e);

        private void cbb_VersionGetcid_SelectedIndexChanged(object sender, EventArgs e)
            => processTabSupportActivate.cbb_VersionGetcid_SelectedIndexChanged(sender, e);

        private void btn_OKGetcid_Click(object sender, EventArgs e)
            => processTabSupportActivate.btn_OKGetcid_Click(sender, e);


        //Check/Remove License
        private void cbb_VersionCheckRemoveLincense_Click(object sender, EventArgs e)
            => processTabSupportActivate.cbb_VersionCheckRemoveLincense_Click(sender, e);

        private void cbb_VersionCheckRemoveLincense_SelectedIndexChanged(object sender, EventArgs e)
            => processTabSupportActivate.cbb_VersionCheckRemoveLincense_Click(sender, e);

        private void btn_OKCheckRemoveLincense_Click(object sender, EventArgs e)
            => processTabSupportActivate.btn_OKCheckRemoveLincense_Click(sender, e);
        //ProcessTabSupportActivate End

        //processTabPIDKEY Start
        private void tbx_PidKeyInput_TextChanged(object sender, EventArgs e)
        => processTabPIDKEY.tbx_PidKeyInput_TextChanged(sender, e);

        private void btn_CheckKey_Click(object sender, EventArgs e)
        => processTabPIDKEY.btn_CheckKey_Click(sender, e);
        private void btn_CheckAdv_Click(object sender, EventArgs e)
        => processTabPIDKEY.btn_CheckAdv_Click(sender, e);

        private void timerPidKey_Tick(object sender, EventArgs e)
        => processTabPIDKEY.timerPidKey_Tick(sender, e);

        private void btn_FileAndCleanPidKey_Click(object sender, EventArgs e)
        => processTabPIDKEY.btn_FileAndCleanPidKey_Click(sender, e);

        private void btn_CopyValidKey_Click(object sender, EventArgs e)
        => processTabPIDKEY.btn_CopyValidKey_Click(sender, e);

        private void btn_CopyInvalidKey_Click(object sender, EventArgs e)
        => processTabPIDKEY.btn_CopyInvalidKey_Click(sender, e);

        private void btn_SoftKey_Click(object sender, EventArgs e)
        => processTabPIDKEY.btn_SoftKey_Click(sender, e);

        private void btn_DataKey_Click(object sender, EventArgs e)
        {
            DataKey dataKey = new DataKey();
            dataKey.Show();
            dataKey.Focus();
        }

        //processTabPIDKEY End

        //processTabCheckKeySame Start
        private void btn_CheckFilter_Click(object sender, EventArgs e)
        => processTabCheckKeySame.btn_CheckFilter_Click(sender, e);

        private void btn_FileFilter_Click(object sender, EventArgs e)
        => processTabCheckKeySame.btn_FileFilter_Click(sender, e);

        private void btn_SaveFilter_Click(object sender, EventArgs e)
        => processTabCheckKeySame.btn_SaveFilter_Click(sender, e);

        private void btn_RefreshFilter_Click(object sender, EventArgs e)
        => processTabCheckKeySame.btn_RefreshFilter_Click(sender, e);

        private void btn_CleanKeyInput1_Click(object sender, EventArgs e)
        => tbx_KeyInput1.Clear();

        private void btn_CleanKeyInput2_Click(object sender, EventArgs e)
        => tbx_KeyInput2.Clear();

        private void btn_CleanKeyOutput1_Click(object sender, EventArgs e)
        => tbx_KeyOutput1.Clear();

        private void btn_CleanKeyOutput2_Click(object sender, EventArgs e)
        => tbx_KeyOutput2.Clear();

        private void btn_CleanKeyOutput3_Click(object sender, EventArgs e)
        => tbx_KeyOutput3.Clear();

        //processTabCheckKeySame End

        //processTabSetting Start
        private void cb_EnablePidKeyAdv_CheckedChanged(object sender, EventArgs e)
        => processTabSetting.cb_EnablePidKeyAdv_CheckedChanged(sender, e);

        private void cb_EnableMultiThread_CheckedChanged(object sender, EventArgs e)
        => processTabSetting.cb_EnableMultiThread_CheckedChanged(sender, e);

        private void btnSaveSetting_Click(object sender, EventArgs e)
        => processTabSetting.btnSaveSetting_Click(sender, e);

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://docs.google.com/forms/d/160fyLjBfqZxERR-z6icmBeHbU-C63-wqTpi3l4Y5HdI/edit?usp=sharing");
        }

        private void btn_CheckUpdate_Click(object sender, EventArgs e)
        => processTabSetting.btn_CheckUpdate_Click(sender, e);

        //processTabSetting End

    }
}
