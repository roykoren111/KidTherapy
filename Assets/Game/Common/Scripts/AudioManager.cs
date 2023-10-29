using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] private AudioSource openingA;
    [SerializeField] private AudioSource openingBLoop;
    [SerializeField] private AudioSource innerTalkLoop;
    [SerializeField] private AudioSource screenTransition;

    public void PlayMainMusic()
    {
        PlayOpeningThenLoop().Forget();
    }

    public void PlayInnerTalkMusic()
    {
        if (openingBLoop.isPlaying)
        {
            openingBLoop.Stop();
        }
        innerTalkLoop.Play();
    }

    public void PlayScreenTransitionSound()
    {
        screenTransition.Stop();
        screenTransition.Play();
    }
    private async UniTask PlayOpeningThenLoop()
    {
        openingA.Play();
        while (openingA.isPlaying) await UniTask.Yield();
        openingBLoop.Play();

    }


    private void Awake()
    {
        SingletonValidation();
    }
    private void SingletonValidation()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(Instance);
        }
        else
        {
            Instance = this;
        }
    }

}
