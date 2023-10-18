using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSlots : MonoBehaviour
{
    [SerializeField]
    private List<CharacterSlot> slots;

    private int _nextFreeSlotIndex = 0;

    private void Awake()
    {
        DependencyManager.SetDependency(this);
    }

    public void Test()
    {
        //Debug.Log("Test");
    }

    public void AddItemToRandomSlot(GameObject item)
    {
        if(_nextFreeSlotIndex == slots.Count - 1)
        {
            Debug.LogError("All slots full!");
            return;
        }

        slots[_nextFreeSlotIndex].ChangeItemInSlot(item);
        _nextFreeSlotIndex++;
    }
}
