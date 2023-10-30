using Cysharp.Threading.Tasks;

namespace Game.Common.Scripts.Services.DataPersistency.Interfaces
{
    public interface IDataPersistencyService
    {
        void Initialize(string path);
        
        UniTask Save<T>(string keyName, T data);
        UniTask<T> Load<T>(string keyName);
        UniTask Delete(string keyName);
        void DeleteAll();
    }
}