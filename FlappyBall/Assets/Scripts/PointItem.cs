using System;
using UnityEngine;

namespace FlappyBall
{
    public class PointItem : MonoBehaviour
    {
        [SerializeField] private float velocity;
        [SerializeField] private float visionRadius;
        [SerializeField] private int points;

        // dependencies
        private Transform _target;

        private Transform _transform;

        public Action<PointItem> OnCollected;

        public int Points => points;

        private void Awake()
        {
            _transform = GetComponent<Transform>();
        }

        public void OuterFixedUpdate()
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
            OnCollected?.Invoke(this);
        }

        public void Construct(Transform target)
        {
            _target = target;
        }
    }
}