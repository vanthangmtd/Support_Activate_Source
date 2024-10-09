using FormWindows.SupportActivate;
using SupportActivate.Common;
using SupportActivate.ProcessBusiness;
using SupportActivate.ProcessSQL;
using System;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace SupportActivate.Setting
{
    public class ConfigSetting
    {
        FormSupportActivate formMain = FormSupportActivate.formMain;
        private log4net.ILog logger = log4net.LogManager.GetLogger(typeof(ConfigSetting));
        SourceData sourceData;
        ProcessTemp processTemp;
        ServerSetting serverSetting;
        public string optionSelect = "Windows/Office";

        public ConfigSetting()
        {
            sourceData = new SourceData();
            processTemp = new ProcessTemp();
            serverSetting = new ServerSetting();
        }

        [DllImport("user32")]
        public static extern UInt32 SendMessage(IntPtr hWnd, UInt32 msg, UInt32 wParam, UInt32 lParam);

        internal const int BCM_FIRST = 0x1600; //Normal button
        internal const int BCM_SETSHIELD = (BCM_FIRST + 0x000C); //Elevated button

        public void ConfigSettingControl()
        {
            int processcpu = Environment.ProcessorCount;
            foreach (var value in sourceData.OptionCbxWindowsOffice())
            {
                formMain.cbb_VersionActivate.Items.Add(value);
                formMain.cbb_VersionActivate.SelectedItem = optionSelect;
            }
            foreach (var value in sourceData.OptionCbxCID())
            {
                formMain.cbb_VersionGetcid.Items.Add(value);
                formMain.cbb_VersionGetcid.SelectedItem = optionSelect;
            }
            foreach (var value in sourceData.OptionCbxCheckRemove())
            {
                formMain.cbb_VersionCheckRemoveLincense.Items.Add(value);
                formMain.cbb_VersionCheckRemoveLincense.SelectedItem = "Check/Remove";
            }

            foreach (var value in sourceData.OptionCbxPidKeyAndTxtFilesArrayPidKey())
            {
                formMain.cbb_VersionPidKey.Items.Add(value.Key);
                formMain.cbb_VersionPidKey.SelectedItem = "All Edition Windows/Office";
            }

            for (int i = 1; i <= processcpu; i++)
            {
                formMain.cbb_MultyThread.Items.Add(i);
                formMain.cbb_MultyThread.SelectedItem = 1;
            }
            bool PIDKeyStatus = serverSetting.GetStatusPIDADV();
            bool LoadKeyInDataKeyStatus = serverSetting.GetStatusLOADKEY();
            bool CloseAppStatus = serverSetting.GetStatusCLOSEAPP();

            formMain.cb_EnablePidKeyAdv.Checked = PIDKeyStatus;
            formMain.btn_CheckAdv.Enabled = PIDKeyStatus;
            formMain.cb_LoadInDataKey.Checked = LoadKeyInDataKeyStatus;
            formMain.cb_QuestionClose.Checked = CloseAppStatus;

            if (PIDKeyStatus)
            {
                formMain.tbx_ValidKey.Text = MessagesResource.AdvancedCheckPidkey;
            }
            else
                formMain.tbx_ValidKey.Text = string.Empty;

            processTemp.CreateFileTemp();
            if (!formMain.IsAdministrator())
            {
                formMain.btn_CheckAdv.Enabled = false;
                formMain.btn_CheckAdv.FlatStyle = FlatStyle.System;
                SendMessage(formMain.btn_CheckAdv.Handle, BCM_SETSHIELD, 0, 0xFFFFFFFF);
            }
            else
            {
                processTemp.RestoreTemp();
                formMain.btn_CheckAdv.FlatStyle = FlatStyle.System;
                SendMessage(formMain.btn_CheckAdv.Handle, BCM_FIRST, 0, 0xFFFFFFFF);
            }

            string infoApp = "Support Activate software is developed by Tran Van Thang. " +
                "The software supports getting the CMD command to activate, " +
                "get the activation code, activate and remove the copyright " +
                "of all versions Windows and Office.\r\nCheck Key all versions of Windows and Office.\r\n" +
                "Final version " + formMain.version;
            formMain.tbx_InfoApp.Text = infoApp;

            //formMain.panel_TokenActivate.Hide();
            formMain.lb_TitleToken.Hide();
            formMain.tbx_TokenActivate.Hide();
            formMain.tbx_TokenActivate.Clear();
            formMain.cbb_VersionVL.Hide();
            formMain.cbb_VersionVL.Items.Clear();
            formMain.cb_SaveKey.Checked = true;
            formMain.lb_NotiUpdate.Text = "";

            Thread update = new Thread(updateOTA);
            update.TrySetApartmentState(ApartmentState.STA);
            update.Start();
        }

        public void updateOTA()
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
                string version = formMain.version;
                if (valueUpdateNew != version.ToString())
                {
                    formMain.lb_NotiUpdate.Invoke(new Action(() =>
                    {
                        formMain.lb_NotiUpdate.ForeColor = System.Drawing.Color.Red;
                        formMain.lb_NotiUpdate.Text = "Please update to a newer version of Support Activate!";
                        formMain.lb_CheckUpdate.ForeColor = System.Drawing.Color.Red;
                        formMain.lb_CheckUpdate.Text = "Please update to a newer version of Support Activate!";
                    }));
                }
                else
                    formMain.lb_NotiUpdate.Invoke(new Action(() =>
                    {
                        formMain.lb_NotiUpdate.ForeColor = System.Drawing.SystemColors.ControlText;
                        formMain.lb_NotiUpdate.Text = "";
                        formMain.lb_CheckUpdate.ForeColor = System.Drawing.SystemColors.ControlText;
                        formMain.lb_CheckUpdate.Text = "Status Update";
                    }));
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
        }
    }
}
