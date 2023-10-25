using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private RoundConfiguration[] _rounds;
    [SerializeField] private RoundManagerIntro roundManagerIntro;
    [SerializeField] private RoundManagerGrounding roundManagerGrounding;
    [SerializeField] private RoundManagerBreathing roundManagerBreathing;
    [SerializeField] private RoundManagerTension roundManagerTension;
    [SerializeField] private RoundManagerInnerTalk roundManagerInnerTalk;
    
    private RoundManager _roundManager;
    
    private void Start()
    {
        RunGameLoop().Forget();
    }
    
    private async UniTask RunGameLoop()
    {
        for (int i = 0; i < _rounds.Length; i++)
        {
            InitRoundManager(_rounds[i]);
            await _roundManager.RunRoundFlow(_rounds[i]);
        }
        
        Debug.Log("Game Ends");
    }

    private void InitRoundManager(RoundConfiguration roundConfiguration)
    {
        switch (roundConfiguration.RoundType)
        {
            case RoundType.Intro:
                _roundManager = roundManagerIntro;
                break;
            case RoundType.Grounding:
                _roundManager = roundManagerGrounding;
                break;
            case RoundType.NameSelection:
                break;
            case RoundType.Breathing:
                _roundManager = roundManagerBreathing;
                break;
            case RoundType.Tension:
                _roundManager = roundManagerTension;
                break;
            case RoundType.InnerTalk:
                _roundManager = roundManagerInnerTalk;
                break;
            case RoundType.Ending:
                break;

        }
    }
}
