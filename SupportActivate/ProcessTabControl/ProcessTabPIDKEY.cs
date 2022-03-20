using FormWindows.SupportActivate;
using Microsoft.Win32;
using SupportActivate.Common;
using SupportActivate.ProcessBusiness;
using SupportActivate.ProcessSQL;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SupportActivate.ProcessTabControl
{
    public class ProcessTabPIDKEY
    {
        FormSupportActivate formMain = FormSupportActivate.formMain;
        ProcessPidkey processPidkey;
        ProcessTemp processTemp;
        SourceData sourceData;
        Validate validate;
        ServerSetting serverSetting;
        private log4net.ILog logger = log4net.LogManager.GetLogger(typeof(ProcessTabPIDKEY));
        List<string> listKeyPIDKey = new List<string>();
        List<pid> listKeyPIDKeyInputSoftPIDKey = new List<pid>();
        private string[] items;
        private bool cancelPIDKey;
        private string fileNamePidKey;
        DateTime dateTimePIDKEY;

        public ProcessTabPIDKEY()
        {
            processPidkey = new ProcessPidkey();
            processTemp = new ProcessTemp();
            sourceData = new SourceData();
            validate = new Validate();
            serverSetting = new ServerSetting();
        }

        public void tbx_PidKeyInput_TextChanged(object sender, EventArgs e)
        {
            listKeyPIDKey.Clear();
            formMain.btn_SoftKey.Enabled = false;
            StringBuilder dataKey = new StringBuilder();
            dataKey.Append(formMain.tbx_PidKeyInput.Text.ToUpper());
            if (string.IsNullOrEmpty(dataKey.ToString()))
                formMain.btn_FileAndCleanPidKey.Text = "File";
            else
            {
                char[] vbCrLf = new char[] { '\r', '\n' };
                formMain.btn_FileAndCleanPidKey.Text = "Clean";
                if (string.IsNullOrEmpty(fileNamePidKey))
                {
                    int tongKey = 0;
                    items = dataKey.ToString().Split(vbCrLf);
                    try
                    {
                        List<string> listPidKeyTemp = new List<string>();
                        for (int i = 0; i < items.Length; i++)
                        {
                            string[] elems = items[i].Split(vbCrLf);
                            var key = validate.NhanDangKey(elems[0].Trim());
                            foreach (string str in key)
                                listPidKeyTemp.Add(str);
                        }
                        listKeyPIDKey = validate.locData(listPidKeyTemp);
                        tongKey = listKeyPIDKey.Count();
                        formMain.lb_CountPidKey.Text = "0/" + tongKey;
                    }
                    catch (Exception ex)
                    {
                        logger.Error(ex);
                        MessageBox.Show(Messages.MemoryOverflow, Messages.error, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void CheckKeyPIDKeyWin7Office2010(string optionVersionPidKey)
        {
            listKeyPIDKeyInputSoftPIDKey.Clear();
            bool loadKeyFromDB = serverSetting.GetStatusLOADKEY();
            bool saveKey = false;
            int count_PidKey = 0, count_InvalidKey = 0, count_ValidKey = 0;
            formMain.cbb_VersionPidKey.Invoke(new Action(() =>
            {
                formMain.lb_CountValidKey.Text = "(0)";
                formMain.lb_CountInvalidKey.Text = "(0)";
                saveKey = formMain.cb_SaveKey.Checked;
            }));

            for (int i = 0; i < listKeyPIDKey.Count; i++)
            {
                TimeZone zone = TimeZone.CurrentTimeZone;
                DateTime local = zone.ToLocalTime(DateTime.Now);
                if (cancelPIDKey == false)
                {
                    count_PidKey += 1;
                    var resultCheckKey = processPidkey.CheckKey(listKeyPIDKey[i], optionVersionPidKey, loadKeyFromDB, saveKey);
                    if (resultCheckKey.Description == Constant.Unsupported_PKeyConfig_InvalidKey)
                    {
                        count_InvalidKey += 1;
                        formMain.tbx_InvalidKey.Invoke(new Action(() =>
                        {
                            formMain.tbx_InvalidKey.AppendText("Key: " + resultCheckKey.Key + "\r\n");
                            formMain.tbx_InvalidKey.AppendText("Description: " + resultCheckKey.Description + "\r\n");
                            formMain.tbx_InvalidKey.AppendText("Time: " + local + "\r\n---------------------\r\n");
                        }));
                    }
                    else if (resultCheckKey.MAKCount == Constant.KeyBlock || resultCheckKey.ErrorCode == Constant.KeyRetaiBlock)
                    {
                        count_InvalidKey += 1;
                        formMain.tbx_InvalidKey.Invoke(new Action(() =>
                        {
                            formMain.tbx_InvalidKey.AppendText("Key: " + resultCheckKey.Key + "\r\n");
                            formMain.tbx_InvalidKey.AppendText("Description: " + resultCheckKey.Description + "\r\n");
                            formMain.tbx_InvalidKey.AppendText("SubType: " + resultCheckKey.SubType + "\r\n");
                            formMain.tbx_InvalidKey.AppendText("LicenseType: " + resultCheckKey.LicenseType + "\r\n");
                            if (resultCheckKey.MAKCount == Constant.KeyBlock)
                                formMain.tbx_InvalidKey.AppendText("MAKCount: " + resultCheckKey.MAKCount + "\r\n");
                            if (resultCheckKey.ErrorCode == Constant.KeyRetaiBlock)
                                formMain.tbx_InvalidKey.AppendText("ErrorCode: " + resultCheckKey.ErrorCode + "\r\n");
                            formMain.tbx_InvalidKey.AppendText("Time: " + local + "\r\n---------------------\r\n");
                        }));
                    }
                    else
                    {
                        listKeyPIDKeyInputSoftPIDKey.Add(resultCheckKey);
                        count_ValidKey += 1;
                        formMain.tbx_ValidKey.Invoke(new Action(() =>
                        {
                            formMain.tbx_ValidKey.AppendText("Key: " + resultCheckKey.Key + "\r\n");
                            formMain.tbx_ValidKey.AppendText("Description: " + resultCheckKey.Description + "\r\n");
                            formMain.tbx_ValidKey.AppendText("SubType: " + resultCheckKey.SubType + "\r\n");
                            formMain.tbx_ValidKey.AppendText("LicenseType: " + resultCheckKey.LicenseType + "\r\n");
                            if (resultCheckKey.LicenseType == Constant.Retail && !string.IsNullOrEmpty(resultCheckKey.ErrorCode))
                                formMain.tbx_ValidKey.AppendText("ErrorCode: " + resultCheckKey.ErrorCode + "\r\n");
                            if (resultCheckKey.LicenseType == Constant.Volume)
                                formMain.tbx_ValidKey.AppendText("MAKCount: " + resultCheckKey.MAKCount + "\r\n");
                            if (resultCheckKey.LicenseType.Contains(Constant.OEM))
                            {
                                formMain.tbx_ValidKey.AppendText("MAKCount: " + resultCheckKey.MAKCount + "\r\n");
                                if (!string.IsNullOrEmpty(resultCheckKey.ErrorCode))
                                    formMain.tbx_ValidKey.AppendText("ErrorCode: " + resultCheckKey.ErrorCode + "\r\n");
                            }
                            formMain.tbx_ValidKey.AppendText("Time: " + local + "\r\n---------------------\r\n");
                        }));
                    }
                    formMain.lb_CountPidKey.Invoke(new Action(() =>
                    {
                        formMain.lb_CountPidKey.Text = count_PidKey + "/" + listKeyPIDKey.Count();
                        formMain.lb_CountInvalidKey.Text = "(" + count_InvalidKey + ")";
                        formMain.lb_CountValidKey.Text = "(" + count_ValidKey + ")";
                    }));
                }
                else break;
            }
            formMain.timerPidKey.Stop();
            formMain.cbb_VersionPidKey.Invoke(new Action(() =>
            {
                formMain.cbb_VersionPidKey.Enabled = true;
                formMain.tbx_PidKeyInput.Enabled = true;
                formMain.btn_CheckKey.Enabled = true;
                formMain.btn_CheckKey.Text = "Check Key";
                formMain.btn_FileAndCleanPidKey.Enabled = true;
                formMain.btn_CopyValidKey.Enabled = true;
                formMain.btn_CopyInvalidKey.Enabled = true;
                formMain.btn_SoftKey.Enabled = true;
                if (serverSetting.GetStatusPIDADV())
                    formMain.btn_CheckAdv.Enabled = true;
            }));
        }

        //Check Key normal
        ThreadLocal<ProcessPidkey> threadPIDKey = new ThreadLocal<ProcessPidkey>(() =>
        {
            return new ProcessPidkey();
        });

        private void CheckKeyPIDKey(string optionVersionPidKey)
        {
            listKeyPIDKeyInputSoftPIDKey.Clear();
            bool loadKeyFromDB = serverSetting.GetStatusLOADKEY();
            bool saveKey = false;
            formMain.cb_SaveKey.Invoke(new Action(() =>
            {
                formMain.lb_CountValidKey.Text = "(0)";
                formMain.lb_CountInvalidKey.Text = "(0)";
                saveKey = formMain.cb_SaveKey.Checked;
            }));
            int count_InvalidKey = 0, count_ValidKey = 0;
            var sortedValid = new SortedList<int, string>();
            var sortedInvalid = new SortedList<int, string>();
            int printingIdx = 0, globalkeyidx = 0, keyidx = 0;
            ParallelOptions po = new ParallelOptions();
            po.MaxDegreeOfParallelism = Environment.ProcessorCount;
            Parallel.For(0, listKeyPIDKey.Count, po, (i, loopState) =>
            {
                int localkeyidx = 0;
                // TA2: note: cho nay la protect bien keyidx k bi conflict thread
                formMain.Invoke(new Action(() =>
                {
                    localkeyidx = globalkeyidx;
                    globalkeyidx += 1;
                }));
                TimeZone zone = TimeZone.CurrentTimeZone;
                DateTime local = zone.ToLocalTime(DateTime.Now);
                string value = listKeyPIDKey[localkeyidx];
                if (cancelPIDKey == false)
                {
                    // TA: change: move pidkey from share to all threads to new instance per thread
                    ProcessPidkey pidkey = threadPIDKey.Value;
                    var resultCheckKey = processPidkey.CheckKey(value, optionVersionPidKey, loadKeyFromDB, saveKey);
                    //TA: change: local j per thread will not cause thread conflict

                    // TA: change: merge listVlaue -> textValue
                    string keyUnsupported_PKeyConfig_InvalidKey = string.Empty;
                    string keyBlock = string.Empty;
                    if (resultCheckKey.Description == Constant.Unsupported_PKeyConfig_InvalidKey)
                    {
                        keyUnsupported_PKeyConfig_InvalidKey = "Key: " + resultCheckKey.Key + "\r\n";
                        keyUnsupported_PKeyConfig_InvalidKey = keyUnsupported_PKeyConfig_InvalidKey + "Description: " + resultCheckKey.Description + "\r\n";
                        keyUnsupported_PKeyConfig_InvalidKey = keyUnsupported_PKeyConfig_InvalidKey + "Time: " + local + "\r\n---------------------\r\n";
                        count_InvalidKey += 1;
                        formMain.tbx_InvalidKey.Invoke(new Action(() =>
                        {
                            formMain.lb_CountInvalidKey.Text = "(" + count_InvalidKey + ")";
                            sortedInvalid.Add(localkeyidx, keyUnsupported_PKeyConfig_InvalidKey);
                        }));
                    }
                    else if (resultCheckKey.MAKCount == Constant.KeyBlock || resultCheckKey.ErrorCode == Constant.KeyRetaiBlock)
                    {
                        keyBlock = "Key: " + resultCheckKey.Key + "\r\n";
                        keyBlock = keyBlock + "Description: " + resultCheckKey.Description + "\r\n";
                        keyBlock = keyBlock + "SubType: " + resultCheckKey.SubType + "\r\n";
                        keyBlock = keyBlock + "LicenseType: " + resultCheckKey.LicenseType + "\r\n";
                        if (resultCheckKey.MAKCount == Constant.KeyBlock)
                            keyBlock = keyBlock + "MAKCount: " + resultCheckKey.MAKCount + "\r\n";
                        if (resultCheckKey.ErrorCode == Constant.KeyRetaiBlock)
                            keyBlock = keyBlock + "ErrorCode: " + resultCheckKey.ErrorCode + "\r\n";
                        keyBlock = keyBlock + "Time: " + local + "\r\n---------------------\r\n";
                        count_InvalidKey += 1;
                        formMain.tbx_InvalidKey.Invoke(new Action(() =>
                        {
                            formMain.lb_CountInvalidKey.Text = "(" + count_InvalidKey + ")";
                            sortedInvalid.Add(localkeyidx, keyBlock);
                        }));
                    }
                    else
                    {
                        listKeyPIDKeyInputSoftPIDKey.Add(resultCheckKey);
                        count_ValidKey += 1;
                        string keyValid = string.Empty;
                        keyValid = "Key: " + resultCheckKey.Key + "\r\n";
                        keyValid = keyValid + "Description: " + resultCheckKey.Description + "\r\n";
                        keyValid = keyValid + "SubType: " + resultCheckKey.SubType + "\r\n";
                        keyValid = keyValid + "LicenseType: " + resultCheckKey.LicenseType + "\r\n";
                        if (resultCheckKey.LicenseType == Constant.Retail && !string.IsNullOrEmpty(resultCheckKey.ErrorCode))
                            keyValid = keyValid + "ErrorCode: " + resultCheckKey.ErrorCode + "\r\n";
                        if (resultCheckKey.LicenseType == Constant.Volume)
                            keyValid = keyValid + "MAKCount: " + resultCheckKey.MAKCount + "\r\n";
                        if (resultCheckKey.LicenseType.Contains(Constant.OEM))
                        {
                            keyValid = keyValid + "MAKCount: " + resultCheckKey.MAKCount + "\r\n";
                            if (!string.IsNullOrEmpty(resultCheckKey.ErrorCode))
                                keyValid = keyValid + "ErrorCode: " + resultCheckKey.ErrorCode + "\r\n";
                        }
                        keyValid = keyValid + "Time: " + local + "\r\n---------------------\r\n";
                        formMain.tbx_ValidKey.Invoke(new Action(() =>
                        {
                            formMain.lb_CountValidKey.Text = "(" + count_ValidKey + ")";
                            sortedValid.Add(localkeyidx, keyValid);
                        }));
                    }

                    formMain.tbx_InvalidKey.Invoke(new Action(() =>
                    {
                        keyidx += 1;
                        formMain.lb_CountPidKey.Text = keyidx + "/" + listKeyPIDKey.Count;
                        while (true)
                        {
                            //TA2: change: only print if there are no missing item
                            if (sortedValid.Count > 0 && sortedValid.ElementAtOrDefault(0).Key == printingIdx)
                            {
                                formMain.tbx_ValidKey.AppendText(sortedValid[printingIdx]);
                                sortedValid.RemoveAt(0);
                            }
                            else if (sortedInvalid.Count > 0 && sortedInvalid.ElementAtOrDefault(0).Key == printingIdx)
                            {
                                formMain.tbx_InvalidKey.AppendText(sortedInvalid[printingIdx]);
                                sortedInvalid.RemoveAt(0);
                            }
                            else
                                break;
                            printingIdx += 1;
                        }
                    }));
                }
                else
                    loopState.Stop();
            });
            formMain.timerPidKey.Stop();
            formMain.cbb_VersionPidKey.Invoke(new Action(() =>
            {
                formMain.cbb_VersionPidKey.Enabled = true;
                formMain.tbx_PidKeyInput.Enabled = true;
                formMain.btn_CheckKey.Enabled = true;
                formMain.btn_CheckKey.Text = "Check Key";
                formMain.btn_FileAndCleanPidKey.Enabled = true;
                formMain.btn_CopyValidKey.Enabled = true;
                formMain.btn_CopyInvalidKey.Enabled = true;
                formMain.btn_SoftKey.Enabled = true;
                if (serverSetting.GetStatusPIDADV())
                    formMain.btn_CheckAdv.Enabled = true;
            }));
        }

        //Check Key Advanced
        private void CheckKeyAdv_PIDKeyWin7Office2010(string optionVersionPidKey)
        {
            string operatingSystem = (string)Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion").GetValue("ProductName");
            if (!operatingSystem.Contains("XP") && !operatingSystem.Contains("7") && !operatingSystem.Contains("8") && !operatingSystem.Contains("8.1")
                && !operatingSystem.Contains("Server 12"))
                processPidkey.DisabledService();
            listKeyPIDKeyInputSoftPIDKey.Clear();
            bool loadKeyFromDB = serverSetting.GetStatusLOADKEY();
            bool saveKey = false;
            int count_PidKey = 0, count_InvalidKey = 0, count_ValidKey = 0;
            formMain.cbb_VersionPidKey.Invoke(new Action(() =>
            {
                formMain.lb_CountValidKey.Text = "(0)";
                formMain.lb_CountInvalidKey.Text = "(0)";
                saveKey = formMain.cb_SaveKey.Checked;
            }));

            for (int i = 0; i < listKeyPIDKey.Count; i++)
            {
                TimeZone zone = TimeZone.CurrentTimeZone;
                DateTime local = zone.ToLocalTime(DateTime.Now);
                if (cancelPIDKey == false)
                {
                    count_PidKey += 1;
                    var resultCheckKey = processPidkey.CheckKeyAdv(listKeyPIDKey[i], optionVersionPidKey, loadKeyFromDB, saveKey);
                    if (resultCheckKey.Description == Constant.Unsupported_PKeyConfig_InvalidKey)
                    {
                        count_InvalidKey += 1;
                        formMain.tbx_InvalidKey.Invoke(new Action(() =>
                        {
                            formMain.tbx_InvalidKey.AppendText("Key: " + resultCheckKey.Key + "\r\n");
                            formMain.tbx_InvalidKey.AppendText("Description: " + resultCheckKey.Description + "\r\n");
                            formMain.tbx_InvalidKey.AppendText("Time: " + local + "\r\n---------------------\r\n");
                        }));
                    }
                    else if (resultCheckKey.MAKCount == Constant.KeyBlock || resultCheckKey.ErrorCode == Constant.KeyRetaiBlock)
                    {
                        count_InvalidKey += 1;
                        formMain.tbx_InvalidKey.Invoke(new Action(() =>
                        {
                            formMain.tbx_InvalidKey.AppendText("Key: " + resultCheckKey.Key + "\r\n");
                            formMain.tbx_InvalidKey.AppendText("Description: " + resultCheckKey.Description + "\r\n");
                            formMain.tbx_InvalidKey.AppendText("SubType: " + resultCheckKey.SubType + "\r\n");
                            formMain.tbx_InvalidKey.AppendText("LicenseType: " + resultCheckKey.LicenseType + "\r\n");
                            if (resultCheckKey.MAKCount == Constant.KeyBlock)
                                formMain.tbx_InvalidKey.AppendText("MAKCount: " + resultCheckKey.MAKCount + "\r\n");
                            if (resultCheckKey.ErrorCode == Constant.KeyRetaiBlock)
                                formMain.tbx_InvalidKey.AppendText("ErrorCode: " + resultCheckKey.ErrorCode + "\r\n");
                            formMain.tbx_InvalidKey.AppendText("Time: " + local + "\r\n---------------------\r\n");
                        }));
                    }
                    else
                    {
                        listKeyPIDKeyInputSoftPIDKey.Add(resultCheckKey);
                        count_ValidKey += 1;
                        formMain.tbx_ValidKey.Invoke(new Action(() =>
                        {
                            formMain.tbx_ValidKey.AppendText("Key: " + resultCheckKey.Key + "\r\n");
                            formMain.tbx_ValidKey.AppendText("Description: " + resultCheckKey.Description + "\r\n");
                            formMain.tbx_ValidKey.AppendText("SubType: " + resultCheckKey.SubType + "\r\n");
                            formMain.tbx_ValidKey.AppendText("LicenseType: " + resultCheckKey.LicenseType + "\r\n");
                            if (resultCheckKey.LicenseType == Constant.Retail && !string.IsNullOrEmpty(resultCheckKey.ErrorCode))
                                formMain.tbx_ValidKey.AppendText("ErrorCode: " + resultCheckKey.ErrorCode + "\r\n");
                            if (resultCheckKey.LicenseType == Constant.Volume)
                            {
                                formMain.tbx_ValidKey.AppendText("MAKCount: " + resultCheckKey.MAKCount + "\r\n");
                                if (!string.IsNullOrEmpty(resultCheckKey.ErrorCode))
                                    formMain.tbx_ValidKey.AppendText("ErrorCode: " + resultCheckKey.ErrorCode + "\r\n");
                                if (!string.IsNullOrEmpty(resultCheckKey.KeyGetWeb))
                                    formMain.tbx_ValidKey.AppendText("Result Get Web: " + resultCheckKey.KeyGetWeb + "\r\n");
                            }

                            if (resultCheckKey.LicenseType.Contains(Constant.OEM))
                            {
                                formMain.tbx_ValidKey.AppendText("MAKCount: " + resultCheckKey.MAKCount + "\r\n");
                                if (!string.IsNullOrEmpty(resultCheckKey.ErrorCode))
                                    formMain.tbx_ValidKey.AppendText("ErrorCode: " + resultCheckKey.ErrorCode + "\r\n");
                                if (!string.IsNullOrEmpty(resultCheckKey.KeyGetWeb))
                                    formMain.tbx_ValidKey.AppendText("Result Get Web: " + resultCheckKey.KeyGetWeb + "\r\n");
                            }
                            formMain.tbx_ValidKey.AppendText("Time: " + local + "\r\n---------------------\r\n");
                        }));
                    }
                    formMain.lb_CountPidKey.Invoke(new Action(() =>
                    {
                        formMain.lb_CountPidKey.Text = count_PidKey + "/" + listKeyPIDKey.Count();
                        formMain.lb_CountInvalidKey.Text = "(" + count_InvalidKey + ")";
                        formMain.lb_CountValidKey.Text = "(" + count_ValidKey + ")";
                    }));
                }
                else break;
            }
            if (!operatingSystem.Contains("XP") && !operatingSystem.Contains("7") && !operatingSystem.Contains("8") && !operatingSystem.Contains("8.1")
                && !operatingSystem.Contains("Server 12"))
                processPidkey.EnabledService();
            formMain.timerPidKey.Stop();
            formMain.cbb_VersionPidKey.Invoke(new Action(() =>
            {
                formMain.cbb_VersionPidKey.Enabled = true;
                formMain.tbx_PidKeyInput.Enabled = true;
                formMain.btn_CheckKey.Enabled = true;
                formMain.btn_CheckAdv.Text = "Check Advanced";
                formMain.btn_FileAndCleanPidKey.Enabled = true;
                formMain.btn_CopyValidKey.Enabled = true;
                formMain.btn_CopyInvalidKey.Enabled = true;
                formMain.btn_SoftKey.Enabled = true;
                if (serverSetting.GetStatusPIDADV())
                    formMain.btn_CheckAdv.Enabled = true;
            }));
        }

        ThreadLocal<ProcessPidkey> threadPIDKeyAdv = new ThreadLocal<ProcessPidkey>(() =>
        {
            return new ProcessPidkey();
        });

        private void CheckKeyPIDKeyAdv(string optionVersionPidKey)
        {
            string operatingSystem = (string)Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion").GetValue("ProductName");
            if (!operatingSystem.Contains("XP") && !operatingSystem.Contains("7") && !operatingSystem.Contains("8") && !operatingSystem.Contains("8.1")
                && !operatingSystem.Contains("Server 12"))
                processPidkey.DisabledService();
            listKeyPIDKeyInputSoftPIDKey.Clear();
            bool loadKeyFromDB = serverSetting.GetStatusLOADKEY();
            bool saveKey = false;
            int count_InvalidKey = 0, count_ValidKey = 0;
            var sortedValid = new SortedList<int, string>();
            var sortedInvalid = new SortedList<int, string>();
            int printingIdx = 0, globalkeyidx = 0, keyidx = 0;
            ParallelOptions po = new ParallelOptions();
            formMain.cb_SaveKey.Invoke(new Action(() =>
            {
                formMain.lb_CountValidKey.Text = "(0)";
                formMain.lb_CountInvalidKey.Text = "(0)";
                saveKey = formMain.cb_SaveKey.Checked;
                po.MaxDegreeOfParallelism = int.Parse(formMain.cbb_MultyThread.Text);
            }));

            Parallel.For(0, listKeyPIDKey.Count, po, (i, loopState) =>
            {
                int localkeyidx = 0;
                // TA2: note: cho nay la protect bien keyidx k bi conflict thread
                formMain.Invoke(new Action(() =>
                {
                    localkeyidx = globalkeyidx;
                    globalkeyidx += 1;
                }));
                TimeZone zone = TimeZone.CurrentTimeZone;
                DateTime local = zone.ToLocalTime(DateTime.Now);
                string value = listKeyPIDKey[localkeyidx];
                if (cancelPIDKey == false)
                {
                    // TA: change: move pidkey from share to all threads to new instance per thread
                    ProcessPidkey pidkey = threadPIDKeyAdv.Value;
                    var resultCheckKey = processPidkey.CheckKeyAdv(value, optionVersionPidKey, loadKeyFromDB, saveKey);
                    //TA: change: local j per thread will not cause thread conflict

                    // TA: change: merge listVlaue -> textValue
                    string keyUnsupported_PKeyConfig_InvalidKey = string.Empty;
                    string keyBlock = string.Empty;
                    if (resultCheckKey.Description == Constant.Unsupported_PKeyConfig_InvalidKey)
                    {
                        keyUnsupported_PKeyConfig_InvalidKey = "Key: " + resultCheckKey.Key + "\r\n";
                        keyUnsupported_PKeyConfig_InvalidKey = keyUnsupported_PKeyConfig_InvalidKey + "Description: " + resultCheckKey.Description + "\r\n";
                        keyUnsupported_PKeyConfig_InvalidKey = keyUnsupported_PKeyConfig_InvalidKey + "Time: " + local + "\r\n---------------------\r\n";
                        count_InvalidKey += 1;
                        formMain.tbx_InvalidKey.Invoke(new Action(() =>
                        {
                            formMain.lb_CountInvalidKey.Text = "(" + count_InvalidKey + ")";
                            sortedInvalid.Add(localkeyidx, keyUnsupported_PKeyConfig_InvalidKey);
                        }));
                    }
                    else if (resultCheckKey.MAKCount == Constant.KeyBlock || resultCheckKey.ErrorCode == Constant.KeyRetaiBlock)
                    {
                        keyBlock = "Key: " + resultCheckKey.Key + "\r\n";
                        keyBlock = keyBlock + "Description: " + resultCheckKey.Description + "\r\n";
                        keyBlock = keyBlock + "SubType: " + resultCheckKey.SubType + "\r\n";
                        keyBlock = keyBlock + "LicenseType: " + resultCheckKey.LicenseType + "\r\n";
                        if (resultCheckKey.MAKCount == Constant.KeyBlock)
                            keyBlock = keyBlock + "MAKCount: " + resultCheckKey.MAKCount + "\r\n";
                        if (resultCheckKey.ErrorCode == Constant.KeyRetaiBlock)
                            keyBlock = keyBlock + "ErrorCode: " + resultCheckKey.ErrorCode + "\r\n";
                        keyBlock = keyBlock + "Time: " + local + "\r\n---------------------\r\n";
                        count_InvalidKey += 1;
                        formMain.tbx_InvalidKey.Invoke(new Action(() =>
                        {
                            formMain.lb_CountInvalidKey.Text = "(" + count_InvalidKey + ")";
                            sortedInvalid.Add(localkeyidx, keyBlock);
                        }));
                    }
                    else
                    {
                        listKeyPIDKeyInputSoftPIDKey.Add(resultCheckKey);
                        count_ValidKey += 1;
                        string keyValid = string.Empty;
                        keyValid = "Key: " + resultCheckKey.Key + "\r\n";
                        keyValid = keyValid + "Description: " + resultCheckKey.Description + "\r\n";
                        keyValid = keyValid + "SubType: " + resultCheckKey.SubType + "\r\n";
                        keyValid = keyValid + "LicenseType: " + resultCheckKey.LicenseType + "\r\n";
                        if (resultCheckKey.LicenseType == Constant.Retail && !string.IsNullOrEmpty(resultCheckKey.ErrorCode))
                            keyValid = keyValid + "ErrorCode: " + resultCheckKey.ErrorCode + "\r\n";
                        if (resultCheckKey.LicenseType == Constant.Volume)
                        {
                            keyValid = keyValid + "MAKCount: " + resultCheckKey.MAKCount + "\r\n";
                            if (!string.IsNullOrEmpty(resultCheckKey.ErrorCode))
                                keyValid = keyValid + "ErrorCode: " + resultCheckKey.ErrorCode + "\r\n";
                            if (!string.IsNullOrEmpty(resultCheckKey.KeyGetWeb))
                                keyValid = keyValid + "Result Get Web: " + resultCheckKey.KeyGetWeb + "\r\n";
                        }
                        if (resultCheckKey.LicenseType.Contains(Constant.OEM))
                        {
                            keyValid = keyValid + "MAKCount: " + resultCheckKey.MAKCount + "\r\n";
                            if (!string.IsNullOrEmpty(resultCheckKey.ErrorCode))
                                keyValid = keyValid + "ErrorCode: " + resultCheckKey.ErrorCode + "\r\n";
                            if (!string.IsNullOrEmpty(resultCheckKey.KeyGetWeb))
                                keyValid = keyValid + "Result Get Web: " + resultCheckKey.KeyGetWeb + "\r\n";
                        }
                        keyValid = keyValid + "Time: " + local + "\r\n---------------------\r\n";
                        formMain.tbx_ValidKey.Invoke(new Action(() =>
                        {
                            formMain.lb_CountValidKey.Text = "(" + count_ValidKey + ")";
                            sortedValid.Add(localkeyidx, keyValid);
                        }));
                    }

                    formMain.tbx_InvalidKey.Invoke(new Action(() =>
                    {
                        keyidx += 1;
                        formMain.lb_CountPidKey.Text = keyidx + "/" + listKeyPIDKey.Count;
                        while (true)
                        {
                            //TA2: change: only print if there are no missing item
                            if (sortedValid.Count > 0 && sortedValid.ElementAtOrDefault(0).Key == printingIdx)
                            {
                                formMain.tbx_ValidKey.AppendText(sortedValid[printingIdx]);
                                sortedValid.RemoveAt(0);
                            }
                            else if (sortedInvalid.Count > 0 && sortedInvalid.ElementAtOrDefault(0).Key == printingIdx)
                            {
                                formMain.tbx_InvalidKey.AppendText(sortedInvalid[printingIdx]);
                                sortedInvalid.RemoveAt(0);
                            }
                            else
                                break;
                            printingIdx += 1;
                        }
                    }));
                }
                else
                    loopState.Stop();
            });
            if (!operatingSystem.Contains("XP") && !operatingSystem.Contains("7") && !operatingSystem.Contains("8") && !operatingSystem.Contains("8.1")
                && !operatingSystem.Contains("Server 12"))
                processPidkey.EnabledService();
            formMain.timerPidKey.Stop();
            formMain.cbb_VersionPidKey.Invoke(new Action(() =>
            {
                formMain.cbb_VersionPidKey.Enabled = true;
                formMain.tbx_PidKeyInput.Enabled = true;
                formMain.btn_CheckKey.Enabled = true;
                formMain.btn_CheckAdv.Text = "Check Advanced";
                formMain.btn_FileAndCleanPidKey.Enabled = true;
                formMain.btn_CopyValidKey.Enabled = true;
                formMain.btn_CopyInvalidKey.Enabled = true;
                formMain.btn_SoftKey.Enabled = true;
                if (serverSetting.GetStatusPIDADV())
                    formMain.btn_CheckAdv.Enabled = true;
            }));
        }

        public void btn_CheckKey_Click(object sender, EventArgs e)
        {
            if (formMain.btn_CheckKey.Text == "Check Key")
            {
                if (string.IsNullOrEmpty(formMain.tbx_PidKeyInput.Text.Replace(" ", "").Trim()))
                    MessageBox.Show(Messages.EnterPidkey, Messages.warning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                else
                {
                    formMain.lb_CountPidKey.Text = "0/" + listKeyPIDKey.Count();
                    formMain.tbx_ValidKey.Clear();
                    formMain.tbx_InvalidKey.Clear();
                    if (listKeyPIDKey.Count() == 0)
                        formMain.tbx_InvalidKey.Text = "Key: " + formMain.tbx_PidKeyInput.Text + "\r\n" + Constant.Unsupported_PKeyConfig_InvalidKey + "\r\n";
                    formMain.btn_CheckKey.Text = "Stop";
                    cancelPIDKey = false;
                    string optionVersionPidKey = formMain.cbb_VersionPidKey.Text;
                    formMain.lb_TimerPidKey.Text = "00:00:00:000";
                    dateTimePIDKEY = DateTime.Now;
                    formMain.timerPidKey.Start();
                    new Thread(() =>
                    {
                        var listOption = sourceData.OptionCbxPidKeyAndTxtFilesArrayPidKey();
                        if (optionVersionPidKey == listOption.ElementAt(0).Key ||
                           optionVersionPidKey == listOption.ElementAt(1).Key ||
                           optionVersionPidKey == listOption.ElementAt(6).Key ||
                           optionVersionPidKey == listOption.ElementAt(11).Key ||
                           optionVersionPidKey == listOption.ElementAt(12).Key ||
                           optionVersionPidKey == listOption.ElementAt(13).Key ||
                           optionVersionPidKey == listOption.ElementAt(14).Key)
                            CheckKeyPIDKeyWin7Office2010(optionVersionPidKey);
                        else
                            CheckKeyPIDKey(optionVersionPidKey);
                    })
                    { IsBackground = true }.Start();
                    formMain.tbx_PidKeyInput.Enabled = false;
                    formMain.cbb_VersionPidKey.Enabled = false;
                    formMain.btn_FileAndCleanPidKey.Enabled = false;
                    formMain.btn_CopyValidKey.Enabled = false;
                    formMain.btn_CopyInvalidKey.Enabled = false;
                    formMain.btn_SoftKey.Enabled = false;
                    if (serverSetting.GetStatusPIDADV())
                        formMain.btn_CheckAdv.Enabled = false;
                }
            }
            else
            {
                cancelPIDKey = true;
                MessageBox.Show(Messages.StopPidKey, Messages.success, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        public void btn_CheckAdv_Click(object sender, EventArgs e)
        {
            if (formMain.IsAdministrator() == false)
            {
                var resultComfirm = MessageBox.Show(Messages.ReopenWithAdmin, Messages.success, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (resultComfirm == DialogResult.Yes)
                {
                    processTemp.CreateFileTemp();
                    formMain.Cursor = Cursors.WaitCursor;
                    ProcessStartInfo proc = new ProcessStartInfo();
                    proc.UseShellExecute = true;
                    proc.WorkingDirectory = Environment.CurrentDirectory;
                    proc.FileName = Application.ExecutablePath;
                    proc.Verb = "runas";
                    try
                    {
                        Process.Start(proc);
                    }
                    catch
                    {
                        return;
                    }
                    Environment.Exit(1);
                }
            }
            else
            {
                if (formMain.btn_CheckAdv.Text == "Check Advanced")
                {
                    if (string.IsNullOrEmpty(formMain.tbx_PidKeyInput.Text.Replace(" ", "").Trim()))
                        MessageBox.Show(Messages.EnterPidkey, Messages.warning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    else
                    {
                        formMain.lb_CountPidKey.Text = "0/" + listKeyPIDKey.Count();
                        cancelPIDKey = false;
                        formMain.tbx_ValidKey.Clear();
                        formMain.tbx_InvalidKey.Clear();
                        formMain.cbb_VersionPidKey.Enabled = false;
                        formMain.tbx_PidKeyInput.Enabled = false;
                        formMain.btn_CheckKey.Enabled = false;
                        formMain.btn_FileAndCleanPidKey.Enabled = false;
                        formMain.btn_SoftKey.Enabled = false;
                        formMain.btn_CopyInvalidKey.Enabled = false;
                        formMain.btn_CopyValidKey.Enabled = false;
                        formMain.btn_CheckAdv.Text = "Stop";
                        string optionVersionPidKey = formMain.cbb_VersionPidKey.Text;
                        formMain.lb_TimerPidKey.Text = "00:00:00:000";
                        dateTimePIDKEY = DateTime.Now;
                        formMain.timerPidKey.Start();
                        new Thread(() =>
                        {
                            var listOption = sourceData.OptionCbxPidKeyAndTxtFilesArrayPidKey();
                            if (optionVersionPidKey == listOption.ElementAt(0).Key ||
                               optionVersionPidKey == listOption.ElementAt(1).Key ||
                               optionVersionPidKey == listOption.ElementAt(6).Key ||
                               optionVersionPidKey == listOption.ElementAt(11).Key ||
                               optionVersionPidKey == listOption.ElementAt(12).Key ||
                               optionVersionPidKey == listOption.ElementAt(13).Key ||
                               optionVersionPidKey == listOption.ElementAt(14).Key)
                                CheckKeyAdv_PIDKeyWin7Office2010(optionVersionPidKey);
                            else
                                CheckKeyPIDKeyAdv(optionVersionPidKey);
                        })
                        { IsBackground = true }.Start();
                    }
                }
                else
                {
                    cancelPIDKey = true;
                    MessageBox.Show(Messages.StopPidKey, Messages.success, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        public void timerPidKey_Tick(object sender, EventArgs e)
        {
            TimeSpan diff = DateTime.Now.Subtract(dateTimePIDKEY);
            formMain.lb_TimerPidKey.Text = diff.ToString(@"hh\:mm\:ss\:fff");
        }

        private void ReadFile()
        {
            string valueReadFile;
            FileStream fs = null;
            StreamReader reader = null;
            try
            {
                fs = new FileStream(fileNamePidKey, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                reader = new StreamReader(fs);
                List<string> list = new List<string>();
                int tongKey = 0;
                while (!reader.EndOfStream)
                {
                    valueReadFile = reader.ReadLine().Trim().ToUpper();
                    var Filekey = validate.NhanDangKey(valueReadFile);
                    foreach (string Str in Filekey)
                    {
                        list.Add(Str.ToString());
                        formMain.tbx_PidKeyInput.Invoke(new Action(() =>
                        {
                            formMain.tbx_PidKeyInput.AppendText(Str + "\r\n");
                        }));
                        tongKey = tongKey + 1;
                        formMain.lb_CountPidKey.Invoke(new Action(() =>
                        {
                            formMain.lb_CountPidKey.Text = "0/" + tongKey;
                        }));
                    }
                }
                listKeyPIDKey = list;
            }
            catch (IOException ex)
            {
                logger.Error(ex);
            }
            finally
            {
                fs.Close();
                reader.Close();
                formMain.cbb_VersionPidKey.Invoke(new Action(() =>
                {
                    formMain.cbb_VersionPidKey.Enabled = true;
                    formMain.tbx_PidKeyInput.Enabled = true;
                    formMain.btn_CheckKey.Enabled = true;
                    formMain.btn_FileAndCleanPidKey.Enabled = true;
                    formMain.btn_CopyValidKey.Enabled = true;
                    formMain.btn_CopyInvalidKey.Enabled = true;
                    formMain.cbb_VersionPidKey.Focus();
                    if (serverSetting.GetStatusPIDADV())
                        formMain.btn_CheckAdv.Enabled = true;
                }));
                fileNamePidKey = string.Empty;
                formMain.timerPidKey.Stop();
            }
            formMain.btn_FileAndCleanPidKey.Invoke(new Action(() =>
            {
                formMain.btn_FileAndCleanPidKey.Text = "Clean";
            }));
        }

        public void btn_FileAndCleanPidKey_Click(object sender, EventArgs e)
        {
            formMain.btn_SoftKey.Enabled = false;
            if (formMain.btn_FileAndCleanPidKey.Text == "File")
            {
                formMain.btn_FileAndCleanPidKey.Text = "File";
                if (string.IsNullOrEmpty(formMain.tbx_PidKeyInput.Text))
                {
                    using (OpenFileDialog dialog = new OpenFileDialog())
                    {
                        if (dialog.ShowDialog() != DialogResult.OK)
                            return;
                        fileNamePidKey = dialog.FileName;
                    }
                    Thread ReadFieKey = new Thread(ReadFile);
                    formMain.lb_TimerPidKey.Text = "00:00:00:000";
                    dateTimePIDKEY = DateTime.Now;
                    formMain.timerPidKey.Start();
                    ReadFieKey.Start();
                    formMain.tbx_PidKeyInput.Enabled = false;
                    formMain.cbb_VersionPidKey.Enabled = false;
                    formMain.btn_CheckKey.Enabled = false;
                    formMain.btn_FileAndCleanPidKey.Enabled = false;
                    formMain.btn_CopyValidKey.Enabled = false;
                    formMain.btn_CopyInvalidKey.Enabled = false;
                    if (serverSetting.GetStatusPIDADV())
                        formMain.btn_CheckAdv.Enabled = false;
                }
                else
                    MessageBox.Show(Messages.DeleteAllKey, Messages.warning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                formMain.btn_FileAndCleanPidKey.Text = "Clean";
            }
            else if (formMain.btn_FileAndCleanPidKey.Text == "Clean")
            {
                formMain.lb_CountPidKey.Text = "0/0";
                formMain.tbx_PidKeyInput.Text = "";
                formMain.tbx_PidKeyInput.Focus();
                formMain.btn_FileAndCleanPidKey.Text = "File";
            }
        }

        public void btn_CopyValidKey_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(formMain.tbx_ValidKey.Text))
            {
                var check = formMain.tbx_ValidKey.Text.IndexOf("Key");
                formMain.tbx_ValidKey.Focus();
                formMain.tbx_ValidKey.SelectionStart = 0;
                formMain.tbx_ValidKey.SelectionLength = formMain.tbx_ValidKey.Text.Length;
                var value = formMain.tbx_ValidKey.Text.Substring(check).Trim();
                Clipboard.SetText(value + "\r\nCheck by Support Activate (TVT)\r\n");
            }
        }

        public void btn_CopyInvalidKey_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(formMain.tbx_InvalidKey.Text))
            {
                formMain.tbx_InvalidKey.Focus();
                formMain.tbx_InvalidKey.SelectionStart = 0;
                formMain.tbx_InvalidKey.SelectionLength = formMain.tbx_InvalidKey.Text.Length;
                Clipboard.SetText(formMain.tbx_InvalidKey.Text + "\r\nCheck by Support Activate (TVT)\r\n");
            }
        }

        public void btn_SoftKey_Click(object sender, EventArgs e)
        {
            new Thread(() =>
            {
                formMain.tbx_ValidKey.Invoke(new Action(() =>
                {
                    formMain.tbx_ValidKey.Clear();
                }));
                List<pid> listKeyOnline = new List<pid>();
                List<pid> listKey008 = new List<pid>();
                List<pid> listKeyOrther = new List<pid>();
                List<pid> listKeyVolumn = new List<pid>();
                List<pid> listKeyRetail = new List<pid>();
                listKeyRetail = listKeyPIDKeyInputSoftPIDKey.Where(x => x.LicenseType == Constant.Retail).Select(x => x).ToList();
                listKeyOnline = listKeyRetail.Where(x => x.ErrorCode == Constant.Online && x.LicenseType == Constant.Retail).Select(x => x).ToList();
                listKey008 = listKeyRetail.Where(x => x.ErrorCode == Constant.KeyRetai4C008 && x.LicenseType == Constant.Retail).Select(x => x).ToList();
                listKeyOrther = listKeyRetail.Where(x => x.ErrorCode != Constant.KeyRetai4C008 && x.ErrorCode != Constant.Online && x.LicenseType == Constant.Retail).Select(x => x).ToList();
                listKeyVolumn = listKeyPIDKeyInputSoftPIDKey
                                    .Where(x => x.LicenseType == Constant.Volume || x.LicenseType.Contains(Constant.OEM)).Select(x => x).ToList();
                
                if (listKeyOnline.Count > 0)
                    WriteResultSoftPidKey(listKeyOnline);
                if (listKey008.Count > 0)
                    WriteResultSoftPidKey(listKey008);
                if (listKeyOrther.Count > 0)
                    WriteResultSoftPidKey(listKeyOrther);
                if (listKeyVolumn.Count > 0)
                {
                    int MAKCount = 0;
                    List<PidKeySoft> listKeyVolumnSoft = new List<PidKeySoft>();
                    List<pid> listKeyVolumnOrther = new List<pid>();
                    for (int i = 0; i < listKeyVolumn.Count; i++)
                    {
                        if (int.TryParse(listKeyVolumn[i].MAKCount, out MAKCount))
                            listKeyVolumnSoft.Add(new PidKeySoft()
                            {
                                Key = listKeyVolumn[i].Key,
                                Description = listKeyVolumn[i].Description,
                                SubType = listKeyVolumn[i].SubType,
                                LicenseType = listKeyVolumn[i].LicenseType,
                                ErrorCode = listKeyVolumn[i].ErrorCode,
                                MAKCount = MAKCount,
                                KeyGetWeb = listKeyVolumn[i].KeyGetWeb
                            });
                        else
                            listKeyVolumnOrther.Add(listKeyVolumn[i]);
                    }
                    WriteResultSoftPidKeyVolumn(listKeyVolumnSoft.OrderByDescending(x => x.MAKCount).Select(x => x).ToList(), listKeyVolumnOrther);
                }
                
            })
            { IsBackground = true }.Start();
        }

        private void WriteResultSoftPidKey(List<pid> listCheckKey)
        {
            foreach (var resultCheckKey in listCheckKey)
            {
                formMain.tbx_ValidKey.Invoke(new Action(() =>
                {
                    formMain.tbx_ValidKey.AppendText("Key: " + resultCheckKey.Key + "\r\n");
                    formMain.tbx_ValidKey.AppendText("Description: " + resultCheckKey.Description + "\r\n");
                    formMain.tbx_ValidKey.AppendText("SubType: " + resultCheckKey.SubType + "\r\n");
                    formMain.tbx_ValidKey.AppendText("LicenseType: " + resultCheckKey.LicenseType + "\r\n");
                    if (resultCheckKey.LicenseType == Constant.Retail && !string.IsNullOrEmpty(resultCheckKey.ErrorCode))
                        formMain.tbx_ValidKey.AppendText("ErrorCode: " + resultCheckKey.ErrorCode + "\r\n");
                    if (resultCheckKey.LicenseType == Constant.Volume)
                    {
                        formMain.tbx_ValidKey.AppendText("MAKCount: " + resultCheckKey.MAKCount + "\r\n");
                        if (!string.IsNullOrEmpty(resultCheckKey.ErrorCode))
                            formMain.tbx_ValidKey.AppendText("ErrorCode: " + resultCheckKey.ErrorCode + "\r\n");
                        if (!string.IsNullOrEmpty(resultCheckKey.KeyGetWeb))
                            formMain.tbx_ValidKey.AppendText("Result Get Web: " + resultCheckKey.KeyGetWeb + "\r\n");
                    }

                    if (resultCheckKey.LicenseType.Contains(Constant.OEM))
                    {
                        formMain.tbx_ValidKey.AppendText("MAKCount: " + resultCheckKey.MAKCount + "\r\n");
                        if (!string.IsNullOrEmpty(resultCheckKey.ErrorCode))
                            formMain.tbx_ValidKey.AppendText("ErrorCode: " + resultCheckKey.ErrorCode + "\r\n");
                        if (!string.IsNullOrEmpty(resultCheckKey.KeyGetWeb))
                            formMain.tbx_ValidKey.AppendText("Result Get Web: " + resultCheckKey.KeyGetWeb + "\r\n");
                    }
                    formMain.tbx_ValidKey.AppendText("---------------------\r\n");
                }));
            }
        }

        private void WriteResultSoftPidKeyVolumn(List<PidKeySoft> listCheckKeyVolumnSoft, List<pid> listCheckKeyVolumnOrther)
        {
            foreach (var resultCheckKey in listCheckKeyVolumnSoft)
            {
                formMain.tbx_ValidKey.Invoke(new Action(() =>
                {
                    formMain.tbx_ValidKey.AppendText("Key: " + resultCheckKey.Key + "\r\n");
                    formMain.tbx_ValidKey.AppendText("Description: " + resultCheckKey.Description + "\r\n");
                    formMain.tbx_ValidKey.AppendText("SubType: " + resultCheckKey.SubType + "\r\n");
                    formMain.tbx_ValidKey.AppendText("LicenseType: " + resultCheckKey.LicenseType + "\r\n");
                    if (resultCheckKey.LicenseType == Constant.Retail && !string.IsNullOrEmpty(resultCheckKey.ErrorCode))
                        formMain.tbx_ValidKey.AppendText("ErrorCode: " + resultCheckKey.ErrorCode + "\r\n");
                    if (resultCheckKey.LicenseType == Constant.Volume)
                    {
                        formMain.tbx_ValidKey.AppendText("MAKCount: " + resultCheckKey.MAKCount + "\r\n");
                        if (!string.IsNullOrEmpty(resultCheckKey.ErrorCode))
                            formMain.tbx_ValidKey.AppendText("ErrorCode: " + resultCheckKey.ErrorCode + "\r\n");
                        if (!string.IsNullOrEmpty(resultCheckKey.KeyGetWeb))
                            formMain.tbx_ValidKey.AppendText("Result Get Web: " + resultCheckKey.KeyGetWeb + "\r\n");
                    }

                    if (resultCheckKey.LicenseType.Contains(Constant.OEM))
                    {
                        formMain.tbx_ValidKey.AppendText("MAKCount: " + resultCheckKey.MAKCount + "\r\n");
                        if (!string.IsNullOrEmpty(resultCheckKey.ErrorCode))
                            formMain.tbx_ValidKey.AppendText("ErrorCode: " + resultCheckKey.ErrorCode + "\r\n");
                        if (!string.IsNullOrEmpty(resultCheckKey.KeyGetWeb))
                            formMain.tbx_ValidKey.AppendText("Result Get Web: " + resultCheckKey.KeyGetWeb + "\r\n");
                    }
                    formMain.tbx_ValidKey.AppendText("---------------------\r\n");
                }));
            }

            foreach (var resultCheckKey in listCheckKeyVolumnOrther)
            {
                formMain.tbx_ValidKey.Invoke(new Action(() =>
                {
                    formMain.tbx_ValidKey.AppendText("Key: " + resultCheckKey.Key + "\r\n");
                    formMain.tbx_ValidKey.AppendText("Description: " + resultCheckKey.Description + "\r\n");
                    formMain.tbx_ValidKey.AppendText("SubType: " + resultCheckKey.SubType + "\r\n");
                    formMain.tbx_ValidKey.AppendText("LicenseType: " + resultCheckKey.LicenseType + "\r\n");
                    if (resultCheckKey.LicenseType == Constant.Retail && !string.IsNullOrEmpty(resultCheckKey.ErrorCode))
                        formMain.tbx_ValidKey.AppendText("ErrorCode: " + resultCheckKey.ErrorCode + "\r\n");
                    if (resultCheckKey.LicenseType == Constant.Volume)
                    {
                        formMain.tbx_ValidKey.AppendText("MAKCount: " + resultCheckKey.MAKCount + "\r\n");
                        if (!string.IsNullOrEmpty(resultCheckKey.ErrorCode))
                            formMain.tbx_ValidKey.AppendText("ErrorCode: " + resultCheckKey.ErrorCode + "\r\n");
                        if (!string.IsNullOrEmpty(resultCheckKey.KeyGetWeb))
                            formMain.tbx_ValidKey.AppendText("Result Get Web: " + resultCheckKey.KeyGetWeb + "\r\n");
                    }

                    if (resultCheckKey.LicenseType.Contains(Constant.OEM))
                    {
                        formMain.tbx_ValidKey.AppendText("MAKCount: " + resultCheckKey.MAKCount + "\r\n");
                        if (!string.IsNullOrEmpty(resultCheckKey.ErrorCode))
                            formMain.tbx_ValidKey.AppendText("ErrorCode: " + resultCheckKey.ErrorCode + "\r\n");
                        if (!string.IsNullOrEmpty(resultCheckKey.KeyGetWeb))
                            formMain.tbx_ValidKey.AppendText("Result Get Web: " + resultCheckKey.KeyGetWeb + "\r\n");
                    }
                    formMain.tbx_ValidKey.AppendText("---------------------\r\n");
                }));
            }
        }
    }
}
