using Microsoft.VisualBasic;
using SupportActivate.Common;
using SupportActivate.ProcessSQL;
using System;
using System.IO;
using System.Linq;
using System.Management;
using System.Runtime.InteropServices;
using System.ServiceProcess;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace SupportActivate.ProcessBusiness
{
    public class ProcessPidkey
    {
        ServerSetting serverSetting;
        ServerKey serverKey;
        SourceData sourceData;
        ProcessGetcid processGetcid;
        Validate validate;
        ProcessGetRemainingActivationsOrCID processGetRemainingActivationsOrCID;
        private log4net.ILog logger = log4net.LogManager.GetLogger(typeof(ProcessPidkey));
        private string part = Application.StartupPath + "\\pkeyconfig";
        private static string objLockInstall = "0";
        private static ProcessPidkey globalPIDKey = new ProcessPidkey();

        public ProcessPidkey()
        {
            serverSetting = new ServerSetting();
            serverKey = new ServerKey();
            sourceData = new SourceData();
            processGetcid = new ProcessGetcid();
            validate = new Validate();
            processGetRemainingActivationsOrCID = new ProcessGetRemainingActivationsOrCID();
        }

        public pid CheckKey(string key, string optionPID, bool loadKeyDB, bool saveKey)
        {
            lock (objLockInstall)
            {
                var resultCheckKey_DB = globalPIDKey.CheckKey_DB(loadKeyDB, key);
                if (!string.IsNullOrEmpty(resultCheckKey_DB.Key))
                    return resultCheckKey_DB;
            }

            pid pid = new pid();
            var inforPidkey = Check_Product_Key(key, optionPID);
            if (inforPidkey.validKey)
            {
                var checkOffice = inforPidkey.ds.IndexOf("RTM_");
                if (checkOffice > -1)
                    inforPidkey.ds = "Office14_" + inforPidkey.ds.Substring(3);
                pid.Key = key;
                pid.Description = inforPidkey.ds;
                pid.SubType = inforPidkey.st;
                pid.LicenseType = inforPidkey.lit;
                pid.ErrorCode = string.Empty;
                pid.MAKCount = inforPidkey.count;
                pid.KeyGetWeb = string.Empty;
                lock (objLockInstall)
                    globalPIDKey.serverKey.CreateDataKey(saveKey, pid, string.Empty);
            }
            else
            {
                pid.Key = key;
                pid.Description = Constant.Unsupported_PKeyConfig_InvalidKey;
                pid.SubType = string.Empty;
                pid.LicenseType = string.Empty;
                pid.ErrorCode = string.Empty;
                pid.MAKCount = string.Empty;
                pid.KeyGetWeb = string.Empty;
                lock (objLockInstall)
                    globalPIDKey.serverKey.CreateDataKey(saveKey, pid, string.Empty);
            }
            return pid;
        }

        public pid CheckKeyAdv(string key, string optionPID, bool loadKeyDB, bool saveKey)
        {
            lock (objLockInstall)
            {
                var resultCheckKey_DB = globalPIDKey.CheckKey_DB(loadKeyDB, key);
                if (!string.IsNullOrEmpty(resultCheckKey_DB.Key))
                    return resultCheckKey_DB;
            }

            string getweb = string.Empty;
            pid pid = new pid();
            try
            {
                var inforPidkey = Check_Product_Key(key, optionPID);
                if (inforPidkey.validKey)
                {
                    var checkOffice = inforPidkey.ds.IndexOf("RTM_");
                    if (checkOffice > -1)
                        inforPidkey.ds = "Office14_" + inforPidkey.ds.Substring(3);
                    pid.Key = key;
                    pid.Description = inforPidkey.ds;
                    pid.SubType = inforPidkey.st;
                    pid.LicenseType = inforPidkey.lit;
                    pid.MAKCount = inforPidkey.count;
                    if (inforPidkey.count == Constant.KeyBlock)
                    {
                        pid.ErrorCode = Constant.KeyRetaiBlock;
                        pid.KeyGetWeb = string.Empty;
                        lock (objLockInstall)
                            globalPIDKey.serverKey.CreateDataKey(saveKey, pid, string.Empty);
                    }
                    else if (inforPidkey.lit == Constant.Retail)
                    {
                        lock (objLockInstall)
                        {
                            pid.ErrorCode = checkOffice > -1 ? globalPIDKey.CheckKey2010Retail(key) : globalPIDKey.CheckKeyRetail(key);
                            pid.KeyGetWeb = getweb;
                            globalPIDKey.serverKey.CreateDataKey(saveKey, pid, string.Empty);
                        }
                    }
                    else if (inforPidkey.count == "0" || inforPidkey.count == "-1")
                    {
                        lock (objLockInstall)
                        {
                            pid.ErrorCode = checkOffice > -1 ? globalPIDKey.CheckKey2010Retail(key) : globalPIDKey.CheckKeyRetail(key);
                            if (inforPidkey.count == "0")
                                pid.KeyGetWeb = globalPIDKey.GetIID(key);
                            globalPIDKey.serverKey.CreateDataKey(saveKey, pid, string.Empty);
                        }
                    }
                    else
                    {
                        pid.ErrorCode = string.Empty;
                        pid.MAKCount = inforPidkey.count;
                        pid.KeyGetWeb = string.Empty;
                        lock (objLockInstall)
                            globalPIDKey.serverKey.CreateDataKey(saveKey, pid, string.Empty);
                    }
                }
                else
                {
                    pid.Key = key;
                    pid.Description = Constant.Unsupported_PKeyConfig_InvalidKey;
                    pid.SubType = string.Empty;
                    pid.LicenseType = string.Empty;
                    pid.ErrorCode = string.Empty;
                    pid.MAKCount = string.Empty;
                    pid.KeyGetWeb = string.Empty;
                    lock (objLockInstall)
                        globalPIDKey.serverKey.CreateDataKey(saveKey, pid, string.Empty);
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
            return pid;
        }

        private pid CheckKey_DB(bool loadKeyDB, string key)
        {
            pid pid = new pid();
            if (loadKeyDB)
                return serverKey.SearchKey(key);
            return pid;
        }

        private inforPidkey Check_Product_Key(string serial, string optionPID)
        {
            IntPtr genPID = Marshal.AllocHGlobal(100);
            Marshal.WriteByte(genPID, 0, 0x32);
            int clearGenPID = 0;
            for (clearGenPID = 1; clearGenPID <= 99; clearGenPID++) // Clear out memory space...
                Marshal.WriteByte(genPID, clearGenPID, 0x0);

            IntPtr oldPID = Marshal.AllocHGlobal(164);
            Marshal.WriteByte(oldPID, 0, 0xA4);
            int clearOldPID = 0;
            for (clearOldPID = 1; clearOldPID <= 163; clearOldPID++) // Clear out memory space...
                Marshal.WriteByte(oldPID, clearOldPID, 0x0);

            IntPtr DPID4 = Marshal.AllocHGlobal(1272);
            Marshal.WriteByte(DPID4, 0, 0xF8);
            Marshal.WriteByte(DPID4, 1, 0x4);
            int clearDPID4 = 0;
            for (clearDPID4 = 2; clearDPID4 <= 1271; clearDPID4++) // Clear out memory space...
                Marshal.WriteByte(DPID4, clearDPID4, 0x0);

            string[] txtFilesArray;
            inforPidkey inforPidkey = new inforPidkey();
            try
            {
                var optionCbxPidKeyAndTxtFilesArrayPidKey = sourceData.OptionCbxPidKeyAndTxtFilesArrayPidKey();
                if (optionPID == optionCbxPidKeyAndTxtFilesArrayPidKey.ElementAt(0).Key)
                {
                    // Set location of pkeyconfig.xrm-ms (needed by pidgenx.dll to verify key)...
                    if (Directory.Exists(part) == true)
                    {
                        txtFilesArray = Directory.GetFiles(part + optionCbxPidKeyAndTxtFilesArrayPidKey.ElementAt(1).Value, "*.xrm-ms", SearchOption.AllDirectories);
                        inforPidkey = checkInfo(genPID, oldPID, DPID4, serial, txtFilesArray);
                        if (string.IsNullOrEmpty(inforPidkey.epid))
                        {
                            txtFilesArray = Directory.GetFiles(part + optionCbxPidKeyAndTxtFilesArrayPidKey.ElementAt(6).Value, "*.xrm-ms", SearchOption.AllDirectories);
                            inforPidkey = checkInfo(genPID, oldPID, DPID4, serial, txtFilesArray);
                            if (string.IsNullOrEmpty(inforPidkey.epid))
                            {
                                txtFilesArray = Directory.GetFiles(part, "*.xrm-ms", SearchOption.AllDirectories);
                                if (txtFilesArray.Count() == 0)
                                    inforPidkey.validKey = false;
                                else
                                    inforPidkey = checkInfo(genPID, oldPID, DPID4, serial, txtFilesArray);
                            }
                        }
                    }
                    else
                        inforPidkey.validKey = false;
                    return inforPidkey;
                }
                for (int i = 1; i < optionCbxPidKeyAndTxtFilesArrayPidKey.Count; i++)
                {
                    if (optionPID == optionCbxPidKeyAndTxtFilesArrayPidKey.ElementAt(i).Key)
                    {
                        if (Directory.Exists(part) == true)
                        {
                            txtFilesArray = Directory.GetFiles(part + optionCbxPidKeyAndTxtFilesArrayPidKey.ElementAt(i).Value, "*.xrm-ms", SearchOption.AllDirectories);
                            if (txtFilesArray.Count() == 0)
                                inforPidkey.validKey = false;
                            else
                                inforPidkey = checkInfo(genPID, oldPID, DPID4, serial, txtFilesArray);
                        }
                        else
                            inforPidkey.validKey = false;
                        return inforPidkey;
                    }
                }
                Marshal.FreeHGlobal(genPID);
                Marshal.FreeHGlobal(oldPID);
                Marshal.FreeHGlobal(DPID4);
            }
            catch
            {
                inforPidkey.validKey = false;
            }
            return inforPidkey;
        }

        public void DisabledService()
        {
            try
            {
                object[] disabled = { "Disabled" };
                ManagementObjectCollection wmic;
                string status = string.Empty;
                // Windows Update
                wmic = new ManagementObjectSearcher("SELECT Name, StartMode FROM Win32_Service WHERE Name LIKE '%wuauserv%'").Get();
                foreach (ManagementObject wm in wmic)
                    status = wm.GetPropertyValue("StartMode").ToString();
                if (status == "Manual" | status == "Automatic")
                {
                    wmic = new ManagementObjectSearcher("SELECT Name, StartMode FROM Win32_Service WHERE Name LIKE '%wuauserv%'").Get();
                    foreach (ManagementObject wm in wmic)
                        wm.InvokeMethod("ChangeStartMode", disabled);
                }
                ServiceController wu = new ServiceController("Windows Update");
                if (wu.Status.Equals(ServiceControllerStatus.StartPending) | wu.Status.Equals(ServiceControllerStatus.Running) | wu.Status.Equals(ServiceControllerStatus.ContinuePending) | wu.Status.Equals(ServiceControllerStatus.Paused) | wu.Status.Equals(ServiceControllerStatus.PausePending))
                    wu.Stop();
                wmic = new ManagementObjectSearcher("SELECT Name, StartMode FROM Win32_Service WHERE Name LIKE '%wuauserv%'").Get();
                foreach (ManagementObject wm in wmic)
                    wm.InvokeMethod("ChangeStartMode", disabled);

                // License Manager
                string stattusLM = string.Empty;
                wmic = new ManagementObjectSearcher("SELECT Name, StartMode FROM Win32_Service WHERE Name LIKE '%LicenseManager%'").Get();
                foreach (ManagementObject wm in wmic)
                    stattusLM = wm.GetPropertyValue("StartMode").ToString();
                if (stattusLM == "Manual" | stattusLM == "Automatic")
                {
                    wmic = new ManagementObjectSearcher("SELECT Name, StartMode FROM Win32_Service WHERE Name LIKE '%LicenseManager%'").Get();
                    foreach (ManagementObject wm in wmic)
                        wm.InvokeMethod("ChangeStartMode", disabled);
                }
                ServiceController wlms = new ServiceController("Windows License Manager Service");
                if (wlms.Status.Equals(ServiceControllerStatus.StartPending) | wlms.Status.Equals(ServiceControllerStatus.Running) | wlms.Status.Equals(ServiceControllerStatus.ContinuePending) | wlms.Status.Equals(ServiceControllerStatus.Paused) | wlms.Status.Equals(ServiceControllerStatus.PausePending))
                    wlms.Stop();
                wmic = new ManagementObjectSearcher("SELECT Name, StartMode FROM Win32_Service WHERE Name LIKE '%LicenseManager%'").Get();
                foreach (ManagementObject wm in wmic)
                    wm.InvokeMethod("ChangeStartMode", disabled);
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                MessageBox.Show(Messages.CannotDisabledService, Messages.error, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void EnabledService()
        {
            try
            {
                object[] automatic = { "Automatic" };
                ManagementObjectCollection wmic;
                // Windows Update
                string status = string.Empty;
                wmic = new ManagementObjectSearcher("SELECT Name, StartMode FROM Win32_Service WHERE Name LIKE '%wuauserv%'").Get();
                foreach (ManagementObject wm in wmic)
                    status = wm.GetPropertyValue("StartMode").ToString();
                if (status == "Disabled" | status == "Manual")
                {
                    wmic = new ManagementObjectSearcher("SELECT Name, StartMode FROM Win32_Service WHERE Name LIKE '%wuauserv%'").Get();
                    foreach (ManagementObject wm in wmic)
                        wm.InvokeMethod("ChangeStartMode", automatic);
                }
                ServiceController wu = new ServiceController("Windows Update");
                if (wu.Status.Equals(ServiceControllerStatus.Stopped) | wu.Status.Equals(ServiceControllerStatus.StopPending))
                    wu.Start();
                wmic = new ManagementObjectSearcher("SELECT Name, StartMode FROM Win32_Service WHERE Name LIKE '%wuauserv%'").Get();
                foreach (ManagementObject wm in wmic)
                    wm.InvokeMethod("ChangeStartMode", automatic);

                // License Manager
                string stattusLM = string.Empty;
                wmic = new ManagementObjectSearcher("SELECT Name, StartMode FROM Win32_Service WHERE Name LIKE '%LicenseManager%'").Get();
                foreach (ManagementObject wm in wmic)
                    stattusLM = wm.GetPropertyValue("StartMode").ToString();
                if (stattusLM == "Disabled" | stattusLM == "Manual")
                {
                    wmic = new ManagementObjectSearcher("SELECT Name, StartMode FROM Win32_Service WHERE Name LIKE '%LicenseManager%'").Get();
                    foreach (ManagementObject wm in wmic)
                        wm.InvokeMethod("ChangeStartMode", automatic);
                }
                ServiceController wlms = new ServiceController("Windows License Manager Service");
                if (wlms.Status.Equals(ServiceControllerStatus.Stopped) | wlms.Status.Equals(ServiceControllerStatus.StopPending))
                    wlms.Start();
                wmic = new ManagementObjectSearcher("SELECT Name, StartMode FROM Win32_Service WHERE Name LIKE '%LicenseManager%'").Get();
                foreach (ManagementObject wm in wmic)
                    wm.InvokeMethod("ChangeStartMode", automatic);
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
        }

        private string CheckErrorcode(string code)
        {
            string maLoi;
            if (code == Constant.KeyRetai4C008)
                maLoi = Constant.KeyRetai4C008;
            else if (code == "0xC004C020")
                maLoi = "0xC004C020";
            else if (code == "0xC004C770" || code == "0xC004FC03" || code == "0x803FA071")
                maLoi = "By phone";
            else if (code == "0xC004C060" || code == "0xC004C003" || code == "0xC004C004" || code == "0xC004C4A2" || code == "0xC004F004" || code == "0xC004C007")
                maLoi = Constant.KeyRetaiBlock;
            else if (code == "0xC004F005" || code == "0xC004C00F" || code == "0xC004B001" || code == "0x803FA067L" || code == "0xC004F061" || code == "0xC004C001")
                maLoi = Constant.KeyRetaiBlock;
            else
                maLoi = code;
            return maLoi;
        }

        private string CheckKeyRetail(string key)
        {
            string errorcode = string.Empty;
            try
            {
                object[] array_key = { key };
                ManagementObjectCollection wmic = new ManagementObjectSearcher("SELECT Version FROM SoftwareLicensingService").Get();
                foreach (ManagementObject wmi in wmic)
                {
                    try
                    {
                        wmi.InvokeMethod("InstallProductKey", array_key);
                    }
                    catch (COMException ex)
                    {
                        errorcode = "0x" + ex.ErrorCode.ToString("X");
                    }
                }
                string last5 = key.Substring(24, 5); // 5 kí tự cuối của key
                wmic = new ManagementObjectSearcher("SELECT ID FROM SoftwareLicensingProduct WHERE PartialProductKey='" + last5 + "'").Get();
                foreach (ManagementObject wmi in wmic)
                {
                    try
                    {
                        wmi.InvokeMethod("Activate", null/* TODO Change to default(_) if this is not a reference type */);
                        errorcode = Constant.Online;
                    }
                    catch (COMException ex)
                    {
                        errorcode = "0x" + ex.ErrorCode.ToString("X");
                    }
                }
            }
            catch (Exception ex)
            {
                errorcode = "No service found or running the wrong version of the application.";
                logger.Error(ex);
            }
            if (errorcode == "0xC004F050")
                errorcode = CheckKey2010Retail(key);
            return CheckErrorcode(errorcode);
        }

        private string CheckKey2010Retail(string key)
        {
            string errorcode = string.Empty;
            try
            {
                object[] array_key = { key };
                ManagementObjectCollection wmic = new ManagementObjectSearcher("SELECT Version FROM OfficeSoftwareProtectionService").Get();
                foreach (ManagementObject wmi in wmic)
                {
                    try
                    {
                        wmi.InvokeMethod("InstallProductKey", array_key);
                    }
                    catch (COMException ex)
                    {
                        errorcode = "0x" + ex.ErrorCode.ToString("X");
                    }
                }
                string last5 = key.Substring(24, 5); // 5 kí tự cuối của key
                wmic = new ManagementObjectSearcher("SELECT ID FROM OfficeSoftwareProtectionProduct WHERE PartialProductKey='" + last5 + "'").Get();
                foreach (ManagementObject wmi in wmic)
                {
                    try
                    {
                        wmi.InvokeMethod("Activate", null/* TODO Change to default(_) if this is not a reference type */);
                        errorcode = Constant.Online;
                    }
                    catch (COMException ex)
                    {
                        errorcode = "0x" + ex.ErrorCode.ToString("X");
                    }
                }
            }
            catch
            {
                errorcode = "No service found or running the wrong version of the application.";
            }
            return CheckErrorcode(errorcode);
        }

        private string GetIID(string key)
        {
            string cid, iid = string.Empty, getWeb = string.Empty;
            if (string.IsNullOrEmpty(serverSetting.GetKEYAPI()))
                return string.Empty;
            ManagementObjectCollection wmic;
            try
            {
                string last5 = key.Substring(24, 5); // 5 kí tự cuối của key
                // Lấy IID:
                wmic = new ManagementObjectSearcher("SELECT OfflineInstallationId FROM SoftwareLicensingProduct WHERE PartialProductKey='" + last5 + "'").Get();
                // Lưu ý :  Trong đó XXXXX là 5 ký tự cuối của key vừa nạp vào
                foreach (ManagementObject wmi in wmic)
                    iid = wmi.GetPropertyValue("OfflineInstallationId").ToString();
                cid = processGetcid.GetConfirmationID(iid, serverSetting.GetKEYAPI());
                if (validate.ValidateCID(cid))
                    getWeb = "Get Web";
                else if (cid == Constant.Need_to_call)
                    getWeb = "Call";
                else if (cid == Constant.Blocked_IID)
                    getWeb = Constant.KeyRetaiBlock;
                else
                    getWeb = cid;
            }
            catch (Exception ex)
            {
                getWeb = "Network Error";
                logger.Error(ex);
            }
            return getWeb;
        }

        [DllImport(@"x64\pidgenx.dll", CharSet = CharSet.Auto)]
        static extern int PidGenX(string one, string two, string three, int four, IntPtr five, IntPtr six, IntPtr seven);

        private inforPidkey checkInfo(IntPtr genPID, IntPtr oldPID, IntPtr DPID4, string serial, string[] txtFilesArray)
        {
            string epid = string.Empty, aid = string.Empty, edi = string.Empty, ds = string.Empty, st = string.Empty, lit = string.Empty, count = string.Empty;
            Encoding enc = Encoding.ASCII;
            bool validKey = false;
            try
            {
                foreach (string abc in txtFilesArray)
                {
                    // Call PidGenX() to determine if key is valid...
                    int RetID = PidGenX(serial, abc, "XXXXX", 0, genPID, oldPID, DPID4);
                    // Check returned value 'RetID' for valid key..
                    if (RetID == 0)
                    {
                        byte[] pidb = new byte[99];
                        for (int i = 0; i <= pidb.Length - 1; i++)
                            pidb[i] = Marshal.ReadByte(genPID, i);
                        byte[] core = new byte[1272];

                        for (int i = 0; i < core.Length; i++)
                            core[i] = Marshal.ReadByte(DPID4, i);
                        //epid = enc.GetString(core, 8, 6).Replace(Constants.vbNullChar, ""); // Extendend PID 
                        epid = enc.GetString(core, 0x0008, 98).Replace(Constants.vbNullChar, "");
                        epid = epid.Replace("XXXXX", "55041");
                        aid = enc.GetString(core, 136, 72).Replace(Constants.vbNullChar, ""); // Activation ID 
                        edi = enc.GetString(core, 280, 40).Replace(Constants.vbNullChar, ""); // Edition
                        ds = GetProductDescription(abc, "{" + aid + "}", edi);
                        st = enc.GetString(core, 888, 30).Replace(Constants.vbNullChar, ""); // SubType
                        lit = enc.GetString(core, 1016, 25).Replace(Constants.vbNullChar, ""); // License Type
                        if (lit != Constant.Retail)
                        {
                            count = processGetRemainingActivationsOrCID.GetRemainingActivationsOrCID(2, string.Empty, epid);
                        }
                        else
                            count = string.Empty;

                        validKey = true;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                ds = Constant.error;
                Console.WriteLine(ex.ToString());
            }
            inforPidkey inforPidkey = new inforPidkey();
            inforPidkey.epid = epid;
            inforPidkey.aid = aid;
            inforPidkey.edi = edi;
            inforPidkey.ds = ds;
            inforPidkey.st = st;
            inforPidkey.lit = lit;
            inforPidkey.count = count;
            inforPidkey.validKey = validKey;
            return inforPidkey;
        }

        private static string GetProductDescription(string pkey, string aid, string edi)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(pkey);
                using (MemoryStream stream = new MemoryStream(Convert.FromBase64String(doc.GetElementsByTagName("tm:infoBin")[0].InnerText)))
                {
                    doc.Load(stream);
                    XmlNamespaceManager ns = new XmlNamespaceManager(doc.NameTable);
                    ns.AddNamespace("pkc", "http://www.microsoft.com/DRM/PKEY/Configuration/2.0");
                    XmlNode node = doc.SelectSingleNode("/pkc:ProductKeyConfiguration/pkc:Configurations/pkc:Configuration[pkc:ActConfigId='" + aid + "']", ns);
                    bool flag = node == null;

                    if (flag)
                        node = doc.SelectSingleNode("/pkc:ProductKeyConfiguration/pkc:Configurations/pkc:Configuration[pkc:ActConfigId='" + aid.ToUpper() + "']", ns);

                    bool flag2 = node != null && node.HasChildNodes;

                    if (flag2)
                    {
                        bool flag3 = node.ChildNodes[2].InnerText.Contains(edi);

                        if (flag3)
                            return node.ChildNodes[3].InnerText;
                        else
                            return "Not Found";
                    }
                    else
                        return "Not Found"; 
                }
            }
            catch
            {
                return "Not Found";
            }
        }
    }
}
