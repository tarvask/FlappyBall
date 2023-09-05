using System;
using UnityEngine;

namespace FlappyBall
{
    public class BallMono : MonoBehaviour
    {
        [SerializeField] private float velocity;
        [SerializeField] private float jumpCoefficient;
        [SerializeField] private Vector2 startPosition;

        private Transform _transform;
        private Rigidbody2D _rigidbody;
        private TrailRenderer _trailRenderer;

        public Action<int> OnPointsCollected;
        public Action OnBallCrashed;

        public Vector3 Position => _transform.localPosition;

        private void Awake()
        {
            _transform = GetComponent<Transform>();
            _rigidbody = GetComponent<Rigidbody2D>();
            _trailRenderer = GetComponent<TrailRenderer>();
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            PointItem pointItem = col.GetComponent<PointItem>();
            
            if (!ReferenceEquals(pointItem, null))
                OnPointsCollected?.Invoke(pointItem.Points);
            else
                OnBallCrashed?.Invoke();
        }

        public void ReturnToStart()
        {
            // doesn't work because it needs 1 FixedUpdate frame to apply, but we switch simulation off
            //_rigidbody.MovePosition(startPosition);
            _transform.localPosition = startPosition;
            _rigidbody.velocity = Vector2.zero;
            _rigidbody.simulated = false;
            _trailRenderer.Clear();
        }

        public void Launch()
        {
            _rigidbody.simulated = true;
            _rigidbody.velocity = new Vector2(velocity, 0);
        }

        public void Jump()
        {
            _rigidbody.AddForce(Vector2.up * jumpCoefficient);
        }
    }
}
