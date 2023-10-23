using System;
using Cysharp.Threading.Tasks;
using Firebase.Database;
using Game.Common.Scripts.Services.Firebase;
using Newtonsoft.Json;
using UnityEngine;

namespace Game.Common.Scripts.Services.DataPersistency.Concretes
{
    public class FirebaseRealtimePersistencyService : BaseDataPersistencyService
    {
        private readonly string USERS_KEY = "Users";

        private readonly IFirebaseService _firebaseService;
        
        public FirebaseRealtimePersistencyService(IFirebaseService firebaseService)
        {
            _firebaseService = firebaseService;
        }
        
        public override async UniTask Save<T>(string keyName, T data)
        {
            try
            {
                await UniTask.WaitUntil(() => _firebaseService.Initialized);
                var jsonData = JsonConvert.SerializeObject(data);
                await FirebaseDatabase.DefaultInstance.RootReference
                    .Child(USERS_KEY)
                    .Child(SubPath)
                    .Child(keyName)
                    .SetRawJsonValueAsync(jsonData)
                    .AsUniTask();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        public override async UniTask<T> Load<T>(string keyName)
        {
            try
            {
                await UniTask.WaitUntil(() => _firebaseService.Initialized);
                var dataSnapshot = await FirebaseDatabase.DefaultInstance.RootReference
                    .Child(USERS_KEY)
                    .Child(SubPath)
                    .Child(keyName)
                    .GetValueAsync()
                    .AsUniTask();
                
                if (!dataSnapshot.Exists)
                {
                    return default;
                }

                var data = JsonConvert.DeserializeObject<T>(dataSnapshot.GetRawJsonValue());
                return data;
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                return default;
            }
       
        }

        public override async UniTask Delete(string keyName)
        {
            try
            {
                await UniTask.WaitUntil(() => _firebaseService.Initialized);
                await FirebaseDatabase.DefaultInstance.RootReference
                    .Child(USERS_KEY)
                    .Child(SubPath)
                    .Child(keyName)
                    .RemoveValueAsync()
                    .AsUniTask();

            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        public override void DeleteAll()
        {
            FirebaseDatabase.DefaultInstance.RootReference
                .Child(USERS_KEY)
                .Child(SubPath)
                .RemoveValueAsync()
                .AsUniTask()
                .Forget();
        }
    }
}