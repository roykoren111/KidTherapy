using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundManagerGrounding : MonoBehaviour, RoundManager
{
    [SerializeField] private EItemCategory[] trials;
    [SerializeField] Vector2 _betweenSpawnsDelay;
    [SerializeField] Vector2 _itemFloatingSpeedRange;
    public async UniTask RunRoundFlow(RoundConfiguration config)
    {
        await UIController.Instance.SetRoundInitialUI(config);
        CharacterController.Instance.InitCharacterToRound(config.RoundType);

        TrialManagerGrounding groundingTrialManager = new TrialManagerGrounding();
        Debug.Log(trials.Length);

        // Start Trials
        for (int i = 0; i < trials.Length; i++)
        {
            await groundingTrialManager.RunTrialFlow(trials[i], _betweenSpawnsDelay, _itemFloatingSpeedRange);
            await UniTask.Yield();

        }
        await UIController.Instance.ClearGroundingUI();
        Debug.Log("Grounding round ended");
        // End
    }
}