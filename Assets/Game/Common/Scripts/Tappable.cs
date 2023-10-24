using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tappable : MonoBehaviour, ITappable
{
    public void OnTap()
    {
        Debug.Log("Tapped on: " + gameObject.name);
    }
}
