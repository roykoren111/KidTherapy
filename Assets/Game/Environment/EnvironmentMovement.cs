using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentMovement : MonoBehaviour
{
    [SerializeField] float speed;
    private float _startingZ = -78f;
    private float _endingZ = 259.3f;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3(0, 0, speed);
        if (transform.position.z > _endingZ)
        {
            Vector3 targetPosition = transform.position;
            targetPosition.z = _startingZ;
            transform.position = targetPosition;
        }
    }
}
