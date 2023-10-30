using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    public static PlayerInfo Instance;

    public string Name { get; private set; }
    public PlayerGender Gender { get; private set; }
    public const string NONESELECTEDNAME = "NONESELECTED";
    public void SetName(string name)
    {
        if (name != null)
        {
            Name = name;
        }
        else
        {
            // UI will know how to handle it.
            Name = NONESELECTEDNAME;
        }
    }

    public void SetGender(PlayerGender gender)
    {
        Gender = gender;
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

public enum PlayerGender { Female, Male, None }