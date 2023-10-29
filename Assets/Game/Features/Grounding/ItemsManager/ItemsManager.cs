using System;
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
    [SerializeField] private List<GameObject> _seeItemsPrefabs;
    [SerializeField] private List<GameObject> _hearItemsPrefabs;
    [SerializeField] private List<GameObject> _tasteItemsPrefabs;

    [SerializeField] private GameObject _itemLocationsParent;

    private EItemCategory _currentItemCategory;

    private readonly List<GameObject> _itemsOnScreen = new List<GameObject>();

    public static ItemsManager Instance;

    private static Dictionary<EItemCategory, List<GameObject>> _itemPrefabsByCategory;

    private static readonly Dictionary<EItemCategory, int> _numberOfItemsToSelectByCategory =
        new Dictionary<EItemCategory, int>
        {
            { EItemCategory.See, 5 }, { EItemCategory.Hear, 4 }, { EItemCategory.Smell, 2 }, { EItemCategory.Taste, 3 },
            { EItemCategory.Touch, 1 }
        };

    private static readonly Dictionary<EItemCategory, int> _numberOfItemsToSpawnByCategory =
        new Dictionary<EItemCategory, int>
        {
            { EItemCategory.See, 14 }, { EItemCategory.Hear, 12 }, { EItemCategory.Smell, 8 },
            { EItemCategory.Taste, 12 },
            { EItemCategory.Touch, 8 }
        };

    private void Start()
    {
        InitializeItemPrefabsByCategoryDictionary();
    }

    private void InitializeItemPrefabsByCategoryDictionary()
    {
        _itemPrefabsByCategory = new Dictionary<EItemCategory, List<GameObject>>
        {
            { EItemCategory.See, _seeItemsPrefabs }, { EItemCategory.Hear, _hearItemsPrefabs },
            { EItemCategory.Taste, _tasteItemsPrefabs },
        };
    }


    public async UniTask SpawnItems(EItemCategory itemCategory, Vector2 betweenSpawnsDelay,
        Vector2 itemFloatingSpeedRange, EItemColor itemsColor)
    {
        Dictionary<Transform, Transform> possibleItemLocations = GetPossibleItemLocations();
        int numberOfItemsToSpawn = _numberOfItemsToSpawnByCategory[itemCategory];
        int numberOfItemsToSelect = _numberOfItemsToSelectByCategory[itemCategory];
        List<GameObject> itemsInColor = GetItemPrefabsByColor(itemsColor);
        List<GameObject> itemsNotInColor = _itemPrefabsByCategory[itemCategory];
        while (numberOfItemsToSpawn > 0)
        {
            if (numberOfItemsToSelect > 0)
            {
                SpawnItem(itemsInColor, possibleItemLocations, itemFloatingSpeedRange);
                numberOfItemsToSelect--;
            }
            else
            {
                SpawnItem(itemsNotInColor, possibleItemLocations, itemFloatingSpeedRange);
            }

            float spawnDelay = Random.Range(betweenSpawnsDelay.x, betweenSpawnsDelay.y);
            await UniTask.Delay(TimeSpan.FromSeconds(spawnDelay));
            numberOfItemsToSpawn--;
        }
    }

    private Dictionary<Transform, Transform> GetPossibleItemLocations()
    {
        //Key is outer position, value is inner position
        Dictionary<Transform, Transform> possibleItemLocations = new Dictionary<Transform, Transform>();
        foreach (Transform itemLocation in _itemLocationsParent.transform)
        {
            possibleItemLocations.Add(itemLocation.GetChild(1), itemLocation.GetChild(0));
        }

        return possibleItemLocations;
    }

    private void SpawnItem(List<GameObject> itemsToSelectFrom, Dictionary<Transform, Transform> possibleItemLocations,
        Vector2 floatingSpeedRange)
    {
        int itemIndex = Random.Range(0, itemsToSelectFrom.Count);
        int itemLocationIndex = Random.Range(0, possibleItemLocations.Count);
        Transform itemOutsideLocation = possibleItemLocations.ElementAt(itemLocationIndex).Key;
        Transform innerPosition = possibleItemLocations[itemOutsideLocation];
        float itemFloatDuration = Random.Range(floatingSpeedRange.x, floatingSpeedRange.y);
        GameObject spawnedItem =
            Instantiate(itemsToSelectFrom[itemIndex], itemOutsideLocation);
        spawnedItem.GetComponent<Item>()?.Spawn(itemOutsideLocation, innerPosition, itemFloatDuration);

        _itemsOnScreen.Add(spawnedItem);

        itemsToSelectFrom.RemoveAt(itemIndex);
        possibleItemLocations.Remove(itemOutsideLocation);
    }

    public int GetRequiredItemsCount(EItemCategory itemCategory)
    {
        return _numberOfItemsToSelectByCategory[itemCategory];
    }

    public Action<Item> ItemCollected; // invoke when Item is collected

    public async UniTask DestroyRemainingItems()
    {
        List<UniTask> returningItemsTasks = new List<UniTask>();
        foreach (GameObject item in _itemsOnScreen)
        {
            UniTask returnItem = item.GetComponent<Item>().ReturnToSpawnPosition();
            returningItemsTasks.Add(returnItem);
        }

        await UniTask.WhenAll(returningItemsTasks);

        foreach (GameObject item in _itemsOnScreen)
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

    public void CollectItem(Item collectedItem)
    {
        _itemsOnScreen.Remove(collectedItem.gameObject);
        ItemCollected?.Invoke(collectedItem);
    }

    public void OnItemWrongPick(Item item)
    {
        _itemsOnScreen.Remove(item.gameObject);
    }

    private List<GameObject> GetItemPrefabsByColor(EItemColor color)
    {
        List<GameObject> itemsInColor = new List<GameObject>();
        foreach (GameObject item in _seeItemsPrefabs)
        {
            if (item.GetComponent<Item>().itemColor == color)
            {
                itemsInColor.Add(item);
                _seeItemsPrefabs.Remove(item);
            }
        }

        return itemsInColor;
    }

#if UNITY_EDITOR
    public void PopulateItemsDataList()
    {
        InitializeItemPrefabsByCategoryDictionary();
        Dictionary<string, EItemCategory> categoryByName = new Dictionary<string, EItemCategory>()
        {
            { "See", EItemCategory.See }, { "Hear", EItemCategory.Hear },
            { "Taste", EItemCategory.Taste },
        };
        AddSeeItemsData();
        AddItemsDataInCategory("Hear", _itemPrefabsByCategory[categoryByName["Hear"]]);
        AddItemsDataInCategory("Taste", _itemPrefabsByCategory[categoryByName["Taste"]]);
        EditorUtility.SetDirty(this);
    }

    private void AddItemsDataInCategory(string categoryFolderName, List<GameObject> itemPrefabsList)
    {
        itemPrefabsList.Clear();
        string[] files =
            Directory.GetFiles(FolderPaths.CategoriesFolderPath + categoryFolderName, "*.prefab");
        foreach (string file in files)
        {
            itemPrefabsList.Add(AssetDatabase.LoadAssetAtPath<GameObject>(file));
        }
    }

    private void AddSeeItemsData()
    {
        string[] files =
            Directory.GetFiles(FolderPaths.CategoriesFolderPath + "See/Blue", "*.prefab");
        LoadFilesToList(files, _seeItemsPrefabs);
        files = Directory.GetFiles(FolderPaths.CategoriesFolderPath + "See/Red", "*.prefab");
        LoadFilesToList(files, _seeItemsPrefabs);
        files = Directory.GetFiles(FolderPaths.CategoriesFolderPath + "See/Green", "*.prefab");
        LoadFilesToList(files, _seeItemsPrefabs);
        files = Directory.GetFiles(FolderPaths.CategoriesFolderPath + "See/Purple", "*.prefab");
        LoadFilesToList(files, _seeItemsPrefabs);
        files = Directory.GetFiles(FolderPaths.CategoriesFolderPath + "See/Orange", "*.prefab");
        LoadFilesToList(files, _seeItemsPrefabs);
    }

    private void LoadFilesToList(string[] files, List<GameObject> itemPrefabs)
    {
        foreach (string file in files)
        {
            itemPrefabs.Add(AssetDatabase.LoadAssetAtPath<GameObject>(file));
        }
    }
#endif
}