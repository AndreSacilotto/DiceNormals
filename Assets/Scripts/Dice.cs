using System;
using System.Linq;
using UnityEngine;

// Base code from: https://www.youtube.com/watch?v=0Rnj7YQJfrc
public class Dice : MonoBehaviour
{
    public Vector3 directionVector = Vector3.up;

    Rigidbody rb;

    public int selectedResult;
    public int selectedVector;

    public bool isPreConfigured;

    [Space]
    public int[] results;
    public Vector3[] points;
    // Could not use dict, it don't serializes
    // These vectors should be normalized. Might be worth adding a task to Start to ensure they are normalized.                                
    // Note: the size of the vectorValues and vectorPoints should be the same.

    void Start () 
    {
        rb = GetComponent<Rigidbody>();
        if (isPreConfigured)
            return;

        var mf = GetComponent<MeshFilter>();
        points = mf.sharedMesh.normals.Distinct().Select(el => el.normalized).ToArray();
        results = new int[points.Length];
        for (int i = 0; i < results.Length; i++)
            results[i] = i+1;
    }
	
	void FixedUpdate()
	{
        if (rb.IsSleeping())
            return;

        float bestDot = -1;
        for(int i = 0; i < points.Length; i++)
        {
            var valueVector = points[i];
            // Each side vector is in local object space. We need them in world space for our calculation.
	        var worldSpaceValueVector = transform.localToWorldMatrix.MultiplyVector(valueVector);
            // Mathf.Arccos of the dot product can be used to get the angle of difference. You can use this to check for a tilt (perhaps requiring a reroll)
            float dot = Vector3.Dot(worldSpaceValueVector, directionVector);
            if (dot > bestDot)
            {
                // The vector with the greatest dot product is the vector in the most "up" direction. This is the current face selected.
                bestDot = dot;
                selectedVector = i;
            }
        }

	    selectedResult = results[selectedVector];
	}

	void OnDrawGizmos()
    {
        if (results == null || points == null)
            return;

        Gizmos.color = Color.green;
        foreach (var valueVector in points)
        {
            var worldSpaceValueVector = this.transform.localToWorldMatrix.MultiplyVector(valueVector);
            Gizmos.DrawLine(this.transform.position, this.transform.position + worldSpaceValueVector);
        }
    }
}