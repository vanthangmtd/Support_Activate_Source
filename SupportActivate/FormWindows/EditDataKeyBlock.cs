using SupportActivate.Common;
using SupportActivate.ProcessTabControl;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace SupportActivate.FormWindows
{
    public partial class EditDataKeyBlock : Form
    {
        string KeyOffice = "KEYOFFICE";
        string KeyWindows = "KEYWINDOWS";
        string KeyServer = "KEYSERVER";
        string KeyOther = "KEYOTHER";
        string key, fileName;
        int tongKey;
        string[] items;
        char[] vbCrLf = new char[] { '\r', '\n' };
        List<string> listKeyPIDKey = new List<string>();
        Validate validate;
        ProcessDataKeyCommon processDataKeyCommon;
        private log4net.ILog logger = log4net.LogManager.GetLogger(typeof(EditDataKeyBlock));

        public EditDataKeyBlock()
        {
            InitializeComponent();
        }

        private void EditDataKeyBlock_Load(object sender, EventArgs e)
        {
            lb_XuLy.Text = "0/0";
            tbx_KeyInput.Clear();
            listKeyPIDKey.Clear();
            validate = new Validate();
            processDataKeyCommon = new ProcessDataKeyCommon();
        }

        private void tbx_KeyInput_TextChanged(object sender, EventArgs e)
        {
            listKeyPIDKey.Clear();
            tongKey = 0;
            items = tbx_KeyInput.Text.ToUpper().Split(vbCrLf);
            try
            {
                for (int i = 0; i <= items.Length - 1; i++)
                {
                    string[] elems = items[i].Split(vbCrLf);
                    var key = validate.NhanDangKey(elems[0].Trim());
                    foreach (string Str in key)
                        listKeyPIDKey.Add(Str);
                }
                listKeyPIDKey = locData(listKeyPIDKey);
                tongKey = listKeyPIDKey.Count;
                lb_XuLy.Text = "0/" + tongKey;
                if (tongKey > 0)
                {
                    btn_AddKeyBlock.Enabled = true;
                    btn_DeleteKeyBlock.Enabled = true;
                }
                else
                {
                    tbx_KeyInput.Clear();
                    btn_AddKeyBlock.Enabled = false;
                    btn_DeleteKeyBlock.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                MessageBox.Show("Memory overflow", MessagesResource.error, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }// "Memory overflow"
        }

        private void tbx_KeyInput_Click(object sender, EventArgs e)
        {
            key = Clipboard.GetText().ToUpper();
            if (string.IsNullOrEmpty(key))
                MessageBox.Show("Please copy the key to the clipboard!", MessagesResource.warning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else
                tbx_KeyInput.Text = key;
        }

        private void btn_File_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                if (dialog.ShowDialog() != DialogResult.OK)
                    return;
                fileName = dialog.FileName;
            }
            Thread ReadFileSameData = new Thread(ReadFile);
            ReadFileSameData.Start();
        }

        private void btn_AddKeyBlock_Click(object sender, EventArgs e)
        {
            Thread KeyBlock = new Thread(addKeyBlock);
            KeyBlock.Start();
        }

        private void btn_DeleteKeyBlock_Click(object sender, EventArgs e)
        {
            Thread delKeyBlock = new Thread(deleteKeyBlock);
            delKeyBlock.Start();
        }

        private void addKeyBlock()
        {
            int i = 0;
            try
            {
                foreach (var values in listKeyPIDKey)
                {
                    processDataKeyCommon.blockKey(KeyOffice, values);
                    processDataKeyCommon.blockKey(KeyWindows, values);
                    processDataKeyCommon.blockKey(KeyServer, values);
                    processDataKeyCommon.blockKey(KeyOther, values);
                    i += 1;
                    lb_XuLy.Invoke(new Action(() =>
                    {
                        lb_XuLy.Text = i + "/" + tongKey;
                    }));
                }
                listKeyPIDKey.Clear();
                MessageBox.Show("Add this key to the blocked ones success", MessagesResource.success, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                MessageBox.Show("Add this key to the blocked ones error", MessagesResource.error, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void deleteKeyBlock()
        {
            int i = 0;
            try
            {
                foreach (var values in listKeyPIDKey)
                {
                    processDataKeyCommon.deleteKey(KeyOffice, values);
                    processDataKeyCommon.deleteKey(KeyWindows, values);
                    processDataKeyCommon.deleteKey(KeyServer, values);
                    processDataKeyCommon.deleteKey(KeyOther, values);
                    i += 1;
                    lb_XuLy.Invoke(new Action(() =>
                    {
                        lb_XuLy.Text = i + "/" + tongKey;
                    }));
                }
                listKeyPIDKey.Clear();
                MessageBox.Show("Delete this key from the blocked ones success", MessagesResource.success, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                MessageBox.Show("Delete this key from the blocked ones error", MessagesResource.error, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ReadFile()
        {
            listKeyPIDKey.Clear();
            string valueReadFile;
            FileStream fs;
            StreamReader reader;
            try
            {
                fs = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                reader = new StreamReader(fs);
                while (!reader.EndOfStream)
                {
                    valueReadFile = reader.ReadLine().Trim().ToUpper();
                    var Filekey = validate.NhanDangKey(valueReadFile);
                    foreach (string Str in Filekey)
                        listKeyPIDKey.Add(Str);
                }
                fs.Close();
                reader.Close();
                tongKey = listKeyPIDKey.Count;
                lb_XuLy.Invoke(new Action(() =>
                {
                    lb_XuLy.Text = "0/" + tongKey;
                }));
            }
            catch (IOException ex)
            {
                logger.Error(ex);
            }
        }

        public List<string> locData(List<string> list)
        {
            List<string> arrTemp = new List<string>();
            foreach (var value in list)
            {
                var str = validate.NhanDangKey(value);
                foreach (string data in str)
                {
                    if (arrTemp.Contains(data) == false)
                        arrTemp.Add(data);
                }
            }
            list = arrTemp;
            return list;
        }
    }
}
