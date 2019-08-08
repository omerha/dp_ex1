using FacebookWrapper.ObjectModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace C19_Ex01_Omer_204059331_Andrey_321082513.sln
{
    public class StoreToken
    {
        public string m_Token { get; set; }
        public void SaveLogin(string i_DataToSave, string i_DataFileName)
        {
            string tempPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
            tempPath = Path.Combine(tempPath, i_DataFileName + ".txt");
            m_Token = i_DataToSave;
            using (Stream stream = new FileStream(tempPath, FileMode.CreateNew))
            {
                XmlSerializer xmlSerializer = new XmlSerializer(this.GetType());
                xmlSerializer.Serialize(stream, this);
            }
        }
        public bool LoadLogin(string i_DataFileName)
        {
            bool loadRes = false;
            string tempPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
            tempPath = Path.Combine(tempPath, i_DataFileName + ".txt");
            StoreToken temp = null;
            if (Directory.Exists(Path.GetDirectoryName(tempPath)))
            {
                using (Stream stream = new FileStream(tempPath, FileMode.Open))
                {
                    XmlSerializer xmlSerializer = new XmlSerializer(this.GetType());
                    temp = xmlSerializer.Deserialize(stream) as StoreToken;
                    m_Token = temp.m_Token;
                    loadRes = true;
                }
            }
            return loadRes;
        }
    }
}
