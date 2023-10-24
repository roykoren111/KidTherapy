using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class InnerTalkManager : MonoBehaviour
{
    public static InnerTalkManager Instance;

    private int _wordCount = 0;
    private int _wordsTapped = 0;
    public async UniTask SpawnSentenceAndWaitForCompletion(InnerTalkData innerTalkData)
    {
        await SpawnSentence(innerTalkData);

        while (_wordCount > _wordsTapped)
        {
            await UniTask.Yield();
        }

        foreach (var word in innerTalkData.Words)
        {
            word.Remove(3f);
        }
        await UniTask.Delay(TimeSpan.FromSeconds(3f));
    }

    public async UniTask SpawnSentence(InnerTalkData innerTalkData)
    {
        _wordCount = innerTalkData.Words.Length;
        _wordsTapped = 0;
        foreach (var word in innerTalkData.Words)
        {
            word.Spawn();
        }
    }

    public void OnWordTap(InnerTalkWord word)
    {
        _wordsTapped++;
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
