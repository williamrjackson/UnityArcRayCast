using System.Collections.Generic;
using UnityEngine;

public class ArcCastExample : MonoBehaviour {

    public LineRenderer line;
    [Range(0f, 100f)]
    public float reach = 10f;
    public LayerMask layerMask;
    public Transform marker;
	
	void Update () {
        RaycastHit hit;
        List<Vector3> arcPositions = ArcCast.Cast(transform.position, transform.forward, layerMask, out hit, reach:reach);
        line.positionCount = arcPositions.Count;
        print(arcPositions.Count);
        line.SetPositions(arcPositions.ToArray());
        if (hit.collider)
        {
            print(hit.collider.name);
            marker.localScale = Vector3.one;
            marker.position = hit.point;
        }
        else
        {
            marker.localScale = Vector3.zero;
        }
	}
}
