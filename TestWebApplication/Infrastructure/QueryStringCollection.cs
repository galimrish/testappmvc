using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TestWebApplication.WebUI.Models;

namespace TestWebApplication.WebUI.Infrastructure
{
    public class QueryStringCollection
    {
        private List<KeyValuesPair> _qsCol;
        public List<string> Keys { get; private set; }        

        public QueryStringCollection()
        {
            _qsCol = new List<KeyValuesPair>();
            Keys = new List<string>();
        }

        public void GetDataFromString(string queryString)
        {
            string[] values = queryString.Split(new char[] { '&' });
            foreach (string pair in values)
            {
                int eqPos = pair.IndexOf('=');
                string key = pair.Substring(0, eqPos);
                string value = eqPos == pair.Length ? "" : pair.Substring(eqPos + 1);
                this.Add(key, value);
            }
        }

        public void Add(KeyValuesPair item)
        {
            if (this.Contains(item))
            {
                this.Set(item);
                return;
            }
            _qsCol.Add(item);
            Keys.Add(item.Key);
        }

        public void Add(string key, string value)
        {
            int keyIndex = this.IndexOf(key);
            if (keyIndex > -1)
            {
                _qsCol[keyIndex].Value = value;
                return;
            }
            _qsCol.Add(new KeyValuesPair(key, value));
            Keys.Add(key);
        }

        public void Clear()
        {
            Keys.Clear();
            _qsCol.Clear();
        }

        public bool Contains(KeyValuesPair item)
        {
            return _qsCol.Contains(item);
        }

        public bool ContainsKey(string key)
        {
            foreach (string k in Keys)
            {
                if (k == key)
                    return true;
            }
            return false;
        }

        public int Count
        {
            get { return Keys.Count; }
        }

        public string this[string key]
        {
            get
            {
                foreach (KeyValuesPair item in _qsCol)
                {
                    if (item.Key == key)
                        return item.Value;
                }
                return null;
            }
        }

        public KeyValuesPair Get(int index)
        {
            return _qsCol[index];
        }

        public void Set(KeyValuesPair pair)
        {
            foreach (KeyValuesPair item in _qsCol)
            {
                if (item.Key == pair.Key)
                {
                    item.Value = pair.Value;
                }
            }
        }

        public int IndexOf(KeyValuesPair pair)
        {
            int i = 0;
            foreach (KeyValuesPair item in _qsCol)
            {
                if (item.Key == pair.Key)
                    return i;
                i++;
            }
            return -1;
        }

        public int IndexOf(string key)
        {
            int i = 0;
            while (i < this.Count)
            {
                if (Keys[i] == key)
                    return i;
                i++;
            }
            return -1;
        }

        public decimal ExtractDec(string key)
        {
            decimal result = 0;
            string value = this[key];
            value = value.Replace("%2c", ",");
            if (SharedLogic.IsDecimal(value))
                result = decimal.Parse(value);
            
            return result;
        }

        public override string ToString()
        {
            if (_qsCol == null)
                return null;
            string result = string.Empty;
            foreach(KeyValuesPair item in _qsCol)
            {
                result += string.Format("{0}={1}", item.Key, item.Value);
                result += '&';
            }
            result = result.Substring(0, result.Length - 1);
            return result;
        }
    }
    
    public class KeyValuesPair
    {
        public string Key { get; set; }
        public string Value { get; set; }
        public KeyValuesPair(string key, string value)
        {
            Key = key;
            Value = value;
        }
    }
}