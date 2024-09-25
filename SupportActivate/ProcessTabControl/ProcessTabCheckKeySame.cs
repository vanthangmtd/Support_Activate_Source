using FormWindows.SupportActivate;
using SupportActivate.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace SupportActivate.ProcessTabControl
{
    public class ProcessTabCheckKeySame
    {
        FormSupportActivate formMain = FormSupportActivate.formMain;
        Validate validate;
        private log4net.ILog logger = log4net.LogManager.GetLogger(typeof(ProcessTabCheckKeySame));

        char[] vbCrLf = new char[] { '\r', '\n' };

        public ProcessTabCheckKeySame()
        {
            validate = new Validate();
        }

        private void KeySame(List<string> listKey1, List<string> listKey2)
        {
            List<string> listKeySame = new List<string>();
            foreach (var value in listKey1)
            {
                bool check = listKey2.Contains(value);
                if (check == true)
                {
                    formMain.tbx_KeyOutput1.Invoke(new Action(() =>
                    {
                        formMain.tbx_KeyOutput1.AppendText(value.Trim() + "\r\n");
                    }));
                    listKeySame.Add(value.Trim());
                }
                else
                    formMain.tbx_KeyOutput2.Invoke(new Action(() =>
                    {
                        formMain.tbx_KeyOutput2.AppendText(value.Trim() + "\r\n");
                    }));
            }
            foreach (var value in listKey2)
            {
                bool check = listKeySame.Contains(value);
                if (check == false)
                {
                    formMain.tbx_KeyOutput3.Invoke(new Action(() =>
                    {
                        formMain.tbx_KeyOutput3.AppendText(value.Trim() + "\r\n");
                    }));
                }
            }
        }

        private void CheckKeySame_Diffent()
        {
            List<string> listKey1 = new List<string>();
            List<string> listKey2 = new List<string>();
            string[] items;
            int i;
            items = formMain.tbx_KeyInput1.Text.ToUpper().Split(vbCrLf);
            for (i = 0; i <= items.Length - 1; i++)
            {
                string[] elems = items[i].Split(vbCrLf);
                listKey1.Add(elems[0].Trim());
            }
            listKey1 = validate.locData(listKey1);
            items = formMain.tbx_KeyInput2.Text.ToUpper().Split(vbCrLf);
            for (i = 0; i <= items.Length - 1; i++)
            {
                string[] elems = items[i].Split(vbCrLf);
                listKey2.Add(elems[0].Trim());
            }
            listKey2 = validate.locData(listKey2);
            KeySame(listKey1, listKey2);
            listKey1.Clear();
            listKey2.Clear();
            formMain.tbx_KeyOutput1.Invoke(new Action(() =>
            {
                formMain.btn_CheckFilter.Enabled = true;
                formMain.btn_RefreshFilter.Enabled = true;
                formMain.btn_FileFilter.Enabled = true;
                formMain.btn_SaveFilter.Enabled = true;
                formMain.btn_CleanKeyInput1.Enabled = true;
                formMain.btn_CleanKeyInput2.Enabled = true;
                formMain.btn_CleanKeyOutput1.Enabled = true;
                formMain.btn_CleanKeyOutput2.Enabled = true;
                formMain.btn_CleanKeyOutput3.Enabled = true;
            }));
        }

        public void btn_CheckFilter_Click(object sender, EventArgs e)
        {
            formMain.tbx_KeyOutput1.Text = "";
            formMain.tbx_KeyOutput2.Text = "";
            formMain.tbx_KeyOutput3.Text = "";
            Thread CheckKeySame = new Thread(CheckKeySame_Diffent);
            CheckKeySame.Start();
            formMain.btn_CheckFilter.Enabled = false;
            formMain.btn_RefreshFilter.Enabled = false;
            formMain.btn_FileFilter.Enabled = false;
            formMain.btn_SaveFilter.Enabled = false;
            formMain.btn_CleanKeyInput1.Enabled = false;
            formMain.btn_CleanKeyInput2.Enabled = false;
            formMain.btn_CleanKeyOutput1.Enabled = false;
            formMain.btn_CleanKeyOutput2.Enabled = false;
            formMain.btn_CleanKeyOutput3.Enabled = false;
        }

        private void ReadFileSame(string fileNameSame)
        {
            string valueReadFile;
            FileStream fs = null;
            StreamReader reader = null;
            try
            {
                fs = new FileStream(fileNameSame, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                reader = new StreamReader(fs);
                string keyInput1 = string.Empty;
                formMain.tbx_KeyInput1.Invoke(new Action(() =>
                {
                    keyInput1 = formMain.tbx_KeyInput1.Text;
                }));
                if (string.IsNullOrEmpty(keyInput1))
                {
                    while (!reader.EndOfStream)
                    {
                        valueReadFile = reader.ReadLine().Trim().ToUpper();
                        var Filekey = validate.NhanDangKey(valueReadFile);
                        foreach (string Str in Filekey)
                        {
                            formMain.tbx_KeyInput1.Invoke(new Action(() =>
                            {
                                formMain.tbx_KeyInput1.AppendText(Str + "\r\n");
                            }));
                        }
                    }
                }
                else
                    while (!reader.EndOfStream)
                    {
                        valueReadFile = reader.ReadLine().Trim().ToUpper();
                        var Filekey = validate.NhanDangKey(valueReadFile);
                        foreach (string Str in Filekey)
                        {
                            formMain.tbx_KeyInput2.Invoke(new Action(() =>
                            {
                                formMain.tbx_KeyInput2.AppendText(Str + "\r\n");
                            }));
                        }
                    }
            }
            catch (IOException ex)
            {
                logger.Error(ex);
            }
            finally
            {
                fs.Close();
                reader.Close();
                fileNameSame = "";
                formMain.btn_CheckFilter.Invoke(new Action(() =>
                {
                    formMain.btn_CheckFilter.Enabled = true;
                    formMain.btn_RefreshFilter.Enabled = true;
                    formMain.btn_FileFilter.Enabled = true;
                    formMain.btn_SaveFilter.Enabled = true;
                    formMain.btn_CleanKeyInput1.Enabled = true;
                    formMain.btn_CleanKeyInput2.Enabled = true;
                    formMain.btn_CleanKeyOutput1.Enabled = true;
                    formMain.btn_CleanKeyOutput2.Enabled = true;
                    formMain.btn_CleanKeyOutput3.Enabled = true;
                }));
            }
        }

        public void btn_FileFilter_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(formMain.tbx_KeyInput1.Text) && !string.IsNullOrEmpty(formMain.tbx_KeyInput2.Text))
                MessageBox.Show(MessagesResource.DeleteSourceKey1Or2, MessagesResource.warning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else
            {
                string fileNameSame = string.Empty;
                using (OpenFileDialog dialog = new OpenFileDialog())
                {
                    if (dialog.ShowDialog() != DialogResult.OK)
                        return;
                    fileNameSame = dialog.FileName;
                }
                new Thread(() =>
                {
                    ReadFileSame(fileNameSame);
                })
                { IsBackground = true }.Start();
                formMain.btn_CheckFilter.Enabled = false;
                formMain.btn_RefreshFilter.Enabled = false;
                formMain.btn_FileFilter.Enabled = false;
                formMain.btn_SaveFilter.Enabled = false;
                formMain.btn_CleanKeyInput1.Enabled = false;
                formMain.btn_CleanKeyInput2.Enabled = false;
                formMain.btn_CleanKeyOutput1.Enabled = false;
                formMain.btn_CleanKeyOutput2.Enabled = false;
                formMain.btn_CleanKeyOutput3.Enabled = false;
            }
        }

        public void btn_SaveFilter_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(formMain.tbx_KeyOutput1.Text.Trim().Replace(" ", "")) && string.IsNullOrEmpty(formMain.tbx_KeyOutput2.Text.Trim().Replace(" ", "")) && string.IsNullOrEmpty(formMain.tbx_KeyOutput3.Text.Trim().Replace(" ", "")))
                MessageBox.Show(MessagesResource.NotSoft, MessagesResource.warning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else
            {
                string data = "Key Same\r\n\r\n" + formMain.tbx_KeyOutput1.Text + "\r\nKey Different 1\r\n\r\n" + formMain.tbx_KeyOutput2.Text + "\r\nKey Different 2\r\n\r\n" + formMain.tbx_KeyOutput3.Text;
                using (SaveFileDialog SaveFileDialog1 = new SaveFileDialog())
                {
                    SaveFileDialog1.Filter = "TXT Files (*.txt)|*.txt";
                    if (SaveFileDialog1.ShowDialog() == DialogResult.OK)
                        using (StreamWriter sw = File.AppendText(SaveFileDialog1.FileName))
                        {
                            sw.WriteLine(data);
                            MessageBox.Show(MessagesResource.SaveSuccess, MessagesResource.success, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                }
            }
        }

        public void btn_RefreshFilter_Click(object sender, EventArgs e)
        {
            formMain.tbx_KeyInput1.Text = "";
            formMain.tbx_KeyInput2.Text = "";
            formMain.tbx_KeyOutput1.Text = "";
            formMain.tbx_KeyOutput2.Text = "";
            formMain.tbx_KeyOutput3.Text = "";
        }
    }
}
