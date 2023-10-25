using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "Scriptable objects/Item data", fileName = "Item data")]
public class ItemData : ScriptableObject
{
    [FormerlySerializedAs("ItemPrefab")] public GameObject Prefab;

    //Should we take name from prefab, or define it separately?
    [FormerlySerializedAs("ItemName")] public string Name;

    [FormerlySerializedAs("ItemCategorey")]
    public EItemCategory Categorey;

    public bool IsWrongPick = false;

    //TODO: add more relevant data here (color, isTasty, etc.)
}