using System.Xml.Serialization;
using System.IO;
using System;
using System.Text;
using System.Collections.Generic;
using System.Xml;

namespace Arc9.Unity.KioskToolkit.Service
{
    public class ContentData
    {
        private Dictionary<string, string> _Dic = new Dictionary<string, string>();

        static private ContentData _Instance;
        static private ContentData Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = CreateInstance();
                }
                return _Instance;
            }
        }
        static string XmlFilePath
        {
            get
            {
                return Directory.GetCurrentDirectory() + "\\XORBIS\\" + "ContentData.xml";
            }
        }

        static ContentData CreateInstance()
        {
            ContentData instance = new ContentData();

            try
            {
                XmlDocument doc = new XmlDocument();

                doc.Load(XmlFilePath);

                XmlNodeList nlContentData = doc.GetElementsByTagName("ContentData");

                if (nlContentData != null && nlContentData.Count > 0)
                {

                    foreach (XmlNode node in nlContentData)
                    {
                        XmlNode el = node.FirstChild;
                        while (el != null)
                        {
                            Console.WriteLine(el.Name);
                            Console.WriteLine(el.InnerText);

                            instance._Dic.Add(el.Name, el.InnerText);

                            el = el.NextSibling;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("*** [ContentData.CreateInstance] : " + e.ToString());

                instance.Save();
            }

            System.Diagnostics.Debug.WriteLine("*** [ContentData.CreateInstance] : ");

            return instance;
        }

        public void Save()
        {
            string directoryName = System.IO.Path.GetDirectoryName(XmlFilePath);

            if (!Directory.Exists(directoryName))
            {
                Directory.CreateDirectory(directoryName);
            }

            XmlDocument doc = new XmlDocument();

            XmlNode rootNode = doc.AppendChild(doc.CreateElement("ContentData"));

            if (_Dic.Count == 0)
            {
                XmlElement dummy = doc.CreateElement("Key");
                dummy.InnerText = "Value";
                rootNode.AppendChild(dummy);
            }

            foreach(var item in _Dic)
            {
                XmlElement el = doc.CreateElement(item.Key);
                el.InnerText = item.Value;
                rootNode.AppendChild(el);
            }

            doc.Save(XmlFilePath);
        }

        static public string GetValue(string key, string defaultValue)
        {
            if (Instance._Dic.ContainsKey(key))
            {
                return Instance._Dic[key];
            }

            Instance._Dic.Add(key, defaultValue);

            Instance.Save();

            return defaultValue;
        }

        static public float GetValue(string key, float defaultValue)
        {
            if (Instance._Dic.ContainsKey(key))
            {
                string tmp = Instance._Dic[key];

                if(float.TryParse(tmp, out float value))
                {
                    return value;
                }

                return defaultValue;
            }

            Instance._Dic.Add(key, defaultValue.ToString());

            Instance.Save();

            return defaultValue;
        }

        static public int GetValue(string key, int defaultValue)
        {
            if (Instance._Dic.ContainsKey(key))
            {
                string tmp = Instance._Dic[key];

                if (int.TryParse(tmp, out int value))
                {
                    return value;
                }

                return defaultValue;
            }

            Instance._Dic.Add(key, defaultValue.ToString());

            Instance.Save();

            return defaultValue;
        }

        static public bool GetValue(string key, bool defaultValue)
        {
            if (Instance._Dic.ContainsKey(key))
            {
                string tmp = Instance._Dic[key];

                if(tmp.Trim().ToLower().Equals("true") || tmp.Trim().ToLower().Equals("yes"))
                {
                    return true;
                }

                return false;
            }
            Instance._Dic.Add(key, defaultValue.ToString());

            Instance.Save();

            return defaultValue;
        }
    }
}
