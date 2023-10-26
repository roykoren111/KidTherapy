using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public static CharacterController Instance;
    [SerializeField] private Animator animator;
    [SerializeField] private List<CharacterItemSlot> _slots;
    private int _nextFreeSlotIndex = 0;

    public void InitCharacterToRound(RoundType roundType)
    {
        switch (roundType)
        {
            case RoundType.Grounding:
                RandomizeSlotsIndexes();
                break;
        }
    }

    public void AddItemToCharacter(Transform item)
    {
        if (_nextFreeSlotIndex == _slots.Count - 1)
        {
            Debug.LogError("All slots full!");
            return;
        }

        _slots[_nextFreeSlotIndex].ChangeItemInSlot(item);
        _nextFreeSlotIndex++;
    }

    public async UniTask PlayBreathingAnimation(int breathingCycles)
    {
        animator.SetBool("IsBreathing", true);

        float inhaleDuration = 4f, holdingDuration = 2f, exhaleDuration = 6f;
        float scaleAmount = .5f; // grow by this amount on all axis

        for (int i = 0; i < breathingCycles; i++)
        {
            float scaleAmountThisBreath = scaleAmount / breathingCycles;
            Vector3 targetScale = transform.localScale + Vector3.one * scaleAmountThisBreath;
            ChangeScale(transform.localScale, targetScale, inhaleDuration).Forget();

            float animationDuration = inhaleDuration + holdingDuration + exhaleDuration;
            await UniTask.Delay(TimeSpan.FromSeconds(animationDuration));
        }
        animator.SetBool("IsBreathing", false);

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

    private void RandomizeSlotsIndexes()
    {
        int n = _slots.Count;
        while (n > 1)
        {
            n--;
            int k = UnityEngine.Random.Range(0, n + 1);
            CharacterItemSlot slot = _slots[k];
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
