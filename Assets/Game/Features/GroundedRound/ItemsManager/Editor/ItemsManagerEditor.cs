using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ItemsManager))]
public class ItemsManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        ItemsManager itemsManager = target as ItemsManager;

        GUILayout.Space(10);
        if (GUILayout.Button("Populate items data lists"))
        {
            itemsManager.PopulateItemsDataLists();
        }

        GUILayout.Space(10);
        GUILayout.Label("Play mode utilities:");
        if (GUILayout.Button("Skip to next trial"))
        {
            itemsManager.SkipToNextTrial();
        }
    }
}