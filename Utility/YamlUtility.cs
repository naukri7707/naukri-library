using Naukri;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
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
            try
            {
                File.WriteAllText(filePath, yaml, new UTF8Encoding(true));
            }
            catch (UnityException e)
            {
                throw e;
            }
        }

        public static T LoadFromYamlFile<T>(string filePath)
        {
            try
            {
                var yaml = File.ReadAllText(filePath);
                return FromYaml<T>(yaml);
            }
            catch (UnityException e)
            {
                throw e;
            }
        }
    }
}