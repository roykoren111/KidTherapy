using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable objects/Item data", fileName = "Item data")]
public class ItemData : ScriptableObject
{
    public GameObject ItemPrefab;

    //Should we take name from prefab, or define it separately?
    public string ItemName;

    //TODO: add more relevant data here (color, isTasty, etc.)
}