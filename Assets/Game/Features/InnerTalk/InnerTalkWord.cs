using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

[Serializable]
public class InnerTalkWord : MonoBehaviour, ITappable
{
    public bool IsTouched = false;
    MeshRenderer wordMesh;
    InnerTalkSentence _sentence;
    Material shader;
    private bool isSpawned = false;

    private void Awake()
    {
        _sentence = transform.parent.GetComponent<InnerTalkSentence>();
    }

    public async UniTask Spawn(InnerTalkSentence sentence)
    {
        await SetAlpha(-.32f, .8f);
        isSpawned = true;
    }

    private async UniTask SetAlpha(float target, float duration)
    {
        float lerpTime = 0;
        float alpha = 0;
        while (lerpTime < duration)
        {
            lerpTime += Time.deltaTime;
            float t = lerpTime / duration;
            t = t * t * t * (t * (6f * t - 15f) + 10f); // very smooth and nice step function

            alpha = Mathf.Lerp(0, target, t);
            shader.SetFloat("_Alpha", alpha);

            await UniTask.Yield();
        }

    }

    public void Hide()
    {
        wordMesh = GetComponent<MeshRenderer>();
        shader = wordMesh.material;
        shader.SetFloat("_Alpha", 0);
    }
    public async UniTask Remove(float duration)
    {
        transform.DOMoveY(transform.position.y + 4, duration).SetEase(Ease.Flash);
        await UniTask.Delay(TimeSpan.FromSeconds(duration));
    }

    public void OnTap()
    {
        if (IsTouched) return;
        IsTouched = true;
        _sentence.OnWordTap(this);
    }

    // triggered from TrialManagerInnerTalk.
    public async UniTask SelectedEffect()
    {
        while (!isSpawned)
            await UniTask.Yield();

        await SetAlpha(1f, 1f);

    }

}
