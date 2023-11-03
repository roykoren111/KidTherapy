using Cysharp.Threading.Tasks;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public static UIController Instance;

    private GameObject _currentScreenInstance;

    [SerializeField] private Transform _gameCanvasTransform;

    [Header("Grounding UI")]
    [SerializeField] private GameObject _groundingCanvasSee;
    [SerializeField] private GameObject _groundingCanvasHear;
    [SerializeField] private GameObject _groundingCanvasTaste;

    [Header("Breathing UI")]
    [SerializeField] private GameObject _breathingInhale;
    [SerializeField] private GameObject _breathingHold;
    [SerializeField] private GameObject _breathingExhale;
    [SerializeField] private GameObject _endingUI;

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

    public async UniTask SetRoundInitialUI(RoundConfiguration roundConfiguration)
    {
        await SetUIAlpha(false, 1f);

        if (_currentScreenInstance != null)
        {
            Destroy(_currentScreenInstance);
        }

        if (roundConfiguration?.UIPrefab != null)
        {
            _currentScreenInstance = Instantiate(roundConfiguration.UIPrefab, _gameCanvasTransform);
        }

        await SetUIAlpha(true, 1f);

    }

    public async UniTask ResetGroundingTrialUI(EItemCategory itemCategory, int requiredItemsCount)
    {
        await SetUIAlpha(false, 1f);

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
        await SetUIAlpha(true, 1f);

        // Reset items collected counter.

    }

    public async UniTask SetBreathingUI(BreathingStage stage)
    {
        await SetUIAlpha(false, .1f);
        if (_currentScreenInstance != null)
        {
            Destroy(_currentScreenInstance);
        }

        GameObject newBreathingCanvas = null;
        switch (stage)
        {
            case BreathingStage.Inhale:
                newBreathingCanvas = _breathingInhale;
                break;
            case BreathingStage.Hold:
                newBreathingCanvas = _breathingHold;
                break;
            case BreathingStage.Exhale:
                newBreathingCanvas = _breathingExhale;
                break;
        }

        if (newBreathingCanvas != null)
        {
            _currentScreenInstance = Instantiate(newBreathingCanvas, _gameCanvasTransform);
        }
        await SetUIAlpha(true, .5f);
    }

    public async UniTask SetUIAlpha(bool shouldAppear, float duration)
    {
        float currentAlpha = shouldAppear ? 0 : 1;
        float targetAlpha = shouldAppear ? 1 : 0;
        float lerpTime = 0;
        TMP_Text[] texts = GetComponentsInChildren<TMP_Text>();
        Image[] images = GetComponentsInChildren<Image>();
        while (lerpTime < duration)
        {
            lerpTime += Time.deltaTime;
            float t = lerpTime / duration;
            t = t * t * t * (t * (6f * t - 15f) + 10f); // very smooth and nice step function

            foreach (TMP_Text text in texts)
            {
                Color targetColor = text.color;
                targetColor.a = Mathf.Lerp(currentAlpha, targetAlpha, t);
                text.color = targetColor;
            }

            foreach (Image image in images)
            {
                Color targetColor = image.color;
                targetColor.a = Mathf.Lerp(currentAlpha, targetAlpha, t);
                image.color = targetColor;
            }
            await UniTask.Yield();
        }
    }

    public async UniTask ClearGroundingUI()
    {
        await SetUIAlpha(false, 1f);
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