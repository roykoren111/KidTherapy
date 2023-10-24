using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Cysharp.Threading.Tasks;
using Game.Common.Scripts.Data;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class ItemsManager : MonoBehaviour
{
    [SerializeField] private LayerMask _selectableLayerMask;

    [SerializeField] private List<ItemData> _itemsData;

    [Space(10)] [SerializeField] private List<EItemCategory> _itemCategories;
    [SerializeField] private GameObject _itemLocationsParent;

    private EItemCategory _currentItemCategory;
    public ItemData _collectedItem;

    private readonly Dictionary<GameObject, ItemData> _itemsOnScreen = new Dictionary<GameObject, ItemData>();

    public static ItemsManager Instance;

    private static readonly Dictionary<EItemCategory, int> _numberOfItemsToSelectByCategory =
        new Dictionary<EItemCategory, int>
        {
            { EItemCategory.See, 5 }, { EItemCategory.Hear, 4 }, { EItemCategory.Smell, 3 }, { EItemCategory.Taste, 2 },
            { EItemCategory.Touch, 1 }
        };

    private static readonly Dictionary<EItemCategory, int> _numberOfItemsToSpawnByCategory =
        new Dictionary<EItemCategory, int>
        {
            { EItemCategory.See, 8 }, { EItemCategory.Hear, 8 }, { EItemCategory.Smell, 8 }, { EItemCategory.Taste, 8 },
            { EItemCategory.Touch, 8 }
        };

    public async UniTask SpawnItems(EItemCategory itemCategory)
    {
        List<Transform> possibleItemLocations = GetPossibleItemLocations();
        int numberOfItemsToSpawn = _numberOfItemsToSpawnByCategory[itemCategory];
        List<ItemData> itemsToSelectFrom = GetItemsDataByCategory(itemCategory);
        while (numberOfItemsToSpawn > 0)
        {
            SpawnItem(itemsToSelectFrom, possibleItemLocations);
            numberOfItemsToSpawn--;
        }
    }

    private List<Transform> GetPossibleItemLocations()
    {
        List<Transform> possibleItemLocations = new List<Transform>();
        foreach (Transform possibleItemLocation in _itemLocationsParent.transform)
        {
            possibleItemLocations.Add(possibleItemLocation);
        }

        return possibleItemLocations;
    }

    private List<ItemData> GetItemsDataByCategory(EItemCategory itemCategory)
    {
        return _itemsData.Where(itemData => itemData.Categorey == itemCategory).ToList();
    }

    private void SpawnItem(List<ItemData> itemsToSelectFrom, List<Transform> possibleItemLocations)
    {
        int itemIndex = Random.Range(0, itemsToSelectFrom.Count);
        ItemData selectedItemData = itemsToSelectFrom[itemIndex];

        int itemLocationIndex = Random.Range(0, possibleItemLocations.Count);
        GameObject selectedItem = Instantiate(selectedItemData.Prefab, possibleItemLocations[itemLocationIndex]);

        _itemsOnScreen.Add(selectedItem, selectedItemData);

        itemsToSelectFrom.RemoveAt(itemIndex);
        possibleItemLocations.RemoveAt(itemLocationIndex);
    }

    public int GetRequiredItemsCount(EItemCategory itemCategory)
    {
        return _numberOfItemsToSelectByCategory[itemCategory];
    }

    public Action ItemCollected; // invoke when Item is collected

    public async UniTask DestroyRemainingItems()
    {
        foreach (GameObject item in _itemsOnScreen.Keys)
        {
            Destroy(item);
        }

        _itemsOnScreen.Clear();
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

    //TODO: remove when integrating with full project, start round in relevant place in flow.
    private void Start()
    {
        // RoundManagerGrounding roundManagerGrounding = gameObject.AddComponent<RoundManagerGrounding>();
        // roundManagerGrounding.RunRoundFlow();
        RunGroundedRound().Forget();
    }

    //TODO: remove when integrating with full project.
    private async UniTask RunGroundedRound()
    {
        // gameStateController.SetGameState(GameState.Grounding);

        while (_itemCategories.Count > 0)
        {
            await RunNextTrial();
        }

        Debug.Log("Finished Grounded round");
    }

    //TODO: remove when integrating with full project.
    private async UniTask RunNextTrial()
    {
        DestroyRemainingItems().Forget();

        _currentItemCategory = GetNextItemCategory();
        SpawnItems(_currentItemCategory);

        // await UniTask.WaitUntil(FinishedSelectingItems);

        Debug.Log("Finished " + _currentItemCategory + " category");
    }

    public async UniTask<ItemData> WaitForItemCollection()
    {
        while (true)
        {
            if (!Input.GetMouseButtonDown(0)) continue;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit raycastHit, 100f, _selectableLayerMask))
            {
                GameObject collectedItem = raycastHit.transform.gameObject;
                ItemData collectedItemData = _itemsOnScreen[collectedItem];

                _itemsOnScreen.Remove(collectedItem);
                Destroy(collectedItem);

                return _itemsOnScreen[raycastHit.transform.gameObject];
            }

            await UniTask.Yield();
        }
    }

    private void SelectItem(GameObject item)
    {
        DependencyManager.GetDependency(out CharacterSlots characterSlots);
        characterSlots.AddItemToRandomSlot(_itemsOnScreen[item]);
        Destroy(item);
    }

    private EItemCategory GetNextItemCategory()
    {
        int itemCategoryIndex = Random.Range(0, _itemCategories.Count);
        EItemCategory itemCategory = _itemCategories[itemCategoryIndex];
        _itemCategories.Remove(itemCategory);
        return itemCategory;
    }


#if UNITY_EDITOR
    public void SkipToNextTrial()
    {
        RunNextTrial().Forget();
    }

    public void PopulateItemsDataList()
    {
        _itemsData.Clear();
        AddItemsDataInCategory("See");
        AddItemsDataInCategory("Taste");
        AddItemsDataInCategory("Hear");
        AddItemsDataInCategory("Smell");
        AddItemsDataInCategory("Touch");
        EditorUtility.SetDirty(this);
    }

    private void AddItemsDataInCategory(string categoryFolderName)
    {
        string[] files = Directory.GetFiles(FolderPaths.ItemsDataPath + "/" + categoryFolderName, "*.asset");
        foreach (string file in files)
        {
            _itemsData.Add(AssetDatabase.LoadAssetAtPath(file, typeof(ItemData)) as ItemData);
        }
    }
#endif
}