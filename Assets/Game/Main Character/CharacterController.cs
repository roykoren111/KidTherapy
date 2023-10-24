using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public static CharacterController Instance;

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

    public void AddItemToCharacter(ItemData itemData)
    {
        if (_nextFreeSlotIndex == _slots.Count - 1)
        {
            Debug.LogError("All slots full!");
            return;
        }

        _slots[_nextFreeSlotIndex].ChangeItemInSlot(itemData);
        _nextFreeSlotIndex++;
    }

    public async UniTask PlayBreathingAnimation()
    {

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
            int k = Random.Range(0, n + 1);
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
