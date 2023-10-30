using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrialManagerInnerTalk
{
    int _tappedWords = 0;

    // sentence is instantiated on RoundManager and then passed to trial manager
    public async UniTask RunTrialFlow(InnerTalkSentence sentence)
    {
        sentence.WordTapped += OnWordTapped;
        int wordCount = sentence.WordCount;

        await sentence.SpawnSentence();

        while (_tappedWords < wordCount)
        {
            await UniTask.Yield();
        }
        await CharacterController.Instance.EyesToCenter(2f);
        // trigger character empowered animation.
        CharacterController.Instance.OnInnerTalkSentenceComplete();
        _tappedWords = 0;
        await UniTask.Delay(500);
        AudioManager.Instance.PlayScreenTransitionSound();

        await sentence.ClearSentence();

        // wait until the whole sentence is marked.

    }

    private void OnWordTapped(InnerTalkWord word)
    {
        _tappedWords++;
        // play sound word selection 
        word.SelectedEffect().Forget();
        AudioManager.Instance.PlayInnerTalkCorrectPick();
        CharacterController.Instance.SetLookTarget(word.transform.position);
    }
}
