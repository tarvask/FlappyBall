using UnityEngine;

namespace FlappyBall
{
    public class LevelModuleFactory
    {
        private readonly LevelModuleSpawnerParameters _spawnParameters;
        private readonly float _gameObjectMaxOffsetX;

        public LevelModuleFactory(LevelModuleSpawnerParameters spawnParameters, float gameObjectMaxOffsetX)
        {
            _spawnParameters = spawnParameters;
            _gameObjectMaxOffsetX = gameObjectMaxOffsetX;
        }

        public LevelModulePresenter SpawnLevelModule(Vector3 newPosition)
        {
            LevelModuleView newModuleView = Object.Instantiate(_spawnParameters.LevelModulePrefab,
                newPosition, Quaternion.identity, null);
            LevelModulePresenter newModule = new LevelModulePresenter(newModuleView, this);

            return newModule;
        }

        public Obstacle SpawnObstacle(Transform rootModule)
        {
            Obstacle obstacle = Object.Instantiate(_spawnParameters.ObstaclePrefab, rootModule);
            obstacle.transform.localPosition = new Vector3(
                    Random.Range(-_gameObjectMaxOffsetX * _spawnParameters.ObstaclesSpreadCoefficient,
                        _gameObjectMaxOffsetX * _spawnParameters.ObstaclesSpreadCoefficient),
                    Random.Range(-_spawnParameters.GameObjectMaxOffsetY, _spawnParameters.GameObjectMaxOffsetY));

            return obstacle;
        }

        public PointItem SpawnPointItem(Transform rootModule, Transform movingTarget)
        {
            PointItem pointItem = Object.Instantiate(_spawnParameters.PointItemPrefab, rootModule);
            pointItem.transform.localPosition = new Vector3(Random.Range(-_gameObjectMaxOffsetX, _gameObjectMaxOffsetX),
                    Random.Range(-_spawnParameters.GameObjectMaxOffsetY, _spawnParameters.GameObjectMaxOffsetY));
            pointItem.Construct(movingTarget);

            return pointItem;
        }
    }
}