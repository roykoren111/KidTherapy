using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BreathingStage { Inhale, Hold, Exhale }

public class RoundManagerBreathing : MonoBehaviour, RoundManager
{
    [SerializeField] private int _breathingCycles = 2;
    [SerializeField] AudioSource[] _breathingSounds;
    [SerializeField] float _characterScaleAmount = .5f;
    [SerializeField] BreathingProgressIndicator _progressIndicator;
    public async UniTask RunRoundFlow(RoundConfiguration config)
    {
        CharacterController character = CharacterController.Instance;
        await UIController.Instance.SetRoundInitialUI(config);
        character.InitCharacterToRound(config.RoundType);

        // wait for kids to confirm they are ready to start breathings.
        UniTask waitForTap = InputManager.Instance.WaitForTapToContinue();
        UniTask waitForTime = UniTask.Delay(3000); // instead of waiting for tap.
        await UniTask.WhenAny(waitForTap, waitForTime);

        AudioManager.Instance.FadeMainMusic().Forget();
        float inhaleDuration = 4f, holdingDuration = 2f, exhaleDuration = 6f;
        character.SetBreathingAnimation(true);

        _progressIndicator.SetActive(true).Forget();
        float sessionDuration = (inhaleDuration + holdingDuration + exhaleDuration) * _breathingCycles;
        _progressIndicator.LaunchProgress(sessionDuration).Forget();
        for (int i = 0; i < _breathingCycles; i++)
        {
            LightsController.Instance.SetLightIntensity(0, 1f).Forget();
            character.ChangeScaleInBreathing(_characterScaleAmount, _breathingCycles, inhaleDuration);
            _breathingSounds[i].Play();

            UIController.Instance.SetBreathingUI(BreathingStage.Inhale).Forget();
            await UniTask.Delay(TimeSpan.FromSeconds(inhaleDuration));

            UIController.Instance.SetBreathingUI(BreathingStage.Hold).Forget();
            await UniTask.Delay(TimeSpan.FromSeconds(holdingDuration));

            UIController.Instance.SetBreathingUI(BreathingStage.Exhale).Forget();
            LightsController.Instance.SetLightIntensity(1.2f, exhaleDuration - .1f).Forget();   //-.1f so it won't collide with the light method in the beginning of the loop
            await UniTask.Delay(TimeSpan.FromSeconds(exhaleDuration));

        }
        character.SetBreathingAnimation(false);
        _progressIndicator.SetActive(false).Forget();

        Debug.Log("Breathing round ended");


        // End
    }
}
