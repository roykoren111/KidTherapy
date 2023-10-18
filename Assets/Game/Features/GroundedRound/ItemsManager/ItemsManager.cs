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
    [SerializeField] private LayerMask _selectableLayerMask;

    [SerializeField] private List<ItemData> _itemsData;

    [Space(10)] [SerializeField] private List<EItemCategory> _itemCategories;
    [SerializeField] private GameObject _itemLocationsParent;

    private EItemCategory _currentItemCategory;

    private readonly Dictionary<GameObject, ItemData> _itemsOnScreen = new Dictionary<GameObject, ItemData>();


    private void Awake()
    {
        DependencyManager.SetDependency(this);
    }

    //TODO: remove when integrating with full project, start round in relevant place in flow.
    private void Start()
    {
        RunGroundedRound().Forget();
    }

    private async UniTask RunGroundedRound()
    {
        // gameStateController.SetGameState(GameState.Grounding);

        while (_itemCategories.Count > 0)
        {
            await RunNextTrial();
        }

        Debug.Log("Finished Grounded round");
    }

    private async UniTask RunNextTrial()
    {
        RemoveItemsFromScreen();

        _currentItemCategory = GetNextItemCategory();
        InstantiateItems();

        await UniTask.WaitUntil(FinishedSelectingItems);

        Debug.Log("Finished " + _currentItemCategory + " category");
    }

    private bool FinishedSelectingItems()
    {
        int numberOfItemsSelected = _currentItemCategory.GetNumberOfItemsToAppear() - _itemsOnScreen.Count;
        bool finishedSelectingItems = numberOfItemsSelected >= _currentItemCategory.GetNumberOfItemsToSelect();
        return finishedSelectingItems;
    }

    private void RemoveItemsFromScreen()
    {
        foreach (GameObject item in _itemsOnScreen.Keys)
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
        List<ItemData> itemsToSelectFrom = GetItemsDataByCategory(_currentItemCategory);
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


    private List<ItemData> GetItemsDataByCategory(EItemCategory itemCategory)
    {
        List<ItemData> itemsData = new List<ItemData>();
        foreach (ItemData itemData in _itemsData)
        {
            if (itemData.Categorey == itemCategory)
            {
                itemsData.Add(itemData);
            }
        }

        return itemsData;
    }

    private void InstantiateItem(List<ItemData> itemsToSelectFrom, List<Transform> possibleItemLocations)
    {
        int itemIndex = Random.Range(0, itemsToSelectFrom.Count);
        ItemData selectedItemData = itemsToSelectFrom[itemIndex];

        int itemLocationIndex = Random.Range(0, possibleItemLocations.Count);
        GameObject selectedItem = Instantiate(selectedItemData.Prefab, possibleItemLocations[itemLocationIndex]);

        _itemsOnScreen.Add(selectedItem, selectedItemData);

        itemsToSelectFrom.RemoveAt(itemIndex);
        possibleItemLocations.RemoveAt(itemLocationIndex);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (!Physics.Raycast(ray, out RaycastHit raycastHit, 100f, _selectableLayerMask)) return;
            SelectItem(raycastHit.transform.gameObject);
        }
    }

    private void SelectItem(GameObject item)
    {
        DependencyManager.GetDependency(out CharacterSlots characterSlots);
        characterSlots.AddItemToRandomSlot(_itemsOnScreen[item]);
        _itemsOnScreen.Remove(item);
        Destroy(item);
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