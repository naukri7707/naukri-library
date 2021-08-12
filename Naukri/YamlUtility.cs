using System;
using System.IO;
using System.Text;
using YamlDotNet.Serialization;

namespace Naukri
{
    public static class YamlUtility
    {
        public static string ToYaml<T>(T obj)
        {
            var serializer = new SerializerBuilder().Build();
            return serializer.Serialize(obj);
        }

        public static T FromYaml<T>(string yaml)
        {
            return (T)FromYaml(yaml, typeof(T));
        }

        public static object FromYaml(string yaml, Type type)
        {
            var deserializer = new DeserializerBuilder().Build();
            return deserializer.Deserialize(yaml, type);
        }

        public static void SaveToYamlFile<T>(string filePath, T obj)
        {
            var yaml = ToYaml(obj);
            File.WriteAllText(filePath, yaml, new UTF8Encoding(true));
        }

        public static T LoadFromYamlFile<T>(string filePath)
        {
            var yaml = File.ReadAllText(filePath);
            return FromYaml<T>(yaml);
        }
    }
}