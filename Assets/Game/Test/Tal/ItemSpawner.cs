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
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (bSpawn)
        {
            Item item = Instantiate(itemPrefab, outerPosition.position, Quaternion.identity).GetComponent<Item>();
            item.Spawn(outerPosition, innerPosition, duration);
        }
    }
}
