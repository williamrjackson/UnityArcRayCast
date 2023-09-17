using UnityEngine;

namespace ArcCast.Example
{
    public class Rotate : MonoBehaviour
    {
        [SerializeField]
        float speed = 0f;

        void Update()
        {
            transform.localEulerAngles = transform.localEulerAngles + (Vector3.up * speed * Time.deltaTime);
        }
    }
}
