using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Game.Common.Scripts.Controllers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Features.NameSelection.Scripts
{
    public class NameSelectionScreenController : MonoBehaviour
    {
        [SerializeField] private TMP_Text _currentPlayerNameText;
        [SerializeField] private Button _changeNameButton;
        
        private string _currentPlayerName;
        
        private void Awake()
        {
            _changeNameButton.onClick.AddListener(ChangePlayerName);
        }
        
        private void ChangePlayerName()
        {
#if UNITY_EDITOR
            GoToNextScreen();
#elif UNITY_ANDROID || UNITY_IOS
            ChangePlayerNameAsync().Forget();
#endif
        }
        
        private async UniTask ChangePlayerNameAsync()
        {
            var statuesToAwait = new List<TouchScreenKeyboard.Status>
            {
                TouchScreenKeyboard.Status.Done,
                TouchScreenKeyboard.Status.Canceled,
                TouchScreenKeyboard.Status.LostFocus
            };
            
            var newName = await GetKeyboardInput(
                _currentPlayerName, TouchScreenKeyboardType.Default, statuesToAwait);
            _currentPlayerName = newName;
            _currentPlayerNameText.text = newName;
            
            // todo: select gender
            // todo: save user name in a global context
            GoToNextScreen();
        }
        
        private async UniTask<string> GetKeyboardInput(string startingText,
            TouchScreenKeyboardType keyboardType, List<TouchScreenKeyboard.Status> statusesToAwait)
        {
            var keyboard = TouchScreenKeyboard.Open(startingText, keyboardType);
            await UniTask.WaitUntil(() => statusesToAwait.Contains(keyboard.status));
            var textInputInKeyboard = keyboard.text;
            return textInputInKeyboard;
        }
        
        private void GoToNextScreen()
        {
            if (DependencyManager.GetDependency(out GameStateController gameStateController))
            {
                gameStateController.SetGameState(RoundType.Breathing);
            }
        }
        
        private void OnDestroy()
        {
            _changeNameButton.onClick.RemoveListener(ChangePlayerName);
        }
    }
}