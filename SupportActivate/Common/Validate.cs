using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace SupportActivate.Common
{
    public class Validate
    {
        public bool ValidateKey(string key)
        {
            var regexKey = new Regex("^[A-Z0-9]{5}-[A-Z0-9]{5}-[A-Z0-9]{5}-[A-Z0-9]{5}-[A-Z0-9]{5}$");
            if(regexKey.IsMatch(key))
                return true;
            else
                return false;
        }

        public bool ValidateCID(string cid)
        {
            var regexCID = new Regex("^[0-9]{48}$");
            if (regexCID.IsMatch(cid.Replace("-", "")))
                return true;
            else
                return false;
        }

        public List<string> locData(List<string> list)
        {
            List<string> arrTemp = new List<string>();
            foreach (var value in list)
            {
                var str = NhanDangKey(value);
                foreach (string data in str)
                {
                    if (arrTemp.Contains(data) == false)
                        arrTemp.Add(data);
                }
            }
            list = arrTemp;
            return list;
        }

        public List<string> NhanDangKey(string value)
        {
            var regex = new Regex("[A-Za-z0-9]{5}-[A-Za-z0-9]{5}-[A-Za-z0-9]{5}-[A-Za-z0-9]{5}-[A-Za-z0-9]{5}");
            var regexMBAM = new Regex("[A-Za-z0-9]{5}:[A-Za-z0-9]{4}-[A-Za-z0-9]{4}-[A-Za-z0-9]{4}-[A-Za-z0-9]{4}");
            List<string> list = new List<string>();
            foreach (var match in regex.Matches(value))
            {
                if (ValidateKey(match.ToString()) == true)
                    list.Add(match.ToString());
            }
            foreach (var match in regexMBAM.Matches(value))
            {
                if (ValidateKeyMBAM(match.ToString()) == true)
                    list.Add(match.ToString());
            }
            return list;
        }

        public bool ValidateKeyMBAM(string Key)
        {
            bool KeyValid;
            string strKeyPattern = "^[A-Z0-9]{5}:[A-Z0-9]{4}-[A-Z0-9]{4}-[A-Z0-9]{4}-[A-Z0-9]{4}$";
            Regex reKey = new Regex(strKeyPattern);
            if (reKey.IsMatch(Key) == true)
                KeyValid = true;
            else
                KeyValid = false;
            return KeyValid;
        }


    }
}
