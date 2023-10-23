using Cysharp.Threading.Tasks;

public class TrialManagerGrounding
{
    int _collectedItemsCount;

    public async UniTask RunTrialFlow(EItemCategory itemCategory)
    {
        _collectedItemsCount = 0;
        int requiredItemsCount = ItemsManager.Instance.GetRequiredItemsCount(itemCategory);

        UIController.Instance.ResetGroundingTrialUI(itemCategory, requiredItemsCount);
        await ItemsManager.Instance.SpawnItems(itemCategory);

        ItemsManager.Instance.ItemCollected += OnItemCollected;

        while (_collectedItemsCount < requiredItemsCount)
        {
            await UniTask.Yield();
        }

        ItemsManager.Instance.ItemCollected -= OnItemCollected;

        await ItemsManager.Instance.DestroyRemainingItems();
    }

    private void OnItemCollected()
    {
        _collectedItemsCount++;
        UIController.Instance.UpdateCollectedItems(_collectedItemsCount);
    }
}
