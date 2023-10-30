using System;
using System.IO;
using System.Text;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;

namespace Game.Common.Scripts.Services.DataPersistency.Concretes
{
    public class JsonDataPersistencyService : BaseDataPersistencyService
    {
        public override void Initialize(string path)
        {
            base.Initialize(path);
            Directory.CreateDirectory(GetFullDirPath());
        }
        
        /// <summary>
        /// Serializes data object to JSON and then saves it to Application.persistentDataPath
        /// </summary>
        /// <typeparam name="T">The generic type to save</typeparam>
        /// <param name="keyName">The file name located inside Application.persistentDataPath</param>
        /// <param name="data">The generic data object</param>
        public override UniTask Save<T>(string keyName, T data)
        {
            try
            {
                var jsonData = JsonConvert.SerializeObject(data);
                var bytesToEncode = Encoding.UTF8.GetBytes(jsonData);

                var filePath = GetFullFilePath(keyName);
                File.WriteAllBytes(filePath, bytesToEncode);
                return UniTask.CompletedTask;
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                throw;
            }
        }
        
        /// <summary>
        /// Loads a JSON data object from Application.persistentDataPath. Can return null in case of error.
        /// </summary>
        /// <typeparam name="T">The generic type to load</typeparam>
        /// <param name="keyName">The JSON file name located inside Application.persistentDataPath</param>
        /// <returns>The generic data object</returns>
        public override UniTask<T> Load<T>(string keyName)
        {
            try
            {
                var decodedText = ReadDataFromFile(keyName);
                var data = JsonConvert.DeserializeObject<T>(decodedText);
                return new UniTask<T>(data);
            }
            catch (Exception e)
            {
                Debug.LogError(e.ToString());
                throw;
            }
        }

        private string ReadDataFromFile(string fileName)
        {
            var decodedText = string.Empty;

            var filePath = GetFullFilePath(fileName);
            
            if (File.Exists(filePath))
            {
                decodedText = File.ReadAllText(filePath, Encoding.UTF8);
            }

            return decodedText;
        }

        /// <summary>
        /// Deletes the file by keyName.
        /// </summary>
        /// <param name="keyName">Name of file</param>
        public override UniTask Delete(string keyName)
        {
            try
            {
                var filePath = GetFullFilePath(keyName);
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }

                return UniTask.CompletedTask;
            }
            catch (Exception e)
            {
                Debug.LogError(e.ToString());
                throw;
            }
        }
  
        public override void DeleteAll()
        {
            var fullPath = GetFullDirPath();
            var directoryInfo = new DirectoryInfo(fullPath);
            foreach (var file in directoryInfo.GetFiles())
            {
                file.Delete(); 
            }
            foreach (var dir in directoryInfo.GetDirectories())
            {
                dir.Delete(true); 
            }
        }
    }
}