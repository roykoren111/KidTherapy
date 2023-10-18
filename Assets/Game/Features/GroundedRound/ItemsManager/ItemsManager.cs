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
    [SerializeField] private List<ItemData> _itemsData;

    [Space(10)] [SerializeField] private List<EItemCategory> _itemCategories;
    [SerializeField] private GameObject _itemLocationsParent;

    private EItemCategory _currentItemCategory;

    private readonly List<GameObject> _itemsOnScreen = new List<GameObject>();

    private void Awake()
    {
        DependencyManager.SetDependency(this);
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

    //TODO: remove when integrating with full project, start round in relevant place in flow.
    private void Start()
    {
        RunItemsRound().Forget();
    }

    private async UniTask RunItemsRound()
    {
        // gameStateController.SetGameState(GameState.Grounding);

        while (_itemCategories.Count > 0)
        {
            await RunNextTrial();
        }
    }

    private async UniTask RunNextTrial()
    {
        RemoveItemsFromScreen();

        _currentItemCategory = GetNextItemCategory();
        InstantiateItems();

        await UniTask.WaitUntil(() =>
        {
            int numberOfItemsSelected = _currentItemCategory.GetNumberOfItemsToAppear() - _itemsOnScreen.Count;
            bool finishedSelectingItems = numberOfItemsSelected >= _currentItemCategory.GetNumberOfItemsToSelect();
            return finishedSelectingItems;
        });

        Debug.Log("finished trial");
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

    private void InstantiateItem(List<ItemData> itemsToSelectFrom, List<Transform> possibleItemLocations)
    {
        int itemIndex = Random.Range(0, itemsToSelectFrom.Count);
        int itemLocationIndex = Random.Range(0, possibleItemLocations.Count);

        GameObject selectedItem =
            Instantiate(itemsToSelectFrom[itemIndex].Prefab, possibleItemLocations[itemLocationIndex]);
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

    //TODO: refactor
    public void PopulateItemsDataList()
    {
        _itemsData.Clear();
        PopulateItemsData("See");
        PopulateItemsData("Taste");
        PopulateItemsData("Hear");
        PopulateItemsData("Smell");
        PopulateItemsData("Touch");
        EditorUtility.SetDirty(this);
    }

    private void PopulateItemsData(string itemsFolderName)
    {
        string[] files = Directory.GetFiles(FolderPaths.ItemsDataPath + "/" + itemsFolderName, "*.asset");
        foreach (string file in files)
        {
            _itemsData.Add(AssetDatabase.LoadAssetAtPath(file, typeof(ItemData)) as ItemData);
        }
    }
#endif
}