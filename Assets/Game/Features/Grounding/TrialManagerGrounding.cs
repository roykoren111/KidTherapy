using Cysharp.Threading.Tasks;

public class TrialManagerGrounding
{
    private int _collectedItemsCount = 0;
    private ItemsManager _itemsManager;

    public async UniTask RunTrialFlow(EItemCategory itemCategory)
    {
        _itemsManager = ItemsManager.Instance;
        int requiredItemsCount = _itemsManager.GetRequiredItemsCount(itemCategory);

        UIController.Instance.ResetGroundingTrialUI(itemCategory, requiredItemsCount);
        await _itemsManager.SpawnItems(itemCategory);

        while (_collectedItemsCount < requiredItemsCount)
        {
            ItemData collectedItemData = await _itemsManager.WaitForItemCollection();
            OnItemCollected(collectedItemData);
        }

        await _itemsManager.DestroyRemainingItems();
    }

    private void OnItemCollected(ItemData collectedItemData)
    {
        _collectedItemsCount++;
        UIController.Instance.UpdateCollectedItems(_collectedItemsCount);
        // character controller - update item slot: Tal
    }
}