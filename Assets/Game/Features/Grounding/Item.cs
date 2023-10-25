using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour, ITappable
{
    public bool IsCollected = false;

    public void OnTap()
    {
        if (IsCollected) return;

        ItemsManager.Instance.CollectItem(gameObject);
    }
}