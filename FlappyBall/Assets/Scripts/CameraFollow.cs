using UnityEngine;

namespace FlappyBall
{
    public class CameraFollow : MonoBehaviour
    {
        [SerializeField] private Transform followObject;
        [SerializeField] private Vector3 offset;

        private Transform _transform;

        private void Awake()
        {
            _transform = GetComponent<Transform>();
        }

        private void Update()
        {
            _transform.localPosition = new Vector3(followObject.localPosition.x, _transform.localPosition.y,
                _transform.localPosition.z) + offset;
        }
    }
}