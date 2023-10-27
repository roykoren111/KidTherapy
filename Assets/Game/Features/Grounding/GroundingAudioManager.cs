using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class GroundingAudioManager : MonoBehaviour
{
    [Header("See sounds")] public AudioSource SeeWrongPick;
    public AudioSource[] SeeCorrectPicks;

    [Header("Hear sounds")] public AudioSource[] HearCorrectPicks;

    public AudioSource[] BubblePops;
    [Header("Taste sounds")] public AudioSource TasteWrongPick;
    public AudioSource[] TasteCorrectPicks;

    public static GroundingAudioManager Instance;

    private Dictionary<EItemCategory, AudioSource[]> _correctPickSoundsByCategory;
    private Dictionary<EItemCategory, AudioSource> _wrongPickSoundByCategory;
    AudioSource _previousSound;
    int _hearingSoundIndex = 0;
    private void Start()
    {
        _previousSound = SeeCorrectPicks[1];

        _correctPickSoundsByCategory = new Dictionary<EItemCategory, AudioSource[]>()
        {
            { EItemCategory.See, SeeCorrectPicks }, { EItemCategory.Taste, TasteCorrectPicks },
            { EItemCategory.Hear, HearCorrectPicks }
        };
        _wrongPickSoundByCategory = new Dictionary<EItemCategory, AudioSource>()
            { { EItemCategory.See, SeeWrongPick }, { EItemCategory.Taste, SeeWrongPick } };

        RandomizeArray(HearCorrectPicks);
    }


    public void PlayRandomCorrectPickSound(EItemCategory category)
    {
        if (category == EItemCategory.Hear)
        {
            PlayRandomBubblePopSound();
            _hearingSoundIndex = _hearingSoundIndex == HearCorrectPicks.Length ? 0 : _hearingSoundIndex;
            HearCorrectPicks[_hearingSoundIndex].Play();
            _hearingSoundIndex++;
            return;
        }

        AudioSource[] pickSounds = _correctPickSoundsByCategory[category];
        int soundIndex = Random.Range(0, pickSounds.Length);
        while (_previousSound == pickSounds[soundIndex])
        {
            soundIndex = Random.Range(0, pickSounds.Length);
        }

        pickSounds[soundIndex].Play();
        _previousSound = pickSounds[soundIndex];
    }

    public void PlayWrongPickSound(EItemCategory category)
    {
        if (category != EItemCategory.Hear)
        {
            _wrongPickSoundByCategory[category].Play();
        }
        else
        {
            PlayRandomBubblePopSound();
        }
    }

    private void PlayRandomBubblePopSound()
    {
        int soundIndex = Random.Range(0, BubblePops.Length);
        BubblePops[soundIndex].Play();
    }

    private void RandomizeArray(AudioSource[] list)
    {
        int n = list.Length;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n + 1);
            AudioSource slot = list[k];
            list[k] = list[n];
            list[n] = slot;
        }
    }

    private void Awake()
    {
        DependencyManager.SetDependency(this);
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