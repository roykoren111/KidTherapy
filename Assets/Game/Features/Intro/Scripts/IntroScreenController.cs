using System;
using Cysharp.Threading.Tasks;
using Game.Common.Scripts.Controllers;
using Game.Common.Scripts.Enums;
using UnityEngine;

namespace Game.Features.Intro.Scripts
{
    public class IntroScreenController : MonoBehaviour
    {
        private void OnEnable()
        {
            TapToContinue().Forget();
        }

        private async UniTask TapToContinue()
        {
            // todo: temp code..
            await UniTask.Delay(TimeSpan.FromSeconds(2f));
            var gameStateController = FindObjectOfType<GameStateController>(); // todo: temp solution!
            gameStateController.SetGameState(GameState.NameSelection);
        }
    }
}