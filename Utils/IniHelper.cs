// Decompiled with JetBrains decompiler
// Type: FireLord.Utils.IniHelper
// Assembly: FireLord, Version=1.1.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 51633F12-6A5F-46B9-B9AF-55B0B570B321
// Assembly location: C:\Users\andre\Documents\FireLord.dll

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
            this.FilePath = _filePath;
            this.Section = _section;
            this.Reload();
        }

        public void Reload()
        {
            this.List = new Dictionary<string, string>();
            foreach (string key in this._getKeyList())
            {
                if (this.List.ContainsKey(key))
                    this.List[key] = this.Get(key);
                else
                    this.List.Add(key, this.Get(key));
            }
        }

        public string[] GetKeyList() => this.List.Keys.ToArray<string>();

        private System.Collections.Generic.List<string> _getKeyList()
        {
            System.Collections.Generic.List<string> keyList = new System.Collections.Generic.List<string>();
            byte[] numArray = new byte[65536];
            uint privateProfileStringA = IniHelper.GetPrivateProfileStringA(this.Section, (string)null, (string)null, numArray, numArray.Length, this.FilePath);
            int index1 = 0;
            for (int index2 = 0; (long)index2 < (long)privateProfileStringA; ++index2)
            {
                if (numArray[index2] == (byte)0)
                {
                    keyList.Add(Encoding.Default.GetString(numArray, index1, index2 - index1));
                    index1 = index2 + 1;
                }
            }
            return keyList;
        }

        public string Get(string key, string defaultVal = "")
        {
            if (this.List.ContainsKey(key))
                return this.List[key];
            StringBuilder retval = new StringBuilder(1024);
            IniHelper.GetPrivateProfileString(this.Section, key, defaultVal, retval, 1024, this.FilePath);
            return retval.ToString();
        }

        public void Set(string key, string val)
        {
            this.List[key] = val;
            IniHelper.WritePrivateProfileString(this.Section, key, val, this.FilePath);
        }

        public void Del(string key)
        {
            this.List.Remove(key);
            IniHelper.WritePrivateProfileString(this.Section, key, (string)null, this.FilePath);
        }

        public int GetInt(string key, int defaultVal = 0)
        {
            string s = this.Get(key, defaultVal.ToString());
            int num = defaultVal;
            ref int local = ref num;
            return !int.TryParse(s, out local) ? defaultVal : num;
        }

        public float GetFloat(string key, float defaultVal = 0.0f)
        {
            string s = this.Get(key, defaultVal.ToString());
            float num = defaultVal;
            ref float local = ref num;
            return !float.TryParse(s, out local) ? defaultVal : num;
        }

        public bool GetBool(string key, bool defaultVal = false) => this.Get(key, defaultVal ? "1" : "0") == "1";

        public void SetInt(string key, int val) => this.Set(key, val.ToString());

        public void SetFloat(string key, float val) => this.Set(key, val.ToString());

        public void SetBool(string key, bool bo) => this.Set(key, bo ? "1" : "0");
    }
}
