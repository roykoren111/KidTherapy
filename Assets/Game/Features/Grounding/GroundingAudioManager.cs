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

    private void Start()
    {
        _correctPickSoundsByCategory = new Dictionary<EItemCategory, AudioSource[]>()
        {
            { EItemCategory.See, SeeCorrectPicks }, { EItemCategory.Taste, SeeCorrectPicks },
            { EItemCategory.Hear, HearCorrectPicks }
        };
        _wrongPickSoundByCategory = new Dictionary<EItemCategory, AudioSource>()
            { { EItemCategory.See, SeeWrongPick }, { EItemCategory.Taste, SeeWrongPick } };
    }


    public void PlayRandomCorrectPickSound(EItemCategory category)
    {
        AudioSource[] pickSounds = _correctPickSoundsByCategory[category];
        int soundIndex = Random.Range(0, pickSounds.Length);
        pickSounds[soundIndex].Play();

        if (category == EItemCategory.Hear)
        {
            PlayRandomBubblePopSound();
        }
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