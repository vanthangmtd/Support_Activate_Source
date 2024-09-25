using SupportActivate.Common;
using System;
using System.Data.SQLite;
using System.IO;
using System.Windows.Forms;

namespace SupportActivate.ProcessSQL
{
    public class ServerKey
    {
        private log4net.ILog logger = log4net.LogManager.GetLogger(typeof(ServerSetting));
        private static string location = Application.StartupPath + @"\pkeyconfig";
        private static string fileName = "data_new.db";
        private static string fullPath = Path.Combine(location, fileName);
        public string connectionString = String.Format("Data Source = {0}; Version=3;", fullPath);

        public const string KeyOffice = "KEYOFFICE";
        public const string KeyWindows = "KEYWINDOWS";
        public const string KeyServer = "KEYSERVER";
        public const string KeyOther = "KEYOTHER";

        public static string fullOldPath = Path.Combine(location, "data.db");
        private string connectionStringOld = String.Format("Data Source = {0}; Version=3;", fullOldPath);

        public void createDataBase()
        {
            try
            {
                if (!File.Exists(fullPath))
                {
                    string createTableCID = "CREATE TABLE 'CID' ('Id' INTEGER PRIMARY KEY AUTOINCREMENT, 'IID' TEXT, 'CID' TEXT);";
                    string createTableKeyOffice = @"CREATE TABLE 'KEYOFFICE' ('Id' INTEGER PRIMARY KEY AUTOINCREMENT, 'Key' TEXT, 'Description' TEXT, 'SubType' TEXT,
	                                        'LicenseType' TEXT, 'MAKCount' INTEGER, 'ErrorCode' TEXT, 'Getweb' TEXT, 'Note' TEXT);";
                    string createTableKeyWindows = @"CREATE TABLE 'KEYWINDOWS' ('Id' INTEGER PRIMARY KEY AUTOINCREMENT, 'Key' TEXT, 'Description' TEXT, 'SubType' TEXT,
	                                        'LicenseType' TEXT, 'MAKCount' INTEGER, 'ErrorCode' TEXT, 'Getweb' TEXT, 'Note' TEXT);";
                    string createTableKeyServer = @"CREATE TABLE 'KEYSERVER' ('Id' INTEGER PRIMARY KEY AUTOINCREMENT, 'Key' TEXT, 'Description' TEXT, 'SubType' TEXT,
	                                        'LicenseType' TEXT, 'MAKCount' INTEGER, 'ErrorCode' TEXT, 'Getweb' TEXT, 'Note' TEXT);";
                    string createTableKeyOther = @"CREATE TABLE 'KEYOTHER' ('Id' INTEGER PRIMARY KEY AUTOINCREMENT, 'Key' TEXT, 'Description' TEXT, 'SubType' TEXT,
	                                        'LicenseType' TEXT, 'MAKCount' INTEGER, 'ErrorCode' TEXT, 'Getweb' TEXT, 'Note' TEXT);";
                    using (SQLiteConnection SqlConn = new SQLiteConnection(connectionString))
                    {
                        SQLiteCommand cmdCreateTableCID = new SQLiteCommand(createTableCID, SqlConn);
                        SQLiteCommand cmdTableKeyOffice = new SQLiteCommand(createTableKeyOffice, SqlConn);
                        SQLiteCommand cmdTableKeyWindows = new SQLiteCommand(createTableKeyWindows, SqlConn);
                        SQLiteCommand cmdTableKeyServer = new SQLiteCommand(createTableKeyServer, SqlConn);
                        SQLiteCommand cmdTableKeyOther = new SQLiteCommand(createTableKeyOther, SqlConn);
                        SqlConn.Open();
                        cmdCreateTableCID.ExecuteNonQuery();
                        cmdTableKeyOffice.ExecuteNonQuery();
                        cmdTableKeyWindows.ExecuteNonQuery();
                        cmdTableKeyServer.ExecuteNonQuery();
                        cmdTableKeyOther.ExecuteNonQuery();
                        SqlConn.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
        }

        private pid getInfoKey(string table, string key)
        {
            pid pid = new pid();
            using (SQLiteConnection SqlConn = new SQLiteConnection(connectionString))
            {
                string query = "SELECT * FROM " + table + " WHERE Key = '" + key + "'";
                SQLiteCommand cmd = new SQLiteCommand(query, SqlConn);
                SqlConn.Open();
                SQLiteDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    pid.Key = reader["Key"].ToString();
                    pid.Description = reader["Description"].ToString();
                    pid.SubType = reader["SubType"].ToString();
                    pid.LicenseType = reader["LicenseType"].ToString();
                    var count = -1;
                    pid.MAKCount = int.TryParse(reader["MAKCount"].ToString(), out count) ? count : -1;
                    pid.ErrorCode = reader["ErrorCode"].ToString();
                    pid.KeyGetWeb = reader["Getweb"].ToString();
                }
                SqlConn.Close();
            }
            return pid;
        }

        public pid SearchKey(string key)
        {
            try
            {
                var selectKeyOffice = getInfoKey(KeyOffice, key);
                var selectKeyWindows = getInfoKey(KeyWindows, key);
                var selectKeyServer = getInfoKey(KeyServer, key);
                if (!string.IsNullOrEmpty(selectKeyOffice.Key))
                    return selectKeyOffice;
                else if (!string.IsNullOrEmpty(selectKeyWindows.Key))
                {
                    return selectKeyWindows;
                }
                else
                    return selectKeyServer;
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                return new pid();
            }
        }

        public void CreateDataKey(bool saveKey, pid pidkey, string Note)
        {
            try
            {
                if (saveKey)
                {
                    string Version;
                    var checkKeyOffice = pidkey.Description.IndexOf("Office");
                    var checkKeyWin = pidkey.Description.IndexOf("Win");
                    var checkKeyServer = pidkey.Description.IndexOf("Server");
                    if (checkKeyServer != -1)
                        Version = KeyServer;
                    else if (checkKeyOffice != -1)
                        Version = KeyOffice;
                    else if (checkKeyWin != -1)
                        Version = KeyWindows;
                    else
                        Version = KeyOther;

                    int idKey = CheckDuplicateDataKey(Version, pidkey.Key);
                    if (idKey == 0)
                    {
                        using (SQLiteConnection SqlConn = new SQLiteConnection(connectionString))
                        {
                            string insert = "INSERT INTO " + Version + @"(Key, Description, SubType, LicenseType, MAKCount, ErrorCode, Getweb, Note) 
                                        VALUES (@Key, @Description, @SubType, @LicenseType, @MAKCount, @ErrorCode, @Getweb, @Note)";
                            SQLiteCommand cmd = new SQLiteCommand(insert, SqlConn);
                            cmd.Parameters.AddWithValue("@Key", pidkey.Key);
                            cmd.Parameters.AddWithValue("@Description", pidkey.Description);
                            cmd.Parameters.AddWithValue("@SubType", pidkey.SubType);
                            cmd.Parameters.AddWithValue("@LicenseType", pidkey.LicenseType);
                            cmd.Parameters.AddWithValue("@MAKCount", pidkey.MAKCount);
                            cmd.Parameters.AddWithValue("@ErrorCode", string.IsNullOrEmpty(pidkey.ErrorCode) ? string.Empty : pidkey.ErrorCode);
                            cmd.Parameters.AddWithValue("@Getweb", pidkey.KeyGetWeb);
                            cmd.Parameters.AddWithValue("@Note", Note);
                            SqlConn.Open();
                            cmd.ExecuteNonQuery();
                            SqlConn.Close();
                        }
                        if (Version == KeyOther)
                            DeleteKeyNotDefined(pidkey.Key);
                    }
                    else
                    {
                        UpdateDataKey(idKey, Version, pidkey, Note);
                        if (Version == KeyOther)
                            DeleteKeyNotDefined(pidkey.Key);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
        }

        private int CheckDuplicateDataKey(string Version, string Key)
        {
            int idKey = 0;
            using (SQLiteConnection SqlConn = new SQLiteConnection(connectionString))
            {
                string selectValue = "SELECT Id FROM " + Version + " WHERE Key=@Key";
                SQLiteCommand cmd = new SQLiteCommand(selectValue, SqlConn);
                cmd.Parameters.AddWithValue("@Key", Key);
                SqlConn.Open();
                SQLiteDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                    idKey = reader.GetInt32(0);
                SqlConn.Close();
            }
            return idKey;
        }

        public void UpdateDataKey(int Id, string Version, pid pidkey, string Note)
        {
            using (SQLiteConnection SqlConn = new SQLiteConnection(connectionString))
            {
                string update = "UPDATE " + Version + " SET ";
                if (!string.IsNullOrEmpty(pidkey.Description))
                    update = update + "Description='" + pidkey.Description + "'";
                if (!string.IsNullOrEmpty(pidkey.SubType))
                    update = update + ", SubType='" + pidkey.SubType + "'";
                if (!string.IsNullOrEmpty(pidkey.LicenseType))
                    update = update + ", LicenseType='" + pidkey.LicenseType + "'";
                update = update + ", MAKCount='" + pidkey.MAKCount + "'";
                if (!string.IsNullOrEmpty(pidkey.ErrorCode))
                    update = update + ", ErrorCode='" + pidkey.ErrorCode + "'";
                if (!string.IsNullOrEmpty(pidkey.KeyGetWeb))
                    update = update + ", Getweb='" + pidkey.KeyGetWeb + "'";
                if (!string.IsNullOrEmpty(Note))
                    update = update + ", Note='" + Note + "'";
                update = update + " WHERE Id='" + Id + "'";
                SQLiteCommand cmd = new SQLiteCommand(update, SqlConn);
                SqlConn.Open();
                cmd.ExecuteNonQuery();
                SqlConn.Close();
            }
        }

        private void DeleteKeyNotDefined(string key)
        {
            using (SQLiteConnection SqlConn = new SQLiteConnection(connectionString))
            {
                string deleteKey = "DELETE FROM KEYOTHER WHERE Key=@Key";
                SQLiteCommand cmd = new SQLiteCommand(deleteKey, SqlConn);
                cmd.Parameters.AddWithValue("@Key", key);
                SqlConn.Open();
                cmd.ExecuteNonQuery();
                SqlConn.Close();
            }
        }

        private void BackupAndRestoreDBOld(string table)
        {
            using (SQLiteConnection SqlConn = new SQLiteConnection(connectionStringOld))
            {
                string sql = "SELECT * FROM " + table + " WHERE MAKCount <> '" + ContantResource.KeyBlock + "' AND ErrorCode <> '" + ContantResource.KeyRetaiBlock + "'";
                SQLiteCommand cmd = new SQLiteCommand(sql, SqlConn);
                SqlConn.Open();
                SQLiteDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    int ActivCount = -1;
                    int.TryParse(reader["MAKCount"].ToString(), out ActivCount);

                    pid pid = new pid();
                    pid.Key = reader.GetString(1);
                    pid.Description = reader.GetString(2);
                    pid.SubType = reader.GetString(3);
                    pid.LicenseType = reader.GetString(4);
                    pid.MAKCount = ActivCount;
                    pid.ErrorCode = reader.GetString(6);
                    pid.KeyGetWeb = reader.GetString(7);
                    CreateDataKey(true, pid, reader.GetString(8));
                }
                SqlConn.Close();
            }
        }

        public void BackupAndRestoreDBOld()
        {
            if (File.Exists(fullOldPath))
            {
                BackupAndRestoreDBOld(KeyWindows);
                BackupAndRestoreDBOld(KeyOffice);
                BackupAndRestoreDBOld(KeyServer);
                BackupAndRestoreDBOld(KeyOther);
                GC.Collect();
                GC.WaitForPendingFinalizers();
                File.Delete(fullOldPath);
            }
        }
    }
}
