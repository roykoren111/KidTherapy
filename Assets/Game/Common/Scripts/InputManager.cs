using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;

    private void Awake()
    {
        SingletonValidation();
    }

    private void SingletonValidation()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(Instance);
        }
        else
        {
            Instance = this;
        }
    }

    public bool isPlayerTapping { get; private set; }

    public async UniTask WaitForTapToContinue()
    {
        // if tap is already on screen - wait for it to disable then get another // TODO: support multi touch- doesn't matter if fingers already tap, wait for an additional. 
        while (isPlayerTapping) await UniTask.Yield();

        // now wait until recieving a new tap
        while (!isPlayerTapping) await UniTask.Yield();
    }

    public async UniTask WaitForTapUpToContinue()
    {
        // if tap is already on screen - wait for it to disable then get another // TODO: support multi touch- doesn't matter if fingers already tap, wait for an additional. 
        while (isPlayerTapping) await UniTask.Yield();

        // now wait until recieving a new tap
        while (!isPlayerTapping) await UniTask.Yield();

        // as long as tap is happaning- wait
        while (isPlayerTapping) await UniTask.Yield();
    }

    void Start()
    {
    }

    public void OnFingerDown()
    {
        isPlayerTapping = true;
    }

    public void OnFingerUp()
    {
        isPlayerTapping = false;
    }

    public void DetectTappedObject()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            GameObject tappedObject = hit.transform.gameObject;
            tappedObject.GetComponentInParent<ITappable>()?.OnTap();    // works if the component is on object not on its parent
        }
    }
}