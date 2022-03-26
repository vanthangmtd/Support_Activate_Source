using SupportActivate.ProcessSQL;
using SupportActivate.FormWindows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SupportActivate.Common;
using System.Data.SQLite;
using System.Windows.Forms;
using System.IO;
using FormWindows.SupportActivate;
using System.Threading;
using SupportActivate.ProcessBusiness;
using System.Threading.Tasks;

namespace SupportActivate.ProcessTabControl
{
    public class ProcessDataKey
    {
        ServerKey serverKey;
        FormSupportActivate formMain = FormSupportActivate.formMain;
        DataKey formDataKey = DataKey.formDataKey;
        ProcessDataKeyCommon processDataKeyCommon;

        static string KeyOffice = "KEYOFFICE";
        static string KeyWindows = "KEYWINDOWS";
        static string KeyServer = "KEYSERVER";
        static string KeyOther = "KEYOTHER";
        int countKey;
        int keyCurrent;

        private log4net.ILog logger = log4net.LogManager.GetLogger(typeof(ProcessDataKey));

        public ProcessDataKey()
        {
            processDataKeyCommon = new ProcessDataKeyCommon();
            serverKey = new ServerKey();
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
                    formDataKey.cbb_Description.Items.Add(Constant.allKeyVolume);
                }));

                string selectKeyOfficeVL = "SELECT DISTINCT Description FROM " + KeyOffice + " WHERE LicenseType LIKE '%Volume:MAK%' AND MAKCount NOT IN ('" + Constant.KeyBlock + "') AND ErrorCode NOT IN ('" + Constant.KeyRetaiBlock + "') ORDER BY Description ASC, LicenseType DESC";
                string selectKeyWindowsVL = "SELECT DISTINCT Description FROM " + KeyWindows + " WHERE LicenseType LIKE '%Volume:MAK%' AND MAKCount NOT IN ('" + Constant.KeyBlock + "') AND ErrorCode NOT IN ('" + Constant.KeyRetaiBlock + "') ORDER BY Description ASC, LicenseType DESC";
                string selectKeyServerVL = "SELECT DISTINCT Description FROM " + KeyServer + " WHERE LicenseType LIKE '%Volume:MAK%' AND MAKCount NOT IN ('" + Constant.KeyBlock + "') AND ErrorCode NOT IN ('" + Constant.KeyRetaiBlock + "') ORDER BY Description ASC, LicenseType DESC";
                processDataKeyCommon.loadDescriptionAllKey(selectKeyOfficeVL);
                processDataKeyCommon.loadDescriptionAllKey(selectKeyWindowsVL);
                processDataKeyCommon.loadDescriptionAllKey(selectKeyServerVL);

                formDataKey.cbb_Description.Invoke(new Action(() =>
                {
                    formDataKey.cbb_Description.Items.Add(Constant.allKeyRetail);
                }));
                string selectKeyOfficeRetail = "SELECT DISTINCT Description FROM " + KeyOffice + " WHERE LicenseType LIKE '%Retail%' AND MAKCount NOT IN ('" + Constant.KeyBlock + "') AND ErrorCode NOT IN ('" + Constant.KeyRetaiBlock + "') ORDER BY Description ASC, LicenseType DESC";
                string selectKeyWindowsRetail = "SELECT DISTINCT Description FROM " + KeyWindows + " WHERE LicenseType LIKE '%Retail%' AND MAKCount NOT IN ('" + Constant.KeyBlock + "') AND ErrorCode NOT IN ('" + Constant.KeyRetaiBlock + "') ORDER BY Description ASC, LicenseType DESC";
                string selectKeyServerRetail = "SELECT DISTINCT Description FROM " + KeyServer + " WHERE LicenseType LIKE '%Retail%' AND MAKCount NOT IN ('" + Constant.KeyBlock + "') AND ErrorCode NOT IN ('" + Constant.KeyRetaiBlock + "') ORDER BY Description ASC, LicenseType DESC";
                processDataKeyCommon.loadDescriptionAllKey(selectKeyOfficeRetail);
                processDataKeyCommon.loadDescriptionAllKey(selectKeyWindowsRetail);
                processDataKeyCommon.loadDescriptionAllKey(selectKeyServerRetail);

                formDataKey.cbb_Description.Invoke(new Action(() =>
                {
                    formDataKey.cbb_Description.Items.Add(Constant.allKeyOEM);
                }));
                string selectKeyOfficeOEM = "SELECT DISTINCT Description FROM " + KeyOffice + " WHERE LicenseType LIKE '%OEM%' AND MAKCount NOT IN ('" + Constant.KeyBlock + "') AND ErrorCode NOT IN ('" + Constant.KeyRetaiBlock + "') ORDER BY Description ASC, LicenseType DESC";
                string selectKeyWindowsOEM = "SELECT DISTINCT Description FROM " + KeyWindows + " WHERE LicenseType LIKE '%OEM%' AND MAKCount NOT IN ('" + Constant.KeyBlock + "') AND ErrorCode NOT IN ('" + Constant.KeyRetaiBlock + "') ORDER BY Description ASC, LicenseType DESC";
                string selectKeyServerOEM = "SELECT DISTINCT Description FROM " + KeyServer + " WHERE LicenseType LIKE '%OEM%' AND MAKCount NOT IN ('" + Constant.KeyBlock + "') AND ErrorCode NOT IN ('" + Constant.KeyRetaiBlock + "') ORDER BY Description ASC, LicenseType DESC";
                processDataKeyCommon.loadDescriptionAllKey(selectKeyOfficeOEM);
                processDataKeyCommon.loadDescriptionAllKey(selectKeyWindowsOEM);
                processDataKeyCommon.loadDescriptionAllKey(selectKeyServerOEM);

                string selectKeyOfficeOther = "SELECT DISTINCT Description FROM " + KeyOffice + " WHERE LicenseType NOT IN (" + "SELECT DISTINCT LicenseType FROM " + KeyOffice + " WHERE LicenseType LIKE '%Volume%' OR LicenseType LIKE '%Retail%' OR LicenseType LIKE '%OEM%'" + ") AND MAKCount NOT IN ('" + Constant.KeyBlock + "') AND ErrorCode NOT IN ('" + Constant.KeyRetaiBlock + "') ORDER BY Description ASC, LicenseType DESC";
                string selectKeyWindowsOther = "SELECT DISTINCT Description FROM " + KeyWindows + " WHERE LicenseType NOT IN (" + "SELECT DISTINCT LicenseType FROM " + KeyWindows + " WHERE LicenseType LIKE '%Volume%' OR LicenseType LIKE '%Retail%' OR LicenseType LIKE '%OEM%'" + ") And MAKCount Not IN ('" + Constant.KeyBlock + "') AND ErrorCode NOT IN ('" + Constant.KeyRetaiBlock + "') ORDER BY Description ASC, LicenseType DESC";
                string selectKeyServerOther = "SELECT DISTINCT Description FROM " + KeyServer + " WHERE LicenseType NOT IN (" + "SELECT DISTINCT LicenseType FROM " + KeyServer + " WHERE LicenseType LIKE '%Volume%' OR LicenseType LIKE '%Retail%' OR LicenseType LIKE '%OEM%'" + ") And MAKCount Not IN ('" + Constant.KeyBlock + "') AND ErrorCode NOT IN ('" + Constant.KeyRetaiBlock + "') ORDER BY Description ASC, LicenseType DESC";
                processDataKeyCommon.loadDescriptionAllKey(selectKeyOfficeOther);
                processDataKeyCommon.loadDescriptionAllKey(selectKeyWindowsOther);
                processDataKeyCommon.loadDescriptionAllKey(selectKeyServerOther);

                formDataKey.cbb_Description.Invoke(new Action(() =>
                {
                    formDataKey.cbb_Description.Items.Add(Constant.allKeyNOTDEFINED);
                }));
                string selectKeyOtherVL = "SELECT DISTINCT Description FROM " + KeyOther + " WHERE LicenseType LIKE '%Volume:MAK%' AND MAKCount NOT IN ('" + Constant.KeyBlock + "') AND ErrorCode NOT IN ('" + Constant.KeyRetaiBlock + "') ORDER BY Description ASC, LicenseType DESC";
                string selectKeyOtherRetail = "SELECT DISTINCT Description FROM " + KeyOther + " WHERE LicenseType LIKE '%Retail%' AND MAKCount NOT IN ('" + Constant.KeyBlock + "') AND ErrorCode NOT IN ('" + Constant.KeyRetaiBlock + "') ORDER BY Description ASC, LicenseType DESC";
                string selectKeyOtherOEM = "SELECT DISTINCT Description FROM " + KeyOther + " WHERE LicenseType LIKE '%OEM%' AND MAKCount NOT IN ('" + Constant.KeyBlock + "') AND ErrorCode NOT IN ('" + Constant.KeyRetaiBlock + "') ORDER BY Description ASC, LicenseType DESC";
                string selectKeyOtherOther = "SELECT DISTINCT Description FROM " + KeyOther + " WHERE LicenseType NOT IN (" + "SELECT DISTINCT LicenseType FROM " + KeyOther + " WHERE LicenseType LIKE '%Volume%' OR LicenseType LIKE '%Retail%' OR LicenseType LIKE '%OEM%'" + ") And MAKCount Not IN ('" + Constant.KeyBlock + "') AND ErrorCode NOT IN ('" + Constant.KeyRetaiBlock + "') ORDER BY Description ASC, LicenseType DESC";
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
                string countKeyOffice = "SELECT count(Id) FROM " + KeyOffice + " WHERE MAKCount NOT IN ('" + Constant.KeyBlock + "') AND ErrorCode NOT IN ('" + Constant.KeyRetaiBlock + "')";
                countKey = processDataKeyCommon.totalKey(countKeyOffice);
                string countKeyWindows = "SELECT count(Id) FROM " + KeyWindows + " WHERE MAKCount NOT IN ('" + Constant.KeyBlock + "') AND ErrorCode NOT IN ('" + Constant.KeyRetaiBlock + "')";
                countKey = countKey + processDataKeyCommon.totalKey(countKeyWindows);
                string countKeyServer = "SELECT count(Id) FROM " + KeyServer + " WHERE MAKCount NOT IN ('" + Constant.KeyBlock + "') AND ErrorCode NOT IN ('" + Constant.KeyRetaiBlock + "')";
                countKey = countKey + processDataKeyCommon.totalKey(countKeyServer);
                string countKeyOther = "SELECT count(Id) FROM " + KeyOther + " WHERE MAKCount NOT IN ('" + Constant.KeyBlock + "') AND ErrorCode NOT IN ('" + Constant.KeyRetaiBlock + "')";
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
                formDataKey.optionVerKey = formDataKey.BoxDescription == Constant.allKeyVolume ? Constant.Volume :
                               formDataKey.BoxDescription == Constant.allKeyRetail ? Constant.Retail :
                               formDataKey.BoxDescription == Constant.allKeyOEM ? Constant.OEM :
                               formDataKey.BoxDescription == Constant.allKeyNOTDEFINED ? Constant.NOTDEFINED : string.Empty;
                if (string.IsNullOrEmpty(formDataKey.optionVerKey))
                {
                    var checkKeyOffice = formDataKey.BoxDescription.IndexOf("Office");
                    var checkKeyWin = formDataKey.BoxDescription.IndexOf("Win");
                    var checkKeyServer = formDataKey.BoxDescription.IndexOf("Server");
                    formDataKey.optionVerKey = formDataKey.BoxDescription;
                    formDataKey.selectVerKey = checkKeyServer != -1 ? KeyServer :
                                   checkKeyOffice != -1 ? KeyOffice :
                                   checkKeyWin != -1 ? KeyWindows : KeyOther;
                    loadDataKeyDescription();
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
                var i = 1;
                string selectKeyOffice = "SELECT * FROM " + KeyOffice + " WHERE MAKCount NOT IN ('" + Constant.KeyBlock + "') AND ErrorCode NOT IN ('" + Constant.KeyRetaiBlock + "') ORDER BY LicenseType DESC, Description ASC";
                string selectKeyWindows = "SELECT * FROM " + KeyWindows + " WHERE MAKCount NOT IN ('" + Constant.KeyBlock + "') AND ErrorCode NOT IN ('" + Constant.KeyRetaiBlock + "')  ORDER BY LicenseType DESC, Description ASC";
                string selectKeyServer = "SELECT * FROM " + KeyServer + " WHERE MAKCount NOT IN ('" + Constant.KeyBlock + "') AND ErrorCode NOT IN ('" + Constant.KeyRetaiBlock + "')  ORDER BY LicenseType DESC, Description ASC";
                string[] rowNull = new string[] { "", "", "", "", "", "", "", "", "" };
                i = processDataKeyCommon.loadDataKeyAll(selectKeyOffice, i);
                formDataKey.dgv_Key.Invoke(new Action(() =>
                {
                    formDataKey.dgv_Key.Rows.Add(rowNull);
                }));
                i = processDataKeyCommon.loadDataKeyAll(selectKeyWindows, i);
                formDataKey.dgv_Key.Invoke(new Action(() =>
                {
                    formDataKey.dgv_Key.Rows.Add(rowNull);
                }));
                i = processDataKeyCommon.loadDataKeyAll(selectKeyServer, i);
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
                var i = 1;
                if (formDataKey.optionVerKey == "KEY NOT DEFINED")
                {
                    string selectKeyOther = "SELECT * FROM " + KeyOther + " WHERE MAKCount NOT IN ('" + Constant.KeyBlock + "') AND ErrorCode NOT IN ('" + Constant.KeyRetaiBlock + "') ORDER BY Description ASC, MAKCount DESC";
                    i = processDataKeyCommon.loadDataKeyAll(selectKeyOther, i);
                }
                else
                {
                    string selectKeyOffice = "SELECT * FROM " + KeyOffice + " WHERE LicenseType LIKE '" + formDataKey.optionVerKey + "%' AND MAKCount NOT IN ('" + Constant.KeyBlock + "') AND ErrorCode NOT IN ('" + Constant.KeyRetaiBlock + "') ORDER BY Description ASC, MAKCount DESC";
                    string selectKeyWindows = "SELECT * FROM " + KeyWindows + " WHERE LicenseType LIKE '" + formDataKey.optionVerKey + "%' AND MAKCount NOT IN ('" + Constant.KeyBlock + "') AND ErrorCode NOT IN ('" + Constant.KeyRetaiBlock + "') ORDER BY Description ASC, MAKCount DESC";
                    string selectKeyServer = "SELECT * FROM " + KeyServer + " WHERE LicenseType LIKE '" + formDataKey.optionVerKey + "%' AND MAKCount NOT IN ('" + Constant.KeyBlock + "') AND ErrorCode NOT IN ('" + Constant.KeyRetaiBlock + "') ORDER BY Description ASC, MAKCount DESC";
                    i = processDataKeyCommon.loadDataKeyAll(selectKeyOffice, i);
                    i = processDataKeyCommon.loadDataKeyAll(selectKeyWindows, i);
                    i = processDataKeyCommon.loadDataKeyAll(selectKeyServer, i);
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
        }

        private void loadDataKeyDescription()
        {
            try
            {
                formDataKey.dgv_Key.Invoke(new Action(() =>
                {
                    formDataKey.dgv_Key.Rows.Clear();
                }));
                var i = 1;
                using (SQLiteConnection SqlConn = new SQLiteConnection(serverKey.connectionString))
                {
                    string selectKey = "SELECT * FROM " + formDataKey.selectVerKey + " WHERE ";
                    if (!string.IsNullOrEmpty(formDataKey.optionVerKey))
                        selectKey = selectKey + " Description=@Description AND ";
                    selectKey = selectKey + "MAKCount NOT IN ('" + Constant.KeyBlock + "') AND ErrorCode NOT IN ('" + Constant.KeyRetaiBlock + "') ORDER BY MAKCount DESC";
                    SQLiteCommand cmdKey = new SQLiteCommand(selectKey, SqlConn);
                    if (!string.IsNullOrEmpty(formDataKey.optionVerKey))
                        cmdKey.Parameters.AddWithValue("@Description", formDataKey.optionVerKey);
                    SqlConn.Open();
                    formDataKey.dgv_Key.Invoke(new Action(() =>
                    {
                        SQLiteDataReader readerKey = cmdKey.ExecuteReader();
                        while (readerKey.Read())
                        {
                            string[] row = new string[] { i.ToString(), readerKey.GetString(1), readerKey.GetString(2), readerKey.GetString(3), readerKey.GetString(4), readerKey.GetString(5), readerKey.GetString(6), readerKey.GetString(7), readerKey.GetString(8) };
                            formDataKey.dgv_Key.Rows.Add(row);
                            i += 1;
                        }
                    }));
                    SqlConn.Close();
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
                if (formDataKey.optionVerKey == Constant.Volume || formDataKey.optionVerKey == Constant.Retail || formDataKey.optionVerKey == Constant.OEM || formDataKey.optionVerKey == Constant.NOTDEFINED)
                {
                    if (formDataKey.optionVerKey == Constant.NOTDEFINED)
                    {
                        string selectKeyOther = "SELECT count(Id) FROM " + KeyOther + " WHERE MAKCount NOT IN ('" + Constant.KeyBlock + "') AND ErrorCode NOT IN ('" + Constant.KeyRetaiBlock + "')";
                        keyCurrent = processDataKeyCommon.totalKey(selectKeyOther);
                    }
                    else
                    {
                        string selectKeyOffice = "SELECT count(Id) FROM " + KeyOffice + " WHERE LicenseType LIKE '" + formDataKey.optionVerKey + "%' AND MAKCount NOT IN ('" + Constant.KeyBlock + "') AND ErrorCode NOT IN ('" + Constant.KeyRetaiBlock + "')";
                        string selectKeyWindows = "SELECT count(Id) FROM " + KeyWindows + " WHERE LicenseType LIKE '" + formDataKey.optionVerKey + "%' AND MAKCount NOT IN ('" + Constant.KeyBlock + "') AND ErrorCode NOT IN ('" + Constant.KeyRetaiBlock + "')";
                        string selectKeyServer = "SELECT count(Id) FROM " + KeyServer + " WHERE LicenseType LIKE '" + formDataKey.optionVerKey + "%' AND MAKCount NOT IN ('" + Constant.KeyBlock + "') AND ErrorCode NOT IN ('" + Constant.KeyRetaiBlock + "')";
                        keyCurrent = processDataKeyCommon.totalKey(selectKeyOffice);
                        keyCurrent = keyCurrent + processDataKeyCommon.totalKey(selectKeyWindows);
                        keyCurrent = keyCurrent + processDataKeyCommon.totalKey(selectKeyServer);
                    }
                }
                else
                {
                    using (SQLiteConnection SqlConn = new SQLiteConnection(serverKey.connectionString))
                    {
                        string selectKey = "SELECT count(Id) FROM " + formDataKey.selectVerKey + " WHERE Description=@Description AND MAKCount NOT IN ('" + Constant.KeyBlock + "') AND ErrorCode NOT IN ('" + Constant.KeyRetaiBlock + "')";
                        SQLiteCommand cmdKey = new SQLiteCommand(selectKey, SqlConn);
                        cmdKey.Parameters.AddWithValue("@Description", formDataKey.optionVerKey);
                        SqlConn.Open();
                        SQLiteDataReader readerKey = cmdKey.ExecuteReader();
                        while (readerKey.Read())
                            keyCurrent = readerKey.GetInt32(0);
                        SqlConn.Close();
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
                    string updateKeyOffice = "UPDATE " + KeyOffice + " SET Note='" + note + "' WHERE Key='" + key + "'";
                    string updateKeyWindows = "UPDATE " + KeyWindows + " SET Note='" + note + "' WHERE Key='" + key + "'";
                    string updateKeyServer = "UPDATE " + KeyServer + " SET Note='" + note + "' WHERE Key='" + key + "'";
                    string updateKeyOther = "UPDATE " + KeyOther + " SET Note='" + note + "' WHERE Key='" + key + "'";
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
                StringBuilder inforKey = new StringBuilder();
                foreach (var values in formDataKey.listKey)
                {
                    StringBuilder resultInfoKeyOffice = new StringBuilder(getInfoKey(KeyOffice, values.key));
                    StringBuilder resultInfoKeyWindows = new StringBuilder(getInfoKey(KeyWindows, values.key));
                    StringBuilder resultInfoKeyOther = new StringBuilder(getInfoKey(KeyOther, values.key));
                    StringBuilder resultInfoKeyServer = new StringBuilder(getInfoKey(KeyServer, values.key));
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

        private string getInfoKey(string table, string key)
        {
            string inforKey = string.Empty;
            using (SQLiteConnection SqlConn = new SQLiteConnection(serverKey.connectionString))
            {
                string selectKey = "SELECT * FROM " + table + " WHERE Key=@Key";
                SQLiteCommand cmdKey = new SQLiteCommand(selectKey, SqlConn);
                cmdKey.Parameters.AddWithValue("@Key", key);
                SqlConn.Open();
                SQLiteDataReader readerKey = cmdKey.ExecuteReader();
                while (readerKey.Read())
                {
                    if (!string.IsNullOrEmpty(readerKey.GetString(1)))
                        inforKey = inforKey + "Key: " + readerKey.GetValue(1);
                    if (!string.IsNullOrEmpty(readerKey.GetString(2)))
                        inforKey = inforKey + "\r\nDescription: " + readerKey.GetValue(2);
                    if (!string.IsNullOrEmpty(readerKey.GetString(3)))
                        inforKey = inforKey + "\r\nSubType: " + readerKey.GetValue(3);
                    if (!string.IsNullOrEmpty(readerKey.GetString(4)))
                        inforKey = inforKey + "\r\nLicenseType: " + readerKey.GetValue(4);
                    if (!string.IsNullOrEmpty(readerKey.GetString(5)))
                        inforKey = inforKey + "\r\nMAKCount: " + readerKey.GetValue(5);
                    if (!string.IsNullOrEmpty(readerKey.GetString(6)))
                        inforKey = inforKey + "\r\nErrorCode: " + readerKey.GetValue(6);
                    if (!string.IsNullOrEmpty(readerKey.GetString(7)))
                        inforKey = inforKey + "\r\nGetweb: " + readerKey.GetValue(7);
                    if (!string.IsNullOrEmpty(readerKey.GetString(8)))
                        inforKey = inforKey + "\r\nNote: " + readerKey.GetValue(8);
                    inforKey = inforKey + "\r\n--------------------\r\n";
                }
                SqlConn.Close();
            }
            return inforKey;
        }

        public void getValuesDgvKey()
        {
            try
            {
                string values = string.Empty;
                for (var i = 0; i <= formDataKey.listKey.Count - 1; i++)
                    values = values + "\r\n" + formDataKey.listKey[i].key;
                if (!string.IsNullOrEmpty(values))
                    Clipboard.SetText(values.Trim().Replace(" ", ""));
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
        }

        public void selectAllDgvKey()
        {
            formDataKey.listKey.Clear();
            bool recoverKey = false;
            formDataKey.dgv_Key.SelectAll();
            int i = 0;
            foreach (DataGridViewRow row in formDataKey.dgv_Key.Rows)
            {
                var selectedRow = row.Index;
                formDataKey.listKey.Add(new dataKey() { id = i, key = formDataKey.dgv_Key[1, selectedRow].Value.ToString() });
                recoverKey = formDataKey.dgv_Key[5, selectedRow].Value.ToString() == Constant.KeyBlock ? true :
                             formDataKey.dgv_Key[6, selectedRow].Value.ToString() == Constant.KeyRetaiBlock ? true : false;
                i += 1;
            }

            formDataKey.contextMenuStrip1.Enabled = true;
            formDataKey.contextMenu_CopyTheSelectedKey.Enabled = true;
            formDataKey.contextMenu_CopyTheSelectedKeyAndTheirInformation.Enabled = true;
            formDataKey.contextMenu_RecheckTheSelectedKey.Enabled = true;
            formDataKey.contextMenu_RecheckInformationTheSelectedKey.Enabled = true;
            formDataKey.contextMenu_DeleteTheSelectedKey.Enabled = true;
            if (recoverKey == true)
            {
                formDataKey.contextMenu_RecoveryTheSelectedKey.Enabled = true;
                formDataKey.menu_RecoveryTheSelectedKey.Enabled = true;
            }
            else
            {
                formDataKey.contextMenu_RecoveryTheSelectedKey.Enabled = false;
                formDataKey.menu_RecoveryTheSelectedKey.Enabled = false;
            }
            formDataKey.contextMenu_ChangeTheSelectedKeyToKeyBlock.Enabled = true;
            formDataKey.menu_RecheckTheSelectedKey.Enabled = true;
            formDataKey.menu_RecheckInformationTheSelectedKey.Enabled = true;
            formDataKey.menu_ChangeTheSelectedKeyToKeyblock.Enabled = true;
            formDataKey.menu_CopyTheSelectedKey.Enabled = true;
            formDataKey.menu_CopyTheSelectedKeyAndTheirInformation.Enabled = true;
            formDataKey.menu_DeleteTheSelectedKey.Enabled = true;
            formDataKey.btn_Copy.Enabled = true;
            formDataKey.btn_DeleteKey.Enabled = true;
        }

        public void changeToKeyBlock()
        {
            try
            {
                foreach (var values in formDataKey.listKey)
                {
                    processDataKeyCommon.blockKey(KeyOffice, values.key);
                    processDataKeyCommon.blockKey(KeyWindows, values.key);
                    processDataKeyCommon.blockKey(KeyServer, values.key);
                    processDataKeyCommon.blockKey(KeyOther, values.key);
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
                foreach (var values in formDataKey.listKey)
                {
                    processDataKeyCommon.deleteKey(KeyOffice, values.key);
                    processDataKeyCommon.deleteKey(KeyWindows, values.key);
                    processDataKeyCommon.deleteKey(KeyServer, values.key);
                    processDataKeyCommon.deleteKey(KeyOther, values.key);
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
                var i = 1;
                string selectKeyOffice = "SELECT * FROM " + KeyOffice + " WHERE Key LIKE '%" + formDataKey.keySearch + "%'";
                string selectKeyWindows = "SELECT * FROM " + KeyWindows + " WHERE Key LIKE '%" + formDataKey.keySearch + "%'";
                string selectKeyServer = "SELECT * FROM " + KeyServer + " WHERE Key LIKE '%" + formDataKey.keySearch + "%'";
                string selectKeyOther = "SELECT * FROM " + KeyOther + " WHERE Key LIKE '%" + formDataKey.keySearch + "%'";
                i = processDataKeyCommon.loadDataKeyAll(selectKeyOffice, i);
                i = processDataKeyCommon.loadDataKeyAll(selectKeyWindows, i);
                i = processDataKeyCommon.loadDataKeyAll(selectKeyServer, i);
                i = processDataKeyCommon.loadDataKeyAll(selectKeyOther, i);
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
                foreach (var values in formDataKey.listKey)
                {
                    processDataKeyCommon.recoveryKeyBlock(KeyOffice, values.key);
                    processDataKeyCommon.recoveryKeyBlock(KeyWindows, values.key);
                    processDataKeyCommon.recoveryKeyBlock(KeyServer, values.key);
                    processDataKeyCommon.recoveryKeyBlock(KeyOther, values.key);
                }
                loadDataKeyCurrent();
                MessageBox.Show(Messages.RecoverKeySuccess, Messages.success, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                MessageBox.Show(Messages.RecoverKeyError, Messages.success, MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                        MessageBox.Show(Messages.SaveSuccess, Messages.success, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        logger.Error(ex);
                        MessageBox.Show(Messages.SaveError, Messages.success, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private List<string> loadDataSave()
        {
            List<string> data = new List<string>();
            try
            {
                foreach (var value in processDataKeyCommon.loadDataSave(KeyOffice))
                    data.Add(value);
                foreach (var value in processDataKeyCommon.loadDataSave(KeyWindows))
                    data.Add(value);
                foreach (var value in processDataKeyCommon.loadDataSave(KeyServer))
                    data.Add(value);
                foreach (var value in processDataKeyCommon.loadDataSave(KeyOther))
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
                count = count + processDataKeyCommon.readDBBK(KeyOffice, count, part);
                count = count + processDataKeyCommon.readDBBK(KeyWindows, count, part);
                count = count + processDataKeyCommon.readDBBK(KeyServer, count, part);
                count = count + processDataKeyCommon.readDBBK(KeyOther, count, part);
                formDataKey.Invoke(new Action(() =>
                {
                    formDataKey.Cursor = Cursors.Default;
                }));
                MessageBox.Show("Restore database success", Messages.success, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                formDataKey.Invoke(new Action(() =>
                {
                    formDataKey.Cursor = Cursors.Default;
                }));
                MessageBox.Show("Restore database error", Messages.success, MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                        string ActivCount = string.Empty, Blocked = string.Empty;
                        if (reader.GetString(4) == "Key Blocked!")
                            ActivCount = "" + Constant.KeyBlock + "";
                        else if (string.IsNullOrEmpty(reader.GetString(4)))
                            ActivCount = "";
                        if (reader.GetString(5) == "Key Blocked!")
                            Blocked = "" + Constant.KeyRetaiBlock + "";
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
                MessageBox.Show("Read database PIDKey Lite success", Messages.success, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                formDataKey.Invoke(new Action(() =>
                {
                    formDataKey.Cursor = Cursors.Default;
                }));
                MessageBox.Show("Read database PIDKey Lite error", Messages.error, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void loadAllDataKeyBlock()
        {
            try
            {
                var i = 1;
                string selectKeyOffice = "SELECT * FROM " + KeyOffice + " WHERE MAKCount IN ('" + Constant.KeyBlock + "') OR ErrorCode IN ('" + Constant.KeyRetaiBlock + "') ORDER BY LicenseType DESC, Description ASC";
                string selectKeyWindows = "SELECT * FROM " + KeyWindows + " WHERE MAKCount IN ('" + Constant.KeyBlock + "') OR ErrorCode IN ('" + Constant.KeyRetaiBlock + "')  ORDER BY LicenseType DESC, Description ASC";
                string selectKeyServer = "SELECT * FROM " + KeyServer + " WHERE MAKCount IN ('" + Constant.KeyBlock + "') OR ErrorCode IN ('" + Constant.KeyRetaiBlock + "')  ORDER BY LicenseType DESC, Description ASC";
                i = processDataKeyCommon.loadDataKeyAll(selectKeyOffice, i);
                i = processDataKeyCommon.loadDataKeyAll(selectKeyWindows, i);
                i = processDataKeyCommon.loadDataKeyAll(selectKeyServer, i);
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
                using (SQLiteConnection SqlConn = new SQLiteConnection(serverKey.connectionString))
                {
                    string deleteKeyOffice = "DELETE FROM " + KeyOffice + " WHERE MAKCount IN ('" + Constant.KeyBlock + "') OR ErrorCode IN ('" + Constant.KeyRetaiBlock + "')";
                    string deleteKeyWindows = "DELETE FROM " + KeyWindows + " WHERE MAKCount IN ('" + Constant.KeyBlock + "') OR ErrorCode IN ('" + Constant.KeyRetaiBlock + "')";
                    string deleteKeyServer = "DELETE FROM " + KeyServer + " WHERE MAKCount IN ('" + Constant.KeyBlock + "') OR ErrorCode IN ('" + Constant.KeyRetaiBlock + "')";
                    string deleteKeyOther = "DELETE FROM " + KeyOther + " WHERE MAKCount IN ('" + Constant.KeyBlock + "') OR ErrorCode IN ('" + Constant.KeyRetaiBlock + "')";
                    SQLiteCommand cmdOffice = new SQLiteCommand(deleteKeyOffice, SqlConn);
                    SQLiteCommand cmdWindows = new SQLiteCommand(deleteKeyWindows, SqlConn);
                    SQLiteCommand cmdServer = new SQLiteCommand(deleteKeyServer, SqlConn);
                    SQLiteCommand cmdOther = new SQLiteCommand(deleteKeyOther, SqlConn);
                    SqlConn.Open();
                    cmdOffice.ExecuteNonQuery();
                    cmdWindows.ExecuteNonQuery();
                    cmdServer.ExecuteNonQuery();
                    cmdOther.ExecuteNonQuery();
                    SqlConn.Close();
                }
                formDataKey.Invoke(new Action(() =>
                {
                    formDataKey.Cursor = Cursors.Default;
                }));
                loadDataKeyCurrent();
                MessageBox.Show("Refresh database success", Messages.success, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                formDataKey.Invoke(new Action(() =>
                {
                    formDataKey.Cursor = Cursors.Default;
                }));
                MessageBox.Show("Refresh database error", Messages.error, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void checkKey()
        {
            string key = "";
            foreach (var values in formDataKey.listKey)
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
            if (formDataKey.optionKey.Contains("Office14"))
            {
                formDataKey.optionCbx = "Office 2010";
                checkKeyWin7Office2010.Start();
            }
            else if (formDataKey.optionKey.Contains("Office15"))
            {
                formDataKey.optionCbx = "Office 2013";
                checkKey.Start();
            }
            else if (formDataKey.optionKey.Contains("Office16"))
            {
                formDataKey.optionCbx = "Office 2016";
                checkKey.Start();
            }
            else if (formDataKey.optionKey.Contains("Office19"))
            {
                formDataKey.optionCbx = "Office 2019";
                checkKey.Start();
            }
            else if (formDataKey.optionKey.Contains("Office21"))
            {
                formDataKey.optionCbx = "Office 2021";
                checkKey.Start();
            }
            else if (formDataKey.optionKey.Contains("Windows 7"))
            {
                formDataKey.optionCbx = "Windows 7 - Server 2008 R2 - Embedded 7";
                checkKeyWin7Office2010.Start();
            }
            else if (formDataKey.optionKey.Contains("Win 8.1"))
            {
                formDataKey.optionCbx = "Windows 8.1 - Server 2012 R2";
                checkKey.Start();
            }
            else if (formDataKey.optionKey.Contains("Win 8"))
            {
                formDataKey.optionCbx = "Windows 8 - Server 2012 - Embedded 8";
                checkKey.Start();
            }
            else if (formDataKey.optionKey.Contains("Win 10"))
            {
                formDataKey.optionCbx = "Windows 10 - Server 2016/2019";
                checkKey.Start();
            }
            else if (formDataKey.optionKey.Contains("Win 2021"))
            {
                formDataKey.optionCbx = "Windows 10 - Server 2016/2019";
                checkKey.Start();
            }
            else if (formDataKey.optionKey.Contains("Win 11"))
            {
                formDataKey.optionCbx = "Windows 11 - Server 2021";
                checkKey.Start();
            }
            else if (formDataKey.optionKey.Contains("Server 12 R2"))
            {
                formDataKey.optionCbx = "Windows 8.1 - Server 2012 R2";
                checkKey.Start();
            }
            else if (formDataKey.optionKey.Contains("Server Essentials 2012 R2"))
            {
                formDataKey.optionCbx = "Windows 8.1 - Server 2012 R2";
                checkKey.Start();
            }
            else if (formDataKey.optionKey.Contains("Server 2012"))
            {
                formDataKey.optionCbx = "Windows 8 - Server 2012 - Embedded 8";
                checkKey.Start();
            }
            else if (formDataKey.optionKey.Contains("Server Essentials 2012"))
            {
                formDataKey.optionCbx = "Windows 8 - Server 2012 - Embedded 8";
                checkKey.Start();
            }
            else if (formDataKey.optionKey.Contains("Server 2016"))
            {
                formDataKey.optionCbx = "Windows 10 - Server 2016/2019";
                checkKey.Start();
            }
            else if (formDataKey.optionKey.Contains("Server Essentials 2016"))
            {
                formDataKey.optionCbx = "Windows 10 - Server 2016/2019";
                checkKey.Start();
            }
            else if (formDataKey.optionKey.Contains("Server 2019"))
            {
                formDataKey.optionCbx = "Windows 10 - Server 2016/2019";
                checkKey.Start();
            }
            else if (formDataKey.optionKey.Contains("Server Essentials 2019"))
            {
                formDataKey.optionCbx = "Windows 10 - Server 2016/2019";
                checkKey.Start();
            }
            else if (formDataKey.optionKey.Contains("Server 2021"))
            {
                formDataKey.optionCbx = "Windows 11 - Server 2021";
                checkKey.Start();
            }
            else if (formDataKey.optionKey.Contains("Embedded Industry 8.1"))
            {
                formDataKey.optionCbx = "Windows 8 - Server 2012 - Embedded 8";
                checkKey.Start();
            }
            else if (formDataKey.optionKey.Contains("Embedded Industry 7"))
            {
                formDataKey.optionCbx = "Windows 7 - Server 2008 R2 - Embedded 7";
                checkKeyWin7Office2010.Start();
            }
            else
            {
                formDataKey.optionCbx = "All Edition Windows/Office";
                checkKeyWin7Office2010.Start();
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
                var globalkeyidx = 0;
                var countKey = formDataKey.listKey.Count;
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
                    var value = formDataKey.listKey[localkeyidx];
                    // TA: change: move pidkey from share to all threads to new instance per thread
                    ProcessPidkey pidkey = threadPIDKey.Value;
                    formDataKey.tbx_KeySearch.Invoke(new Action(() =>
                    {
                        formDataKey.tbx_KeySearch.Text = "Recheck infor key: " + globalkeyidx + "/" + countKey + " key";
                    }));
                    pidkey.CheckKey(value.key, formDataKey.optionCbx, false, true);
                });
                formDataKey.timer1.Stop();
                MessageBox.Show(Messages.RecheckInforSuccess, Messages.success, MessageBoxButtons.OK, MessageBoxIcon.Information);
                loadDataKeyCurrent();
                countKeyCurrent();
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                formDataKey.timer1.Stop();
                MessageBox.Show(Messages.RecheckInforError, Messages.success, MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                ProcessPidkey pidkey = new ProcessPidkey();
                var j = 1;
                var countKey = formDataKey.listKey.Count;
                foreach (var value in formDataKey.listKey)
                {
                    formDataKey.tbx_KeySearch.Invoke(new Action(() =>
                    {
                        formDataKey.tbx_KeySearch.Text = "Recheck infor key: " + j + "/" + countKey + " key";
                    }));
                    pidkey.CheckKey(value.key, formDataKey.optionCbx, false, true);
                    j += 1;
                }
                formDataKey.timer1.Stop();
                MessageBox.Show(Messages.RecheckInforSuccess, Messages.success, MessageBoxButtons.OK, MessageBoxIcon.Information);
                loadDataKeyCurrent();
                countKeyCurrent();
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                formDataKey.timer1.Stop();
                MessageBox.Show(Messages.RecheckInforError, Messages.success, MessageBoxButtons.OK, MessageBoxIcon.Error);
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
