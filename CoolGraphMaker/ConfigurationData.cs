using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;


namespace CoolGraphMaker
{
    class ConfigurationData
    {
        public Data data;
        public void ReadConfiguration()
        {
            // Reading config xml file
            // This should be customized as your environment
            System.IO.FileStream fs = new System.IO.FileStream(@".\GraphDataSetting.xml", System.IO.FileMode.Open);

            // Serialize from xml stream
            System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(Data));
            data = (Data)serializer.Deserialize(fs);
        }

        public void WriteConfiguration()
        {
            // not implemented yet.
        }

        public void TestFunction()
        {
            // Just test function.
            // No implementation here.
        }
    }
}
