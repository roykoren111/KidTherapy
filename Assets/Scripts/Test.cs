using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class Test : MonoBehaviour
{
    private void OnEnable()
    {
        DoAsync().Forget();
    }
    
    private async UniTask DoAsync()
    {
        Debug.Log("HELLO!");
        await UniTask.Delay(TimeSpan.FromSeconds(2f));
        Debug.Log("HELLO ASYNC..");
    }
}
