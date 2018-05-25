using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcCast : MonoBehaviour {
    public static List<Vector3> Cast(Vector3 startPos, Vector3 direction, out RaycastHit hit, float reach = 10f, float drop = 10f, int segments = 50)
    {
        hit = new RaycastHit();
        List<Vector3> arcPositions = new List<Vector3>();
        Vector3 lastLinePos = startPos;
        float airTime = CalculateAirTime(direction, reach, drop);
        for (int i = 0; i < segments; i++)
        {
            Vector3 nextLinePos = PlotProjectileTrajectory(startPos, direction * reach, drop, Remap(i, 0, segments, 0, airTime));
            arcPositions.Add(nextLinePos);
            Physics.Raycast(lastLinePos, nextLinePos - lastLinePos, out hit, Vector3.Distance(nextLinePos, lastLinePos));
            if (hit.collider != null)
            {
                break;
            }
            lastLinePos = nextLinePos;
        }
        return arcPositions;
    }

    private static Vector3 PlotProjectileTrajectory(Vector3 startPos, Vector3 initialVel, float drop, float time)
    {
        return startPos + initialVel * time + (Vector3.down * drop) * Mathf.Pow(time, 2) * 0.5f;
    }

    private static float Remap(float sourceValue, float sourceMin, float sourceMax, float destMin, float destMax)
    {
        return Mathf.Lerp(destMin, destMax, Mathf.InverseLerp(sourceMin, sourceMax, sourceValue));
    }

    private static float CalculateAirTime(Vector3 direction, float reach, float drop)
    {
        float result = reach * Mathf.Sin((Vector3.Angle(direction, Vector3.forward) * Mathf.PI) / 180) / drop;
        // Fudge to accommodate start position elevation
        // An angle of zero (aiming straight) produces 0 seconds.
        // Don't ever let airtime below 2 seconds; Add 25% extra time to actually reach the ground. 
        return Mathf.Max(result * 2.5f, 2); 
    }
}
