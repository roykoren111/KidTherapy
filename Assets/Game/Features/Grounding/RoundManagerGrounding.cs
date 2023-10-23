using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundManagerGrounding : MonoBehaviour, RoundManager
{
    [SerializeField] private EItemCategory[] trials;

    public async UniTask RunRoundFlow(RoundConfiguration config)
    {
        UIController.Instance.SetRoundInitialUI(config.RoundType);
        await CameraController.Instance.MoveToTransform(config.CameraTransform);

        await InputManager.Instance.WaitForTapToContinue();

        TrialManagerGrounding trialManager = new TrialManagerGrounding();
        // Start Trials
        for (int i = 0; i < trials.Length; i++)
        {
            await trialManager.RunTrialFlow(trials[i]);
            await UniTask.Yield();
        }

    }

}
