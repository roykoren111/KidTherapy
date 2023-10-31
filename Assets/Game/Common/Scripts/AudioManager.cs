using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] private AudioSource openingA;
    [SerializeField] private AudioSource openingBLoop;
    [SerializeField] private AudioSource innerTalkLoop;
    [SerializeField] private AudioSource screenTransition;

    [SerializeField] private AudioSource[] _correctPick;
    int _lastCorrectPickIndex = 0;

    [SerializeField] private float _musicVolume;
    private void Start()
    {
        openingA.volume = _musicVolume;
        openingBLoop.volume = _musicVolume;
        innerTalkLoop.volume = _musicVolume;
    }
    public void PlayMainMusic()
    {
        PlayOpeningThenLoop().Forget();
    }

    public async UniTask FadeMainMusic()
    {
        UniTask openingAFade = FadeMainMusicToVolume(openingA, 0, 2f);
        UniTask openingBFade = FadeMainMusicToVolume(openingBLoop, 0, 2f);
        await UniTask.WhenAll(openingAFade, openingBFade);
        Debug.Log("Faded all");
        openingA.Pause();
        openingBLoop.Pause();
    }

    private async UniTask FadeMainMusicToVolume(AudioSource audio, float targetVolume, float duration)
    {
        Debug.Log("Start fading " + audio.name);
        float lerpTime = 0;

        float currentVolume = audio.volume;
        while (lerpTime < duration)
        {
            lerpTime += Time.deltaTime;
            float t = lerpTime / duration;
            audio.volume = Mathf.Lerp(currentVolume, targetVolume, t);

            await UniTask.Yield();
        }
    }

    public async UniTask PlayInnerTalkMusic()
    {
        if (openingBLoop.isPlaying)
        {
            openingBLoop.Stop();
        }
        innerTalkLoop.volume = 0;
        innerTalkLoop.Play();
        await FadeMainMusicToVolume(innerTalkLoop, _musicVolume, 2f);
    }

    public void PlayInnerTalkCorrectPick()
    {
        if (_lastCorrectPickIndex == _correctPick.Length)
        {
            _lastCorrectPickIndex = 0;
        }

        _correctPick[_lastCorrectPickIndex].Stop();
        _correctPick[_lastCorrectPickIndex].Play();
        _lastCorrectPickIndex++;
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
        openingA.Stop();
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
