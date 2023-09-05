using UnityEngine;

namespace FlappyBall
{
    public class CameraFollow : MonoBehaviour
    {
        [SerializeField] private Vector3 offset;

        private Transform _transform;
        private Transform _followObject;

        private void Awake()
        {
            _transform = GetComponent<Transform>();
        }

        public void Construct(Transform targetToFollow)
        {
            _followObject = targetToFollow;
        }

        public void OuterFixedUpdate()
        {
            _transform.localPosition = new Vector3(_followObject.localPosition.x, _transform.localPosition.y,
                _transform.localPosition.z) + offset;
        }
    }
}