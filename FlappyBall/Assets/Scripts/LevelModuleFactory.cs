using System.Collections.Generic;
using UnityEngine;

namespace FlappyBall
{
    public class LevelModuleFactory
    {
        private readonly LevelModuleSpawnerParameters _spawnParameters;
        private readonly float _gameObjectMaxOffsetX;

        private readonly Stack<LevelModulePresenter> _modulesPool;
        private readonly Stack<Obstacle> _obstaclesPool;
        private readonly Stack<PointItem> _pointItemsPool;

        public LevelModuleFactory(LevelModuleSpawnerParameters spawnParameters, float gameObjectMaxOffsetX)
        {
            _spawnParameters = spawnParameters;
            _gameObjectMaxOffsetX = gameObjectMaxOffsetX;

            _modulesPool = new Stack<LevelModulePresenter>(_spawnParameters.ModulesOnSceneMaxCount);
            _obstaclesPool = new Stack<Obstacle>(LevelModulePresenter.ObstaclesHeuristicMaxCount * _spawnParameters.ModulesOnSceneMaxCount);
            _pointItemsPool = new Stack<PointItem>(LevelModulePresenter.PointItemsHeuristicMaxCount * _spawnParameters.ModulesOnSceneMaxCount);
        }

        public LevelModulePresenter SpawnLevelModule(Vector3 newPosition)
        {
            if (_modulesPool.TryPop(out LevelModulePresenter pooledModule))
            {
                pooledModule.SetPosition(newPosition);
                pooledModule.Activate(true);
                return pooledModule;
            }

            return CreateLevelModule(newPosition);
        }

        private LevelModulePresenter CreateLevelModule(Vector3 newPosition)
        {
            LevelModuleView newModuleView = Object.Instantiate(_spawnParameters.LevelModulePrefab,
                newPosition, Quaternion.identity, null);
            LevelModulePresenter newModule = new LevelModulePresenter(newModuleView, this);

            return newModule;
        }

        public Obstacle SpawnObstacle(Transform rootModule)
        {
            Vector3 obstaclePosition = new Vector3(
                Random.Range(-_gameObjectMaxOffsetX * _spawnParameters.ObstaclesSpreadCoefficient,
                    _gameObjectMaxOffsetX * _spawnParameters.ObstaclesSpreadCoefficient),
                Random.Range(-_spawnParameters.GameObjectMaxOffsetY, _spawnParameters.GameObjectMaxOffsetY));
            
            if (_obstaclesPool.TryPop(out Obstacle pooledObstacle))
            {
                pooledObstacle.transform.SetParent(rootModule);
                pooledObstacle.transform.localPosition = obstaclePosition;
                pooledObstacle.gameObject.SetActive(true);
                return pooledObstacle;
            }
            
            return CreateObstacle(obstaclePosition, rootModule);
        }

        private Obstacle CreateObstacle(Vector3 obstaclePosition, Transform rootModule)
        {
            return Object.Instantiate(_spawnParameters.ObstaclePrefab, obstaclePosition, Quaternion.identity, rootModule);
        }

        public PointItem SpawnPointItem(Transform rootModule, Transform movingTarget)
        {
            Vector3 pointItemPosition = new Vector3(Random.Range(-_gameObjectMaxOffsetX, _gameObjectMaxOffsetX),
                Random.Range(-_spawnParameters.GameObjectMaxOffsetY, _spawnParameters.GameObjectMaxOffsetY));
            
            if (_pointItemsPool.TryPop(out PointItem pooledPointItem))
            {
                pooledPointItem.transform.SetParent(rootModule);
                pooledPointItem.transform.localPosition = pointItemPosition;
                pooledPointItem.Construct(movingTarget);
                pooledPointItem.gameObject.SetActive(true);
                return pooledPointItem;
            }
            
            return CreatePointItem(pointItemPosition, rootModule, movingTarget);
        }
        
        private PointItem CreatePointItem(Vector3 pointItemPosition, Transform rootModule, Transform movingTarget)
        {
            PointItem pointItem = Object.Instantiate(_spawnParameters.PointItemPrefab, rootModule);
            pointItem.transform.localPosition = pointItemPosition;
            pointItem.Construct(movingTarget);
            return pointItem;
        }

        public void ReturnSpawnLevelModule(LevelModulePresenter levelModulePresenter)
        {
            levelModulePresenter.Activate(false);
            _modulesPool.Push(levelModulePresenter);
        }

        public void ReturnObstacle(Obstacle obstacle)
        {
            obstacle.transform.SetParent(null);
            obstacle.gameObject.SetActive(false);
            _obstaclesPool.Push(obstacle);
        }
        
        public void ReturnPointItem(PointItem pointItem)
        {
            pointItem.transform.SetParent(null);
            pointItem.gameObject.SetActive(false);
            _pointItemsPool.Push(pointItem);
        }
    }
}