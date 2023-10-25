using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class ModelResizer : MonoBehaviour
{
    [SerializeField] private SphereCollider sphereCollider;
    [SerializeField] private SkinnedMeshRenderer skinnedMeshRenderer;
    [SerializeField] Transform objectToScale;
    private void Start()
    {
        // scale object to the right size
        float meshLength = CalculateLongestDistance(skinnedMeshRenderer) * 1.2f; // the 1.2f is to make it aliitle extra small
        float scaleFactor = sphereCollider.radius * 2f / meshLength;
        objectToScale.localScale *= scaleFactor;

        // move to sphere center
        Vector3 localCenter = skinnedMeshRenderer.sharedMesh.bounds.center;
        Vector3 meshCenterPoint = skinnedMeshRenderer.transform.TransformPoint(localCenter);
        //Instantiate(sphereCollider.gameObject, meshCenterPoint, Quaternion.identity);
        Debug.Log("test");
        Vector3 offset = sphereCollider.center - meshCenterPoint;
        //  objectToScale.position += offset;
    }

    private float CalculateMaxDistanceFromCenter()
    {
        // Calculate the maximum distance from the object's pivot to any vertex in the skinned mesh.
        Mesh skinnedMesh = skinnedMeshRenderer.sharedMesh;
        Vector3[] vertices = skinnedMesh.vertices;
        Vector3 objectCenter = skinnedMesh.bounds.center;
        float maxDistance = 0f;

        foreach (Vector3 vertex in vertices)
        {
            float distance = Vector3.Distance(objectCenter, transform.TransformPoint(vertex));
            maxDistance = Mathf.Max(maxDistance, distance);
        }

        return maxDistance;
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

    private Vector3 GetSMRCenter(SkinnedMeshRenderer smr)
    {
        Mesh mesh = new Mesh();
        smr.BakeMesh(mesh);

        Vector3[] vertices = mesh.vertices;
        Vector3 sum = Vector3.zero;

        foreach (Vector3 vertex in vertices)
        {
            sum += smr.transform.TransformPoint(vertex); // Convert vertex position to world space
        }

        return sum / vertices.Length; // Return the average
    }
}


