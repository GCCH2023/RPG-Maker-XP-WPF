using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XP.Utility
{
    class XML<T>
    {
        /// <summary>
        /// 将对象写入到XML文件中
        /// </summary>
        /// <param name="obj">要写入的对象</param>
        /// <param name="path">xml文件路径</param>
        public static void Write(T obj, string path)
        {
            System.Xml.Serialization.XmlSerializer writer =
                new System.Xml.Serialization.XmlSerializer(typeof(T));

            var directory = Path.GetDirectoryName(path);
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            using (StreamWriter file = new StreamWriter(path))
            {
                writer.Serialize(file, obj);
            }
        }
        /// <summary>
        /// 从XML文件中 读取对象
        /// </summary>
        /// <param name="path">XML文件路径</param>
        /// <returns></returns>
        public static T Read(string path)
        {
            System.Xml.Serialization.XmlSerializer reader =
                  new System.Xml.Serialization.XmlSerializer(typeof(T));

            if (!System.IO.File.Exists(path))
                return default(T);

            System.IO.StreamReader file = new System.IO.StreamReader(path);
            var obj = (T)reader.Deserialize(file);
            file.Close();
            return obj;
        }
    }
}
