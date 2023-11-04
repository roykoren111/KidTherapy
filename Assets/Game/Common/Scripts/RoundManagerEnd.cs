using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundManagerEnd : MonoBehaviour, RoundManager
{
    public async UniTask RunRoundFlow(RoundConfiguration config)
    {
        await UIController.Instance.SetEndingUI();
    }
}
