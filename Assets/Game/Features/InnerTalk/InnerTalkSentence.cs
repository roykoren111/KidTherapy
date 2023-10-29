using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InnerTalkSentence : MonoBehaviour
{
    [SerializeField] InnerTalkWord[] _words;
    int _tappedWordsCount = 0;
    public Action<InnerTalkWord> WordTapped;
    public int WordCount => _words.Length;

    public void Hide()
    {
        for (int i = 0; i < _words.Length; i++)
        {
            _words[i].Hide();
        }
    }
    public async UniTask SpawnSentence()
    {
        for (int i = 0; i < _words.Length; i++)
        {
            Debug.Log("Spwaning " + _words[i].name);
            await _words[i].Spawn(this);
        }
    }

    public async UniTask ClearSentence()
    {
        for (int i = 0; i < _words.Length; i++)
        {
            if (i == _words.Length - 1)
            {
                await _words[i].Remove(4f);
            }
            else
            {
                _words[i].Remove(4f).Forget();
            }
        }
    }

    // the only listener is TrialManagerInnerTalk
    public void OnWordTap(InnerTalkWord word)
    {
        WordTapped?.Invoke(word);
    }
}
