using System.Xml.Serialization;
using System.IO;
using System;
using System.Text;

namespace Arc9.Unity.KioskToolkit.Service
{
    public class AppData
    {
        #region Public
        [XmlElement("Window")]
        public AppData_Window _Window = new AppData_Window();

        [XmlElement("Serial")]
        public AppData_Serial _Serial = new AppData_Serial();

        [XmlElement("Tcp")]
        public AppData_Tcp _Tcp = new AppData_Tcp();

        public void Save()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(AppData));

            string directoryName = System.IO.Path.GetDirectoryName(XmlFilePath);

            if(!Directory.Exists(directoryName))
            {
                Directory.CreateDirectory(directoryName);
            }

            using (TextWriter writer = new StreamWriter(XmlFilePath, false, Encoding.UTF8))
            {
                try
                {
                    serializer.Serialize(writer, this);
                }
                catch (Exception)
                {

                }
                finally
                {

                }
            }
        }
        #endregion

        #region Static
        static public AppData_Window Window => Instance._Window;
        static public AppData_Serial Serial => Instance._Serial;
        static public AppData_Tcp Tcp => Instance._Tcp;

        static private AppData _Instance;
        static private AppData Instance
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
                return Directory.GetCurrentDirectory() + "\\XORBIS\\" + "AppData.xml";
            }
        }

        static AppData CreateInstance()
        {
            AppData instance = new AppData();

            XmlSerializer serializerObj = new XmlSerializer(typeof(AppData));
            try
            {
                TextReader textgReader = new StreamReader(XmlFilePath);
                instance = (AppData)serializerObj.Deserialize(textgReader);
                textgReader.Close();
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("*** [AppData.CreateInstance] : " + e.ToString());

                instance.Save();
            }

            System.Diagnostics.Debug.WriteLine("*** [AppData.CreateInstance] : ");

            return instance;
        }

        static public void SaveInstance()
        {
            Instance.Save();
        }
        #endregion

        #region inner class

        public class AppData_Window
        {
            public bool TopMost { get; set; } = true;
            public bool HideCursor { get; set; } = true;
            public float Left { get; set; } = 0;
            public float Top { get; set; } = 0;
            public float Width { get; set; } = 1920;
            public float Height { get; set; } = 1080;
        }

        public class AppData_Serial
        {
            public string PortNumber { get; set; } = "COM1";
            public int Baudrate { get; set; } = 9600;
        }

        public class AppData_Tcp
        {
            public string IpAddress { get; set; } = "127.0.0.1";
            public int PortNumber { get; set; } = 9070;
        }
        #endregion
    }
}
