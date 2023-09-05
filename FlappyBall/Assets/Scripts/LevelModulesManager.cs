using System.Collections.Generic;
using UnityEngine;

namespace FlappyBall
{
    public class LevelModulesManager
    {
        // dependencies
        private readonly BallMono _ball;
        private readonly LevelModuleSpawnerParameters _levelModuleSpawnerParameters;
        
        private readonly Queue<LevelModulePresenter> _modules;
        private readonly LevelModuleFactory _levelModuleFactory;
        private readonly Vector3 _spawnPositionDelta;
        private Vector3 _lastModulePosition;
        private int _currentDifficultyLevel;
        
        public LevelModulesManager(BallMono ball, LevelModuleSpawnerParameters levelModuleSpawnerParameters)
        {
            _ball = ball;
            _levelModuleSpawnerParameters = levelModuleSpawnerParameters;
            
            _modules = new Queue<LevelModulePresenter>(_levelModuleSpawnerParameters.ModulesOnSceneMaxCount);
            _levelModuleFactory = new LevelModuleFactory(_levelModuleSpawnerParameters,
                _levelModuleSpawnerParameters.LevelModulePrefab.Size * 0.5f);
            _spawnPositionDelta = new Vector3(_levelModuleSpawnerParameters.LevelModulePrefab.Size, 0, 0);
        }

        public void OuterUpdate()
        {
            if (_ball.Position.x > _lastModulePosition.x - _spawnPositionDelta.x * ((_levelModuleSpawnerParameters.ModulesOnSceneMaxCount / 2) - 0.5f))
                OnBallCrossedFrontBorder();
        }

        public void OuterFixedUpdate()
        {
            foreach (LevelModulePresenter module in _modules)
                module.OuterFixedUpdate();
        }

        public void UpdateWithPoints(int pointsCount)
        {
            _currentDifficultyLevel = pointsCount;
        }

        public void StartSpawn()
        {
            while (_modules.Count > 0)
                _modules.Dequeue().Remove();
            
            Vector3 currentModulePosition = -(_levelModuleSpawnerParameters.ModulesOnSceneMaxCount / 2) * _spawnPositionDelta;

            for (int i = 0; i < _levelModuleSpawnerParameters.ModulesOnSceneMaxCount; i++)
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
            if (_modules.Count == _levelModuleSpawnerParameters.ModulesOnSceneMaxCount
                && _modules.TryDequeue(out LevelModulePresenter tailModule))
                tailModule.Remove();
        }

        private void SpawnNewModule(Vector3 newPosition, bool spawnGameObjects = true)
        {
            LevelModulePresenter newModule = _levelModuleFactory.SpawnLevelModule(newPosition);
            _modules.Enqueue(newModule);
            _lastModulePosition = newPosition;

            if (spawnGameObjects)
                newModule.SpawnObjectsForModule(_currentDifficultyLevel, _levelModuleSpawnerParameters.PointItemsMaxCount,
                    _ball.transform);
        }
    }
}