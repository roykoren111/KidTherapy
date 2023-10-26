using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Item : MonoBehaviour, ITappable
{

    public EItemCategory Categorey;

    public bool IsWrongPick = false;

    public bool IsCollected = false;

    public bool IsSpawnedRightToCharacter = false;
    [SerializeField] MeshRenderer bubbleMR;
    private Vector3 spawnPosition;
    private float movementDuration;

    public void Spawn(Transform outerPosition, Transform innerTransform, float duration)
    {
        // TODO: make sure MR is disabled.

        spawnPosition = outerPosition.position;
        transform.localScale = outerPosition.localScale;
        movementDuration = duration;

        // TODO: Calculate isRightToCharacter

        // if spawned to the right- should look left
        transform.eulerAngles = IsSpawnedRightToCharacter ? new Vector3(0, 180f, 0) : Vector3.zero;

        MoveToPosition(transform.position, innerTransform.position, duration).Forget();
    }

    private async UniTask MoveToPosition(Vector3 current, Vector3 target, float duration)
    {
        float lerpTime = 0;
        // todo add cancle upon selecting
        while (lerpTime < duration)
        {
            lerpTime += Time.deltaTime;
            float t = lerpTime / duration;
            t = t * t * t * (t * (6f * t - 15f) + 10f); // very smooth and nice step function

            transform.position = Vector3.Lerp(current, target, t);

            await UniTask.Yield();
        }
    }

    // should be called from the character controller
    public void OnEnterToCharacter(Transform insideCharacterPosition)
    {
        // TODO: If bubble animation - wait for finish then go inside.
        // trigger bubble animation

        transform.position = insideCharacterPosition.position;
        transform.rotation = insideCharacterPosition.rotation;
        transform.localScale = insideCharacterPosition.localScale;

        GetComponent<SphereCollider>().enabled = false;
        if (bubbleMR != null)
        {
            bubbleMR.enabled = false;
        }
    }

    public void OnWrongPick(EItemCategory itemCategory)
    {
        switch (itemCategory)
        {
            case EItemCategory.Hear:
                // TODO: Play bubble animation
                Destroy(gameObject);
                break;
            default:
                MoveToPosition(transform.position, spawnPosition, movementDuration / 2f).Forget();
                GetComponent<SphereCollider>().enabled = false;
                break;

        }
    }

    public void OnTap()
    {
        if (IsCollected) return;

        ItemsManager.Instance.CollectItem(gameObject);
    }
}