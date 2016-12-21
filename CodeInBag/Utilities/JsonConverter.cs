using System.IO;
using System.Text;

namespace CodeInBag.Utilities
{
    public class JsonConverter
    {
        /// <summary>
        /// Default encoding: UTF8
        /// </summary>
        public static Encoding Encoding = Encoding.UTF8;

        /// <summary>
        /// Deserialize object from file
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static T DeserializeFromFile<T>(string filePath)
        {
            T obj = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(File.ReadAllText(filePath, Encoding), new Newtonsoft.Json.JsonSerializerSettings
            {
                TypeNameHandling = Newtonsoft.Json.TypeNameHandling.Auto,
                NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore,
            });

            return obj;
        }

        /// <summary>
        /// Serialize object to file
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="filePath"></param>
        public static void SerializeToFile(object obj, string filePath)
        {
            var serializer = new Newtonsoft.Json.JsonSerializer();
            serializer.Converters.Add(new Newtonsoft.Json.Converters.JavaScriptDateTimeConverter());
            serializer.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
            serializer.TypeNameHandling = Newtonsoft.Json.TypeNameHandling.Auto;

            using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                using (StreamWriter sw = new StreamWriter(fs, Encoding))
                {
                    using (Newtonsoft.Json.JsonWriter writer = new Newtonsoft.Json.JsonTextWriter(sw))
                    {
                        serializer.Serialize(writer, obj);
                    }
                }
            }
        }
    }
}