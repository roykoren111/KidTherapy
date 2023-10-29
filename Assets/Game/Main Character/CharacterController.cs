using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public static CharacterController Instance;
    [SerializeField] private Animator animator;
    [SerializeField] private List<Transform> _slots;
    private int _nextFreeSlotIndex = 0;
    [SerializeField] private Transform eyeRight;
    [SerializeField] private Transform eyeLeft;
    [SerializeField] private EyesTarget eyesTarget;

    public void InitCharacterToRound(RoundType roundType)
    {
        switch (roundType)
        {
            case RoundType.Grounding:
                RandomizeSlotsIndexes();
                break;
        }
    }

    public void AddItemToCharacter(Item item)
    {
        if (_nextFreeSlotIndex == _slots.Count)
        {
            Debug.LogError("All slots full!");
            return;
        }
        SetLookTarget(item.transform.position);

        item.OnEnterToCharacter(_slots[_nextFreeSlotIndex]);
        _nextFreeSlotIndex++;
    }

    public void SetBreathingAnimation(bool state)
    {
        animator.SetBool("IsBreathing", state);
    }

    public void ChangeScaleInBreathing(float scaleAmount, int breathingCycles, float inhaleDuration)
    {
        float scaleAmountThisBreath = scaleAmount / breathingCycles;
        Vector3 targetScale = transform.localScale + Vector3.one * scaleAmountThisBreath;
        ChangeScale(transform.localScale, targetScale, inhaleDuration).Forget();
    }
    private async UniTask ChangeScale(Vector3 current, Vector3 target, float duration)
    {
        float lerpTime = 0;
        while (lerpTime < duration)
        {
            lerpTime += Time.deltaTime;
            float t = lerpTime / duration;
            transform.localScale = Vector3.Lerp(current, target, t);
            await UniTask.Yield();
        }
    }
    public async UniTask PlayStretchingAnimation()
    {

    }

    public void SlideToScreenButtom()
    {

    }

    public void OnInnerTalkSentenceComplete()
    {

    }

    public void SetLookTarget(Vector3 targetPosition)
    {
        eyesTarget.SetTarget(targetPosition);
    }

    public async UniTask EyesToCenter(float delay)
    {
        await UniTask.Delay(TimeSpan.FromSeconds(delay));
        eyesTarget.BackToCenter();
    }

    private void Update()
    {
        eyeRight.up = (eyesTarget.transform.position - eyeRight.position).normalized;
        eyeLeft.up = (eyesTarget.transform.position - eyeLeft.position).normalized;
    }

    private void RandomizeSlotsIndexes()
    {
        int n = _slots.Count;
        while (n > 1)
        {
            n--;
            int k = UnityEngine.Random.Range(0, n + 1);
            Transform slot = _slots[k];
            _slots[k] = _slots[n];
            _slots[n] = slot;
        }
    }

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


}
