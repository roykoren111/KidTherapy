using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundManagerIntro : MonoBehaviour, RoundManager
{
    public async UniTask RunRoundFlow(RoundConfiguration config)
    {
        UIController.Instance.SetRoundInitialUI(config.RoundType);
        await CameraController.Instance.MoveToTransform(config.CameraTransform, config.CameraLerpDuration);

        // make sure character appears in its basic form

        await InputManager.Instance.WaitForTapToContinue();

        // End
    }

}
