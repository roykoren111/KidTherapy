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
    [SerializeField] private List<ItemData> _itemsData;

    [SerializeField] private GameObject _itemLocationsParent;

    private EItemCategory _currentItemCategory;

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
        Debug.Log(itemsToSelectFrom[0].Categorey);
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
        //selectedItem.GetComponent<Item>()?.MoveToInnerScreenPosition(); -> TODO: add inner location

        _itemsOnScreen.Add(selectedItem, selectedItemData);

        itemsToSelectFrom.RemoveAt(itemIndex);
        possibleItemLocations.RemoveAt(itemLocationIndex);
    }

    public int GetRequiredItemsCount(EItemCategory itemCategory)
    {
        return _numberOfItemsToSelectByCategory[itemCategory];
    }

    public Action<Transform> ItemCollected; // invoke when Item is collected

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

    public void CollectItem(GameObject collectedItem)
    {
        ItemData collectedItemData = _itemsOnScreen[collectedItem];
        if (collectedItemData.IsWrongPick)
        {
            GroundingAudioManager.Instance.PlayWrongPickSound(collectedItemData.Categorey);
            // TODO: call OnWrongPick on the item script.
            return;
        }

        GroundingAudioManager.Instance.PlayRandomCorrectPickSound(collectedItemData.Categorey);
        collectedItem.GetComponent<Item>().IsCollected = true;
        _itemsOnScreen.Remove(collectedItem);
        Destroy(collectedItem);

        ItemCollected?.Invoke(collectedItem.transform);
    }

#if UNITY_EDITOR
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
        string[] files =
            Directory.GetFiles(FolderPaths.CategoriesFolderPath + categoryFolderName + "/ItemsData", "*.asset");
        foreach (string file in files)
        {
            _itemsData.Add(AssetDatabase.LoadAssetAtPath(file, typeof(ItemData)) as ItemData);
        }
    }
#endif
}