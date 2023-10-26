using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Rendering;

public class EyesTarget : MonoBehaviour
{
    private Vector3 _spawnPosition;
    [SerializeField] private float __backToCenterFreq = 2f;
    [SerializeField] private float _lookSpeed;
    [SerializeField] private float _zPosition;
    Vector3 targetPosition;
    private bool isOnTarget = false;
    float lastSetTargetTime = 0;
    private void Start()
    {
        _spawnPosition = transform.position;
        _spawnPosition.z = _zPosition;
        targetPosition = _spawnPosition;
    }
    private void Update()
    {
        if (isOnTarget)
        {
            if (Time.time > lastSetTargetTime + __backToCenterFreq)
            {
                BackToCenter();
            }
        }

        transform.position = Vector3.Lerp(transform.position, targetPosition, _lookSpeed * Time.deltaTime);

    }

    public void BackToCenter()
    {
        targetPosition = _spawnPosition;
        isOnTarget = false;
    }
    public void SetTarget(Vector3 target)
    {
        isOnTarget = true;
        targetPosition = target;
        targetPosition.z = _zPosition;
        lastSetTargetTime = Time.time;
    }
}
