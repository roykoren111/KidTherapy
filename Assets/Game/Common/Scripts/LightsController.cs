using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class LightsController : MonoBehaviour
{
    public static LightsController Instance;
    [SerializeField] Light mainLight;
    public float DefaultIntensity;

    public async UniTaskVoid SetLightIntensity(float targetIntensity, float duration)
    {
        float currentIntencity = mainLight.intensity;
        float lerpTime = 0;
        while (lerpTime < duration)
        {
            lerpTime += Time.deltaTime;
            float t = lerpTime / duration;
            mainLight.intensity = Mathf.Lerp(currentIntencity, targetIntensity, t);
            await UniTask.Yield();
        }
    }


    private void Awake()
    {
        SingletonValidation();
        DefaultIntensity = mainLight.intensity;
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

}
