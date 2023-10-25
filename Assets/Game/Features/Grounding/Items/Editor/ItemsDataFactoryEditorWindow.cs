using System.Collections;
using System;
using System.Collections.Generic;
using System.IO;
using Game.Common.Scripts.Data;
using UnityEditor;
using UnityEngine;
using Object = System.Object;

public class ItemsDataFactoryEditorWindow : EditorWindow
{
    private string _itemPrefabsFolderName;
    private string _itemPrefabsFolderPath => FolderPaths.ItemsFolderPath + _itemPrefabsFolderName;
    private bool _showFolderDoesNotExistLabel = true;

    private Dictionary<string, EItemCategory> _categoryByFolderName = new Dictionary<string, EItemCategory>()
    {
        { "Hear", EItemCategory.Hear }, { "See", EItemCategory.See }, { "Taste", EItemCategory.Taste },
        { "Smell", EItemCategory.Smell },
        { "Touch", EItemCategory.Touch }
    };

    [MenuItem("Tools/Items Data Factory")]
    public static void ShowWindow()
    {
        GetWindow(typeof(ItemsDataFactoryEditorWindow));
    }

    private void OnGUI()
    {
        GUILayout.Label("Item prefabs folder name:");
        _itemPrefabsFolderName = GUILayout.TextField(_itemPrefabsFolderName);
        GUILayout.Space(10);

        GUILayout.Label("Warning, creating items data will override existing.");
        if (GUILayout.Button("Create items data"))
        {
            if (FolderExists())
            {
                DeleteExistingItemsData();
                CreateItemsData();
            }
            else
            {
                _showFolderDoesNotExistLabel = true;
            }
        }

        if (_showFolderDoesNotExistLabel)
        {
            _showFolderDoesNotExistLabel = !FolderExists();
            GUI.color = Color.red;
            GUILayout.Label("Folder does not exist.");
        }
    }

    private bool FolderExists()
    {
        return Directory.Exists(_itemPrefabsFolderPath);
    }

    private void DeleteExistingItemsData()
    {
        string itemsDataDirectoryPath = _itemPrefabsFolderPath + "/ItemsData/";
        string[] prefabGuids = AssetDatabase.FindAssets("t:asset", new[] { itemsDataDirectoryPath });
        foreach (string prefabGuid in prefabGuids)
        {
            string path = AssetDatabase.GUIDToAssetPath(prefabGuid);
            AssetDatabase.DeleteAsset(path);
        }
    }

    private void CreateItemsData()
    {
        EItemCategory itemsCategory = _categoryByFolderName[_itemPrefabsFolderName];

        string[] prefabGuids = AssetDatabase.FindAssets("t:prefab", new[] { _itemPrefabsFolderPath });
        foreach (string prefabGuid in prefabGuids)
        {
            string path = AssetDatabase.GUIDToAssetPath(prefabGuid);
            GameObject itemPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            CreateItemDataFromPrefab(itemPrefab, itemsCategory);
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    private void CreateItemDataFromPrefab(GameObject itemPrefab, EItemCategory itemCategory)
    {
        ItemData itemData = (ItemData)CreateInstance(typeof(ItemData));
        itemData.Categorey = itemCategory;
        itemData.Name = itemPrefab.name;
        itemData.Prefab = itemPrefab;

        string itemsDataDirectoryPath = _itemPrefabsFolderPath + "/ItemsData/";
        Directory.CreateDirectory(itemsDataDirectoryPath);
        string itemDataPath = itemsDataDirectoryPath + itemData.Name + ".asset";
        AssetDatabase.CreateAsset(itemData, itemDataPath);
    }
}