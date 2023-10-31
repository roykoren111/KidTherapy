using System;
using Cysharp.Threading.Tasks;

namespace Game.Common.Scripts.Services.Firebase
{
    public interface IFirebaseService : IDisposable
    {
        bool Initialized { get; }
        UniTask Initialize();
        void SetUserId(string id);
        void SendEvent(string eventName);
    }
}