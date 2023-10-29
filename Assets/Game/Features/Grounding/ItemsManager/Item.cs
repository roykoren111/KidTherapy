using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using UnityEngine.Serialization;

public class Item : MonoBehaviour, ITappable
{
    public EItemCategory Categorey;

    public bool IsWrongPick = false;

    public bool IsCollected = false;

    [FormerlySerializedAs("ItemColor")] public EItemColor itemColor;

    private bool isSpawnedRightToCharacter = false;
    [SerializeField] MeshRenderer bubbleMR;
    private Vector3 spawnPosition;
    private float movementDuration;

    public void Spawn(Transform outerPosition, Transform innerTransform, float duration)
    {
        GetComponent<MeshRenderer>().enabled = false;

        spawnPosition = outerPosition.position + new Vector3(0, 0, UnityEngine.Random.Range(-2f, 2f));
        transform.localScale = outerPosition.localScale;
        movementDuration = duration;

        isSpawnedRightToCharacter = transform.position.x > CharacterController.Instance.transform.position.x;
        // if spawned to the right- should look left
        transform.eulerAngles = isSpawnedRightToCharacter ? new Vector3(0, 180f, 0) : Vector3.zero;

        MoveToPosition(transform.position, innerTransform.position, duration).Forget();
    }

    private async UniTask MoveToPosition(Vector3 current, Vector3 target, float duration)
    {
        float lerpTime = 0;
        while (lerpTime < duration && !IsCollected)
        {
            lerpTime += Time.deltaTime;
            float t = lerpTime / duration;
            t = t * t * t * (t * (6f * t - 15f) + 10f); // very smooth and nice step function

            transform.position = Vector3.Lerp(current, target, t);

            await UniTask.Yield();
        }
    }

    public async UniTask ReturnToSpawnPosition()
    {
        GetComponent<SphereCollider>().enabled = false;

        float lerpTime = 0;
        float duration = movementDuration;
        Vector3 current = transform.position;
        Vector3 target = spawnPosition;

        // add delay so every animal leaves in a little different time
        await UniTask.Delay(TimeSpan.FromSeconds(UnityEngine.Random.Range(0f, .3f)));

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

        Debug.Log("OnEnterToCharacter");
        GroundingAudioManager.Instance.PlayRandomCorrectPickSound(Categorey);
        Debug.Log("Played Sound");

        transform.position = insideCharacterPosition.position;
        transform.localScale = insideCharacterPosition.localScale;
        transform.parent = insideCharacterPosition;
        Debug.Log("Moved to char");

        GetComponent<SphereCollider>().enabled = false;
        if (bubbleMR != null)
        {
            bubbleMR.enabled = false;
        }
    }

    public void OnWrongPick()
    {
        GroundingAudioManager.Instance.PlayWrongPickSound(Categorey);

        switch (Categorey)
        {
            case EItemCategory.Hear:
                // TODO: Play bubble animation
                Destroy(gameObject);
                break;
            default:
                ReturnToSpawnPosition().Forget();
                GetComponent<SphereCollider>().enabled = false;
                break;
        }
    }

    public void OnTap()
    {
        if (IsCollected) return;

        IsCollected = true;
        if (IsWrongPick)
        {
            OnWrongPick();
            ItemsManager.Instance.OnItemWrongPick(this);
            return;
        }

        ItemsManager.Instance.CollectItem(this);
    }

    private void ChangeTexture(Texture2D texture)
    {
        foreach (SkinnedMeshRenderer skinnedMeshRenderer in GetComponentsInChildren<SkinnedMeshRenderer>())
        {
            skinnedMeshRenderer.material.mainTexture = texture;
        }
    }
}