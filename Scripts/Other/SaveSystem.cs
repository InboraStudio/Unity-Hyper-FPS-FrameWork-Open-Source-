using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace Inbora.FPSFramework
{
    /// <summary>
    /// A simple save system based on Unity's Json Utility.
    /// </summary>
    public static class SaveSystem
    {
        /// <summary>
        /// The default save path.
        /// </summary>
        public static readonly string savePath = $"{Application.persistentDataPath}/";

        /// <summary>
        /// Saves a an object as a .json file
        /// </summary>
        /// <param name="_object"></param>
        /// <param name="fileName"></param>
        public static void Save(object _object, string fileName = "file")
        {
            string _data = JsonUtility.ToJson(_object, true);
            string _path = $"{savePath}{fileName}.json";

            File.WriteAllText(_path, _data);
        }

        /// <summary>
        /// Reads a file and loads its values from .json file.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static T Load<T>(string fileName = "file")
        {
            string _path = $"{savePath}{fileName}.json";
            string _data = null;

            if (File.Exists(_path))
            {
                _data = File.ReadAllText(_path);
                return JsonUtility.FromJson<T>(_data);
            }

            return default;
        }

        /// <summary>
        /// Loads all files in an array from all .json files in the default save path.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T[] LoadAll<T>()
        {
            List<T> ts = new List<T>();

            foreach (string file in Directory.GetFiles(savePath))
            {
                ts.Add(Load<T>(file));
            }

            return default;
        }

        /// <summary>
        /// Deletes a file with the given name in the default save path, file extension isn't required.
        /// </summary>
        /// <param name="fileName"></param>
        public static void DeleteSave(string fileName = "file")
        {
            File.Delete($"{savePath}{fileName}.json");
        }

        /// <summary>
        /// Deletes all the .json files at the default save path.
        /// </summary>
        public static void DeleteAllSaves()
        {
            foreach (string file in Directory.GetFiles(savePath))
            {
                if (Path.GetExtension(file) == ".json")
                {
                    File.Delete(file);
                }
            }
        }

        //TODO: Local key saving for (int, float, bool & string).
    }
}