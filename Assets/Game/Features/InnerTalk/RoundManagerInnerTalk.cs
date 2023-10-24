using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundManagerInnerTalk : MonoBehaviour, RoundManager
{
    [SerializeField] private TrialInnerTalk[] _trials;
    public async UniTask RunRoundFlow(RoundConfiguration config)
    {
        UIController.Instance.SetRoundInitialUI(config.RoundType);
        await CameraController.Instance.MoveToTransform(config.CameraTransform, config.CameraLerpDuration);
        CharacterController.Instance.InitCharacterToRound(config.RoundType);

        // TODO: Add tutorial round with tap indicator before starting trials.

        TrialManagerInnerTalk trialManager = new TrialManagerInnerTalk();

        for (int i = 0; i < _trials.Length; i++)
        {
            await trialManager.RunTrialFlow(_trials[i]);
            await UniTask.Yield();
        }

        // End
    }

    private async UniTask RunTutorialTrial()
    {
        await InnerTalkManager.Instance.SpawnSentence(_trials[0]);

        // spawn tap indicator


    }
}
