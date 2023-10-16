using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Serialization;
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
    private EItemCategory _currentItemCategory;
    private int _numberOfItemsToSelect;

    private List<GameObject> _itemsOnScreen = new List<GameObject>();

    private void Awake()
    {
        InitializeItemsByCategoryDictionary();
    }

    private void Start()
    {
        RunItemsRound().Forget();
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
        _numberOfItemsToSelect = _currentItemCategory.GetNumberOfItemsInCategory();
        InstantiateItems();
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
        int numberOfItemsSelected = 0;
        List<GameObject> itemsToSelectFrom = _itemsPrefabsByCategory[_currentItemCategory];
        while (numberOfItemsSelected < _numberOfItemsToSelect)
        {
            int itemIndex = Random.Range(0, itemsToSelectFrom.Count);
            GameObject selectedItem = Instantiate(itemsToSelectFrom[itemIndex]);
            itemsToSelectFrom.RemoveAt(itemIndex);
            _itemsOnScreen.Add(selectedItem);
            numberOfItemsSelected++;
        }
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