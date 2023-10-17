using System;
using Game.Common.Scripts.Enums;
using UnityEngine;

namespace Game.Common.Scripts.Data
{
    [Serializable]
    public class ScreenInfo
    {
        public GameState GameState;
        public GameObject ScreenPrefab;
    }
}