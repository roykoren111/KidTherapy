using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public GameObject itemPrefab;
    public Transform outerPosition;
    public Transform innerPosition;
    public Transform charPosition;
    public float duration = 3f;
    public bool bSpawn = false;
    public bool bTap = false;
    public bool bWrongPick = false;
    Item item;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (bSpawn)
        {
            bSpawn = false;
            item = Instantiate(itemPrefab, outerPosition.position, Quaternion.identity).GetComponent<Item>();
            item.Spawn(outerPosition, innerPosition, duration);
        }
        if (bTap)
        {
            bTap = false;
            if (bWrongPick)
            {
                item.IsCollected = true;
                //item.OnWrongPick(EItemCategory.See);
                return;
            }
            item.IsCollected = true;
            item.OnEnterToCharacter(charPosition);
        }
    }
}
