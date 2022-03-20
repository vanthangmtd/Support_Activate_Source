using System.Collections.Generic;

namespace SupportActivate.Common
{
    public class SourceData
    {
        public List<string> OptionCbxWindowsOffice()
        {
            List<string> listOption = new List<string>();
            listOption.Add("Windows/Office");
            listOption.Add("Activate Windows Online");
            listOption.Add("Activate Windows Byphone");
            listOption.Add("Activate Windows Byphone For Win7/8/8.1");
            listOption.Add("Activate Windows Byphone All"); //4
            listOption.Add("Update Windows 10 Home To Windows 10 Pro");
            listOption.Add("Activate Office Online");
            listOption.Add("Activate Office Byphone All"); //7
            listOption.Add("Activate Office 2010 Byphone");
            listOption.Add("Activate Office 2013 Byphone");
            listOption.Add("Activate Office 2016/2019/2021 Byphone");
            listOption.Add("Activate Office 2019 Volume");
            listOption.Add("Activate Office 2021 Volume");
            return listOption;
        }

        public List<string> OptionCbxCID()
        {
            List<string> listOption = new List<string>();
            listOption.Add("Windows/Office");
            listOption.Add("Windows");
            listOption.Add("Office All");
            listOption.Add("Office 2010");
            listOption.Add("Office 2013");
            listOption.Add("Office 2016/2019/2021");
            return listOption;
        }

        public List<string> OptionCbxCheckRemove()
        {
            List<string> listOption = new List<string>();
            listOption.Add("Check/Remove");
            listOption.Add("Check Licensed Window");
            listOption.Add("Check Licensed Office All");
            listOption.Add("Check Licensed Office 2010");
            listOption.Add("Check Licensed Office 2013");
            listOption.Add("Check Licensed Office 2016/2019/2021");
            listOption.Add("Remove Key Office All");
            listOption.Add("Remove Key Office");
            return listOption;
        }

        public Dictionary<string, string> OptionCbxPidKeyAndTxtFilesArrayPidKey()
        {
            Dictionary<string, string> listOption = new Dictionary<string, string>();
            listOption.Add("All Edition Windows/Office", "All Edition Windows/Office");//0
            listOption.Add("Windows 7 - Server 2008 R2 - Embedded 7", @"\windows7-server2008r2-thinpc-embsta-winPosReady7");//1
            listOption.Add("Windows 8 - Server 2012 - Embedded 8", @"\windows8-server2012-winemb8");//2
            listOption.Add("Windows 8.1 - Server 2012 R2", @"\windows8.1-server2012r2");//3
            listOption.Add("Windows 10 - Server 2016/2019", @"\windows10-server2016-2019");//4
            listOption.Add("Windows 11 - Server 2021", @"\windows11-server2021");//5
            listOption.Add("Office 2010", @"\office2010");//6
            listOption.Add("Office 2013", @"\office2013");//7
            listOption.Add("Office 2016", @"\office2016");//8
            listOption.Add("Office 2019", @"\office2019");//9
            listOption.Add("Office 2021", @"\office2021");//10
            listOption.Add("Windows Vista - Server 2008", @"\windowsvista-server2008");//11
            listOption.Add("SQL", @"\sql");//12
            listOption.Add("Visual Studio", @"\vs");//13
            listOption.Add("Other", @"\other");//14
            return listOption;
        }

        public string ScriptWindowsOffice(int luaChon, string key, string tokenApi)
        {
            Dictionary<int, string> script = new Dictionary<int, string>();
            script.Add(1, @"cd %windir%\system32" + "\r\n" +
                        @"set k1=" + key + "" + "\r\n" +
                        @"cls" + "\r\n" +
                        @"cscript slmgr.vbs /rilc" + "\r\n" +
                        @"cscript slmgr.vbs /upk" + "\r\n" +
                        @"cscript slmgr.vbs /cpky" + "\r\n" +
                        @"cscript slmgr.vbs /ckms" + "\r\n" +
                        @"sc config Winmgmt start=demand & net start Winmgmt" + "\r\n" +
                        @"sc config LicenseManager start= auto & net start LicenseManager" + "\r\n" +
                        @"sc config wuauserv start= auto & net start wuauserv" + "\r\n" +
                        @"@echo on&mode con: cols=20 lines=2" + "\r\n" +
                        @"cscript slmgr.vbs /ipk %k1%" + "\r\n" +
                        @"@mode con: cols=100 lines=30" + "\r\n" +
                        @"clipup -v -o -altto c:\" + "\r\n" +
                        @"cscript slmgr.vbs -ato" + "\r\n" +
                        @"start ms-settings:activation" + "\r\n");
            script.Add(2, @"cd %windir%\system32" + "\r\n" +
                        @"set k1=" + key + "" + "\r\n" +
                        @"cls" + "\r\n" +
                        @"cscript slmgr.vbs /rilc" + "\r\n" +
                        @"cscript slmgr.vbs /upk" + "\r\n" +
                        @"cscript slmgr.vbs /ckms" + "\r\n" +
                        @"cscript slmgr.vbs /cpky" + "\r\n" +
                        @"sc config Winmgmt start=demand & net start Winmgmt" + "\r\n" +
                        @"sc config LicenseManager start=auto & net start LicenseManager" + "\r\n" +
                        @"sc config wuauserv start=auto & sc start wuauserv" + "\r\n" +
                        @"@echo on&mode con: cols=20 lines=2" + "\r\n" +
                        @"cscript slmgr.vbs /ipk %k1%" + "\r\n" +
                        @"@mode con: cols=100 lines=30" + "\r\n" +
                        @"cscript slmgr.vbs /dti>C:\IID.txt" + "\r\n" +
                        @"start C:\IID.txt" + "\r\n");
            script.Add(3, @"cd %windir%\system32" + "\r\n" +
                        @"set key=" + key + "" + "\r\n" +
                        @"cls" + "\r\n" +
                        @"for /f ""tokens=6 delims=[.] "" %a in ('ver') do set ver=%a" + "\r\n" +
                        @"cscript slmgr.vbs /rilc >nul 2>&1" + "\r\n" +
                        @"cscript slmgr.vbs /upk >nul 2>&1" + "\r\n" +
                        @"cscript slmgr.vbs /ckms >nul 2>&1" + "\r\n" +
                        @"cscript slmgr.vbs /cpky >nul 2>&1" + "\r\n" +
                        @"@echo on&mode con: cols=20 lines=2" + "\r\n" +
                        @"cscript slmgr.vbs /ipk %key%" + "\r\n" +
                        @"@mode con: cols=100 lines=30" + "\r\n" +
                        @"cscript slmgr.vbs /dti>C:\IID.txt" + "\r\n" +
                        @"start C:\IID.txt" + "\r\n");
            script.Add(4, @"cd %windir%\system32" + "\r\n" +
                          @"@echo on&mode con: cols=20 lines=2" + "\r\n" +
                          @"set codes=" + tokenApi + "" + "\r\n" +
                          @"set key=" + key + "" + "\r\n" +
                          @"echo off | clip&cls" + "\r\n" +
                          @"sc config LicenseManager start= auto & net start LicenseManager" + "\r\n" +
                          @"sc config wuauserv start= auto & net start wuauserv" + "\r\n" +
                          @"cscript slmgr.vbs /rilc" + "\r\n" +
                          @"cscript slmgr.vbs /upk" + "\r\n" +
                          @"cscript slmgr.vbs /cpky" + "\r\n" +
                          @"cscript slmgr.vbs /ckms" + "\r\n" +
                          @"cscript slmgr.vbs /ipk %key%&cls" + "\r\n" +
                          @"for /f ""tokens=3"" %b in ('cscript.exe %windir%\system32\slmgr.vbs /dti') do set IID=%b" + "\r\n" +
                          @"for /f ""tokens=*"" %b in ('powershell -Command ""$req = [System.Net.WebRequest]::Create('https://getcid.info/api/%IID%/%codes%');$resp = New-Object System.IO.StreamReader $req.GetResponse().GetResponseStream(); $resp.ReadToEnd()""') do set ACID=%b" + "\r\n" +
                          @"set CID=%ACID:~1,48%" + "\r\n" +
                          @"cscript slmgr.vbs /atp %CID%" + "\r\n" +
                          @"cscript slmgr.vbs /ato" + "\r\n" +
                          @"for /f ""tokens=2,3,4,5,6 usebackq delims=: / "" %%a in ('%date% %time%') do echo %%c-%%a-%%b %%d%%e" + "\r\n" +
                          @"echo DATE: %date% >status.txt" + "\r\n" +
                          @"echo TIME: %time% >>status.txt" + "\r\n" +
                          @"echo IID: %IID% >>status.txt" + "\r\n" +
                          @"echo CID: %ACID% >>status.txt" + "\r\n" +
                          @"cscript slmgr.vbs /dli >>status.txt" + "\r\n" +
                          @"cscript slmgr.vbs /xpr >>status.txt" + "\r\n" +
                          @"start status.txt" + "\r\n" +
                          @"start ms-settings:activation" + "\r\n" +
                          @"@mode con: cols=100 lines=30" + "\r\n");
            script.Add(5, @"sc config LicenseManager start= auto & net start LicenseManager" + "\r\n" +
                            @"sc config wuauserv start= auto & net start wuauserv" + "\r\n" +
                            @"changepk.exe /productkey VK7JG-NPHTM-C97JM-9MPGT-3V66T" + "\r\n" +
                            @"exit" + "\r\n");
            script.Add(6, @"set k1=" + key + "" + "\r\n" +
                        @"cls" + "\r\n" +
                        @"for %a in (4,5,6) do (if exist ""%ProgramFiles%\Microsoft Office\Office1%a\ospp.vbs"" (cd /d ""%ProgramFiles%\Microsoft Office\Office1%a"")" + "\r\n" +
                        @"if exist ""%ProgramFiles% (x86)\Microsoft Office\Office1%a\ospp.vbs"" (cd /d ""%ProgramFiles% (x86)\Microsoft Office\Office1%a""))" + "\r\n" +
                        @"@echo on&mode con: cols=20 lines=2" + "\r\n" +
                        @"cscript OSPP.VBS /inpkey:%k1%" + "\r\n" +
                        @"@mode con: cols=100 lines=30" + "\r\n" +
                        @"cscript ospp.vbs /act" + "\r\n");
            script.Add(7, @"for %a in (4,5,6) do (if exist ""%ProgramFiles%\Microsoft Office\Office1%a\ospp.vbs"" (cd /d ""%ProgramFiles%\Microsoft Office\Office1%a"")" + "\r\n" +
                          @"If exist ""%ProgramFiles% (x86)\Microsoft Office\Office1%a\ospp.vbs"" (cd /d ""%ProgramFiles% (x86)\Microsoft Office\Office1%a""))" + "\r\n" +
                          @"for /f ""tokens= 8"" %b in ('cscript //nologo OSPP.VBS /dstatus ^| findstr /b /c:""Last 5""') do (cscript //nologo ospp.vbs /unpkey:%b)" + "\r\n" +
                          @"@echo on&mode con: cols=20 lines=2" + "\r\n" +
                          @"set codes=" + tokenApi + "" + "\r\n" +
                          @"set key=" + key + "" + "\r\n" +
                          @"echo off | clip&cls" + "\r\n" +
                          @"for %a in (4,5,6) do (if exist ""%ProgramFiles%\Microsoft Office\Office1%a\ospp.vbs"" (cd /d ""%ProgramFiles%\Microsoft Office\Office1%a"")" + "\r\n" +
                          @"if exist ""%ProgramFiles(x86)%\Microsoft Office\Office1%a\ospp.vbs"" (cd /d ""%ProgramFiles(x86)%\Microsoft Office\Office1%a""))&cls" + "\r\n" +
                          @"cscript ospp.vbs /inpkey:%key%" + "\r\n" +
                          @"for /f ""tokens=8"" %b in ('cscript ospp.vbs /dinstid ^| findstr /b /c:""Installation ID""') do set IID=%b" + "\r\n" +
                          @"for /f ""tokens=*"" %b in ('powershell -Command ""$req = [System.Net.WebRequest]::Create('https://getcid.info/api/%IID%/%codes%');$resp = New-Object System.IO.StreamReader $req.GetResponse().GetResponseStream(); $resp.ReadToEnd()""') do set ACID=%b" + "\r\n" +
                          @"set CID=%ACID:~1,48%" + "\r\n" +
                          @"cscript ospp.vbs /actcid:%CID%" + "\r\n" +
                          @"cscript ospp.vbs /act" + "\r\n" +
                          @"for /f ""tokens=2,3,4,5,6 usebackq delims=:/ "" %%a in ('%date% %time%') do echo %%c-%%a-%%b %%d%%e" + "\r\n" +
                          @"echo DATE: %date% >status.txt" + "\r\n" +
                          @"echo TIME: %time% >>status.txt" + "\r\n" +
                          @"echo IID: %IID%>>status.txt" + "\r\n" +
                          @"echo CID: %ACID%>>status.txt" + "\r\n" +
                          @"cscript ospp.vbs /dstatus >>status.txt" + "\r\n" +
                          @"start status.txt" + "\r\n" +
                          @"@mode con: cols=100 lines=30" + "\r\n");
            script.Add(8, @"if exist ""%ProgramFiles%\Microsoft Office\Office14\ospp.vbs"" (cd /d ""%ProgramFiles%\Microsoft Office\Office14"")" + "\r\n" +
                        @"if exist ""%ProgramFiles(x86)%\Microsoft Office\Office14\ospp.vbs"" (cd /d ""%ProgramFiles(x86)%\Microsoft Office\Office14"")" + "\r\n" +
                        @"set k1=" + key + "" + "\r\n" +
                        @"cls" + "\r\n" +
                        @"@echo on&mode con: cols=20 lines=2" + "\r\n" +
                        @"cscript ospp.vbs /inpkey:%k1%" + "\r\n" +
                        @"@mode con: cols=100 lines=30" + "\r\n" +
                        @"cscript ospp.vbs /dinstid>id.txt " + "\r\n" +
                        @"start id.txt" + "\r\n");
            script.Add(9, @"if exist ""%ProgramFiles%\Microsoft Office\Office15\ospp.vbs"" (cd /d ""%ProgramFiles%\Microsoft Office\Office15"")" + "\r\n" +
                        @"if exist ""%ProgramFiles(x86)%\Microsoft Office\Office15\ospp.vbs"" (cd /d ""%ProgramFiles(x86)%\Microsoft Office\Office15"")" + "\r\n" +
                        @"set k1=" + key + "" + "\r\n" +
                        @"cls" + "\r\n" +
                        @"@echo on&mode con: cols=20 lines=2" + "\r\n" +
                        @"cscript ospp.vbs /inpkey:%k1%" + "\r\n" +
                        @"@mode con: cols=100 lines=30" + "\r\n" +
                        @"cscript ospp.vbs /dinstid>id.txt" + "\r\n" +
                        @"start id.txt" + "\r\n");
            script.Add(10, @"if exist ""%ProgramFiles%\Microsoft Office\Office16\ospp.vbs"" (cd /d ""%ProgramFiles%\Microsoft Office\Office16"")" + "\r\n" +
                        @"if exist ""%ProgramFiles(x86)%\Microsoft Office\Office16\ospp.vbs"" (cd /d ""%ProgramFiles(x86)%\Microsoft Office\Office16"")" + "\r\n" +
                        @"set k1=" + key + "" + "\r\n" +
                        @"cls" + "\r\n" +
                        @"@echo on&mode con: cols=20 lines=2" + "\r\n" +
                        @"cscript ospp.vbs /inpkey:%k1%" + "\r\n" +
                        @"@mode con: cols=100 lines=30" + "\r\n" +
                        @"cscript ospp.vbs /dinstid>id.txt " + "\r\n" +
                        @"start id.txt" + "\r\n");
            script.Add(11, @"set k1=" + key + "" + "\r\n" +
                        @"cls" + "\r\n" +
                        @"if exist ""%ProgramFiles%\Microsoft Office\Office16\ospp.vbs"" cd /d ""%ProgramFiles%\Microsoft Office\Office16""" + "\r\n" +
                        @"if exist ""%ProgramFiles(x86)%\Microsoft Office\Office16\ospp.vbs"" cd /d ""%ProgramFiles(x86)%\Microsoft Office\Office16""" + "\r\n" +
                        @"for /f %i in ('dir /b ..\root\Licenses16\ProPlus2019VL_MAK_AE*.xrm-ms') do cscript ospp.vbs /inslic:""..\root\Licenses16\%i""" + "\r\n" +
                        @"for /f %i in ('dir /b ..\root\Licenses16\ProPlus2019VL_KMS_Client_AE*.xrm-ms') do cscript ospp.vbs /inslic:""..\root\Licenses16\%i""" + "\r\n" +
                        @"@echo on&mode con: cols=20 lines=2" + "\r\n" +
                        @"cscript OSPP.VBS /inpkey:%k1%" + "\r\n" +
                        @"@mode con: cols=100 lines=30" + "\r\n" +
                        @"cscript ospp.vbs /act" + "\r\n");
            script.Add(12, @"set k1=" + key + "" + "\r\n" +
                    @"cls" + "\r\n" +
                    @"if exist ""%ProgramFiles%\Microsoft Office\Office16\ospp.vbs"" cd /d ""%ProgramFiles%\Microsoft Office\Office16""" + "\r\n" +
                    @"if exist ""%ProgramFiles(x86)%\Microsoft Office\Office16\ospp.vbs"" cd /d ""%ProgramFiles(x86)%\Microsoft Office\Office16""" + "\r\n" +
                    @"for /f %i in ('dir /b ..\root\Licenses16\ProPlus2021VL_MAK_AE*.xrm-ms') do cscript ospp.vbs /inslic:""..\root\Licenses16\%i""" + "\r\n" +
                    @"for /f %i in ('dir /b ..\root\Licenses16\ProPlus2021VL_KMS_Client_AE*.xrm-ms') do cscript ospp.vbs /inslic:""..\root\Licenses16\%i""" + "\r\n" +
                    @"@echo on&mode con: cols=20 lines=2" + "\r\n" +
                    @"cscript OSPP.VBS /inpkey:%k1%" + "\r\n" +
                    @"@mode con: cols=100 lines=30" + "\r\n" +
                    @"cscript ospp.vbs /act" + "\r\n");
            switch (luaChon)
            {
                case 1:
                    return script[1];
                case 2:
                    return script[2];
                case 3:
                    return script[3];
                case 4:
                    return script[4];
                case 5:
                    return script[5];
                case 6:
                    return script[6];
                case 7:
                    return script[7];
                case 8:
                    return script[8];
                case 9:
                    return script[9];
                case 10:
                    return script[10];
                case 11:
                    return script[11];
                case 12:
                    return script[12];
                default:
                    return string.Empty;
            }
        }

        public string ScriptCID(int luaChon, string CIDUI)
        {
            Dictionary<int, string> script = new Dictionary<int, string>();
            script.Add(1, @"cd %windir%\system32" + "\r\n" +
                    @"set CID=" + CIDUI + "" + "\r\n" +
                    @"cscript slmgr.vbs /atp %CID%" + "\r\n" +
                    @"cscript slmgr.vbs /ato" + "\r\n" +
                    @"for /f ""tokens=2,3,4,5,6 usebackq delims=: / "" %%a in ('%date% %time%') do echo %%c-%%a-%%b %%d%%e" + "\r\n" +
                    @"echo DATE: %date% >status.txt" + "\r\n" +
                    @"echo TIME: %time% >>status.txt" + "\r\n" +
                    @"for /f ""tokens=3"" %b in ('cscript.exe %windir%\system32\slmgr.vbs /dti') do set IID=%b" + "\r\n" +
                    @"echo IID: %IID% >>status.txt" + "\r\n" +
                    @"echo CID: " + CIDUI + " >>status.txt" + "\r\n" +
                    @"cscript slmgr.vbs /dli >>status.txt" + "\r\n" +
                    @"cscript slmgr.vbs /xpr >>status.txt" + "\r\n" +
                    @"start status.txt" + "\r\n");
            script.Add(2, @"for %a in (4,5,6) do (if exist ""%ProgramFiles%\Microsoft Office\Office1%a\ospp.vbs"" (cd /d ""%ProgramFiles%\Microsoft Office\Office1%a"")" + "\r\n" +
                        @"if exist ""%ProgramFiles% (x86)\Microsoft Office\Office1%a\ospp.vbs"" (cd /d ""%ProgramFiles% (x86)\Microsoft Office\Office1%a""))" + "\r\n" +
                        @"set CID=" + CIDUI + "" + "\r\n" +
                        @"cscript //nologo OSPP.VBS /actcid:%CID%" + "\r\n" +
                        @"cscript.exe OSPP.vbs /act" + "\r\n" +
                        @"for /f ""tokens=2,3,4,5,6 usebackq delims=:/ "" %%a in ('%date% %time%') do echo %%c-%%a-%%b %%d%%e" + "\r\n" +
                        @"echo DATE: %date% >status.txt" + "\r\n" +
                        @"echo TIME: %time% >>status.txt" + "\r\n" +
                        @"for /f ""tokens=8"" %b in ('cscript ospp.vbs /dinstid ^| findstr /b /c:""Installation ID""') do set IID=%b" + "\r\n" +
                        @"echo IID: %IID%>>status.txt" + "\r\n" +
                        @"echo CID: " + CIDUI + ">>status.txt" + "\r\n" +
                        @"cscript ospp.vbs /dstatus >>status.txt" + "\r\n" +
                        @"start status.txt" + "\r\n");
            script.Add(3, @"if exist ""%ProgramFiles%\Microsoft Office\Office14\ospp.vbs"" (cd /d ""%ProgramFiles%\Microsoft Office\Office14"")" + "\r\n" +
                        @"if exist ""%ProgramFiles(x86)%\Microsoft Office\Office14\ospp.vbs"" (cd /d ""%ProgramFiles(x86)%\Microsoft Office\Office14"")" + "\r\n" +
                        @"set CID=" + CIDUI + "" + "\r\n" +
                        @"cscript //nologo OSPP.VBS /actcid:%CID%" + "\r\n" +
                        @"cscript.exe OSPP.vbs /act" + "\r\n" +
                        @"for /f ""tokens=2,3,4,5,6 usebackq delims=:/ "" %%a in ('%date% %time%') do echo %%c-%%a-%%b %%d%%e" + "\r\n" +
                        @"echo DATE: %date% >status.txt" + "\r\n" +
                        @"echo TIME: %time% >>status.txt" + "\r\n" +
                        @"for /f ""tokens=8"" %b in ('cscript ospp.vbs /dinstid ^| findstr /b /c:""Installation ID""') do set IID=%b" + "\r\n" +
                        @"echo IID: %IID%>>status.txt" + "\r\n" +
                        @"echo CID: " + CIDUI + ">>status.txt" + "\r\n" +
                        @"cscript ospp.vbs /dstatus >>status.txt" + "\r\n" +
                        @"start status.txt" + "\r\n");
            script.Add(4, @"if exist ""%ProgramFiles%\Microsoft Office\Office15\ospp.vbs"" (cd /d ""%ProgramFiles%\Microsoft Office\Office15"")" + "\r\n" +
                        @"if exist ""%ProgramFiles(x86)%\Microsoft Office\Office15\ospp.vbs"" (cd /d ""%ProgramFiles(x86)%\Microsoft Office\Office15"")" + "\r\n" +
                        @"set CID=" + CIDUI + "" + "\r\n" +
                        @"cscript //nologo OSPP.VBS /actcid:%CID%" + "\r\n" +
                        @"cscript.exe OSPP.vbs /act" + "\r\n" +
                        @"for /f ""tokens=2,3,4,5,6 usebackq delims=:/ "" %%a in ('%date% %time%') do echo %%c-%%a-%%b %%d%%e" + "\r\n" +
                        @"echo DATE: %date% >status.txt" + "\r\n" +
                        @"echo TIME: %time% >>status.txt" + "\r\n" +
                        @"for /f ""tokens=8"" %b in ('cscript ospp.vbs /dinstid ^| findstr /b /c:""Installation ID""') do set IID=%b" + "\r\n" +
                        @"echo IID: %IID%>>status.txt" + "\r\n" +
                        @"echo CID: " + CIDUI + ">>status.txt" + "\r\n" +
                        @"cscript ospp.vbs /dstatus >>status.txt" + "\r\n" +
                        @"start status.txt" + "\r\n");
            script.Add(5, @"if exist ""%ProgramFiles%\Microsoft Office\Office16\ospp.vbs"" (cd /d ""%ProgramFiles%\Microsoft Office\Office16"")" + "\r\n" +
                        @"if exist ""%ProgramFiles(x86)%\Microsoft Office\Office16\ospp.vbs"" (cd /d ""%ProgramFiles(x86)%\Microsoft Office\Office16"")" + "\r\n" +
                        @"set CID=" + CIDUI + "" + "\r\n" +
                        @"cscript //nologo OSPP.VBS /actcid:%CID%" + "\r\n" +
                        @"cscript.exe OSPP.vbs /act" + "\r\n" +
                        @"for /f ""tokens=2,3,4,5,6 usebackq delims=:/ "" %%a in ('%date% %time%') do echo %%c-%%a-%%b %%d%%e" + "\r\n" +
                        @"echo DATE: %date% >status.txt" + "\r\n" +
                        @"echo TIME: %time% >>status.txt" + "\r\n" +
                        @"for /f ""tokens=8"" %b in ('cscript ospp.vbs /dinstid ^| findstr /b /c:""Installation ID""') do set IID=%b" + "\r\n" +
                        @"echo IID: %IID%>>status.txt" + "\r\n" +
                        @"echo CID: " + CIDUI + ">>status.txt" + "\r\n" +
                        @"cscript ospp.vbs /dstatus >>status.txt" + "\r\n" +
                        @"start status.txt" + "\r\n");
            switch (luaChon)
            {
                case 1:
                    return script[1];
                case 2:
                    return script[2];
                case 3:
                    return script[3];
                case 4:
                    return script[4];
                case 5:
                    return script[5];
                default:
                    return string.Empty;
            }
        }

        public string ScriptCheckRemove(int luaChon)
        {
            Dictionary<int, string> script = new Dictionary<int, string>();
            script.Add(1, @"cd %windir%\system32" + "\r\n" +
                    @"for /f ""tokens=2,3,4,5,6 usebackq delims=: / "" %%a in ('%date% %time%') do echo %%c-%%a-%%b %%d%%e" + "\r\n" +
                    @"echo DATE: %date% >status.txt" + "\r\n" +
                    @"echo TIME: %time% >>status.txt" + "\r\n" +
                    @"for /f ""tokens=3"" %b in ('cscript.exe %windir%\system32\slmgr.vbs /dti') do set IID=%b" + "\r\n" +
                    @"echo IID: %IID% >>status.txt" + "\r\n" +
                    @"cscript slmgr.vbs /dli >>status.txt" + "\r\n" +
                    @"cscript slmgr.vbs /xpr >>status.txt" + "\r\n" +
                    @"start status.txt" + "\r\n");
            script.Add(2, @"for %a in (4,5,6) do (if exist ""%ProgramFiles%\Microsoft Office\Office1%a\ospp.vbs"" (cd /d ""%ProgramFiles%\Microsoft Office\Office1%a"")" + "\r\n" +
                        @"if exist ""%ProgramFiles% (x86)\Microsoft Office\Office1%a\ospp.vbs"" (cd /d ""%ProgramFiles% (x86)\Microsoft Office\Office1%a""))" + "\r\n" +
                        @"cls" + "\r\n" +
                        @"start WINWORD" + "\r\n" +
                        @"for /f ""tokens=2,3,4,5,6 usebackq delims=:/ "" %%a in ('%date% %time%') do echo %%c-%%a-%%b %%d%%e" + "\r\n" +
                        @"echo DATE: %date% >status.txt" + "\r\n" +
                        @"echo TIME: %time% >>status.txt" + "\r\n" +
                        @"for /f ""tokens=8"" %b in ('cscript ospp.vbs /dinstid ^| findstr /b /c:""Installation ID""') do set IID=%b" + "\r\n" +
                        @"echo IID: %IID%>>status.txt" + "\r\n" +
                        @"cscript ospp.vbs /dstatus >>status.txt" + "\r\n" +
                        @"start status.txt" + "\r\n");
            script.Add(3, @"if exist ""%ProgramFiles%\Microsoft Office\Office14\ospp.vbs"" (cd /d ""%ProgramFiles%\Microsoft Office\Office14"")" + "\r\n" +
                        @"if exist ""%ProgramFiles(x86)%\Microsoft Office\Office14\ospp.vbs"" (cd /d ""%ProgramFiles(x86)%\Microsoft Office\Office14"")" + "\r\n" +
                        @"start WINWORD" + "\r\n" +
                        @"for /f ""tokens=2,3,4,5,6 usebackq delims=:/ "" %%a in ('%date% %time%') do echo %%c-%%a-%%b %%d%%e" + "\r\n" +
                        @"echo DATE: %date% >status.txt" + "\r\n" +
                        @"echo TIME: %time% >>status.txt" + "\r\n" +
                        @"for /f ""tokens=8"" %b in ('cscript ospp.vbs /dinstid ^| findstr /b /c:""Installation ID""') do set IID=%b" + "\r\n" +
                        @"echo IID: %IID%>>status.txt" + "\r\n" +
                        @"cscript ospp.vbs /dstatus >>status.txt" + "\r\n" +
                        @"start status.txt" + "\r\n");
            script.Add(4, @"if exist ""%ProgramFiles%\Microsoft Office\Office15\ospp.vbs"" (cd /d ""%ProgramFiles%\Microsoft Office\Office15"")" + "\r\n" +
                        @"if exist ""%ProgramFiles(x86)%\Microsoft Office\Office15\ospp.vbs"" (cd /d ""%ProgramFiles(x86)%\Microsoft Office\Office15"")" + "\r\n" +
                        @"start WINWORD" + "\r\n" +
                        @"for /f ""tokens=2,3,4,5,6 usebackq delims=:/ "" %%a in ('%date% %time%') do echo %%c-%%a-%%b %%d%%e" + "\r\n" +
                        @"echo DATE: %date% >status.txt" + "\r\n" +
                        @"echo TIME: %time% >>status.txt" + "\r\n" +
                        @"for /f ""tokens=8"" %b in ('cscript ospp.vbs /dinstid ^| findstr /b /c:""Installation ID""') do set IID=%b" + "\r\n" +
                        @"echo IID: %IID%>>status.txt" + "\r\n" +
                        @"cscript ospp.vbs /dstatus >>status.txt" + "\r\n" +
                        @"start status.txt" + "\r\n");
            script.Add(5, @"if exist ""%ProgramFiles%\Microsoft Office\Office16\ospp.vbs"" (cd /d ""%ProgramFiles%\Microsoft Office\Office16"")" + "\r\n" +
                        @"if exist ""%ProgramFiles(x86)%\Microsoft Office\Office16\ospp.vbs"" (cd /d ""%ProgramFiles(x86)%\Microsoft Office\Office16"")" + "\r\n" +
                        @"start WINWORD" + "\r\n" +
                        @"for /f ""tokens=2,3,4,5,6 usebackq delims=:/ "" %%a in ('%date% %time%') do echo %%c-%%a-%%b %%d%%e" + "\r\n" +
                        @"echo DATE: %date% >status.txt" + "\r\n" +
                        @"echo TIME: %time% >>status.txt" + "\r\n" +
                        @"for /f ""tokens=8"" %b in ('cscript ospp.vbs /dinstid ^| findstr /b /c:""Installation ID""') do set IID=%b" + "\r\n" +
                        @"echo IID: %IID%>>status.txt" + "\r\n" +
                        @"cscript ospp.vbs /dstatus >>status.txt" + "\r\n" +
                        @"start status.txt" + "\r\n");
            script.Add(6, @"for %a in (4,5,6) do (if exist ""%ProgramFiles%\Microsoft Office\Office1%a\ospp.vbs"" (cd /d ""%ProgramFiles%\Microsoft Office\Office1%a"")" + "\r\n" +
                        @"If exist ""%ProgramFiles% (x86)\Microsoft Office\Office1%a\ospp.vbs"" (cd /d ""%ProgramFiles% (x86)\Microsoft Office\Office1%a""))" + "\r\n" +
                        @"for /f ""tokens= 8"" %b in ('cscript //nologo OSPP.VBS /dstatus ^| findstr /b /c:""Last 5""') do (cscript //nologo ospp.vbs /unpkey:%b)" + "\r\n");
            script.Add(7, "cscript ospp.vbs /unpkey:");
            switch (luaChon)
            {
                case 1:
                    return script[1];
                case 2:
                    return script[2];
                case 3:
                    return script[3];
                case 4:
                    return script[4];
                case 5:
                    return script[5];
                case 6:
                    return script[6];
                case 7:
                    return script[7];
                default:
                    return string.Empty;
            }
        }
    }
}
