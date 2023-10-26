using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour, ITappable
{
    public bool IsCollected = false;

    public void ScaleToOutsideSphere()
    {

    }

    public void ScaleToInsideCharacter()
    {

    }

    public void MoveToInnerScreenPosition(Transform innerTransform)
    {

    }

    public void OnWrongPick(EItemCategory itemCategory)
    {

    }

    public void OnTap()
    {
        if (IsCollected) return;

        ItemsManager.Instance.CollectItem(gameObject);
    }
}