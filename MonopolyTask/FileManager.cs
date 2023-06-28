using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MonopolyTask
{
    public static class FileManager
    {
        public static void AppendToFile<T>(string path, T data)
        {
            if (File.Exists(path))
            {
                File.WriteAllText(path, JsonConvert.SerializeObject(data));
            }
            else
            {
                throw new FileNotFoundException();
            }
        }

        public static T ReadFromFile<T>(string path)
        {
            if (File.Exists(path))
            {
                var data = JsonConvert.DeserializeObject<T>(File.ReadAllText(path, Encoding.UTF8));
                return data;
            }
            else
            {
                throw new FileNotFoundException();
            }
        }

        public static void ClearFile(string path)
        {
            if(File.Exists(path)) 
            {
                File.WriteAllText(path, "");
            }
            else
            {
                File.Create(path);
            }
        }

        public static void SaveToFile<T>(string path, T data)
        {
            ClearFile(path);
            AppendToFile(path, data);
        }
    }
}
