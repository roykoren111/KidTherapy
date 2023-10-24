using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class InnerTalkManager : MonoBehaviour
{
    public static InnerTalkManager Instance;

    private int _wordCount = 0;
    public async UniTask SpawnSentenceAndWaitForCompletion()
    {
        // wait for whole sentence to complete.

        // trigger sentence disappear animation
    }

    public async UniTask SpawnSentence(TrialInnerTalk trialConfiguration)
    {
        _wordCount = trialConfiguration.Words.Length;
        foreach(var word in trialConfiguration.Words)
        {
            word.Spawn();
        }
    }

    private void Awake()
    {
        SingletonValidation();    
    }
    private void SingletonValidation()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(Instance);
        }
        else
        {
            Instance = this;
        }
    }
   
}
