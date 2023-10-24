using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrialManagerInnerTalk : MonoBehaviour
{
    public async UniTask RunTrialFlow(TrialInnerTalk trialConfiguration)
    {
        CharacterController.Instance.SlideToScreenButtom();

        await InnerTalkManager.Instance.SpawnSentenceAndWaitForCompletion();


        // trigger character empowered animation.
        CharacterController.Instance.OnInnerTalkSentenceComplete();

        

        // wait until the whole sentence is marked.

    }
}

[Serializable]
public class TrialInnerTalk
{
    public InnerTalkWord[] Words;
}
