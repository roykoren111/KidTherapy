using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class GroundingAudioManager : MonoBehaviour
{
    [Header("See sounds")] public AudioSource SeeWrongPick;

    [SerializeField] public AudioSource[] SeeCorrectPicks;

    [Header("Hear sounds")] [SerializeField]
    public AudioSource[] HearCorrectPicks;

    [Header("Taste sounds")] [SerializeField]
    public AudioSource TasteWrongPick;

    [SerializeField] public AudioSource[] TasteCorrectPicks;

    public static GroundingAudioManager Instance;

    private Dictionary<EItemCategory, AudioSource[]> _correctPickSoundsByCategory;
    private Dictionary<EItemCategory, AudioSource> _wrongPickSoundByCategory;

    private void Start()
    {
        _correctPickSoundsByCategory = new Dictionary<EItemCategory, AudioSource[]>()
        {
            { EItemCategory.See, SeeCorrectPicks }, { EItemCategory.Taste, TasteCorrectPicks },
            { EItemCategory.Hear, HearCorrectPicks }
        };
        _wrongPickSoundByCategory = new Dictionary<EItemCategory, AudioSource>()
            { { EItemCategory.See, SeeWrongPick }, { EItemCategory.Taste, TasteWrongPick } };
    }


    public void PlayRandomCorrectPickSound(EItemCategory category)
    {
        AudioSource[] pickSounds = _correctPickSoundsByCategory[category];
        int soundIndex = Random.Range(0, pickSounds.Length);
        pickSounds[soundIndex].Play();
    }

    public void PlayWrongPickSound(EItemCategory category)
    {
        _wrongPickSoundByCategory[category].Play();
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