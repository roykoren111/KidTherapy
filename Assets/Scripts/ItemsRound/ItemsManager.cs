using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class ItemsManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> _seeItems;
    [SerializeField] private List<GameObject> _hearItems;
    [SerializeField] private List<GameObject> _smellItems;
    [SerializeField] private List<GameObject> _tasteItems;
    [SerializeField] private List<GameObject> _touchItems;

    private Dictionary<EItemCategory, List<GameObject>> _itemsByCategory;

    private List<EItemCategory> _itemCategories;
    private int _numberOfItemsToSelect;

    private List<GameObject> _items;

    private void Awake()
    {
        InitializeItemsByCategoryDictionary();
    }

    private void InitializeItemsByCategoryDictionary()
    {
        _itemsByCategory = new Dictionary<EItemCategory, List<GameObject>>()
        {
            { EItemCategory.See, _seeItems }, { EItemCategory.Hear, _hearItems }, { EItemCategory.Smell, _smellItems },
            { EItemCategory.Taste, _tasteItems }, { EItemCategory.Touch, _touchItems }
        };
    }

    private async UniTask RunItemsRound()
    {
        while (_itemCategories.Count > 0)
        {
            EItemCategory itemCategory = GetNextItemCategory();
            _items = _itemsByCategory[itemCategory];
            _numberOfItemsToSelect = itemCategory.GetNumberOfItemsInCategory();
        }
    }

    private EItemCategory GetNextItemCategory()
    {
        int itemCategoryIndex = Random.Range(0, _itemCategories.Count);
        EItemCategory itemCategory = _itemCategories[itemCategoryIndex];
        _itemCategories.Remove(itemCategory);
        return itemCategory;
    }
}