using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSlot : MonoBehaviour
{
    private void Start()
    {
        if (DependencyManager.GetDependency(out CharacterSlots charSlots))
        {
            charSlots.Test();
        }
    }

    public void ChangeItemInSlot(ItemData itemData)
    {
        GameObject createdItem = Instantiate(itemData.Prefab, transform);
        createdItem.layer = 0;
    }
}