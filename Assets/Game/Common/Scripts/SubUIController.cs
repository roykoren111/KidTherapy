using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build;
using UnityEngine;
using RTLTMPro;

public class SubUIController : MonoBehaviour
{
    private async UniTask SetAppearance(bool shouldAppear)
    {
        RTLTextMeshPro[] uiTexts = GetComponentsInChildren<RTLTextMeshPro>();
        foreach (var txt in uiTexts)
        {
            //txt.mesh.SetColors(Color.white);
        }
    }
}
