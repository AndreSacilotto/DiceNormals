using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Dot : MonoBehaviour
{
    public bool noramalize;

    public Transform t1, t2;

    public float result;

    void Update()
    {
        if (noramalize)
            result = Vector3.Dot(t1.position.normalized, t2.position.normalized);
        else
            result = Vector3.Dot(t1.position, t2.position);
    }

	private void OnDrawGizmos()
	{
        var pos = transform.position;
        Gizmos.color = Color.green;
        Gizmos.DrawLine(pos, t1.position);
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(pos, t2.position);
        Gizmos.color = Color.red;
        Gizmos.DrawLine(pos, new Vector3(result, 0, 0));
    }
}
