using System.Collections.Generic;
using System.Linq;
using Game.Common.Scripts.Data;
using Game.Common.Scripts.Enums;
using UnityEngine;

namespace Game.Common.Scripts.Controllers
{
    public class GameStateController : MonoBehaviour
    {
        [SerializeField] private List<ScreenInfo> _screensInfo;
        
        private GameObject _currentScreenInstance;
        
        public GameState CurrentGameState { get; private set; }
        
        private void Awake()
        {
            DependencyManager.SetDependency(this);
            CurrentGameState = GameState.Intro;
            SetGameState(CurrentGameState);
        }
        
        public void SetGameState(GameState newGameState)
        {
            CurrentGameState = newGameState;
            CreateScreenInstance(CurrentGameState);
        }
        
        private void CreateScreenInstance(GameState gameState)
        {
            if (_currentScreenInstance != null)
            {
                Destroy(_currentScreenInstance);
            }
            
            var screenInfo = _screensInfo.FirstOrDefault(screen => screen.GameState.Equals(gameState));
            if (screenInfo?.ScreenPrefab != null)
            {
                _currentScreenInstance = Instantiate(screenInfo.ScreenPrefab, transform);
            }
        }
    }
}