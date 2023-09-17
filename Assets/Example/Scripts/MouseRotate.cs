using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseRotate : MonoBehaviour
{
    [SerializeField]
    Transform caster;
    [SerializeField]
    float sensitivity = 1f;

    Vector3 _lastMousePos;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _lastMousePos = Input.mousePosition;
        }
        else if (Input.GetMouseButton(0))
        {
            var currentMousePos = Input.mousePosition;
            var delta = _lastMousePos - currentMousePos;
            _lastMousePos = currentMousePos;
            Vector3 initEuler = caster.localEulerAngles;
            initEuler.y -= delta.x * sensitivity;
            initEuler.x += delta.y * sensitivity;
            caster.localEulerAngles = initEuler;
        }
    }
}
