using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class RoundConfiguration
{
    public RoundType RoundType;
    public GameObject UIPrefab;
    public Transform CameraTransform;
    public int CameraLerpDuration;
}

public interface RoundManager
{
    UniTask RunRoundFlow(RoundConfiguration config);
}