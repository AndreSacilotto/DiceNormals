using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dot : MonoBehaviour
{
    public Transform t1, t2;

    public float result;

    void Update()
    {
        result = Vector3.Dot(t1.position.normalized, t2.position.normalized);
    }

	private void OnDrawGizmos()
	{
        Gizmos.color = Color.green;
        Gizmos.DrawLine(Vector3.zero, t1.position);
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(Vector3.zero, t2.position);
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, 1);
    }
}
