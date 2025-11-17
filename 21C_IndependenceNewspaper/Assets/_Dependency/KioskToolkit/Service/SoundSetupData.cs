using System.Xml.Serialization;
using System.IO;
using System;
using System.Text;
using System.Collections.Generic;
using System.Xml;

namespace Arc9.Unity.KioskToolkit.Service
{
    public class SoundSetupData
    {
        private Dictionary<string, string> _Dic = new Dictionary<string, string>();

        static private SoundSetupData _Instance;
        static private SoundSetupData Instance
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
                return Directory.GetCurrentDirectory() + "\\ARC9\\" + "SoundSetupData.xml";
            }
        }

        static SoundSetupData CreateInstance()
        {
            SoundSetupData instance = new SoundSetupData();

            try
            {
                XmlDocument doc = new XmlDocument();

                doc.Load(XmlFilePath);

                XmlNodeList nlSoundSetupData = doc.GetElementsByTagName("SoundSetupData");

                if (nlSoundSetupData != null && nlSoundSetupData.Count > 0)
                {

                    foreach (XmlNode node in nlSoundSetupData)
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
                System.Diagnostics.Debug.WriteLine("*** [SoundSetupData.CreateInstance] : " + e.ToString());

                instance.Save();
            }

            System.Diagnostics.Debug.WriteLine("*** [SoundSetupData.CreateInstance] : ");

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

            XmlNode rootNode = doc.AppendChild(doc.CreateElement("SoundSetupData"));

            if (_Dic.Count == 0)
            {
                XmlElement dummy = doc.CreateElement("Key");
                dummy.InnerText = "Value";
                rootNode.AppendChild(dummy);
            }

            foreach (var item in _Dic)
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

                if (float.TryParse(tmp, out float value))
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

                if (tmp.Trim().ToLower().Equals("true") || tmp.Trim().ToLower().Equals("yes"))
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

