using System.Collections.Generic;
using UnityEngine;

namespace FlappyBall
{
    public class LevelModulePresenter
    {
        public const int ObstaclesHeuristicMaxCount = 16;
        public const int PointItemsHeuristicMaxCount = 1;
        
        private readonly LevelModuleView _view;
        private readonly LevelModuleFactory _factory;

        private readonly List<Obstacle> _obstacles;
        private readonly List<PointItem> _pointItems;

        public LevelModulePresenter(LevelModuleView view, LevelModuleFactory factory)
        {
            _view = view;
            _factory = factory;

            _obstacles = new List<Obstacle>(ObstaclesHeuristicMaxCount);
            _pointItems = new List<PointItem>(PointItemsHeuristicMaxCount);
        }

        public void Activate(bool setActive)
        {
            _view.gameObject.SetActive(setActive);
        }

        public void SetPosition(Vector3 newPosition)
        {
            _view.transform.localPosition = newPosition;
        }

        public void Remove()
        {
            foreach (Obstacle obstacleObject in _obstacles)
                _factory.ReturnObstacle(obstacleObject);

            foreach (PointItem pointItemObject in _pointItems)
                _factory.ReturnPointItem(pointItemObject);
            
            _obstacles.Clear();
            _pointItems.Clear();
            
            _factory.ReturnSpawnLevelModule(this);
        }

        public void OuterFixedUpdate()
        {
            foreach (Obstacle obstacleObject in _obstacles)
                obstacleObject.OuterFixedUpdate();

            foreach (PointItem pointItemObject in _pointItems)
                pointItemObject.OuterFixedUpdate();
        }

        public void SpawnObjectsForModule(int currentDifficultyLevel, int pointItemsMaxCount, Transform pointItemTarget)
        {
            SpawnObstaclesForModule(Mathf.Max(1, currentDifficultyLevel));
            SpawnPointItemsForModule(Random.Range(0, pointItemsMaxCount + 1), pointItemTarget);
        }
        
        private void SpawnObstaclesForModule(int obstaclesCount)
        {
            for (int i = 0; i < obstaclesCount; i++)
            {
                Obstacle obstacle = _factory.SpawnObstacle(_view.transform);
                _obstacles.Add(obstacle);
            }
        }

        private void SpawnPointItemsForModule(int pointItemsCount, Transform movingTarget)
        {
            for (int i = 0; i < pointItemsCount; i++)
            {
                PointItem pointItem = _factory.SpawnPointItem(_view.transform, movingTarget);
                _pointItems.Add(pointItem);
                pointItem.OnCollected += RemovePointItem;
            }
        }

        private void RemovePointItem(PointItem pointItem)
        {
            pointItem.OnCollected -= RemovePointItem;
            _pointItems.Remove(pointItem);
            _factory.ReturnPointItem(pointItem);
        }
    }
}