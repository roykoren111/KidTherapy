using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundManagerEnd : MonoBehaviour, RoundManager
{
    [SerializeField] RestartButton restartButton;
    private bool _shouldRestart = false;
    public async UniTask RunRoundFlow(RoundConfiguration config)
    {
        restartButton.gameObject.SetActive(true);
        restartButton.Init();
        restartButton.RestartButtonTapped += OnRestartButtonTapped;

        await UIController.Instance.SetEndingUI();
        await UniTask.Delay(2000);
        await restartButton.Appear();
        while (!_shouldRestart) await UniTask.Yield();

        // when restart is pressed - this round ends and game manager restarts.
    }

    private void OnRestartButtonTapped()
    {
        _shouldRestart = true;
    }
}
