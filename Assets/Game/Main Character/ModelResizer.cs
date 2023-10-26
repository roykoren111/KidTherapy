using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class ModelResizer : MonoBehaviour
{
    [SerializeField] private SphereCollider sphereCollider;
    [SerializeField] private SkinnedMeshRenderer skinnedMeshRenderer;
    [SerializeField] Transform objectToScale;
    [SerializeField] private Transform tempObject;
    private void Start()
    {
        // scale object to the right size
        float meshLength = CalculateLongestDistance(skinnedMeshRenderer) * 1.2f; // the 1.2f is to make it aliitle extra small
        float scaleFactor = sphereCollider.radius * 2f * transform.localScale.x / meshLength;
        objectToScale.localScale *= scaleFactor;

        // move to sphere center - currently not working.

        Vector3 localCenter = skinnedMeshRenderer.sharedMesh.bounds.center;
        Vector3 meshCenterPoint = skinnedMeshRenderer.transform.TransformPoint(localCenter);
        Vector3 offset = transform.position - meshCenterPoint;
        objectToScale.position += offset;
    }

    private float CalculateLongestDistance(SkinnedMeshRenderer smr)
    {
        Mesh mesh = new Mesh();
        smr.BakeMesh(mesh);

        Vector3[] vertices = mesh.vertices;

        float maxDistance = 0f;
        for (int i = 0; i < vertices.Length; i++)
        {
            for (int j = i + 1; j < vertices.Length; j++)
            {
                float distance = Vector3.Distance(vertices[i], vertices[j]);
                if (distance > maxDistance)
                {
                    maxDistance = distance;
                }
            }
        }

        return maxDistance;
    }
}


