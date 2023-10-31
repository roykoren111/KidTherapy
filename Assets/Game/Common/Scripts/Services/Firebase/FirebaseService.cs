using System;
using Cysharp.Threading.Tasks;
using Firebase;
using Firebase.Analytics;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine;

namespace Game.Common.Scripts.Services.Firebase
{
    public class FirebaseService : IFirebaseService
    {
        public bool Initialized { get; private set; }

        private UniTask AwaitInitialization() => UniTask.WaitUntil(() => Initialized);
      
        public async UniTask Initialize()
        {
            if (Initialized)
            {
                Debug.Log("FirebaseService.Init - already initialized");
                return;
            }
            
            Debug.Log("FirebaseService.Init started");

            try
            {
                await FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(status =>
                {
                    if (status.Result == DependencyStatus.Available)
                    {
                        FirebaseApp.LogLevel = LogLevel.Debug;
                        FirebaseDatabase.DefaultInstance.SetPersistenceEnabled(false);
                        Initialized = true;
                        Debug.Log("FirebaseService.Init is successfully completed");
                    }
                    else
                    {
                        Debug.LogError($"FirebaseService.Init: Firebase Services could not be resolved: {status.Result}");
                    }
                });
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        public void SetUserId(string id)
        {
            SetUserIdAsync(id);
        }

        public void SendEvent(string eventName)
        {
            SendEventAsync(eventName);
        }

        private void SetUserIdAsync(string userId)
        {
            FirebaseAnalytics.SetUserId(userId);
        }

        private void SendEventAsync(string eventName)
        {
            FirebaseAnalytics.LogEvent(eventName);
        }

        public void Dispose()
        {
            Debug.Log("FirebaseService: dispose on destroy");
            try
            {
                FirebaseApp.DefaultInstance?.Dispose();
            }
            catch (Exception ex)
            {
                // firebase tends to throw errors on dispose for some reason
                Debug.LogException(ex);
            }
            
        }
    }
}
