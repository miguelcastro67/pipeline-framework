using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using Pipeline.Configuration;

namespace Pipeline
{
    public class ObjectBuilder
    {
        public static T NewUp<T>() where T : class, new()
        {
            T obj = new T();

            return obj;
        }

        public static T LoadUp<T,U>(U id) where T : class, new()
        {
            T obj = new T();

            return obj;
        }

        public static void SaveToFile<T>(T obj, string filePath)
        {
            
        }

        public static T LoadFromFile<T>(string filePath) where T : class, new()
        {
            T obj = new T();

            return obj;
        }
    }
}
