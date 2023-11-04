using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RestartButton : MonoBehaviour, ITappable
{
    public Action RestartButtonTapped;
    Color txtColor, btnColor;
    public void OnTap()
    {
        RestartButtonTapped?.Invoke();
    }

    public void Init()
    {
        MeshRenderer mr = GetComponent<MeshRenderer>();
        TMPro.TextMeshPro tmPro = GetComponentInChildren<TMPro.TextMeshPro>();

        txtColor = tmPro.color;
        btnColor = mr.material.color;

        tmPro.color = Color.clear;
        mr.material.color = Color.clear;
    }

    public async UniTask Appear()
    {
        MeshRenderer mr = GetComponent<MeshRenderer>();
        TMPro.TextMeshPro tmPro = GetComponentInChildren<TMPro.TextMeshPro>();

        float lerpTime = 0;
        float duration = 1f;
        while (lerpTime < duration)
        {
            lerpTime += Time.deltaTime;
            float t = lerpTime / duration;
            t = t * t * t * (t * (6f * t - 15f) + 10f); // very smooth and nice step function

            mr.material.color = Color.Lerp(Color.clear, btnColor, t);
            tmPro.color = Color.Lerp(Color.clear, txtColor, t);

            await UniTask.Yield();
        }
    }

}
