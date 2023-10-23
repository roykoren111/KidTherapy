using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public static UIController Instance;

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

    public void SetRoundInitialUI(RoundType roundType)
    {
        // clear current -> spawn target
    }

    public void ResetGroundingTrialUI(EItemCategory itemCategory, int requiredItemsCount)
    {
        // Set category title

        // Reset items collected counter.
    }

    public void UpdateCollectedItems(int collectedItems)
    {
        // update collected items amount in grounding UI
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
