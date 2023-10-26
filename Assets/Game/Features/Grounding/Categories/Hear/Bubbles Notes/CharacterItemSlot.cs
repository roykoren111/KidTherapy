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

    public void ChangeItemInSlot(Transform item)
    {
        item.transform.position = transform.position;
        item.transform.rotation = transform.rotation;
        item.transform.localScale = transform.localScale;
        item.parent = transform;
    }
}