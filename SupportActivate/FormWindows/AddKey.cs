using SupportActivate.Common;
using SupportActivate.ProcessSQL;
using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace SupportActivate.FormWindows
{
    public partial class AddKey : Form
    {
        Validate validate;
        ServerKey serverKey;
        public AddKey()
        {
            InitializeComponent();
        }

        private void AddKey_Load(object sender, EventArgs e)
        {
            validate = new Validate();
            serverKey = new ServerKey();
            cbb_Getweb.SelectedItem = "Select Option";
        }

        private void btnAddKey_Click(object sender, EventArgs e)
        {
            string key = tbx_Key.Text;
            if (key.Length == 29)
            {
                string keyVaild = string.Empty;
                foreach (var values in validate.NhanDangKey(key))
                    keyVaild = values;
                var getWeb = cbb_Getweb.Text;
                string Note = string.IsNullOrEmpty(tbx_Note.Text) ? string.Empty : tbx_Note.Text;

                pid pid = new pid();
                pid.Key = keyVaild;
                pid.Description = string.IsNullOrEmpty(tbx_Description.Text) ? string.Empty : tbx_Description.Text;
                pid.SubType = string.IsNullOrEmpty(tbx_SubType.Text) ? string.Empty : tbx_SubType.Text;
                pid.LicenseType = string.IsNullOrEmpty(tbx_LicenseType.Text) ? string.Empty : tbx_LicenseType.Text;
                var count = -1;
                pid.MAKCount = int.TryParse(Regex.Replace(tbx_MAKCount.Text, @"\D", ""), out count) ? count : -1;
                pid.ErrorCode = string.IsNullOrEmpty(tbx_ErrorCode.Text) ? string.Empty : tbx_ErrorCode.Text;
                pid.KeyGetWeb = getWeb == "Select Option" ? string.Empty : getWeb;

                serverKey.CreateDataKey(true, pid, Note);
                MessageBox.Show("Add product key success.\r\nPlease refresh the Data key.");
                ActiveForm.Close();
            }
            else
                MessageBox.Show("Textbox Key cannot be empty.");
        }
    }
}
