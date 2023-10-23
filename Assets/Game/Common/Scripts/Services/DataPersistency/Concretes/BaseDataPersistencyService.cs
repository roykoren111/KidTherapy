using Cysharp.Threading.Tasks;
using Game.Common.Scripts.Services.DataPersistency.Interfaces;
using UnityEngine;

namespace Game.Common.Scripts.Services.DataPersistency.Concretes
{
    public abstract class BaseDataPersistencyService : IDataPersistencyService
    {
        protected string SubPath { get; private set; }
        
        public virtual void Initialize(string path)
        {
            SubPath = path;
        }

        public abstract UniTask Save<T>(string keyName, T data);

        public abstract UniTask<T> Load<T>(string keyName);

        public abstract UniTask Delete(string keyName);

        public abstract void DeleteAll();

        protected string GetFullFilePath(string keyName)
        {
            return $"{GetFullDirPath()}/{keyName}";
        }
        
        protected string GetFullDirPath()
        {
            return string.IsNullOrEmpty(SubPath)
                ? $"{Application.persistentDataPath}"
                : $"{Application.persistentDataPath}/{SubPath}";

        }
    }
}