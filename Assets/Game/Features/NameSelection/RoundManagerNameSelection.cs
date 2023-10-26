using Cysharp.Threading.Tasks;
using Game.Features.NameSelection.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundManagerNameSelection : MonoBehaviour, RoundManager
{
    public async UniTask RunRoundFlow(RoundConfiguration config)
    {
        UIController.Instance.SetRoundInitialUI(config);

        // name
        await UIController.Instance.ListenToNameFieldClick();
        await CameraController.Instance.MoveToKeyboardPosition(true);
        string playerName = await UIController.Instance.GetKeyboardInput();
        PlayerInfo.Instance.SetName(playerName);
        await CameraController.Instance.MoveToKeyboardPosition(false);

        // gender
        PlayerGender gender = await UIController.Instance.GenderSelection();
        PlayerInfo.Instance.SetGender(gender);

        // wait until tapped on continue OR continue after gender selection
        // End
    }
}
