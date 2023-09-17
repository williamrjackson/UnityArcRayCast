using System.Collections.Generic;
using UnityEngine;

namespace ArcCast.Example
{
    public class ArcCastExample : MonoBehaviour {

        public Vector3 gravityOverride = Physics.gravity;
        public LineRenderer line;
        [Range(0f, 100f)]
        public float reach = 10f;
        [Range(0f, 1)]
        public float extent = .25f;
        public LayerMask layerMask;
        public Transform marker;
        private string _lastHit;
        bool MarkerVisible 
        {
            get => marker.gameObject.activeInHierarchy;
            set
            {
                if (marker.gameObject.activeInHierarchy == value) return;
                marker.gameObject.SetActive(value);
            }
        }

        void Update () {
            RaycastHit hit;
            List<Vector3> arcPositions = ArcCast.Cast(transform.position, transform.forward, layerMask, out hit, gravityOverride, extendedFlightTime: extent, reach:reach);
            line.positionCount = arcPositions.Count;
            line.SetPositions(arcPositions.ToArray());
            if (hit.collider)
            {
                ReportName(hit.collider.name);
                MarkerVisible = true;
                marker.position = hit.point;
                marker.rotation = Quaternion.FromToRotation (Vector3.up, hit.normal) ;
            }
            else
            {
                MarkerVisible = false;
            }
        }

        void ReportName(string name)
        {
            if (_lastHit != name)
            {
                _lastHit = name;
                Debug.Log($"Hit: {name}");
            }
        }
    }
}