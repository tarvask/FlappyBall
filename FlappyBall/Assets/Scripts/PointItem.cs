using UnityEngine;

namespace FlappyBall
{
    public class PointItem : MonoBehaviour
    {
        [SerializeField] private float velocity;
        [SerializeField] private float visionRadius;
        [SerializeField] private int points;

        private Transform _transform;
        private Transform _target;

        public int Points => points;

        private void Awake()
        {
            _transform = GetComponent<Transform>();
        }

        private void FixedUpdate()
        {
            if (ReferenceEquals(_target, null))
                return;

            if ((_transform.position - _target.transform.position).sqrMagnitude > visionRadius * visionRadius)
                return;
            
            _transform.position = Vector3.MoveTowards(_transform.position, _target.transform.position,
                velocity * Time.fixedDeltaTime);
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            Destroy(gameObject);
        }

        public void SetTarget(Transform target)
        {
            _target = target;
        }
    }
}