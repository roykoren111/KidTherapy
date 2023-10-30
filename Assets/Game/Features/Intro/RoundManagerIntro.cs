using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundManagerIntro : MonoBehaviour, RoundManager
{
    public async UniTask RunRoundFlow(RoundConfiguration config)
    {
        CharacterController.Instance.MoveToBirthPosition();
        await UniTask.Delay(1000);
        await CharacterController.Instance.MoveToCenter(2f);

        await UIController.Instance.SetRoundInitialUI(config);
        // await CameraController.Instance.MoveToTransform(config.CameraTransform, config.CameraLerpDuration);
        // make sure character appears in its basic form

        await InputManager.Instance.WaitForTapUpToContinue();
        Debug.Log("Intro round ended");
        // End
    }

}
