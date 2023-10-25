using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterItemSlot : MonoBehaviour
{
    [SerializeField] GameObject _bubble;
    private void Start()
    {
        _bubble.SetActive(false);
    }

    public void ChangeItemInSlot(ItemData itemData)
    {
        //_bubble.SetActive(true);
        GameObject createdItem = Instantiate(itemData.Prefab, transform);
    }
}