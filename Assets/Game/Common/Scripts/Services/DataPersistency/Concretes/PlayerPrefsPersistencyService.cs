using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;

namespace Game.Common.Scripts.Services.DataPersistency.Concretes
{
    public class PlayerPrefsPersistencyService : BaseDataPersistencyService
    {
        private const string KeysPrefix = nameof(PlayerPrefsPersistencyService);

        /// <summary>
        /// Serializes the data to JSON and saves it to player prefs.
        /// </summary>
        /// <typeparam name="T">The generic type to save</typeparam>
        /// <param name="keyName">Key in the player prefs</param>
        /// <param name="data">The data that is going to be saved</param>
        public override UniTask Save<T>(string keyName, T data)
        {
            try
            {
                var key = GetKeyName(keyName);
                var jsonData = JsonConvert.SerializeObject(data);
                PlayerPrefs.SetString(key, jsonData);
                PlayerPrefs.Save();
                return UniTask.CompletedTask;
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                throw;
            }
        }

        private string GetKeyName(string keyName)
        {
            return $"{KeysPrefix}-{SubPath}-{keyName}";
        }

        /// <summary>
        /// Loads the data from player prefs. Can return null in case of error.
        /// </summary>
        /// <typeparam name="T">The generic type to save</typeparam>
        /// <param name="keyName">Key in the player prefs</param>
        /// <returns>The data that was saved as T, null in case of error</returns>
        public override UniTask<T> Load<T>(string keyName)
        {
            try
            {
                var key = GetKeyName(keyName);
                if (PlayerPrefs.HasKey(key))
                {
                    var jsonData = PlayerPrefs.GetString(key);
                    var data = JsonConvert.DeserializeObject<T>(jsonData);
                    return new UniTask<T>(data);
                }

                return new UniTask<T>(default);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                throw;
            }
        }

        /// <summary>
        /// Deletes the data by key.
        /// </summary>
        /// <param name="keyName">Key in the player prefs</param>
        public override UniTask Delete(string keyName)
        {
            try
            {
                var key = GetKeyName(keyName);
                if (PlayerPrefs.HasKey(key))
                {
                    PlayerPrefs.DeleteKey(key);
                    return UniTask.CompletedTask;
                }

                throw new KeyNotFoundException($"no such key {key} in local DataPersistencyService");
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                throw;
            }
        }

        public override void DeleteAll()
        {
            PlayerPrefs.DeleteAll();
        }
    }
}