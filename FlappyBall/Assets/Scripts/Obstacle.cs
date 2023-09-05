using UnityEngine;

namespace FlappyBall
{
    public class Obstacle : MonoBehaviour
    {
        [SerializeField] private Vector3 offsetDelta;
        [SerializeField] private float velocity;

        private Transform _transform;
        private Vector3 _startPosition;
        private Vector3[] _targetPositions;
        private int _currentTargetIndex;

        private void Awake()
        {
            _transform = GetComponent<Transform>();
        }

        private void OnEnable()
        {
            Prepare();
        }

        private void Prepare()
        {
            _startPosition = _transform.localPosition;
            _targetPositions = new[]
            {
                _startPosition - offsetDelta,
                _startPosition + offsetDelta
            };

            // randomize first target
            _currentTargetIndex = Random.Range(0, _targetPositions.Length);
        }

        public void OuterFixedUpdate()
        {
            _transform.localPosition = Vector3.MoveTowards(_transform.localPosition,
                _targetPositions[_currentTargetIndex], velocity * Time.deltaTime);

            if ((_targetPositions[_currentTargetIndex] - _transform.localPosition).sqrMagnitude < 0.0001)
                _currentTargetIndex = ++_currentTargetIndex % _targetPositions.Length;
        }
    }
}