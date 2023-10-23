using System;
using UnityEngine;

namespace Game.Common.Scripts.Data
{
    [Serializable]
    public class ScreenInfo
    {
        public RoundType GameState;
        public GameObject ScreenPrefab;
    }
}