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

    private float yPositionOnBirth = -6f;
    private float yPositionInnerTalk = -1f;
    private float xAngleInnerTalk = -8f;

    private Vector3 centerPosition;
    private Vector3 centerEuler;


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

    public void MoveToInnerTalkPosition()
    {
        Vector3 targetPosition = transform.position;
        targetPosition.y = yPositionInnerTalk;
        Vector3 targetEuler = transform.eulerAngles;
        targetEuler.x = xAngleInnerTalk;

        ChangeTransform(targetPosition, targetEuler, 1f).Forget();
    }

    public async UniTask MoveToCenter(float duration)
    {
        await ChangeTransform(centerPosition, centerEuler, duration);
    }

    public void MoveToBirthPosition()
    {
        Vector3 targetPosition = transform.position;
        targetPosition.y = yPositionOnBirth;
        transform.position = targetPosition;
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

    private async UniTask ChangeTransform(Vector3 targetPosition, Vector3 targetEuler, float duration)
    {
        float lerpTime = 0;
        Vector3 currentPosition = transform.position;
        Vector3 currentEuler = transform.eulerAngles;

        while (lerpTime < duration)
        {
            lerpTime += Time.deltaTime;
            float t = lerpTime / duration;
            t = t * t * t * (t * (6f * t - 15f) + 10f); // very smooth and nice step function

            transform.position = Vector3.Lerp(currentPosition, targetPosition, t);
            transform.eulerAngles = Vector3.Lerp(currentEuler, targetEuler, t);

            await UniTask.Yield();
        }
    }

    private void Awake()
    {
        SingletonValidation();
        centerEuler = transform.eulerAngles;
        centerPosition = transform.position;
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
