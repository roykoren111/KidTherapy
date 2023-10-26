using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundManagerGrounding : MonoBehaviour, RoundManager
{
    [SerializeField] private EItemCategory[] trials;

    public async UniTask RunRoundFlow(RoundConfiguration config)
    {
        UIController.Instance.SetRoundInitialUI(config);
        CharacterController.Instance.InitCharacterToRound(config.RoundType);
        // await CameraController.Instance.MoveToTransform(config.CameraTransform);

        // wait for confirmation on round instructions. - no need to
        //await InputManager.Instance.WaitForTapToContinue();

        TrialManagerGrounding groundingTrialManager = new TrialManagerGrounding();
        Debug.Log(trials.Length);

        // Start Trials
        for (int i = 0; i < trials.Length; i++)
        {
            await groundingTrialManager.RunTrialFlow(trials[i]);
            await UniTask.Yield();
        }
        UIController.Instance.ClearGroundingUI();
        Debug.Log("Grounding round ended");
        // End
    }
}