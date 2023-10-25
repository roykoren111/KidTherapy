using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundManagerBreathing : MonoBehaviour, RoundManager
{
    [SerializeField] private int _breathingCycles = 2;
    public async UniTask RunRoundFlow(RoundConfiguration config)
    {
        UIController.Instance.SetRoundInitialUI(config);
        //await CameraController.Instance.MoveToTransform(config.CameraTransform, config.CameraLerpDuration);
        CharacterController.Instance.InitCharacterToRound(config.RoundType);

        // maybe wait to start breathing animation or make character go into breathing state.

        // wait for kids to confirm they are ready to start breathings.
        await InputManager.Instance.WaitForTapToContinue();

        await CharacterController.Instance.PlayBreathingAnimation(_breathingCycles);

        Debug.Log("Breathing round ended");


        // End
    }
}
