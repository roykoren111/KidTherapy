using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class WordsSpawner : MonoBehaviour
{
    public static WordsSpawner Instance;

    InnerTalkWord[] sentenceWords;
    private int _wordCount = 0;
    private int _wordsTapped = 0;
    public async UniTask SpawnSentenceAndWaitForCompletion(InnerTalkData innerTalkData)
    {
        await SpawnSentence(innerTalkData);

        while (_wordCount > _wordsTapped)
        {
            await UniTask.Yield();

            // TODO: should wait until tap animation of last word is finished.
        }

        // tell all words to disappear
        foreach (InnerTalkWord word in sentenceWords)
        {
            word.Remove(2f);
        }

        await UniTask.Delay(TimeSpan.FromSeconds(2.5f));
    }

    public async UniTask SpawnSentence(InnerTalkData innerTalkData)
    {
        _wordCount = innerTalkData.WordPrefabs.Length;
        sentenceWords = new InnerTalkWord[_wordCount];
        _wordsTapped = 0;

        for (int i = 0; i < innerTalkData.WordPrefabs.Length; i++)
        {
            GameObject wordGO = Instantiate(innerTalkData.WordPrefabs[i]);
            sentenceWords[i] = wordGO.GetComponent<InnerTalkWord>();

            //sentenceWords[i].Spawn(); // will be awaitable when spawning is an animation.

            await UniTask.Yield();
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
