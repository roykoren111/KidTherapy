using Cysharp.Threading.Tasks;
using UnityEngine;

public class TrialManagerGrounding
{
    private int _collectedItemsCount = 0;
    private ItemsManager _itemsManager;


    public async UniTask RunTrialFlow(EItemCategory itemCategory, Vector2 spawnDelayRange, Vector2 itemSpeedRange)
    {
        _collectedItemsCount = 0;
        _itemsManager = ItemsManager.Instance;
        int requiredItemsCount = _itemsManager.GetRequiredItemsCount(itemCategory);

        UniTask spawnUI = UIController.Instance.ResetGroundingTrialUI(itemCategory, requiredItemsCount);
        UniTask spawnItems = _itemsManager.SpawnItems(itemCategory, spawnDelayRange, itemSpeedRange);
        await UniTask.WhenAll(spawnUI, spawnItems);
        _itemsManager.ItemCollected += OnItemCollected;

        while (_collectedItemsCount < requiredItemsCount)
        {
            await UniTask.Yield();
        }

        _itemsManager.ItemCollected -= OnItemCollected;
        UniTask destroyRemainingItems = _itemsManager.DestroyRemainingItems();
        UniTask clearUI = UIController.Instance.ClearGroundingUI();
        AudioManager.Instance.PlayScreenTransitionSound();
        CharacterController.Instance.EyesToCenter(2f).Forget();
        await UniTask.WhenAll(destroyRemainingItems, clearUI);
    }

    private void OnItemCollected(Item collectedItem)
    {
        _collectedItemsCount++;
        UIController.Instance.UpdateCollectedItems(_collectedItemsCount);
        CharacterController.Instance.AddItemToCharacter(collectedItem);
    }

    private EItemColors GetRandomItemColor()
    {
        EItemColors[] possibleItemColors = new EItemColors[]
            { EItemColors.Green, EItemColors.Blue, EItemColors.Orange, EItemColors.Red, EItemColors.Red };
        int itemColorIndex = Random.Range(0, possibleItemColors.Length);
        return possibleItemColors[itemColorIndex];
    }
}