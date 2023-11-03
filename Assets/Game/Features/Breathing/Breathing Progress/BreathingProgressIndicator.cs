using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class BreathingProgressIndicator : MonoBehaviour
{
    [SerializeField] Transform _background;
    [SerializeField] Transform _fill;

    MeshRenderer[] _childsMR;
    void Start()
    {
        _background.gameObject.SetActive(false);
        _childsMR = _background.GetComponentsInChildren<MeshRenderer>();
        _fill.gameObject.SetActive(false);
    }

    void Update()
    {

    }

    public async UniTask SetActive(bool isActive)
    {
        if (isActive)
        {
            await Appear();
        }
        else
        {
            await Disappear();
        }
    }

    public async UniTask LaunchProgress(float wholeSessionDuration)
    {
        float lerpTime = 0;
        while (lerpTime < wholeSessionDuration)
        {
            lerpTime += Time.deltaTime;
            float t = lerpTime / wholeSessionDuration;
            _fill.localScale = Vector3.Lerp(Vector3.one * 1.6f, Vector3.one, t);
            await UniTask.Yield();
        }
    }
    private async UniTask Appear()
    {
        _background.gameObject.SetActive(true);
        _fill.gameObject.SetActive(true);
        _fill.localScale = Vector3.one * 1.6f;
        float appearDuration = 10f;
        foreach (MeshRenderer mr in _childsMR)
        {
            SpawnBackground(mr, appearDuration, 0, mr.material.color.a).Forget();
        }

        await UniTask.Delay(TimeSpan.FromSeconds(appearDuration));
    }

    private async UniTask Disappear()
    {
        foreach (MeshRenderer mr in _childsMR)
        {
            SpawnBackground(mr, 1f, mr.material.color.a, 0).Forget();
        }
        await UniTask.Delay(TimeSpan.FromSeconds(1f));
        _background.gameObject.SetActive(false);
    }

    private async UniTask SpawnFill(float duration)
    {
        float lerpTime = 0;
        while (lerpTime < duration)
        {
            lerpTime += Time.deltaTime;
            float t = lerpTime / duration;
            t = Mathf.SmoothStep(0, t, t);
            _fill.localScale = Vector3.Lerp(Vector3.zero, new Vector3(.1f, .1f, .1f), t);
            await UniTask.Yield();
        }
    }

    private async UniTask SpawnBackground(MeshRenderer mr, float duration, float fromAlpha, float targetAlpha)
    {
        // hide 
        Color current = mr.material.color;

        float lerpTime = 0;
        while (lerpTime < duration)
        {
            lerpTime += Time.deltaTime;
            float t = lerpTime / duration;

            current.a = Mathf.Lerp(fromAlpha, targetAlpha, t);
            mr.material.color = current;
            await UniTask.Yield();
        }
    }
}
