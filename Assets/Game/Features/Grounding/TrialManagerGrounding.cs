using Cysharp.Threading.Tasks;
using UnityEngine;

public class TrialManagerGrounding
{
    private int _collectedItemsCount = 0;
    private ItemsManager _itemsManager;

    public async UniTask RunTrialFlow(EItemCategory itemCategory)
    {
        _collectedItemsCount = 0;
        _itemsManager = ItemsManager.Instance;
        int requiredItemsCount = _itemsManager.GetRequiredItemsCount(itemCategory);

        UIController.Instance.ResetGroundingTrialUI(itemCategory, requiredItemsCount);
        await _itemsManager.SpawnItems(itemCategory);

        _itemsManager.ItemCollected += OnItemCollected;

        while (_collectedItemsCount < requiredItemsCount)
        {
            await UniTask.Yield();
        }

        _itemsManager.ItemCollected -= OnItemCollected;

        await _itemsManager.DestroyRemainingItems();
    }

    private void OnItemCollected(Transform collectedItem)
    {
        _collectedItemsCount++;
        UIController.Instance.UpdateCollectedItems(_collectedItemsCount);
        // CharacterController.Instance.AddItemToCharacter(collectedItem);
    }
}