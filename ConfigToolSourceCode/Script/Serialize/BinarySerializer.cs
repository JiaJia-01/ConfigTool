using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace ConfigTool
{
    class BinarySerializer
    {
        public static byte[] Serialize(object obj)
        {
            MemoryStream ms = new MemoryStream();
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(ms, obj);
            return ms.GetBuffer();
        }
    }
}
