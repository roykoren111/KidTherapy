using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

public class ItemsManager : MonoBehaviour
{
    [Header("Item prefabs")] [SerializeField]
    private List<GameObject> _seeItemsPrefabs;

    [SerializeField] private List<GameObject> _hearItemsPrefabs;
    [SerializeField] private List<GameObject> _smellItemsPrefabs;
    [SerializeField] private List<GameObject> _tasteItemsPrefabs;
    [SerializeField] private List<GameObject> _touchItemsPrefabs;

    private Dictionary<EItemCategory, List<GameObject>> _itemsPrefabsByCategory;

    [Space(10)] [SerializeField] private List<EItemCategory> _itemCategories;
    [SerializeField] private GameObject _itemLocationsParent;

    private EItemCategory _currentItemCategory;

    private List<GameObject> _itemsOnScreen = new List<GameObject>();

    private void Awake()
    {
        InitializeItemsByCategoryDictionary();
    }


    private void InitializeItemsByCategoryDictionary()
    {
        _itemsPrefabsByCategory = new Dictionary<EItemCategory, List<GameObject>>()
        {
            { EItemCategory.See, _seeItemsPrefabs }, { EItemCategory.Hear, _hearItemsPrefabs },
            { EItemCategory.Smell, _smellItemsPrefabs },
            { EItemCategory.Taste, _tasteItemsPrefabs }, { EItemCategory.Touch, _touchItemsPrefabs }
        };
    }

    //TODO: remove when integrating with full project, start round in relevant place in flow.
    private void Start()
    {
        RunItemsRound().Forget();
    }

    private async UniTask RunItemsRound()
    {
        // while (_itemCategories.Count > 0)
        // {
        await RunNextTrial();
        // }
    }

    private async UniTask RunNextTrial()
    {
        _currentItemCategory = GetNextItemCategory();
        InstantiateItems();
        bool finishedSelectingItems = _currentItemCategory.GetNumberOfItemsToAppear() - _itemsOnScreen.Count >=
                                      _currentItemCategory.GetNumberOfItemsToSelect();
        await UniTask.WaitUntil(() => finishedSelectingItems);
        //TODO: whenever the player clicks on an item, remove it from the items on screen list.
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
        List<GameObject> itemsToSelectFrom = _itemsPrefabsByCategory[_currentItemCategory];
        while (numberOfItemsToAppear > 0)
        {
            int itemIndex = Random.Range(0, itemsToSelectFrom.Count);
            int itemLocationIndex = Random.Range(0, possibleItemLocations.Count);
            GameObject selectedItem =
                Instantiate(itemsToSelectFrom[itemIndex], possibleItemLocations[itemLocationIndex]);
            _itemsOnScreen.Add(selectedItem);

            itemsToSelectFrom.RemoveAt(itemIndex);
            possibleItemLocations.RemoveAt(itemLocationIndex);
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

#if UNITY_EDITOR
    public void SkipToNextTrial()
    {
        foreach (GameObject item in _itemsOnScreen)
        {
            Destroy(item);
        }

        _itemsOnScreen.Clear();
        RunNextTrial().Forget();
    }
#endif
}