using System.Collections.Generic;
using UnityEngine;

public class ArcCast : MonoBehaviour {
    public static float GravityMagnitude = Physics.gravity.magnitude;
    public static float AdditionalFlightTimePercentage = .25f;

    private const float DEFAULT_REACH = 10f;
    private const uint DEFAULT_SEGMENTS = 50;

    public static List<Vector3> Cast(Vector3 startPos, Vector3 direction, LayerMask layerMask, out RaycastHit hit, float reach = DEFAULT_REACH, uint segments = DEFAULT_SEGMENTS)
    {
        hit = new RaycastHit();
        List<Vector3> arcPositions = new List<Vector3>();
        Vector3 lastLinePos = startPos;
        float airTime = CalculateAirTime(direction, reach);
        for (int i = 0; i < segments; i++)
        {
            Vector3 nextLinePos = PlotProjectileTrajectory(startPos, direction * reach, Remap(i, 0, segments, 0, airTime));
            arcPositions.Add(nextLinePos);
            Physics.Raycast(lastLinePos, nextLinePos - lastLinePos, out hit, Vector3.Distance(nextLinePos, lastLinePos), layerMask);
            if (hit.collider != null)
            {
                break;
            }
            lastLinePos = nextLinePos;
        }
        return arcPositions;
    }

    public static List<Vector3> Cast(Vector3 startPos, Vector3 direction, out RaycastHit hit, float reach = DEFAULT_REACH, uint segments = DEFAULT_SEGMENTS)
    {
        return Cast(startPos, direction, Physics.DefaultRaycastLayers, out hit, reach, segments);
    }

    private static Vector3 PlotProjectileTrajectory(Vector3 startPos, Vector3 initialVel, float time)
    {
        return startPos + initialVel * time + (Vector3.down * GravityMagnitude) * Mathf.Pow(time, 2) * 0.5f;
    }

    private static float Remap(float sourceValue, float sourceMin, float sourceMax, float destMin, float destMax)
    {
        return Mathf.Lerp(destMin, destMax, Mathf.InverseLerp(sourceMin, sourceMax, sourceValue));
    }

    private static float CalculateAirTime(Vector3 direction, float reach)
    {
        float result = 2f * (reach / GravityMagnitude) * (Mathf.Sin(Vector3.Angle(direction, Vector3.forward) * Mathf.PI / 180f));
        // An angle of zero (aiming straight) produces 0 seconds.
        // Don't ever let airtime below 2 seconds; Add 25% extra time by default
        // to accommodate start position elevation. 
        return Mathf.Max(result * (1 + Mathf.Max(AdditionalFlightTimePercentage, 0f)), 2f); 
    }
}
