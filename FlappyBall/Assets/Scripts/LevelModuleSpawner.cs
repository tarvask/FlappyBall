using System.Collections.Generic;
using UnityEngine;

namespace FlappyBall
{
    public class LevelModuleSpawner : MonoBehaviour
    {
        [SerializeField] private BallMono ball;
        [SerializeField] private LevelModule modulePrefab;
        [SerializeField] private GameObject obstaclePrefab;
        [SerializeField] private PointItem pointItemPrefab;
        [SerializeField] private int modulesOnSceneMaxCount;
        [SerializeField] private float gameObjectMaxOffsetY;
        [SerializeField] private float obstaclesSpreadCoefficient;
        [SerializeField] private int pointItemsMaxCount;

        private Queue<GameObject> _modules;
        private Vector3 _lastModulePosition;
        private Vector3 _spawnPositionDelta;

        private void Awake()
        {
            _modules = new Queue<GameObject>(modulesOnSceneMaxCount);
            _spawnPositionDelta = new Vector3(modulePrefab.Size, 0, 0);
            ball.OnReturnedToStart += StartSpawn;
        }

        private void Update()
        {
            if (ball.Position.x > _lastModulePosition.x - _spawnPositionDelta.x * ((modulesOnSceneMaxCount / 2) - 0.5f))
                OnBallCrossedFrontBorder();
        }

        private void StartSpawn()
        {
            while (_modules.Count > 0)
                Destroy(_modules.Dequeue());
            
            Vector3 currentModulePosition = -(modulesOnSceneMaxCount / 2) * _spawnPositionDelta;

            for (int i = 0; i < modulesOnSceneMaxCount; i++)
            {
                SpawnNewModule(currentModulePosition, false);
                // compute new position
                currentModulePosition += _spawnPositionDelta;
            }
        }

        private void OnBallCrossedFrontBorder()
        {
            TryRemoveTailModule();
            
            // compute new position
            _lastModulePosition += _spawnPositionDelta;

            SpawnNewModule(_lastModulePosition);
        }
        
        private void TryRemoveTailModule()
        {
            // remove tail module
            if (_modules.Count == modulesOnSceneMaxCount && _modules.TryDequeue(out GameObject tailModule))
                Destroy(tailModule);
        }

        private void SpawnNewModule(Vector3 newPosition, bool spawnGameObjects = true)
        {
            // create new module
            LevelModule newModule = Instantiate(modulePrefab, newPosition, Quaternion.identity, null);
            _modules.Enqueue(newModule.gameObject);
            _lastModulePosition = newPosition;

            if (spawnGameObjects)
            {
                SpawnObstaclesForModule(newModule.transform,
                    Mathf.Max(1, ball.Points),
                    modulePrefab.Size * 0.5f,
                    gameObjectMaxOffsetY,
                    obstaclesSpreadCoefficient);
                SpawnPointItemsForModule(newModule.transform,
                    Random.Range(0, pointItemsMaxCount + 1),
                    modulePrefab.Size * 0.5f,
                    gameObjectMaxOffsetY,
                    ball.transform);
            }
        }

        private void SpawnObstaclesForModule(Transform rootModule, int obstaclesCount,
            float obstacleMaxOffsetX, float obstacleMaxOffsetY, float spreadCoefficientX)
        {
            for (int i = 0; i < obstaclesCount; i++)
            {
                GameObject obstacle = Instantiate(obstaclePrefab, rootModule);
                obstacle.transform.localPosition = new Vector3(
                    Random.Range(-obstacleMaxOffsetX * spreadCoefficientX, obstacleMaxOffsetX * spreadCoefficientX),
                    Random.Range(-obstacleMaxOffsetY, obstacleMaxOffsetY));
            }
        }

        private void SpawnPointItemsForModule(Transform rootModule, int pointItemsCount,
            float pointItemMaxOffsetX, float pointItemMaxOffsetY, Transform movingTarget)
        {
            for (int i = 0; i < pointItemsCount; i++)
            {
                PointItem pointItem = Instantiate(pointItemPrefab, rootModule);
                pointItem.transform.localPosition = new Vector3(Random.Range(-pointItemMaxOffsetX, pointItemMaxOffsetX),
                    Random.Range(-pointItemMaxOffsetY, pointItemMaxOffsetY));
                pointItem.SetTarget(movingTarget);
            }
        }
    }
}