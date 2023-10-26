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
            { EItemCategory.See, 8 }, { EItemCategory.Hear, 8 }, { EItemCategory.Smell, 8 }, { EItemCategory.Taste, 8 },
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


    public async UniTask SpawnItems(EItemCategory itemCategory)
    {
        Dictionary<Transform, Transform> possibleItemLocations = GetPossibleItemLocations();
        int numberOfItemsToSpawn = _numberOfItemsToSpawnByCategory[itemCategory];
        List<GameObject> itemsToSelectFrom = _itemPrefabsByCategory[itemCategory];
        while (numberOfItemsToSpawn > 0)
        {
            SpawnItem(itemsToSelectFrom, possibleItemLocations);
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

    private void SpawnItem(List<GameObject> itemsToSelectFrom, Dictionary<Transform, Transform> possibleItemLocations)
    {
        int itemIndex = Random.Range(0, itemsToSelectFrom.Count);
        int itemLocationIndex = Random.Range(0, possibleItemLocations.Count);
        Transform itemOutsideLocation = possibleItemLocations.ElementAt(itemLocationIndex).Key;
        GameObject selectedItem =
            Instantiate(itemsToSelectFrom[itemIndex], itemOutsideLocation);
        selectedItem.GetComponent<Item>()?.MoveToInnerScreenPosition(possibleItemLocations[itemOutsideLocation]);

        _itemsOnScreen.Add(selectedItem);

        itemsToSelectFrom.RemoveAt(itemIndex);
        possibleItemLocations.Remove(itemOutsideLocation);
    }

    public int GetRequiredItemsCount(EItemCategory itemCategory)
    {
        return _numberOfItemsToSelectByCategory[itemCategory];
    }

    public Action<Transform> ItemCollected; // invoke when Item is collected

    public async UniTask DestroyRemainingItems()
    {
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

    public void CollectItem(GameObject collectedItemGameObject)
    {
        Item collectedItem = collectedItemGameObject.GetComponent<Item>();
        if (collectedItem.IsWrongPick)
        {
            GroundingAudioManager.Instance.PlayWrongPickSound(collectedItem.Categorey);
            // TODO: call OnWrongPick on the item script.
            return;
        }

        GroundingAudioManager.Instance.PlayRandomCorrectPickSound(collectedItem.Categorey);
        collectedItem.GetComponent<Item>().IsCollected = true;

        _itemsOnScreen.Remove(collectedItemGameObject);
        ItemCollected?.Invoke(collectedItemGameObject.transform);
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
        AddItemsDataInCategory("See", _itemPrefabsByCategory[categoryByName["See"]]);
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
#endif
}