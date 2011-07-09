using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;

namespace DebugCommander
{
    [Serializable]
    public class DebuggerCollection : Collection<Debugger>
    {
        public static DebuggerCollection Load(string fileName)
        {
            fileName = Path.Combine(Environment.GetFolderPath(
                                    Environment.SpecialFolder.ApplicationData),
                                    Path.Combine("DebugCommander", fileName));

            TextReader reader = new StreamReader(fileName);
            XmlSerializer serializer = new XmlSerializer(typeof(DebuggerCollection));
            DebuggerCollection debuggers = (DebuggerCollection)serializer.Deserialize(reader);
            return debuggers;
        }

        public void Save(string fileName)
        {
            string dir = Path.Combine(Environment.GetFolderPath(
                                    Environment.SpecialFolder.ApplicationData),
                                    "DebugCommander");
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            fileName = Path.Combine(dir, fileName);

            TextWriter writer = new StreamWriter(fileName);
            XmlSerializer serializer = new XmlSerializer(typeof(DebuggerCollection));
            serializer.Serialize(writer, this);
        }
    }
}
