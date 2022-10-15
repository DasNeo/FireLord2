using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace FireLord.Utils
{
    public class IniHelper
    {
        private string FilePath = "";
        private string Section = "";
        private Dictionary<string, string> List = new Dictionary<string, string>();

        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(
          string section,
          string key,
          string val,
          string filepath);

        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(
          string section,
          string key,
          string def,
          StringBuilder retval,
          int size,
          string filePath);

        [DllImport("kernel32", EntryPoint = "GetPrivateProfileString")]
        private static extern uint GetPrivateProfileStringA(
          string section,
          string key,
          string def,
          byte[] retVal,
          int size,
          string filePath);

        public IniHelper(string _filePath = "config.ini", string _section = "default")
        {
            FilePath = _filePath;
            Section = _section;
            Reload();
        }

        public void Reload()
        {
            List = new Dictionary<string, string>();
            foreach (string key in _getKeyList())
            {
                if (List.ContainsKey(key))
                    List[key] = Get(key);
                else
                    List.Add(key, Get(key));
            }
        }

        public string[] GetKeyList() => List.Keys.ToArray();

        private List<string> _getKeyList()
        {
            List<string> keyList = new List<string>();
            byte[] numArray = new byte[65536];
            uint privateProfileStringA = GetPrivateProfileStringA(Section, null, null, numArray, numArray.Length, FilePath);
            int index1 = 0;
            for (int index2 = 0; index2 < privateProfileStringA; ++index2)
            {
                if (numArray[index2] == 0)
                {
                    keyList.Add(Encoding.Default.GetString(numArray, index1, index2 - index1));
                    index1 = index2 + 1;
                }
            }
            return keyList;
        }

        public string Get(string key, string defaultVal = "")
        {
            if (List.ContainsKey(key))
                return List[key];
            StringBuilder retval = new StringBuilder(1024);
            GetPrivateProfileString(Section, key, defaultVal, retval, 1024, FilePath);
            return retval.ToString();
        }

        public void Set(string key, string val)
        {
            List[key] = val;
            WritePrivateProfileString(Section, key, val, FilePath);
        }

        public void Del(string key)
        {
            List.Remove(key);
            WritePrivateProfileString(Section, key, null, FilePath);
        }

        public int GetInt(string key, int defaultVal = 0)
        {
            string s = Get(key, defaultVal.ToString());
            int num = defaultVal;
            ref int local = ref num;
            return !int.TryParse(s, out local) ? defaultVal : num;
        }

        public float GetFloat(string key, float defaultVal = 0.0f)
        {
            string s = Get(key, defaultVal.ToString());
            float num = defaultVal;
            ref float local = ref num;
            return !float.TryParse(s, out local) ? defaultVal : num;
        }

        public bool GetBool(string key, bool defaultVal = false) => Get(key, defaultVal ? "1" : "0") == "1";

        public void SetInt(string key, int val) => Set(key, val.ToString());

        public void SetFloat(string key, float val) => Set(key, val.ToString());

        public void SetBool(string key, bool bo) => Set(key, bo ? "1" : "0");
    }
}
