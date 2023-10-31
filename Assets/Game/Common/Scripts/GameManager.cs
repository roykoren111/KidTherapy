using Cysharp.Threading.Tasks;
using Game.Common.Scripts.Services.Firebase;
using UnityEngine;

namespace Game.Common.Scripts
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private RoundConfiguration[] _rounds;
        [SerializeField] private RoundManagerIntro roundManagerIntro;
        [SerializeField] private RoundManagerNameSelection roundManagerNameSelection;
        [SerializeField] private RoundManagerGrounding roundManagerGrounding;
        [SerializeField] private RoundManagerBreathing roundManagerBreathing;
        [SerializeField] private RoundManagerTension roundManagerTension;
        [SerializeField] private RoundManagerInnerTalk roundManagerInnerTalk;

        private IFirebaseService _firebaseService;
        private RoundManager _roundManager;

        private void Start()
        {
            InitFirebase();
            RunGameLoop().Forget();
        }

        private void InitFirebase()
        {
            _firebaseService = new FirebaseService();
            _firebaseService.Initialize().Forget();
        }

        private async UniTask RunGameLoop()
        {
            AudioManager.Instance.PlayMainMusic();

            for (int i = 0; i < _rounds.Length; i++)
            {
                var round = _rounds[i];
                InitRoundManager(round);
                _firebaseService.SendEvent($"RoundStart_{round.RoundType.ToString()}");
                await _roundManager.RunRoundFlow(round);
                _firebaseService.SendEvent($"RoundEnd_{round.RoundType.ToString()}");
                CharacterController.Instance.EyesToCenter(0).Forget();
                //AudioManager.Instance.PlayScreenTransitionSound();
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
                    _roundManager = roundManagerNameSelection;
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

        private void OnDestroy()
        {
            _firebaseService?.Dispose();
        }
    }
}
