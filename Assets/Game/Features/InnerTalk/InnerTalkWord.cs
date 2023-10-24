using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InnerTalkWord : MonoBehaviour, ITappable
{
    public bool IsTouched = false;
    [SerializeField] MeshRenderer wordMesh;
    private void Start()
    {
        wordMesh.enabled = false;
    }

    public void Spawn()
    {
        wordMesh.enabled = true;
    }

    public void Remove(float duration)
    {
        transform.DOMoveY(transform.position.y + 5, duration);
    }

    public void OnTap()
    {
        InnerTalkManager.Instance.OnWordTap(this);
        WordSelectedEffect();
    }

    private void WordSelectedEffect()
    {
        wordMesh.material.color = Color.blue;
    }
}
