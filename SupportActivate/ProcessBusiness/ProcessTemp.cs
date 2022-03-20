using FormWindows.SupportActivate;
using System;
using System.IO;

namespace SupportActivate.ProcessBusiness
{
    public class ProcessTemp
    {
        private static string pathTemp = Path.GetTempPath() + @"Support_Activate";
        private string filePathtbxKey = pathTemp + @"\tbxKey.txt";
        private string filePathtbxIID = pathTemp + @"\tbxIID.txt";
        private string filePathtextBoxCID = pathTemp + @"\cid.txt";
        private string filePathlog = pathTemp + @"\log.txt";
        private string filePathtbxKeyInput = pathTemp + @"\tbxKeyInput.txt";
        private string filePathtbxKeyVaild = pathTemp + @"\tbxKeyVaild.txt";
        private string filePathtbxKeyInvaild = pathTemp + @"\tbxKeyInvaild.txt";
        private string filePathtbxSourceKey1 = pathTemp + @"\tbxSourceKey1.txt";
        private string filePathtbxSourceKey2 = pathTemp + @"\tbxSourceKey2.txt";
        private string filePathtbxSourceKeySame = pathTemp + @"\tbxSourceKeySame.txt";
        private string filePathtbxSourceKeyDiffent1 = pathTemp + @"\tbxSourceKeyDiffent1.txt";
        private string filePathtbxSourceKeyDiffent2 = pathTemp + @"\tbxSourceKeyDiffent2.txt";
        private log4net.ILog logger = log4net.LogManager.GetLogger(typeof(ProcessTemp).ToString());
        FormSupportActivate formMain = FormSupportActivate.formMain;

        public void CreateFileTemp()
        {
            try
            {
                StreamWriter write;
                if (!Directory.Exists(pathTemp))
                    Directory.CreateDirectory(pathTemp);
                if (!string.IsNullOrEmpty(formMain.tbx_Key.Text))
                {
                    if (!File.Exists(filePathtbxKey))
                        File.Create(filePathtbxKey).Dispose();
                    write = new StreamWriter(filePathtbxKey);
                    write.Write(formMain.tbx_Key.Text);
                    write.Close();
                }
                if (!string.IsNullOrEmpty(formMain.tbx_IID.Text))
                {
                    if (!File.Exists(filePathtbxIID))
                        File.Create(filePathtbxIID).Dispose();
                    write = new StreamWriter(filePathtbxIID);
                    write.Write(formMain.tbx_IID.Text);
                    write.Close();
                }
                if (!string.IsNullOrEmpty(formMain.tbx_CID.Text))
                {
                    if (!File.Exists(filePathtextBoxCID))
                        File.Create(filePathtextBoxCID).Dispose();
                    write = new StreamWriter(filePathtextBoxCID);
                    write.Write(formMain.tbx_CID.Text);
                    write.Close();
                }
                if (!string.IsNullOrEmpty(formMain.tbx_Log.Text))
                {
                    if (!File.Exists(filePathlog))
                        File.Create(filePathlog).Dispose();
                    write = new StreamWriter(filePathlog);
                    write.WriteLine(formMain.tbx_Log.Text);
                    write.Close();
                }
                if (!string.IsNullOrEmpty(formMain.tbx_PidKeyInput.Text))
                {
                    if (!File.Exists(filePathtbxKeyInput))
                        File.Create(filePathtbxKeyInput).Dispose();
                    write = new StreamWriter(filePathtbxKeyInput);
                    write.WriteLine(formMain.tbx_PidKeyInput.Text);
                    write.Close();
                }
                if (!string.IsNullOrEmpty(formMain.tbx_ValidKey.Text))
                {
                    if (!File.Exists(filePathtbxKeyVaild))
                        File.Create(filePathtbxKeyVaild).Dispose();
                    write = new StreamWriter(filePathtbxKeyVaild);
                    write.WriteLine(formMain.tbx_ValidKey.Text);
                    write.Close();
                }
                if (!string.IsNullOrEmpty(formMain.tbx_InvalidKey.Text))
                {
                    if (!File.Exists(filePathtbxKeyInvaild))
                        File.Create(filePathtbxKeyInvaild).Dispose();
                    write = new StreamWriter(filePathtbxKeyInvaild);
                    write.WriteLine(formMain.tbx_InvalidKey.Text);
                    write.Close();
                }
                if (!string.IsNullOrEmpty(formMain.tbx_KeyInput1.Text))
                {
                    if (!File.Exists(filePathtbxSourceKey1))
                        File.Create(filePathtbxSourceKey1).Dispose();
                    write = new StreamWriter(filePathtbxSourceKey1);
                    write.WriteLine(formMain.tbx_KeyInput1.Text);
                    write.Close();
                }
                if (!string.IsNullOrEmpty(formMain.tbx_KeyInput2.Text))
                {
                    if (!File.Exists(filePathtbxSourceKey2))
                        File.Create(filePathtbxSourceKey2).Dispose();
                    write = new StreamWriter(filePathtbxSourceKey2);
                    write.WriteLine(formMain.tbx_KeyInput2.Text);
                    write.Close();
                }
                if (!string.IsNullOrEmpty(formMain.tbx_KeyOutput1.Text))
                {
                    if (!File.Exists(filePathtbxSourceKeySame))
                        File.Create(filePathtbxSourceKeySame).Dispose();
                    write = new StreamWriter(filePathtbxSourceKeySame);
                    write.WriteLine(formMain.tbx_KeyOutput1.Text);
                    write.Close();
                }
                if (!string.IsNullOrEmpty(formMain.tbx_KeyOutput2.Text))
                {
                    if (!File.Exists(filePathtbxSourceKeyDiffent1))
                        File.Create(filePathtbxSourceKeyDiffent1).Dispose();
                    write = new StreamWriter(filePathtbxSourceKeyDiffent1);
                    write.WriteLine(formMain.tbx_KeyOutput2.Text);
                    write.Close();
                }
                if (!string.IsNullOrEmpty(formMain.tbx_KeyOutput3.Text))
                {
                    if (!File.Exists(filePathtbxSourceKeyDiffent2))
                        File.Create(filePathtbxSourceKeyDiffent2).Dispose();
                    write = new StreamWriter(filePathtbxSourceKeyDiffent2);
                    write.WriteLine(formMain.tbx_KeyOutput3.Text);
                    write.Close();
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
        }

        public void RestoreTemp()
        {
            try
            {
                FileStream fs;
                StreamReader reader;
                if (File.Exists(filePathtbxKeyInput))
                {
                    fs = new FileStream(filePathtbxKeyInput, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                    reader = new StreamReader(fs);
                    string valueReadFile;
                    while (!reader.EndOfStream)
                    {
                        valueReadFile = reader.ReadLine();
                        formMain.tbx_PidKeyInput.AppendText(valueReadFile + "\r\n");
                    }
                    fs.Close();
                    reader.Close();
                }
                if (File.Exists(filePathtbxKey))
                {
                    fs = new FileStream(filePathtbxKey, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                    reader = new StreamReader(fs);
                    string valueReadFile;
                    while (!reader.EndOfStream)
                    {
                        valueReadFile = reader.ReadLine();
                        formMain.tbx_Key.AppendText(valueReadFile);
                    }
                    fs.Close();
                    reader.Close();
                }
                if (File.Exists(filePathtbxIID))
                {
                    fs = new FileStream(filePathtbxIID, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                    reader = new StreamReader(fs);
                    string valueReadFile;
                    while (!reader.EndOfStream)
                    {
                        valueReadFile = reader.ReadLine();
                        formMain.tbx_IID.AppendText(valueReadFile);
                    }
                    fs.Close();
                    reader.Close();
                }
                if (File.Exists(filePathtextBoxCID))
                {
                    fs = new FileStream(filePathtextBoxCID, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                    reader = new StreamReader(fs);
                    string valueReadFile;
                    while (!reader.EndOfStream)
                    {
                        valueReadFile = reader.ReadLine();
                        formMain.tbx_CID.AppendText(valueReadFile);
                    }
                    fs.Close();
                    reader.Close();
                }
                if (File.Exists(filePathlog))
                {
                    fs = new FileStream(filePathlog, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                    reader = new StreamReader(fs);
                    string valueReadFile;
                    while (!reader.EndOfStream)
                    {
                        valueReadFile = reader.ReadLine();
                        formMain.tbx_Log.AppendText(valueReadFile + "\r\n");
                    }
                    fs.Close();
                    reader.Close();
                }

                if (File.Exists(filePathtbxKeyVaild))
                {
                    fs = new FileStream(filePathtbxKeyVaild, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                    reader = new StreamReader(fs);
                    string valueReadFile;
                    while (!reader.EndOfStream)
                    {
                        valueReadFile = reader.ReadLine();
                        formMain.tbx_ValidKey.Text = valueReadFile + "\r\n";
                    }
                    fs.Close();
                    reader.Close();
                }
                if (File.Exists(filePathtbxKeyInvaild))
                {
                    fs = new FileStream(filePathtbxKeyInvaild, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                    reader = new StreamReader(fs);
                    string valueReadFile;
                    while (!reader.EndOfStream)
                    {
                        valueReadFile = reader.ReadLine();
                        formMain.tbx_InvalidKey.AppendText(valueReadFile + "\r\n");
                    }
                    fs.Close();
                    reader.Close();
                }
                if (File.Exists(filePathtbxSourceKey1))
                {
                    fs = new FileStream(filePathtbxSourceKey1, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                    reader = new StreamReader(fs);
                    string valueReadFile;
                    while (!reader.EndOfStream)
                    {
                        valueReadFile = reader.ReadLine();
                        formMain.tbx_KeyInput1.AppendText(valueReadFile + "\r\n");
                    }
                    fs.Close();
                    reader.Close();
                }
                if (File.Exists(filePathtbxSourceKey2))
                {
                    fs = new FileStream(filePathtbxSourceKey2, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                    reader = new StreamReader(fs);
                    string valueReadFile;
                    while (!reader.EndOfStream)
                    {
                        valueReadFile = reader.ReadLine();
                        formMain.tbx_KeyInput2.AppendText(valueReadFile + "\r\n");
                    }
                    fs.Close();
                    reader.Close();
                }
                if (File.Exists(filePathtbxSourceKeySame))
                {
                    fs = new FileStream(filePathtbxSourceKeySame, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                    reader = new StreamReader(fs);
                    string valueReadFile;
                    while (!reader.EndOfStream)
                    {
                        valueReadFile = reader.ReadLine();
                        formMain.tbx_KeyOutput1.AppendText(valueReadFile + "\r\n");
                    }
                    fs.Close();
                    reader.Close();
                }
                if (File.Exists(filePathtbxSourceKeyDiffent1))
                {
                    fs = new FileStream(filePathtbxSourceKeyDiffent1, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                    reader = new StreamReader(fs);
                    string valueReadFile;
                    while (!reader.EndOfStream)
                    {
                        valueReadFile = reader.ReadLine();
                        formMain.tbx_KeyOutput2.AppendText(valueReadFile + "\r\n");
                    }
                    fs.Close();
                    reader.Close();
                }
                if (File.Exists(filePathtbxSourceKeyDiffent2))
                {
                    fs = new FileStream(filePathtbxSourceKeyDiffent2, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                    reader = new StreamReader(fs);
                    string valueReadFile;
                    while (!reader.EndOfStream)
                    {
                        valueReadFile = reader.ReadLine();
                        formMain.tbx_KeyOutput3.AppendText(valueReadFile + "\r\n");
                    }
                    fs.Close();
                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
        }

        public void DeleteFile()
        {
            if (Directory.Exists(pathTemp))
            {
                string directoryName = pathTemp;
                foreach (var deleteAllFile in Directory.GetFiles(directoryName, "*.*", SearchOption.TopDirectoryOnly))
                    File.Delete(deleteAllFile);
            }
        }
    }
}
