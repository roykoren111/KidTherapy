using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour, ITappable
{
    public bool IsCollected => _isCollected;
    private bool _isCollected = false;

    public void OnTap()
    {
        if (_isCollected) return;

        _isCollected = true;
        ItemsManager.Instance.CollectItem(gameObject);
    }
}