using System.Collections.Generic;
using UnityEngine;

namespace ArcCast
{

    public static class ArcCast {

        private const float DEFAULT_EXTENDED_FLIGHT_TIME = .25f;
        private const float DEFAULT_REACH = 10f;
        private const uint DEFAULT_SEGMENTS = 50;

        public static List<Vector3> Cast(Vector3 startPos, Vector3 direction, out RaycastHit hit, float reach = DEFAULT_REACH, float extendedFlightTime = DEFAULT_EXTENDED_FLIGHT_TIME, uint segments = DEFAULT_SEGMENTS)
        {
            return CastArc(startPos, direction, Physics.DefaultRaycastLayers, out hit, reach, segments, Physics.gravity, extendedFlightTime);
        }
        public static List<Vector3> Cast(Vector3 startPos, Vector3 direction, out RaycastHit hit, Vector3 gravity, float reach = DEFAULT_REACH, float extendedFlightTime = DEFAULT_EXTENDED_FLIGHT_TIME, uint segments = DEFAULT_SEGMENTS)
        {
            return CastArc(startPos, direction, Physics.DefaultRaycastLayers, out hit, reach, segments, gravity, extendedFlightTime);
        }
        public static List<Vector3> Cast(Vector3 startPos, Vector3 direction, LayerMask layerMask, out RaycastHit hit, float reach = DEFAULT_REACH, float extendedFlightTime = DEFAULT_EXTENDED_FLIGHT_TIME, uint segments = DEFAULT_SEGMENTS)
        {
            return CastArc(startPos, direction, layerMask, out hit, reach, segments, Physics.gravity, extendedFlightTime);
        }    public static List<Vector3> Cast(Vector3 startPos, Vector3 direction, LayerMask layerMask, out RaycastHit hit, Vector3 gravity, float reach = DEFAULT_REACH, float extendedFlightTime = DEFAULT_EXTENDED_FLIGHT_TIME, uint segments = DEFAULT_SEGMENTS)
        {
            return CastArc(startPos, direction, layerMask, out hit, reach, segments, gravity, extendedFlightTime);
        }

        private static List<Vector3> CastArc(Vector3 startPos, Vector3 direction, LayerMask layerMask, out RaycastHit hit, float reach, uint segments, Vector3 gravityOverride, float extendedFlightTime)
        {
            hit = new RaycastHit();
            List<Vector3> arcPositions = new List<Vector3>();
            Vector3 lastLinePos = startPos;
            float airTime = CalculateAirTime(direction, reach, gravityOverride, extendedFlightTime);
            for (int i = 0; i < segments; i++)
            {
                Vector3 nextLinePos = PlotProjectileTrajectory(startPos, direction * reach, Remap(i, 0, segments, 0, airTime), gravityOverride);
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

        private static Vector3 PlotProjectileTrajectory(Vector3 startPos, Vector3 initialVel, float time, Vector3 gravityOverride)
        {
            return startPos + initialVel * time + gravityOverride * Mathf.Pow(time, 2) * 0.5f;
        }

        private static float Remap(float sourceValue, float sourceMin, float sourceMax, float destMin, float destMax)
        {
            return Mathf.Lerp(destMin, destMax, Mathf.InverseLerp(sourceMin, sourceMax, sourceValue));
        }

        private static float CalculateAirTime(Vector3 direction, float reach, Vector3 gravityOverride, float extendedFlightTime)
        {
            float result = Mathf.Max(2f, 2f * (reach / gravityOverride.magnitude) * (Mathf.Sin(Vector3.Angle(direction, Vector3.forward) * Mathf.PI / 180f)));
            // An angle of zero (aiming straight) produces 0 seconds.
            // Don't ever let airtime below 2 seconds; Add 25% extra time by default
            // to accommodate start position elevation. 
            return result * (1 + Mathf.Max(extendedFlightTime, 0f)); 
        }
    }
}