using UnityEngine;

namespace FlappyBall
{
    [CreateAssetMenu(fileName = "LevelModuleSpawnerParameters", menuName = "Configs/LevelModuleSpawnerParameters")]
    public class LevelModuleSpawnerParameters : ScriptableObject
    {
        [SerializeField] private LevelModuleView levelModulePrefab;
        [SerializeField] private Obstacle obstaclePrefab;
        [SerializeField] private PointItem pointItemPrefab;
        [SerializeField] private int modulesOnSceneMaxCount;
        [SerializeField] private float gameObjectMaxOffsetY;
        [SerializeField] private float obstaclesSpreadCoefficient;
        [SerializeField] private int pointItemsMaxCount;

        public LevelModuleView LevelModulePrefab => levelModulePrefab;
        public Obstacle ObstaclePrefab => obstaclePrefab;
        public PointItem PointItemPrefab => pointItemPrefab;
        public int ModulesOnSceneMaxCount => modulesOnSceneMaxCount;
        public float GameObjectMaxOffsetY => gameObjectMaxOffsetY;
        public float ObstaclesSpreadCoefficient => obstaclesSpreadCoefficient;
        public int PointItemsMaxCount => pointItemsMaxCount;
    }
}