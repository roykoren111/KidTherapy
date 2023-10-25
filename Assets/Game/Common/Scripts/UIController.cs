using UnityEngine;

public class UIController : MonoBehaviour
{
    public static UIController Instance;
    
    private GameObject _currentScreenInstance;
    
    [SerializeField] private Transform _gameCanvasTransform;

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

    public void SetRoundInitialUI(RoundConfiguration roundConfiguration)
    {
        if (_currentScreenInstance != null)
        {
            Destroy(_currentScreenInstance);
        }
        
        if (roundConfiguration?.UIPrefab != null)
        {
            _currentScreenInstance = Instantiate(roundConfiguration.UIPrefab, _gameCanvasTransform);
        }
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
}