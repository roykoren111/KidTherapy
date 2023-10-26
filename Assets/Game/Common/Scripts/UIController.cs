using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public static UIController Instance;

    private GameObject _currentScreenInstance;

    [SerializeField] private Transform _gameCanvasTransform;

    [SerializeField] private GameObject _groundingCanvasSee;
    [SerializeField] private GameObject _groundingCanvasHear;
    [SerializeField] private GameObject _groundingCanvasTaste;

    private GameObject _currentGroundingCanvas;
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
        if (_currentGroundingCanvas != null)
        {
            Destroy(_currentGroundingCanvas);
        }

        GameObject newGroundingCanvas = null;
        // Set category title
        switch (itemCategory)
        {
            case EItemCategory.See:
                newGroundingCanvas = _groundingCanvasSee;
                break;
            case EItemCategory.Hear:
                newGroundingCanvas = _groundingCanvasHear;
                break;
            case EItemCategory.Taste:
                newGroundingCanvas = _groundingCanvasTaste;
                break;
        }

        if (newGroundingCanvas != null)
        {
            _currentGroundingCanvas = Instantiate(newGroundingCanvas, _gameCanvasTransform);
        }

        // Reset items collected counter.

    }

    public void ClearGroundingUI()
    {
        Destroy(_currentGroundingCanvas);
    }

    public void UpdateCollectedItems(int collectedItems)
    {
        // update collected items amount in grounding UI
    }

    public async UniTask ListenToNameFieldClick()
    {
        // wait until clicking the name field to open keyboard
    }

    public async UniTask<string> GetKeyboardInput()
    {
        string selectedName = "";
        // open keyboard and wait until input is recieved and done
        return selectedName;
    }

    public async UniTask<PlayerGender> GenderSelection()
    {
        PlayerGender selectedGender = PlayerGender.None;
        // display gender selection UI
        // wait until selecting a gender
        return selectedGender;
    }
    

}