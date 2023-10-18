using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Cysharp.Threading.Tasks;
using Game.Common.Scripts.Data;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class ItemsManager : MonoBehaviour
{
    [Header("Item prefabs")] [SerializeField]
    private List<ItemData> _seeItemsData;

    [SerializeField] private List<ItemData> _hearItemsData;
    [SerializeField] private List<ItemData> _smellItemsData;
    [SerializeField] private List<ItemData> _tasteItemsData;
    [SerializeField] private List<ItemData> _touchItemsData;

    private Dictionary<EItemCategory, List<ItemData>> _itemsDataByCategory;

    [Space(10)] [SerializeField] private List<EItemCategory> _itemCategories;
    [SerializeField] private GameObject _itemLocationsParent;

    private EItemCategory _currentItemCategory;

    private readonly List<GameObject> _itemsOnScreen = new List<GameObject>();

    private void Awake()
    {
        DependencyManager.SetDependency(this);
        InitializeItemsByCategoryDictionary();
    }


    private void InitializeItemsByCategoryDictionary()
    {
        _itemsDataByCategory = new Dictionary<EItemCategory, List<ItemData>>()
        {
            { EItemCategory.See, _seeItemsData }, { EItemCategory.Hear, _hearItemsData },
            { EItemCategory.Smell, _smellItemsData },
            { EItemCategory.Taste, _tasteItemsData }, { EItemCategory.Touch, _touchItemsData }
        };
    }

    //TODO: remove when integrating with full project, start round in relevant place in flow.
    private void Start()
    {
        RunItemsRound().Forget();
    }

    private async UniTask RunItemsRound()
    {
        // gameStateController.SetGameState(GameState.Grounding);

        // while (_itemCategories.Count > 0)
        // {
        await RunNextTrial();
        // }
    }

    private async UniTask RunNextTrial()
    {
        RemoveItemsFromScreen();

        _currentItemCategory = GetNextItemCategory();
        InstantiateItems();

        //Whenever the player clicks on an item, we remove it from the items on screen list.
        bool finishedSelectingItems = _currentItemCategory.GetNumberOfItemsToAppear() - _itemsOnScreen.Count >=
                                      _currentItemCategory.GetNumberOfItemsToSelect();
        await UniTask.WaitUntil(() => finishedSelectingItems);
    }

    private void RemoveItemsFromScreen()
    {
        foreach (GameObject item in _itemsOnScreen)
        {
            Destroy(item);
        }

        _itemsOnScreen.Clear();
    }

    private EItemCategory GetNextItemCategory()
    {
        int itemCategoryIndex = Random.Range(0, _itemCategories.Count);
        EItemCategory itemCategory = _itemCategories[itemCategoryIndex];
        _itemCategories.Remove(itemCategory);
        return itemCategory;
    }

    private void InstantiateItems()
    {
        List<Transform> possibleItemLocations = GetPossibleItemLocations();
        int numberOfItemsToAppear = _currentItemCategory.GetNumberOfItemsToAppear();
        List<ItemData> itemsToSelectFrom = _itemsDataByCategory[_currentItemCategory];
        while (numberOfItemsToAppear > 0)
        {
            InstantiateItem(itemsToSelectFrom, possibleItemLocations);
            numberOfItemsToAppear--;
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

    private void InstantiateItem(List<ItemData> itemsToSelectFrom, List<Transform> possibleItemLocations)
    {
        int itemIndex = Random.Range(0, itemsToSelectFrom.Count);
        int itemLocationIndex = Random.Range(0, possibleItemLocations.Count);

        GameObject selectedItem =
            Instantiate(itemsToSelectFrom[itemIndex].ItemPrefab, possibleItemLocations[itemLocationIndex]);
        _itemsOnScreen.Add(selectedItem);

        itemsToSelectFrom.RemoveAt(itemIndex);
        possibleItemLocations.RemoveAt(itemLocationIndex);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (!Physics.Raycast(ray, out RaycastHit raycastHit, 100f)) return;
            if (raycastHit.transform.CompareTag("Item"))
            {
                SelectItem(raycastHit.transform.gameObject);
            }
        }
    }

    public void SelectItem(GameObject item)
    {
        _itemsOnScreen.Remove(item);
        DependencyManager.GetDependency(out CharacterSlots characterSlots);
        characterSlots.AddItemToRandomSlot(item);
    }

#if UNITY_EDITOR
    public void SkipToNextTrial()
    {
        RunNextTrial().Forget();
    }

    public void PopulateItemsDataLists()
    {
        PopulateItemsDataList(_seeItemsData, "See");
        PopulateItemsDataList(_tasteItemsData, "Taste");
        PopulateItemsDataList(_hearItemsData, "Hear");
        PopulateItemsDataList(_smellItemsData, "Smell");
        PopulateItemsDataList(_touchItemsData, "Touch");
        EditorUtility.SetDirty(this);
    }

    private void PopulateItemsDataList(List<ItemData> itemsData, string itemsFolderName)
    {
        itemsData.Clear();
        string[] files = Directory.GetFiles(FolderPaths.ItemsDataPath + "/" + itemsFolderName, "*.asset");
        foreach (string file in files)
        {
            itemsData.Add(AssetDatabase.LoadAssetAtPath(file, typeof(ItemData)) as ItemData);
        }
    }
#endif
}