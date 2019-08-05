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
        public void SaveLogin(string i_DataToSave, string i_DataFileName)
        {
            string tempPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
            tempPath = Path.Combine(tempPath, i_DataFileName + ".txt");

            using (Stream stream = new FileStream(tempPath, FileMode.CreateNew))
            {
                XmlSerializer xmlSerializer = new XmlSerializer(this.GetType());
                xmlSerializer.Serialize(stream, this);
            }
        }
        public User LoadLogin(string i_DataFileName)
        {
            string tempPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
            tempPath = Path.Combine(tempPath, i_DataFileName + ".txt");
            User res = null;
            if (!Directory.Exists(Path.GetDirectoryName(tempPath)))
            {
                using (Stream stream = new FileStream(tempPath, FileMode.Open))
                {
                    XmlSerializer xmlSerializer = new XmlSerializer(this.GetType());
                    res = xmlSerializer.Deserialize(stream) as User;
                }
            }
            return res;

        }
    }
}
