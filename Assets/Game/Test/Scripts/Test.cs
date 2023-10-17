using System;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField] private TMP_Text _testText;
    
    private void OnEnable()
    {
        DoAsync().Forget();
    }
    
    private async UniTask DoAsync()
    {
        _testText.text = "Kid";
        await UniTask.Delay(TimeSpan.FromSeconds(1f));
        _testText.text += " Therapy";
        await UniTask.Delay(TimeSpan.FromSeconds(1f));
        _testText.text += " Game";
    }
}
