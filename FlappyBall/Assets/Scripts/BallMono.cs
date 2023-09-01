using System;
using UnityEngine;

namespace FlappyBall
{
    public class BallMono : MonoBehaviour
    {
        [SerializeField] private float velocity;
        [SerializeField] private float jumpCoefficient;
        [SerializeField] private Vector2 startPosition;

        [SerializeField] private StartUi startUi;
        [SerializeField] private PointsUi pointsUi;

        private Transform _transform;
        private Rigidbody2D _rigidbody;
        private TrailRenderer _trailRenderer;

        private int _points;

        public Action OnReturnedToStart;

        public Vector3 Position => _transform.localPosition;
        public int Points => _points;

        // Start is called before the first frame update
        private void Start()
        {
            _transform = GetComponent<Transform>();
            _rigidbody = GetComponent<Rigidbody2D>();
            _trailRenderer = GetComponent<TrailRenderer>();

            ReturnToStart();
            
            startUi.OnStart += StartGame;
        }

        // Update is called once per frame
        private void Update()
        {
            if (Input.GetKey(KeyCode.Space))
                Jump();
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            PointItem pointItem = col.GetComponent<PointItem>();
            
            if (!ReferenceEquals(pointItem, null))
                AddPoints(pointItem.Points);
            else
                ReturnToStart();
        }

        private void ReturnToStart()
        {
            // doesn't work because it needs 1 FixedUpdate frame to apply, but we switch simulation off
            //_rigidbody.MovePosition(startPosition);
            _transform.localPosition = startPosition;
            _rigidbody.velocity = Vector2.zero;
            _rigidbody.simulated = false;
            _trailRenderer.Clear();
            OnReturnedToStart?.Invoke();

            SetPoints(0);
            
            startUi.ShowWindow(true);
        }

        private void StartGame()
        {
            _rigidbody.simulated = true;
            _rigidbody.velocity = new Vector2(velocity, 0);
        }

        private void Jump()
        {
            _rigidbody.AddForce(Vector2.up * jumpCoefficient);
        }
        
        private void AddPoints(int points)
        {
            _points += points;
            pointsUi.SetPoints(_points);
        }
        
        private void SetPoints(int points)
        {
            _points = points;
            pointsUi.SetPoints(_points);
        }
    }
}
