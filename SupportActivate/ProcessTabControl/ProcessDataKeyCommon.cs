using SupportActivate.Common;
using SupportActivate.FormWindows;
using SupportActivate.ProcessSQL;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Text;

namespace SupportActivate.ProcessTabControl
{
    public class ProcessDataKeyCommon
    {
        ServerKey serverKey;
        DataKey formDataKey = DataKey.formDataKey;

        private log4net.ILog logger = log4net.LogManager.GetLogger(typeof(ProcessDataKeyCommon));

        public ProcessDataKeyCommon()
        {
            serverKey = new ServerKey();
        }

        public void addNote(string query)
        {
            using (SQLiteConnection SqlConn = new SQLiteConnection(serverKey.connectionString))
            {
                SQLiteCommand cmd = new SQLiteCommand(query, SqlConn);
                SqlConn.Open();
                cmd.ExecuteNonQuery();
                SqlConn.Close();
            }
        }

        public void deleteKey(string table, string key)
        {
            using (SQLiteConnection SqlConn = new SQLiteConnection(serverKey.connectionString))
            {
                string deleteKey = "DELETE FROM " + table + " WHERE Key=@Key";
                SQLiteCommand cmd = new SQLiteCommand(deleteKey, SqlConn);
                cmd.Parameters.AddWithValue("@Key", key);
                SqlConn.Open();
                cmd.ExecuteNonQuery();
                SqlConn.Close();
            }
        }

        public void blockKey(string table, string key)
        {
            using (SQLiteConnection SqlConn = new SQLiteConnection(serverKey.connectionString))
            {
                string updateKey = "UPDATE " + table + " SET ErrorCode='" + ContantResource.KeyRetaiBlock + "' WHERE Key=@Key";
                SQLiteCommand cmd = new SQLiteCommand(updateKey, SqlConn);
                cmd.Parameters.AddWithValue("@Key", key);
                SqlConn.Open();
                cmd.ExecuteNonQuery();
                SqlConn.Close();
            }
        }

        public void recoveryKeyBlock(string table, string key)
        {
            using (SQLiteConnection SqlConn = new SQLiteConnection(serverKey.connectionString))
            {
                string query = "UPDATE " + table + " SET MAKCount='-1', ErrorCode='' WHERE Key='" + key + "'";
                SQLiteCommand cmd = new SQLiteCommand(query, SqlConn);
                SqlConn.Open();
                cmd.ExecuteNonQuery();
                SqlConn.Close();
            }
        }

        public void loadDescriptionAllKey(string query)
        {
            using (SQLiteConnection SqlConn = new SQLiteConnection(serverKey.connectionString))
            {
                SQLiteCommand cmd = new SQLiteCommand(query, SqlConn);
                SqlConn.Open();
                SQLiteDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    formDataKey.cbb_Description.Invoke(new Action(() =>
                    {
                        formDataKey.cbb_Description.Items.Add(reader.GetValue(0));
                    }));
                }
                SqlConn.Close();
            }
        }

        public void loadDataKeyAll(string query)
        {
            int index_row = 0;
            using (SQLiteConnection SqlConn = new SQLiteConnection(serverKey.connectionString))
            {
                SQLiteCommand cmd = new SQLiteCommand(query, SqlConn);
                SqlConn.Open();
                SQLiteDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    object[] row = new object[] { index_row + 1, reader.GetString(1), reader.GetString(2), reader.GetString(3), reader.GetString(4), reader.GetInt32(5), reader.GetString(6), reader.GetString(7), reader.GetString(8) };
                    formDataKey.dgv_Key.Invoke(new Action(() =>
                    {
                        formDataKey.dgv_Key.Rows.Add(row);
                    }));
                    index_row += 1;
                }
                SqlConn.Close();
            }
        }

        public void loadDataKeyDescription()
        {
            try
            {
                formDataKey.dgv_Key.Invoke(new Action(() =>
                {
                    formDataKey.dgv_Key.Rows.Clear();
                }));
                int index_row = 0;
                using (SQLiteConnection SqlConn = new SQLiteConnection(serverKey.connectionString))
                {
                    string selectKey = "SELECT * FROM " + formDataKey.selectVerKey + " WHERE ";
                    if (!string.IsNullOrEmpty(formDataKey.optionVerKey))
                        selectKey = selectKey + " Description=@Description AND ";
                    selectKey = selectKey + "MAKCount <> '-2' AND ErrorCode <> '" + ContantResource.KeyRetaiBlock + "' ORDER BY MAKCount DESC";
                    SQLiteCommand cmdKey = new SQLiteCommand(selectKey, SqlConn);
                    if (!string.IsNullOrEmpty(formDataKey.optionVerKey))
                        cmdKey.Parameters.AddWithValue("@Description", formDataKey.optionVerKey);
                    SqlConn.Open();
                    SQLiteDataReader readerKey = cmdKey.ExecuteReader();
                    while (readerKey.Read())
                    {
                        object[] row = new object[] { index_row + 1, readerKey.GetString(1), readerKey.GetString(2), readerKey.GetString(3), readerKey.GetString(4), readerKey.GetInt32(5), readerKey.GetString(6), readerKey.GetString(7), readerKey.GetString(8) };
                        formDataKey.dgv_Key.Invoke(new Action(() =>
                        {
                            formDataKey.dgv_Key.Rows.Add(row);
                        }));
                        index_row += 1;
                    }
                    SqlConn.Close();
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
        }

        public int totalKey(string query)
        {
            int countKey = 0;
            using (SQLiteConnection SqlConn = new SQLiteConnection(serverKey.connectionString))
            {
                SQLiteCommand cmd = new SQLiteCommand(query, SqlConn);
                SqlConn.Open();
                SQLiteDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                    countKey = reader.GetInt32(0);
                SqlConn.Close();
            }
            return countKey;
        }

        public string getInfoKey(string table, string key)
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
                    if (!string.IsNullOrEmpty(readerKey.GetValue(5).ToString()))
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

        public void DeleteAllKeyBlock(string table)
        {
            using (SQLiteConnection SqlConn = new SQLiteConnection(serverKey.connectionString))
            {
                string deleteKey = "DELETE FROM " + table + " WHERE MAKCount = '-2' OR ErrorCode = '" + ContantResource.KeyRetaiBlock + "'";
                SQLiteCommand cmd = new SQLiteCommand(deleteKey, SqlConn);
                SqlConn.Open();
                cmd.ExecuteNonQuery();
                SqlConn.Close();
            }
        }

        public List<string> loadDataSave(string table)
        {
            List<string> data = new List<string>();
            try
            {
                using (SQLiteConnection SqlConn = new SQLiteConnection(serverKey.connectionString))
                {
                    string query = "SELECT * FROM " + table + " ORDER BY Description ASC";
                    SqlConn.Open();
                    SQLiteCommand cmd = new SQLiteCommand(query, SqlConn);
                    SQLiteDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        int ActivCount = -1;
                        int.TryParse(reader["MAKCount"].ToString(), out ActivCount);
                        if (ActivCount > -1)
                        {
                            if (!string.IsNullOrEmpty(reader.GetString(1)))
                                data.Add("Key: " + reader.GetString(1));
                            if (!string.IsNullOrEmpty(reader.GetString(2)))
                                data.Add("Description: " + reader.GetString(2));
                            if (!string.IsNullOrEmpty(reader.GetString(3)))
                                data.Add("SubType: " + reader.GetString(3));
                            if (!string.IsNullOrEmpty(reader.GetString(4)))
                                data.Add("LicenseType: " + reader.GetString(4));
                            if (!string.IsNullOrEmpty(reader.GetValue(5).ToString()))
                                data.Add("MAKCount: " + reader.GetValue(5));
                            if (!string.IsNullOrEmpty(reader.GetString(6)))
                                data.Add("ErrorCode: " + reader.GetString(6));
                            if (!string.IsNullOrEmpty(reader.GetString(7)))
                                data.Add("Getweb: " + reader.GetString(7));
                            if (!string.IsNullOrEmpty(reader.GetString(8)))
                                data.Add("Note: " + reader.GetString(8));
                            data.Add("-------------------");
                        }
                    }
                    SqlConn.Close();
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                data.Add("");
            }
            return data;
        }

        public int readDBBK(string table, int count, string part)
        {
            string connectionStringDBBK1 = string.Format("Data Source = {0}; Version=3;", part);
            using (SQLiteConnection SqlConn = new SQLiteConnection(connectionStringDBBK1))
            {
                string query = "SELECT * FROM " + table + "";
                SqlConn.Open();
                SQLiteCommand sQLiteCommand = new SQLiteCommand(query, SqlConn);
                SQLiteDataReader dataReader = sQLiteCommand.ExecuteReader();
                while (dataReader.Read())
                {
                    int ActivCount = -1;
                    int.TryParse(dataReader["MAKCount"].ToString(), out ActivCount);
                    pid pid = new pid();
                    pid.Key = dataReader.GetString(1);
                    pid.Description = dataReader.GetString(2);
                    pid.SubType = dataReader.GetString(3);
                    pid.LicenseType = dataReader.GetString(4);
                    pid.MAKCount = ActivCount;
                    pid.ErrorCode = dataReader.GetString(6);
                    pid.KeyGetWeb = dataReader.GetString(7);
                    StringBuilder note = new StringBuilder(dataReader.GetString(8));
                    serverKey.CreateDataKey(true, pid, note.ToString());
                    count += 1;
                    formDataKey.tbx_KeySearch.Invoke(new Action(() =>
                    {
                        formDataKey.tbx_KeySearch.Text = "Restore " + count + " key";
                    }));
                }
                SqlConn.Close();
            }
            return count;
        }
    }
}
