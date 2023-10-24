using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InnerTalkWord : MonoBehaviour, ITappable
{
    public bool IsTouched = false;
    [SerializeField] MeshRenderer wordMesh;

    public void Spawn()
    {
        wordMesh.enabled = true;
    }

    public void Remove(float duration)
    {
        transform.DOMoveY(transform.position.y + 4, duration);
    }

    public void OnTap()
    {
        if (IsTouched) return;
        IsTouched = true;
        WordsSpawner.Instance.OnWordTap(this);
        WordSelectedEffect();
    }

    private void WordSelectedEffect()
    {
        wordMesh.material.color = Color.blue;
    }
}
