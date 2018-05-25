using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcCastExample : MonoBehaviour {

    public LineRenderer line;
    public Transform marker;
	
	// Update is called once per frame
	void Update () {
        RaycastHit hit;
        List<Vector3> arcPositions = ArcCast.Cast(transform.position, transform.forward, out hit);
        line.positionCount = arcPositions.Count;
        print(arcPositions.Count);
        line.SetPositions(arcPositions.ToArray());
        if (hit.collider)
        {
            print(hit.collider.name);
            if (marker.localScale.magnitude < 0)
                marker.localScale = Vector3.one;
            marker.position = hit.point;
        }
        else
        {
            if (marker.localScale.magnitude > 0)
                marker.localScale = Vector3.zero;
        }
	}
}
