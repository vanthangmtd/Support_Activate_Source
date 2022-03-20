using System;
using System.Data.SQLite;
using System.IO;
using System.Windows.Forms;

namespace SupportActivate.ProcessSQL
{
    public class ServerSetting
    {
        private log4net.ILog logger = log4net.LogManager.GetLogger(typeof(ServerSetting));
        static string location = Application.StartupPath + @"\pkeyconfig";
        static string fileName = "setting.db";
        static string fullPath = Path.Combine(location, fileName);
        string connectionString = String.Format("Data Source = {0}; Version=3;", fullPath);

        public void CreateDataBase()
        {
            try
            {
                if (!File.Exists(fullPath))
                {
                    string createTableSETTING = "CREATE TABLE 'SETTING' ('ID' INTEGER PRIMARY KEY AUTOINCREMENT, 'KEYAPI' TEXT, 'PIDADV' TEXT, 'LOADKEY' TEXT, 'CLOSEAPP' TEXT);";
                    using (var db = new SQLiteConnection(connectionString))
                    {
                        var cmdCreateTableSETTING = new SQLiteCommand(createTableSETTING, db);
                        db.Open();
                        cmdCreateTableSETTING.ExecuteNonQuery();
                        db.Close();
                    }
                }
                CreateDataSetting(string.Empty, false, false, true);
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
        }

        private void CreateDataSetting(string KEYAPI, bool PIDADV, bool LOADKEY, bool CLOSEAPP)
        {
            try
            {
                if (File.Exists(fullPath) && CheckDuplicateDataSetting() <= 0)
                {
                    string insert = "INSERT INTO SETTING(KEYAPI, PIDADV, LOADKEY, CLOSEAPP) VALUES (@KEYAPI, @PIDADV, @LOADKEY, @CLOSEAPP)";
                    using (var db = new SQLiteConnection(connectionString))
                    {
                        var cmdInsert = new SQLiteCommand(insert, db);
                        var cmd = new SQLiteCommand(insert, db);
                        cmd.Parameters.AddWithValue("@KEYAPI", KEYAPI);
                        cmd.Parameters.AddWithValue("@PIDADV", Convert.ToString(PIDADV));
                        cmd.Parameters.AddWithValue("@LOADKEY", Convert.ToString(LOADKEY));
                        cmd.Parameters.AddWithValue("@CLOSEAPP", Convert.ToString(CLOSEAPP));
                        db.Open();
                        cmd.ExecuteNonQuery();
                        db.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
        }

        private int CheckDuplicateDataSetting()
        {
            int countDataSetting = 0;
            try
            {
                if (File.Exists(fullPath))
                {
                    string insert = "SELECT Count(ID) FROM SETTING";
                    using (var db = new SQLiteConnection(connectionString))
                    {
                        var cmdInsert = new SQLiteCommand(insert, db);
                        var cmd = new SQLiteCommand(insert, db);
                        db.Open();
                        SQLiteDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            if (!reader.IsDBNull(0))
                                countDataSetting = reader.GetInt32(0);
                        }
                        db.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
            return countDataSetting;
        }

        public void UpdateDataSetting(string KEYAPI, bool PIDADV, bool LOADKEY, bool CLOSEAPP)
        {
            try
            {
                string insert = "UPDATE SETTING SET KEYAPI=@KEYAPI, PIDADV=@PIDADV, LOADKEY=@LOADKEY, CLOSEAPP=@CLOSEAPP";
                using (var db = new SQLiteConnection(connectionString))
                {
                    var cmdInsert = new SQLiteCommand(insert, db);
                    var cmd = new SQLiteCommand(insert, db);
                    cmd.Parameters.AddWithValue("@KEYAPI", KEYAPI);
                    cmd.Parameters.AddWithValue("@PIDADV", PIDADV);
                    cmd.Parameters.AddWithValue("@LOADKEY", LOADKEY);
                    cmd.Parameters.AddWithValue("@CLOSEAPP", CLOSEAPP);
                    db.Open();
                    cmd.ExecuteNonQuery();
                    db.Close();
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
        }

        public string GetKEYAPI()
        {
            string keyApi = string.Empty;
            try
            {
                using (var conn = new SQLiteConnection(connectionString))
                {
                    string selectValue = "SELECT KEYAPI FROM SETTING";
                    SQLiteCommand cmd = new SQLiteCommand(selectValue, conn);
                    conn.Open();
                    SQLiteDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                        keyApi = reader.GetValue(0).ToString();
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
            return keyApi;
        }

        public bool GetStatusPIDADV()
        {
            bool statusPIDADV = false;
            try
            {
                using (var conn = new SQLiteConnection(connectionString))
                {
                    string selectValue = "SELECT PIDADV FROM SETTING";
                    SQLiteCommand cmd = new SQLiteCommand(selectValue, conn);
                    conn.Open();
                    SQLiteDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        var result = reader["PIDADV"].ToString();
                        statusPIDADV = bool.Parse(result);
                    }
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
            return statusPIDADV;
        }

        public bool GetStatusLOADKEY()
        {
            bool statusLOADKEY = false;
            try
            {
                using (var conn = new SQLiteConnection(connectionString))
                {
                    string selectValue = "SELECT LOADKEY FROM SETTING";
                    SQLiteCommand cmd = new SQLiteCommand(selectValue, conn);
                    conn.Open();
                    SQLiteDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        var result = reader["LOADKEY"].ToString();
                        statusLOADKEY = bool.Parse(result);
                    }
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
            return statusLOADKEY;
        }

        public bool GetStatusCLOSEAPP()
        {
            bool statusCLOSEAPP = false;
            try
            {
                using (var conn = new SQLiteConnection(connectionString))
                {
                    string selectValue = "SELECT CLOSEAPP FROM SETTING";
                    SQLiteCommand cmd = new SQLiteCommand(selectValue, conn);
                    conn.Open();
                    SQLiteDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        var result = reader["CLOSEAPP"].ToString();
                        statusCLOSEAPP = bool.Parse(result);
                    }
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
            return statusCLOSEAPP;
        }

        public void UpdateSetting(bool PIDADV, bool LOADKEY, bool CLOSEAPP)
        {
            try
            {
                string insert = "UPDATE SETTING SET PIDADV=@PIDADV, LOADKEY=@LOADKEY, CLOSEAPP=@CLOSEAPP";
                using (var db = new SQLiteConnection(connectionString))
                {
                    var cmdInsert = new SQLiteCommand(insert, db);
                    var cmd = new SQLiteCommand(insert, db);
                    cmd.Parameters.AddWithValue("@PIDADV", Convert.ToString(PIDADV));
                    cmd.Parameters.AddWithValue("@LOADKEY", Convert.ToString(LOADKEY));
                    cmd.Parameters.AddWithValue("@CLOSEAPP", Convert.ToString(CLOSEAPP));
                    db.Open();
                    cmd.ExecuteNonQuery();
                    db.Close();
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
        }

        public void UpdateKEYAPI(string KEYAPI)
        {
            try
            {
                using (var conn = new SQLiteConnection(connectionString))
                {
                    string insert = "UPDATE SETTING SET KEYAPI=@KEYAPI";
                    SQLiteCommand cmd = new SQLiteCommand(insert, conn);
                    cmd.Parameters.AddWithValue("@KEYAPI", KEYAPI);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
        }

    }
}
