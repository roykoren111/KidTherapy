using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundManagerInnerTalk : MonoBehaviour, RoundManager
{
    [SerializeField] private GameObject[] _sentences;
    public async UniTask RunRoundFlow(RoundConfiguration config)
    {


        await UIController.Instance.SetRoundInitialUI(config);

        // after UI is displayed, wait for confirmation.
        await InputManager.Instance.WaitForTapUpToContinue();
        AudioManager.Instance.PlayInnerTalkMusic().Forget();

        await UIController.Instance.SetUIAlpha(false, 1f);
        // await CameraController.Instance.MoveToTransform(config.CameraTransform, config.CameraLerpDuration);
        CharacterController.Instance.InitCharacterToRound(config.RoundType);

        // TODO: Add tutorial round with tap indicator before starting trials.

        TrialManagerInnerTalk trialManager = new TrialManagerInnerTalk();
        CharacterController.Instance.MoveToInnerTalkPosition();

        for (int i = 0; i < _sentences.Length; i++)
        {

            InnerTalkSentence sentence = Instantiate(_sentences[i]).GetComponent<InnerTalkSentence>();
            sentence.transform.parent = transform;
            sentence.Hide();
            await trialManager.RunTrialFlow(sentence);
            await UniTask.Yield();
        }

        await CharacterController.Instance.MoveToCenter(2f);

        // End
    }
}
