using System;
using Firebase.Database;
using Newtonsoft.Json;
using UnityEngine;

namespace Game.Common.Scripts.Services.Firebase
{
    public class FirebaseListener<T> : IDisposable
    {
        private const string DATABASE_REFERENCE_NOT_FOUND_EXCEPTION = "Database Reference not found for path: {0}";
        
        private DatabaseReference _databaseReference;
        private Action<T> _callback;

        public FirebaseListener(string path, Action<T> callback, FirebaseDatabase firebaseDatabase)
        {
            _callback = callback;
            _databaseReference = firebaseDatabase?.GetReference(path);
            if (_databaseReference == null)
            {
                Debug.LogError(string.Format(DATABASE_REFERENCE_NOT_FOUND_EXCEPTION, path));
                return;
            }
            
            _databaseReference.ValueChanged += DatabaseReferenceOnValueChanged;
        }

        private void DatabaseReferenceOnValueChanged(object sender, ValueChangedEventArgs args)
        {
            var rawValue = args.Snapshot.GetRawJsonValue();
            if (string.IsNullOrEmpty(rawValue))
            {
                return;
            }

            var newValue = (T) JsonConvert.DeserializeObject(rawValue, typeof(T));
            _callback?.Invoke(newValue);
        }

        public void Dispose()
        {
            if (_databaseReference != null)
            {
                _databaseReference.ValueChanged -= DatabaseReferenceOnValueChanged;
                _databaseReference = null;
            }
    
            _callback = null;
        }
    }
}