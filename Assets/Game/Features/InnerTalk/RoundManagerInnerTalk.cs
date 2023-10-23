using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundManagerInnerTalk : MonoBehaviour, RoundManager
{

    public async UniTask RunRoundFlow(RoundConfiguration config)
    {
        UIController.Instance.SetRoundInitialUI(config.RoundType);
        await CameraController.Instance.MoveToTransform(config.CameraTransform, config.CameraLerpDuration);
        CharacterController.Instance.InitCharacterToRound(config.RoundType);

        // maybe wait to start tension animation or make character go into tension state.


        // wait for kids to confirm they are ready to start stretching.
        await InputManager.Instance.WaitForTapToContinue();

        await CharacterController.Instance.PlayStretchingAnimation();

        // End
    }
}
