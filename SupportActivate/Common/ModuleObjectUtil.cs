using System.Collections.Generic;

namespace SupportActivate.Common
{
    public class pid
    {
        public string Key { get; set; }
        public string Description { get; set; }
        public string SubType { get; set; }
        public string LicenseType { get; set; }
        public string ErrorCode { get; set; }
        public string MAKCount { get; set; }
        public string KeyGetWeb { get; set; }
    }

    public class inforPidkey
    {
        public string epid { get; set; }// Extendend PID
        public string aid { get; set; }// Activation ID
        public string edi { get; set; }// Edition
        public string st { get; set; }// SubType
        public string lit { get; set; }// License Type
        public string ds { get; set; }// Description
        public string count { get; set; }// count
        public bool validKey { get; set; }
    }

    public class PidKeySoft
    {
        public string Key { get; set; }
        public string Description { get; set; }
        public string SubType { get; set; }
        public string LicenseType { get; set; }
        public string ErrorCode { get; set; }
        public int MAKCount { get; set; }
        public string KeyGetWeb { get; set; }
    }

    public class data
    {
        public List<Pidkey_Up_syn_pidkey> list_pidkey = new List<Pidkey_Up_syn_pidkey>();
        public string token { get; set; }
    }

    public class Pidkey_Up_syn_pidkey
    {
        public int Id { get; set; }
        public string Key { get; set; }
        public string Description { get; set; }
        public string SubType { get; set; }
        public string LicenseType { get; set; }
        public string MAKCount { get; set; }
        public string Errorcode { get; set; }
    }

    public class dataKey
    {
        public int id { get; set; }
        public string key { get; set; }
    }
}
