using System.IO;
using System.Text;
using System.Xml.Serialization;
using UnityEngine;

namespace FRPG.SMSaveManager {
    
    public class SMSaveManager {
        public const string Version = "1.0";

        public FileInfo FileInfo { get; set; }
        public string Password { get; set; }
        public bool Exists {

            get {
#if UNITY_WEBPLAYER || UNITY_WEBGL
                return PlayerPrefs.HasKey(FileInfo.FullName);
#else
                return FileInfo.Exists;
#endif
            }
        }
        
        SMSerializableDictionary<string, string> library;
        XmlSerializer fileSerializer = new XmlSerializer(typeof(SMSerializableDictionary<string, string>));

        public SMSaveManager(string fileName, string password = null) : this(MakeDefaultFileInfo(fileName), password) {
        }
        public SMSaveManager(FileInfo fileInfo, string password = null) {
            FileInfo = fileInfo;
            Password = password;
        }
        
        public void Delete() {
#if UNITY_WEBPLAYER || UNITY_WEBGL
            PlayerPrefs.DeleteKey(FileInfo.FullName);
#else
            FileInfo.Delete();
#endif
        }
        
        public void Open() {
            if (Exists) {
                string toRead = null;
#if UNITY_WEBPLAYER || UNITY_WEBGL
                toRead = PlayerPrefs.GetString(FileInfo.FullName);
#else
                using (StreamReader fileReader = new StreamReader(FileInfo.FullName, Encoding.Unicode))
                    toRead = fileReader.ReadToEnd();
#endif
                if (!string.IsNullOrEmpty(Password))
                    toRead = SMCrypt.DecryptString(toRead, Password);

                using (StringReader sr = new StringReader(toRead))
                    library = fileSerializer.Deserialize(sr) as SMSerializableDictionary<string, string>;
            }
            else {
                library = new SMSerializableDictionary<string, string>();
                Save();
            }
        }

        public void Save() {
            using (StringWriter textWriter = new StringWriter()) {
                fileSerializer.Serialize(textWriter, library);

                string toWrite = textWriter.ToString();
                if (!string.IsNullOrEmpty(Password))
                    toWrite = SMCrypt.EncryptString(toWrite, Password);

#if UNITY_WEBPLAYER || UNITY_WEBGL
                PlayerPrefs.SetString(FileInfo.FullName, toWrite);
#else
                Directory.CreateDirectory(FileInfo.DirectoryName);
                using (StreamWriter sw = new StreamWriter(FileInfo.FullName, false, Encoding.Unicode))
                    sw.Write(toWrite);
#endif
            }
        }
        
        public bool HasKey(string key) {
            return library.ContainsKey(key);
        }
        
        public void SetValue<T>(string key, T value, string password = null) {
            using (StringWriter textWriter = new StringWriter()) {
                XmlSerializer valueSerializer = new XmlSerializer(typeof(T));
                valueSerializer.Serialize(textWriter, value);
                string toWrite = textWriter.ToString();

                if (!string.IsNullOrEmpty(password))
                    toWrite = SMCrypt.EncryptString(toWrite, password);
                
                library[key] = toWrite;
            }
        }
        
        public T GetValue<T>(string key, T defaultValue = default(T), string password = null) {
            if (HasKey(key)) {
                string toRead = library[key];

                if (!string.IsNullOrEmpty(password))
                    toRead = SMCrypt.DecryptString(toRead, password);

                using (StringReader sr = new StringReader(toRead)) {
                    XmlSerializer valueSerializer = new XmlSerializer(typeof(T));
                    return (T)valueSerializer.Deserialize(sr);
                }
            }
            else
                return defaultValue;
        }

        static FileInfo MakeDefaultFileInfo(string name) {
            return new FileInfo(Application.persistentDataPath + Path.DirectorySeparatorChar + name);
        }
    }
}
