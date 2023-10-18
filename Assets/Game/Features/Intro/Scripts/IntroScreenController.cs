using System;
using Cysharp.Threading.Tasks;
using Game.Common.Scripts.Controllers;
using Game.Common.Scripts.Enums;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Features.Intro.Scripts
{
    public class IntroScreenController : MonoBehaviour
    {
        [SerializeField] private Button _tapToContinueButton;
        
        private const float SecondsBeforeEnableUserInput = 2f;
        
        private void Awake()
        {
            WaitAndListenToButtonClick().Forget();
        }
        
        private async UniTask WaitAndListenToButtonClick()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(SecondsBeforeEnableUserInput));
            _tapToContinueButton.onClick.AddListener(OnButtonClicked);
        }
        
        private void OnButtonClicked()
        {
            _tapToContinueButton.onClick.RemoveListener(OnButtonClicked);
            
            if (DependencyManager.GetDependency(out GameStateController gameStateController))
            {
                gameStateController.SetGameState(GameState.NameSelection);
            }
        }
    }
}