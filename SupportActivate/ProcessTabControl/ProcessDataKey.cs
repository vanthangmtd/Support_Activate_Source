using FormWindows.SupportActivate;
using SupportActivate.Common;
using SupportActivate.FormWindows;
using SupportActivate.ProcessBusiness;
using SupportActivate.ProcessSQL;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SupportActivate.ProcessTabControl
{
    public class ProcessDataKey
    {
        ServerKey serverKey;
        FormSupportActivate formMain = FormSupportActivate.formMain;
        DataKey formDataKey = DataKey.formDataKey;
        ProcessDataKeyCommon processDataKeyCommon;
        SourceData sourceData;
        int countKey;
        int keyCurrent;
        static Dictionary<string, string> optionCbxPidKey;
        static Dictionary<string, string> listOptionCbxPidKey;

        private log4net.ILog logger = log4net.LogManager.GetLogger(typeof(ProcessDataKey));

        public ProcessDataKey()
        {
            processDataKeyCommon = new ProcessDataKeyCommon();
            serverKey = new ServerKey();
            sourceData = new SourceData();
            optionCbxPidKey = sourceData.OptionCbxPidKeyAndTxtFilesArrayPidKey();
            listOptionCbxPidKey = new Dictionary<string, string>();
            listOptionCbxPidKey.Add("All Edition Windows/Office", optionCbxPidKey.ElementAt(0).Key);
            listOptionCbxPidKey.Add("Windows 7", optionCbxPidKey.ElementAt(1).Key);
            listOptionCbxPidKey.Add("Embedded Industry 7", optionCbxPidKey.ElementAt(1).Key);

            listOptionCbxPidKey.Add("Win 8", optionCbxPidKey.ElementAt(2).Key);
            listOptionCbxPidKey.Add("Server 2012", optionCbxPidKey.ElementAt(2).Key);
            listOptionCbxPidKey.Add("Server Essentials 2012", optionCbxPidKey.ElementAt(2).Key);

            listOptionCbxPidKey.Add("Win 8.1", optionCbxPidKey.ElementAt(3).Key);
            listOptionCbxPidKey.Add("Server 12 R2", optionCbxPidKey.ElementAt(3).Key);
            listOptionCbxPidKey.Add("Server Essentials 2012 R2", optionCbxPidKey.ElementAt(3).Key);
            listOptionCbxPidKey.Add("Embedded Industry 8.1", optionCbxPidKey.ElementAt(3).Key);

            listOptionCbxPidKey.Add("Win 10", optionCbxPidKey.ElementAt(4).Key);
            listOptionCbxPidKey.Add("Win 2021", optionCbxPidKey.ElementAt(4).Key);
            listOptionCbxPidKey.Add("Server 2016", optionCbxPidKey.ElementAt(4).Key);
            listOptionCbxPidKey.Add("Server Essentials 2016", optionCbxPidKey.ElementAt(4).Key);
            listOptionCbxPidKey.Add("Server 2019", optionCbxPidKey.ElementAt(4).Key);
            listOptionCbxPidKey.Add("Server Essentials 2019", optionCbxPidKey.ElementAt(4).Key);
            listOptionCbxPidKey.Add("Server 2021", optionCbxPidKey.ElementAt(4).Key);

            listOptionCbxPidKey.Add("Win 11", optionCbxPidKey.ElementAt(5).Key);
            listOptionCbxPidKey.Add("Win 2024", optionCbxPidKey.ElementAt(5).Key);
            listOptionCbxPidKey.Add("Server 2025", optionCbxPidKey.ElementAt(5).Key);

            listOptionCbxPidKey.Add("Office14", optionCbxPidKey.ElementAt(6).Key);
            listOptionCbxPidKey.Add("Office15", optionCbxPidKey.ElementAt(7).Key);
            listOptionCbxPidKey.Add("Office16", optionCbxPidKey.ElementAt(8).Key);
            listOptionCbxPidKey.Add("Office19", optionCbxPidKey.ElementAt(9).Key);
            listOptionCbxPidKey.Add("Office21", optionCbxPidKey.ElementAt(10).Key);
            listOptionCbxPidKey.Add("Office24", optionCbxPidKey.ElementAt(11).Key);
            listOptionCbxPidKey.Add("Visual", optionCbxPidKey.ElementAt(12).Key);
            listOptionCbxPidKey.Add("Vista", optionCbxPidKey.ElementAt(13).Key);
            listOptionCbxPidKey.Add("Server 2008", optionCbxPidKey.ElementAt(13).Key);
        }

        private List<dataKey> getDataFromDataGridView()
        {
            var listData = new List<dataKey>();
            var listRowIndex = new List<int>();
            DataGridViewSelectedCellCollection selectedCells = formDataKey.dgv_Key.SelectedCells;
            foreach (DataGridViewCell row in selectedCells)
                listRowIndex.Add(row.RowIndex);
            listRowIndex = listRowIndex.Distinct().ToList();
            int i = 0;
            foreach (var item in listRowIndex)
            {
                listData.Add(new dataKey() { id = i, key = formDataKey.dgv_Key[1, item].Value.ToString() });
                i += 1;
            }
            if (listData.Count > 0)
                listData = listData.OrderByDescending(x => x.id).ToList();
            return listData;
        }

        public void loadAllDescription()
        {
            try
            {
                formDataKey.cbb_Description.Invoke(new Action(() =>
                {
                    formDataKey.cbb_Description.Items.Clear();
                    formDataKey.cbb_Description.Items.Add("Select Version Key");
                    formDataKey.cbb_Description.Items.Add("Select All Key");
                    formDataKey.cbb_Description.SelectedItem = "Select Version Key";
                    formDataKey.cbb_Description.Items.Add(ContantResource.allKeyVolume);
                }));

                string selectKeyOfficeVL = "SELECT DISTINCT Description FROM " + ServerKey.KeyOffice + " WHERE LicenseType LIKE '%Volume:MAK%' AND MAKCount <> '-2' AND ErrorCode <> '" + ContantResource.KeyRetaiBlock + "' ORDER BY Description ASC, LicenseType DESC";
                string selectKeyWindowsVL = "SELECT DISTINCT Description FROM " + ServerKey.KeyWindows + " WHERE LicenseType LIKE '%Volume:MAK%' AND MAKCount <> '-2' AND ErrorCode <> '" + ContantResource.KeyRetaiBlock + "' ORDER BY Description ASC, LicenseType DESC";
                string selectKeyServerVL = "SELECT DISTINCT Description FROM " + ServerKey.KeyServer + " WHERE LicenseType LIKE '%Volume:MAK%' AND MAKCount <> '-2' AND ErrorCode <> '" + ContantResource.KeyRetaiBlock + "' ORDER BY Description ASC, LicenseType DESC";
                processDataKeyCommon.loadDescriptionAllKey(selectKeyOfficeVL);
                processDataKeyCommon.loadDescriptionAllKey(selectKeyWindowsVL);
                processDataKeyCommon.loadDescriptionAllKey(selectKeyServerVL);

                formDataKey.cbb_Description.Invoke(new Action(() =>
                {
                    formDataKey.cbb_Description.Items.Add(ContantResource.allKeyRetail);
                }));
                string selectKeyOfficeRetail = "SELECT DISTINCT Description FROM " + ServerKey.KeyOffice + " WHERE LicenseType LIKE '%Retail%' AND MAKCount <> '-2' AND ErrorCode <> '" + ContantResource.KeyRetaiBlock + "' ORDER BY Description ASC, LicenseType DESC";
                string selectKeyWindowsRetail = "SELECT DISTINCT Description FROM " + ServerKey.KeyWindows + " WHERE LicenseType LIKE '%Retail%' AND MAKCount <> '-2' AND ErrorCode <> '" + ContantResource.KeyRetaiBlock + "' ORDER BY Description ASC, LicenseType DESC";
                string selectKeyServerRetail = "SELECT DISTINCT Description FROM " + ServerKey.KeyServer + " WHERE LicenseType LIKE '%Retail%' AND MAKCount <> '-2' AND ErrorCode <> '" + ContantResource.KeyRetaiBlock + "' ORDER BY Description ASC, LicenseType DESC";
                processDataKeyCommon.loadDescriptionAllKey(selectKeyOfficeRetail);
                processDataKeyCommon.loadDescriptionAllKey(selectKeyWindowsRetail);
                processDataKeyCommon.loadDescriptionAllKey(selectKeyServerRetail);

                formDataKey.cbb_Description.Invoke(new Action(() =>
                {
                    formDataKey.cbb_Description.Items.Add(ContantResource.allKeyOEM);
                }));
                string selectKeyOfficeOEM = "SELECT DISTINCT Description FROM " + ServerKey.KeyOffice + " WHERE LicenseType LIKE '%OEM%' AND MAKCount <> '-2' AND ErrorCode <> '" + ContantResource.KeyRetaiBlock + "' ORDER BY Description ASC, LicenseType DESC";
                string selectKeyWindowsOEM = "SELECT DISTINCT Description FROM " + ServerKey.KeyWindows + " WHERE LicenseType LIKE '%OEM%' AND MAKCount <> '-2' AND ErrorCode <> '" + ContantResource.KeyRetaiBlock + "' ORDER BY Description ASC, LicenseType DESC";
                string selectKeyServerOEM = "SELECT DISTINCT Description FROM " + ServerKey.KeyServer + " WHERE LicenseType LIKE '%OEM%' AND MAKCount <> '-2' AND ErrorCode <> '" + ContantResource.KeyRetaiBlock + "' ORDER BY Description ASC, LicenseType DESC";
                processDataKeyCommon.loadDescriptionAllKey(selectKeyOfficeOEM);
                processDataKeyCommon.loadDescriptionAllKey(selectKeyWindowsOEM);
                processDataKeyCommon.loadDescriptionAllKey(selectKeyServerOEM);

                string selectKeyOfficeOther = "SELECT DISTINCT Description FROM " + ServerKey.KeyOffice + " WHERE LicenseType NOT IN (SELECT DISTINCT LicenseType FROM " + ServerKey.KeyOffice + " WHERE LicenseType LIKE '%Volume%' OR LicenseType LIKE '%Retail%' OR LicenseType LIKE '%OEM%'" + ") AND MAKCount <> '-2' AND ErrorCode <> '" + ContantResource.KeyRetaiBlock + "' ORDER BY Description ASC, LicenseType DESC";
                string selectKeyWindowsOther = "SELECT DISTINCT Description FROM " + ServerKey.KeyWindows + " WHERE LicenseType NOT IN (SELECT DISTINCT LicenseType FROM " + ServerKey.KeyWindows + " WHERE LicenseType LIKE '%Volume%' OR LicenseType LIKE '%Retail%' OR LicenseType LIKE '%OEM%'" + ") And MAKCount <> '-2' AND ErrorCode <> '" + ContantResource.KeyRetaiBlock + "' ORDER BY Description ASC, LicenseType DESC";
                string selectKeyServerOther = "SELECT DISTINCT Description FROM " + ServerKey.KeyServer + " WHERE LicenseType NOT IN (SELECT DISTINCT LicenseType FROM " + ServerKey.KeyServer + " WHERE LicenseType LIKE '%Volume%' OR LicenseType LIKE '%Retail%' OR LicenseType LIKE '%OEM%'" + ") And MAKCount <> '-2' AND ErrorCode <> '" + ContantResource.KeyRetaiBlock + "' ORDER BY Description ASC, LicenseType DESC";
                processDataKeyCommon.loadDescriptionAllKey(selectKeyOfficeOther);
                processDataKeyCommon.loadDescriptionAllKey(selectKeyWindowsOther);
                processDataKeyCommon.loadDescriptionAllKey(selectKeyServerOther);

                formDataKey.cbb_Description.Invoke(new Action(() =>
                {
                    formDataKey.cbb_Description.Items.Add(ContantResource.allKeyNOTDEFINED);
                }));
                string selectKeyOtherVL = "SELECT DISTINCT Description FROM " + ServerKey.KeyOther + " WHERE LicenseType LIKE '%Volume:MAK%' AND MAKCount <> '-2' AND ErrorCode <> '" + ContantResource.KeyRetaiBlock + "' ORDER BY Description ASC, LicenseType DESC";
                string selectKeyOtherRetail = "SELECT DISTINCT Description FROM " + ServerKey.KeyOther + " WHERE LicenseType LIKE '%Retail%' AND MAKCount <> '-2' AND ErrorCode <> '" + ContantResource.KeyRetaiBlock + "' ORDER BY Description ASC, LicenseType DESC";
                string selectKeyOtherOEM = "SELECT DISTINCT Description FROM " + ServerKey.KeyOther + " WHERE LicenseType LIKE '%OEM%' AND MAKCount <> '-2' AND ErrorCode <> '" + ContantResource.KeyRetaiBlock + "' ORDER BY Description ASC, LicenseType DESC";
                string selectKeyOtherOther = "SELECT DISTINCT Description FROM " + ServerKey.KeyOther + " WHERE LicenseType NOT IN (SELECT DISTINCT LicenseType FROM " + ServerKey.KeyOther + " WHERE LicenseType LIKE '%Volume%' OR LicenseType LIKE '%Retail%' OR LicenseType LIKE '%OEM%'" + ") And MAKCount <> '-2' AND ErrorCode <> '" + ContantResource.KeyRetaiBlock + "' ORDER BY Description ASC, LicenseType DESC";
                processDataKeyCommon.loadDescriptionAllKey(selectKeyOtherVL);
                processDataKeyCommon.loadDescriptionAllKey(selectKeyOtherRetail);
                processDataKeyCommon.loadDescriptionAllKey(selectKeyOtherOEM);
                processDataKeyCommon.loadDescriptionAllKey(selectKeyOtherOther);
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
        }

        public void tongKey()
        {
            try
            {
                string countKeyOffice = "SELECT count(Id) FROM " + ServerKey.KeyOffice + " WHERE MAKCount <> '-2' AND ErrorCode <> '" + ContantResource.KeyRetaiBlock + "'";
                countKey = processDataKeyCommon.totalKey(countKeyOffice);
                string countKeyWindows = "SELECT count(Id) FROM " + ServerKey.KeyWindows + " WHERE MAKCount <> '-2' AND ErrorCode <> '" + ContantResource.KeyRetaiBlock + "'";
                countKey = countKey + processDataKeyCommon.totalKey(countKeyWindows);
                string countKeyServer = "SELECT count(Id) FROM " + ServerKey.KeyServer + " WHERE MAKCount <> '-2' AND ErrorCode <> '" + ContantResource.KeyRetaiBlock + "'";
                countKey = countKey + processDataKeyCommon.totalKey(countKeyServer);
                string countKeyOther = "SELECT count(Id) FROM " + ServerKey.KeyOther + " WHERE MAKCount <> '-2' AND ErrorCode <> '" + ContantResource.KeyRetaiBlock + "'";
                countKey = countKey + processDataKeyCommon.totalKey(countKeyOther);
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
        }

        public void loadDataKeyCurrent()
        {
            formDataKey.cbb_Description.Invoke(new Action(() =>
            {
                formDataKey.BoxDescription = formDataKey.cbb_Description.Text;
            }));

            if (formDataKey.BoxDescription == "Select All Key")
            {
                formDataKey.optionVerKey = string.Empty;
                loadAllDataKey();
            }
            else
            {
                formDataKey.optionVerKey = formDataKey.BoxDescription == ContantResource.allKeyVolume ? ContantResource.Volume :
                               formDataKey.BoxDescription == ContantResource.allKeyRetail ? ContantResource.Retail :
                               formDataKey.BoxDescription == ContantResource.allKeyOEM ? ContantResource.OEM :
                               formDataKey.BoxDescription == ContantResource.allKeyNOTDEFINED ? ContantResource.NOTDEFINED : string.Empty;
                if (string.IsNullOrEmpty(formDataKey.optionVerKey))
                {
                    var checkKeyOffice = formDataKey.BoxDescription.IndexOf("Office");
                    var checkKeyWin = formDataKey.BoxDescription.IndexOf("Win");
                    var checkKeyServer = formDataKey.BoxDescription.IndexOf("Server");
                    formDataKey.optionVerKey = formDataKey.BoxDescription;
                    formDataKey.selectVerKey = checkKeyServer != -1 ? ServerKey.KeyServer :
                                   checkKeyOffice != -1 ? ServerKey.KeyOffice :
                                   checkKeyWin != -1 ? ServerKey.KeyWindows : ServerKey.KeyOther;
                    processDataKeyCommon.loadDataKeyDescription();
                    countKeyCurrent();
                }
                else
                {
                    loadDataKeyAllVer();
                    countKeyCurrent();
                }
            }
        }

        private void loadAllDataKey()
        {
            try
            {
                formDataKey.dgv_Key.Invoke(new Action(() =>
                {
                    formDataKey.dgv_Key.Rows.Clear();
                }));
                string selectKeyOffice = "SELECT * FROM " + ServerKey.KeyOffice + " WHERE MAKCount <> '-2' AND ErrorCode <> '" + ContantResource.KeyRetaiBlock + "' ORDER BY LicenseType DESC, Description ASC";
                string selectKeyWindows = "SELECT * FROM " + ServerKey.KeyWindows + " WHERE MAKCount <> '-2' AND ErrorCode <> '" + ContantResource.KeyRetaiBlock + "'  ORDER BY LicenseType DESC, Description ASC";
                string selectKeyServer = "SELECT * FROM " + ServerKey.KeyServer + " WHERE MAKCount <> '-2' AND ErrorCode <> '" + ContantResource.KeyRetaiBlock + "'  ORDER BY LicenseType DESC, Description ASC";
                object[] rowNull = new object[] { 0, "", "", "", "", 0, "", "", "" };
                processDataKeyCommon.loadDataKeyAll(selectKeyOffice);
                formDataKey.dgv_Key.Invoke(new Action(() =>
                {
                    formDataKey.dgv_Key.Rows.Add(rowNull);
                }));
                processDataKeyCommon.loadDataKeyAll(selectKeyWindows);
                formDataKey.dgv_Key.Invoke(new Action(() =>
                {
                    formDataKey.dgv_Key.Rows.Add(rowNull);
                }));
                processDataKeyCommon.loadDataKeyAll(selectKeyServer);
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
        }

        private void loadDataKeyAllVer()
        {
            try
            {
                formDataKey.dgv_Key.Invoke(new Action(() =>
                {
                    formDataKey.dgv_Key.Rows.Clear();
                }));
                if (formDataKey.optionVerKey == "KEY NOT DEFINED")
                {
                    string selectKeyOther = "SELECT * FROM " + ServerKey.KeyOther + " WHERE MAKCount <> '-2' AND ErrorCode <> '" + ContantResource.KeyRetaiBlock + "' ORDER BY Description ASC, MAKCount DESC";
                    processDataKeyCommon.loadDataKeyAll(selectKeyOther);
                }
                else
                {
                    string selectKeyOffice = "SELECT * FROM " + ServerKey.KeyOffice + " WHERE LicenseType LIKE '" + formDataKey.optionVerKey + "%' AND MAKCount <> '-2' AND ErrorCode <> '" + ContantResource.KeyRetaiBlock + "' ORDER BY Description ASC, MAKCount DESC";
                    string selectKeyWindows = "SELECT * FROM " + ServerKey.KeyWindows + " WHERE LicenseType LIKE '" + formDataKey.optionVerKey + "%' AND MAKCount <> '-2' AND ErrorCode <> '" + ContantResource.KeyRetaiBlock + "' ORDER BY Description ASC, MAKCount DESC";
                    string selectKeyServer = "SELECT * FROM " + ServerKey.KeyServer + " WHERE LicenseType LIKE '" + formDataKey.optionVerKey + "%' AND MAKCount <> '-2' AND ErrorCode <> '" + ContantResource.KeyRetaiBlock + "' ORDER BY Description ASC, MAKCount DESC";
                    processDataKeyCommon.loadDataKeyAll(selectKeyOffice);
                    processDataKeyCommon.loadDataKeyAll(selectKeyWindows);
                    processDataKeyCommon.loadDataKeyAll(selectKeyServer);
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
        }

        private void countKeyCurrent()
        {
            try
            {
                keyCurrent = 0;
                if (formDataKey.optionVerKey == ContantResource.Volume || formDataKey.optionVerKey == ContantResource.Retail || formDataKey.optionVerKey == ContantResource.OEM || formDataKey.optionVerKey == ContantResource.NOTDEFINED)
                {
                    if (formDataKey.optionVerKey == ContantResource.NOTDEFINED)
                    {
                        string selectKeyOther = "SELECT count(Id) FROM " + ServerKey.KeyOther + " WHERE MAKCount <> '-2' AND ErrorCode <> '" + ContantResource.KeyRetaiBlock + "'";
                        keyCurrent = processDataKeyCommon.totalKey(selectKeyOther);
                    }
                    else
                    {
                        string selectKeyOffice = "SELECT count(Id) FROM " + ServerKey.KeyOffice + " WHERE LicenseType LIKE '" + formDataKey.optionVerKey + "%' AND MAKCount <> '-2' AND ErrorCode <> '" + ContantResource.KeyRetaiBlock + "'";
                        string selectKeyWindows = "SELECT count(Id) FROM " + ServerKey.KeyWindows + " WHERE LicenseType LIKE '" + formDataKey.optionVerKey + "%' AND MAKCount <> '-2' AND ErrorCode <> '" + ContantResource.KeyRetaiBlock + "'";
                        string selectKeyServer = "SELECT count(Id) FROM " + ServerKey.KeyServer + " WHERE LicenseType LIKE '" + formDataKey.optionVerKey + "%' AND MAKCount <> '-2' AND ErrorCode <> '" + ContantResource.KeyRetaiBlock + "'";
                        keyCurrent = processDataKeyCommon.totalKey(selectKeyOffice);
                        keyCurrent = keyCurrent + processDataKeyCommon.totalKey(selectKeyWindows);
                        keyCurrent = keyCurrent + processDataKeyCommon.totalKey(selectKeyServer);
                    }
                }
                else
                {
                    using (SQLiteConnection SqlConn = new SQLiteConnection(serverKey.connectionString))
                    {
                        string selectKey = "SELECT count(Id) FROM " + formDataKey.selectVerKey + " WHERE Description='" + formDataKey.optionVerKey + "' AND MAKCount <> '-2' AND ErrorCode <> '" + ContantResource.KeyRetaiBlock + "'";
                        keyCurrent = processDataKeyCommon.totalKey(selectKey);
                    }
                }
                formDataKey.tbx_KeySearch.Invoke(new Action(() =>
                {
                    formDataKey.tbx_KeySearch.Text = keyCurrent + "/" + countKey + " key";
                }));
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
        }

        public void addNoteDataKey(string key, string note)
        {
            try
            {
                using (SQLiteConnection SqlConn = new SQLiteConnection(serverKey.connectionString))
                {
                    string updateKeyOffice = "UPDATE " + ServerKey.KeyOffice + " SET Note='" + note + "' WHERE Key='" + key + "'";
                    string updateKeyWindows = "UPDATE " + ServerKey.KeyWindows + " SET Note='" + note + "' WHERE Key='" + key + "'";
                    string updateKeyServer = "UPDATE " + ServerKey.KeyServer + " SET Note='" + note + "' WHERE Key='" + key + "'";
                    string updateKeyOther = "UPDATE " + ServerKey.KeyOther + " SET Note='" + note + "' WHERE Key='" + key + "'";
                    processDataKeyCommon.addNote(updateKeyOffice);
                    processDataKeyCommon.addNote(updateKeyWindows);
                    processDataKeyCommon.addNote(updateKeyServer);
                    processDataKeyCommon.addNote(updateKeyOther);
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
        }

        public void copyInforKey()
        {
            try
            {
                var listKey = getDataFromDataGridView();
                StringBuilder inforKey = new StringBuilder();
                foreach (var values in listKey)
                {
                    StringBuilder resultInfoKeyOffice = new StringBuilder(processDataKeyCommon.getInfoKey(ServerKey.KeyOffice, values.key));
                    StringBuilder resultInfoKeyWindows = new StringBuilder(processDataKeyCommon.getInfoKey(ServerKey.KeyWindows, values.key));
                    StringBuilder resultInfoKeyOther = new StringBuilder(processDataKeyCommon.getInfoKey(ServerKey.KeyOther, values.key));
                    StringBuilder resultInfoKeyServer = new StringBuilder(processDataKeyCommon.getInfoKey(ServerKey.KeyServer, values.key));
                    if (!string.IsNullOrEmpty(resultInfoKeyOffice.ToString()))
                        inforKey.AppendLine(resultInfoKeyOffice.ToString());

                    if (!string.IsNullOrEmpty(resultInfoKeyWindows.ToString()))
                        inforKey.AppendLine(resultInfoKeyWindows.ToString());

                    if (!string.IsNullOrEmpty(resultInfoKeyOther.ToString()))
                        inforKey.AppendLine(resultInfoKeyOther.ToString());

                    if (!string.IsNullOrEmpty(resultInfoKeyServer.ToString()))
                        inforKey.AppendLine(resultInfoKeyServer.ToString());

                    if (string.IsNullOrEmpty(resultInfoKeyOffice.ToString()) && string.IsNullOrEmpty(resultInfoKeyWindows.ToString()) &&
                        string.IsNullOrEmpty(resultInfoKeyOther.ToString()) && string.IsNullOrEmpty(resultInfoKeyServer.ToString()))
                        inforKey.AppendLine(string.Empty);
                }
                Clipboard.SetText(inforKey.ToString());
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
        }

        public void getValuesDgvKey()
        {
            try
            {
                string values = string.Empty;
                var listKey = getDataFromDataGridView();
                for (var i = 0; i <= listKey.Count - 1; i++)
                    values = values + "\r\n" + listKey[i].key;
                if (!string.IsNullOrEmpty(values))
                    Clipboard.SetText(values.Trim().Replace(" ", ""));
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
        }

        public void changeToKeyBlock()
        {
            try
            {
                var listKey = getDataFromDataGridView();
                foreach (var values in listKey)
                {
                    processDataKeyCommon.blockKey(ServerKey.KeyOffice, values.key);
                    processDataKeyCommon.blockKey(ServerKey.KeyWindows, values.key);
                    processDataKeyCommon.blockKey(ServerKey.KeyServer, values.key);
                    processDataKeyCommon.blockKey(ServerKey.KeyOther, values.key);
                }
                loadDataKeyCurrent();
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
        }

        public void deleteDgvKey()
        {
            try
            {
                var listKey = getDataFromDataGridView();
                foreach (var values in listKey)
                {
                    processDataKeyCommon.deleteKey(ServerKey.KeyOffice, values.key);
                    processDataKeyCommon.deleteKey(ServerKey.KeyWindows, values.key);
                    processDataKeyCommon.deleteKey(ServerKey.KeyServer, values.key);
                    processDataKeyCommon.deleteKey(ServerKey.KeyOther, values.key);
                }
                loadDataKeyCurrent();
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
        }

        public void searchDataKey()
        {
            try
            {
                formDataKey.tbx_KeySearch.Invoke(new Action(() =>
                {
                    formDataKey.keySearch = formDataKey.tbx_KeySearch.Text.ToUpper().Trim().Replace(" ", "");
                    formDataKey.cbb_Description.SelectedItem = "Select Version Key";
                    formDataKey.dgv_Key.Rows.Clear();
                }));
                string selectKeyOffice = "SELECT * FROM " + ServerKey.KeyOffice + " WHERE Key LIKE '%" + formDataKey.keySearch + "%'";
                string selectKeyWindows = "SELECT * FROM " + ServerKey.KeyWindows + " WHERE Key LIKE '%" + formDataKey.keySearch + "%'";
                string selectKeyServer = "SELECT * FROM " + ServerKey.KeyServer + " WHERE Key LIKE '%" + formDataKey.keySearch + "%'";
                string selectKeyOther = "SELECT * FROM " + ServerKey.KeyOther + " WHERE Key LIKE '%" + formDataKey.keySearch + "%'";
                processDataKeyCommon.loadDataKeyAll(selectKeyOffice);
                processDataKeyCommon.loadDataKeyAll(selectKeyWindows);
                processDataKeyCommon.loadDataKeyAll(selectKeyServer);
                processDataKeyCommon.loadDataKeyAll(selectKeyOther);
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
        }

        public void recoveryKeyBlock()
        {
            try
            {
                var listKey = getDataFromDataGridView();
                foreach (var values in listKey)
                {
                    processDataKeyCommon.recoveryKeyBlock(ServerKey.KeyOffice, values.key);
                    processDataKeyCommon.recoveryKeyBlock(ServerKey.KeyWindows, values.key);
                    processDataKeyCommon.recoveryKeyBlock(ServerKey.KeyServer, values.key);
                    processDataKeyCommon.recoveryKeyBlock(ServerKey.KeyOther, values.key);
                }
                loadDataKeyCurrent();
                MessageBox.Show(MessagesResource.RecoverKeySuccess, MessagesResource.success, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                MessageBox.Show(MessagesResource.RecoverKeyError, MessagesResource.success, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void saveKey()
        {
            using (SaveFileDialog SaveFileDialog1 = new SaveFileDialog())
            {
                SaveFileDialog1.Filter = "TXT Files (*.txt)|*.txt";
                if (SaveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        string File = SaveFileDialog1.FileName;
                        StreamWriter write;
                        write = new StreamWriter(File, false);
                        foreach (var value in loadDataSave())
                            write.WriteLine(value);
                        write.Close();
                        MessageBox.Show(MessagesResource.SaveSuccess, MessagesResource.success, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        logger.Error(ex);
                        MessageBox.Show(MessagesResource.SaveError, MessagesResource.success, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private List<string> loadDataSave()
        {
            List<string> data = new List<string>();
            try
            {
                foreach (var value in processDataKeyCommon.loadDataSave(ServerKey.KeyOffice))
                    data.Add(value);
                foreach (var value in processDataKeyCommon.loadDataSave(ServerKey.KeyWindows))
                    data.Add(value);
                foreach (var value in processDataKeyCommon.loadDataSave(ServerKey.KeyServer))
                    data.Add(value);
                foreach (var value in processDataKeyCommon.loadDataSave(ServerKey.KeyOther))
                    data.Add(value);
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                data.Add("");
            }
            return data;
        }

        public void readDatabaseBK(string part)
        {
            try
            {
                var count = 0;
                formDataKey.Invoke(new Action(() =>
                {
                    formDataKey.Cursor = Cursors.WaitCursor;
                }));
                count = count + processDataKeyCommon.readDBBK(ServerKey.KeyOffice, count, part);
                count = count + processDataKeyCommon.readDBBK(ServerKey.KeyWindows, count, part);
                count = count + processDataKeyCommon.readDBBK(ServerKey.KeyServer, count, part);
                count = count + processDataKeyCommon.readDBBK(ServerKey.KeyOther, count, part);
                formDataKey.Invoke(new Action(() =>
                {
                    formDataKey.Cursor = Cursors.Default;
                }));
                MessageBox.Show("Restore database success", MessagesResource.success, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                formDataKey.Invoke(new Action(() =>
                {
                    formDataKey.Cursor = Cursors.Default;
                }));
                MessageBox.Show("Restore database error", MessagesResource.success, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void readDatabasePIDKeyLite(string fileName)
        {
            try
            {
                formDataKey.Invoke(new Action(() =>
                {
                    formDataKey.Cursor = Cursors.WaitCursor;
                }));
                var count = 0;
                string connectionStringDBBK1 = string.Format("Data Source = {0}; Version=3;", fileName);
                using (SQLiteConnection SqlConn = new SQLiteConnection(connectionStringDBBK1))
                {
                    string selectvalues = "SELECT key, Description, SubType, LicenseType, ActivCount, Blocked, Comment FROM keys";
                    SQLiteCommand cmd = new SQLiteCommand(selectvalues, SqlConn);
                    SqlConn.Open();
                    SQLiteDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        string Blocked = string.Empty;
                        int ActivCount = -1;
                        if (reader.GetString(4) == "Key Blocked!")
                            ActivCount = -2;
                        else if (string.IsNullOrEmpty(reader.GetString(4)))
                            ActivCount = -1;
                        if (reader.GetString(5) == "Key Blocked!")
                            Blocked = "" + ContantResource.KeyRetaiBlock + "";
                        else if (string.IsNullOrEmpty(reader.GetString(5)))
                            Blocked = "";
                        pid pid = new pid();
                        pid.Key = reader.GetString(0);
                        pid.Description = reader.GetString(1);
                        pid.SubType = reader.GetString(2);
                        pid.LicenseType = reader.GetString(3);
                        pid.MAKCount = ActivCount;
                        pid.ErrorCode = Blocked;
                        pid.KeyGetWeb = string.Empty;
                        StringBuilder note = new StringBuilder(reader.GetString(6));
                        serverKey.CreateDataKey(true, pid, note.ToString());
                        count += 1;
                        formDataKey.tbx_KeySearch.Invoke(new Action(() =>
                        {
                            formDataKey.tbx_KeySearch.Text = "Read " + count + " key";
                        }));
                    }
                    SqlConn.Close();
                }
                formDataKey.Invoke(new Action(() =>
                {
                    formDataKey.Cursor = Cursors.Default;
                }));
                MessageBox.Show("Read database PIDKey Lite success", MessagesResource.success, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                formDataKey.Invoke(new Action(() =>
                {
                    formDataKey.Cursor = Cursors.Default;
                }));
                MessageBox.Show("Read database PIDKey Lite error", MessagesResource.error, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void loadAllDataKeyBlock()
        {
            try
            {
                string selectKeyOffice = "SELECT * FROM " + ServerKey.KeyOffice + " WHERE MAKCount = '-2' OR ErrorCode = '" + ContantResource.KeyRetaiBlock + "' ORDER BY LicenseType DESC, Description ASC";
                string selectKeyWindows = "SELECT * FROM " + ServerKey.KeyWindows + " WHERE MAKCount = '-2' OR ErrorCode = '" + ContantResource.KeyRetaiBlock + "' ORDER BY LicenseType DESC, Description ASC";
                string selectKeyServer = "SELECT * FROM " + ServerKey.KeyServer + " WHERE MAKCount = '-2' OR ErrorCode = '" + ContantResource.KeyRetaiBlock + "' ORDER BY LicenseType DESC, Description ASC";
                processDataKeyCommon.loadDataKeyAll(selectKeyOffice);
                processDataKeyCommon.loadDataKeyAll(selectKeyWindows);
                processDataKeyCommon.loadDataKeyAll(selectKeyServer);
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
        }

        public void refreshDatabase()
        {
            try
            {
                formDataKey.Invoke(new Action(() =>
                {
                    formDataKey.Cursor = Cursors.WaitCursor;
                }));
                processDataKeyCommon.DeleteAllKeyBlock(ServerKey.KeyOffice);
                processDataKeyCommon.DeleteAllKeyBlock(ServerKey.KeyWindows);
                processDataKeyCommon.DeleteAllKeyBlock(ServerKey.KeyServer);
                processDataKeyCommon.DeleteAllKeyBlock(ServerKey.KeyOther);
                formDataKey.Invoke(new Action(() =>
                {
                    formDataKey.Cursor = Cursors.Default;
                }));
                loadDataKeyCurrent();
                MessageBox.Show("Refresh database success", MessagesResource.success, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                formDataKey.Invoke(new Action(() =>
                {
                    formDataKey.Cursor = Cursors.Default;
                }));
                MessageBox.Show("Refresh database error", MessagesResource.error, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void checkKey()
        {
            string key = "";
            var listKey = getDataFromDataGridView();
            foreach (var values in listKey)
            {
                if (key == "")
                    key = values.key;
                else
                    key += "\r\n" + values.key;
            }
            formMain.tbx_PidKeyInput.AppendText(key + "\r\n");
            formMain.Show();
            formMain.Focus();
        }

        public void reCheckInforDataKey()
        {
            Thread checkKey = new Thread(checkKeyWin8_Office2019);
            Thread checkKeyWin7Office2010 = new Thread(checkKeyPIDKeyWin7Office2010);
            formDataKey.optionKey = formDataKey.BoxDescription;
            string optionKey = formDataKey.optionKey;

            var result = listOptionCbxPidKey.Where(x => optionKey.Contains(x.Key)).Select(x => x.Value).FirstOrDefault();
            if (string.IsNullOrEmpty(result))
            {
                result = optionCbxPidKey.ElementAt(0).Key;
            }
            if (result == optionCbxPidKey.ElementAt(1).Key || result == optionCbxPidKey.ElementAt(6).Key ||
                result == optionCbxPidKey.ElementAt(12).Key || result == optionCbxPidKey.ElementAt(13).Key ||
                result == optionCbxPidKey.ElementAt(14).Key || result == optionCbxPidKey.ElementAt(0).Key)
            {
                formDataKey.optionCbx = result;
                checkKeyWin7Office2010.Start();
            }
            else
            {
                formDataKey.optionCbx = result;
                checkKey.Start();
            }
            formDataKey.timer1.Start();
        }

        private ThreadLocal<ProcessPidkey> threadPIDKey = new ThreadLocal<ProcessPidkey>(() =>
        {
            return new ProcessPidkey();
        });

        private void checkKeyWin8_Office2019()
        {
            try
            {
                formDataKey.tbx_KeySearch.Invoke(new Action(() =>
                {
                    formDataKey.tbx_KeySearch.Clear();
                }));
                var listKey = getDataFromDataGridView();
                var globalkeyidx = 0;
                var countKey = listKey.Count;
                ParallelOptions po = new ParallelOptions();
                po.MaxDegreeOfParallelism = Environment.ProcessorCount;
                Parallel.For(0, countKey, po, i =>
                {
                    int localkeyidx = 0;
                    // TA2: note: cho nay la protect bien keyidx k bi conflict thread
                    formDataKey.Invoke(new Action(() =>
                    {
                        localkeyidx = globalkeyidx;
                        globalkeyidx += 1;
                    }));
                    var value = listKey[localkeyidx];
                    // TA: change: move pidkey from share to all threads to new instance per thread
                    ProcessPidkey pidkey = threadPIDKey.Value;
                    formDataKey.tbx_KeySearch.Invoke(new Action(() =>
                    {
                        formDataKey.tbx_KeySearch.Text = "Recheck infor key: " + globalkeyidx + "/" + countKey + " key";
                    }));
                    pidkey.CheckKey(value.key, formDataKey.optionCbx, false, true);
                });
                formDataKey.timer1.Stop();
                MessageBox.Show(MessagesResource.RecheckInforSuccess, MessagesResource.success, MessageBoxButtons.OK, MessageBoxIcon.Information);
                loadDataKeyCurrent();
                countKeyCurrent();
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                formDataKey.timer1.Stop();
                MessageBox.Show(MessagesResource.RecheckInforError, MessagesResource.success, MessageBoxButtons.OK, MessageBoxIcon.Error);
                loadDataKeyCurrent();
                countKeyCurrent();
            }
            finally
            {
                formDataKey.timer1.Stop();
                formDataKey.btn_Search.Invoke(new Action(() =>
                {
                    formDataKey.label1.Hide();
                    formDataKey.lb_Timer.Hide();
                    formDataKey.btn_Search.Enabled = true;
                    formDataKey.btn_Copy.Enabled = true;
                    formDataKey.btn_KeyBlock.Enabled = true;
                    formDataKey.btn_DeleteKey.Enabled = true;
                    formDataKey.btn_Refresh.Enabled = true;
                    formDataKey.cbb_Description.Enabled = true;
                    formDataKey.dgv_Key.Enabled = true;
                    formDataKey.menuStrip1.Enabled = true;
                }));
            }
        }

        private void checkKeyPIDKeyWin7Office2010()
        {
            try
            {
                formDataKey.tbx_KeySearch.Invoke(new Action(() =>
                {
                    formDataKey.tbx_KeySearch.Clear();
                }));
                var listKey = getDataFromDataGridView();
                ProcessPidkey pidkey = new ProcessPidkey();
                var j = 1;
                var countKey = listKey.Count;
                foreach (var value in listKey)
                {
                    formDataKey.tbx_KeySearch.Invoke(new Action(() =>
                    {
                        formDataKey.tbx_KeySearch.Text = "Recheck infor key: " + j + "/" + countKey + " key";
                    }));
                    pidkey.CheckKey(value.key, formDataKey.optionCbx, false, true);
                    j += 1;
                }
                formDataKey.timer1.Stop();
                MessageBox.Show(MessagesResource.RecheckInforSuccess, MessagesResource.success, MessageBoxButtons.OK, MessageBoxIcon.Information);
                loadDataKeyCurrent();
                countKeyCurrent();
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                formDataKey.timer1.Stop();
                MessageBox.Show(MessagesResource.RecheckInforError, MessagesResource.success, MessageBoxButtons.OK, MessageBoxIcon.Error);
                loadDataKeyCurrent();
                countKeyCurrent();
            }
            finally
            {
                formDataKey.timer1.Stop();
                formDataKey.btn_Search.Invoke(new Action(() =>
                {
                    formDataKey.label1.Hide();
                    formDataKey.lb_Timer.Hide();
                    formDataKey.btn_Search.Enabled = true;
                    formDataKey.btn_Copy.Enabled = true;
                    formDataKey.btn_KeyBlock.Enabled = true;
                    formDataKey.btn_DeleteKey.Enabled = true;
                    formDataKey.btn_Refresh.Enabled = true;
                    formDataKey.cbb_Description.Enabled = true;
                    formDataKey.dgv_Key.Enabled = true;
                    formDataKey.menuStrip1.Enabled = true;
                }));
            }
        }
    }
}
