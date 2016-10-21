using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Utils
{
    public class Serializer
    {
        public static object DeepCopy(object o)
        {
            using (MemoryStream m = new MemoryStream())
            {
                BinaryFormatter b = new BinaryFormatter();
                {
                    b.Serialize(m, o);
                    m.Position = 0;
                    return (object)b.Deserialize(m);
                }
            }
        }
        public static byte[] Serialize(object o)
        {
            using (MemoryStream m = new MemoryStream())
            {
                BinaryFormatter b = new BinaryFormatter();
                {
                    b.Serialize(m, o);
                    return m.ToArray();
                }
            }
        }
        public static object Deserialize(byte[] serializedObject)
        {
            using (MemoryStream ms = new MemoryStream(serializedObject))
            {
                BinaryFormatter b = new BinaryFormatter();
                ms.Position = 0;

                return b.Deserialize(ms);
            }
        }

    }
}
