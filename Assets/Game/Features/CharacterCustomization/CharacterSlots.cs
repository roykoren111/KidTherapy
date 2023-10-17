using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSlots : MonoBehaviour
{
    [SerializeField]
    private List<CharacterSlot> slots;

    private void Awake()
    {
        DependencyManager.SetDependency(this);
    }

    public void Test()
    {
        Debug.Log("Test");
    }

    public void AddItemToRandomSlot(object item)
    {
        slots[Random.Range(0, slots.Count - 1)].ChangeItemInSlot(item);
    }
}
