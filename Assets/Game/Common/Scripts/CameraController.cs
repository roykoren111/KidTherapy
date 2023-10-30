using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance;

    Transform _targetTransform;

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

    public async UniTask MoveToTransform(Transform targetTransform, float duration = 1f)
    {
        float lerpTime = 0;
        Vector3 currentPosition = transform.position;
        while (lerpTime < duration)
        {
            lerpTime += Time.deltaTime;
            float t = lerpTime / duration;
            t = t * t * t * (t * (6f * t - 15f) + 10f); // very smooth and nice step function

            transform.position = Vector3.Lerp(currentPosition, targetTransform.position, t);

            await UniTask.Yield();
        }
    }

    public async UniTask MoveToKeyboardPosition(bool ToKeyboard)
    {

    }

    void Start()
    {

    }

    void Update()
    {

    }


}