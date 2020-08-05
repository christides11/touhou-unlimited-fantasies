using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace TUF
{
    public static class SaveLoadService
    {

        public static void Save(string fileName, string jsonObject)
        {
            try
            {
                using (StreamWriter streamWriter = File.CreateText(Path.Combine(Application.persistentDataPath, fileName)))
                {
                    streamWriter.Write(jsonObject);
                }
            }
            catch (Exception e)
            {
                Debug.Log($"Exception thrown while saving {fileName}. {e.Message}");
            }
        }

        public static T Load<T>(string path)
        {
            try
            {
                string p = Path.Combine(Application.persistentDataPath, path);
                if (File.Exists(p))
                {
                    string jsonString = null;
                    using (StreamReader streamReader = File.OpenText(p))
                    {
                        jsonString = streamReader.ReadToEnd();
                    }
                    return JsonConvert.DeserializeObject<T>(jsonString);
                }

            }
            catch (Exception e)
            {
                Debug.Log($"Exception thrown while loading {path}. {e.Message}");
            }
            return default(T);
        }
    }
}
