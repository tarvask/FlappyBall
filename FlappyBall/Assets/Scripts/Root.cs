using FlappyBall.Ui;
using UnityEngine;

namespace FlappyBall
{
    public class Root : MonoBehaviour
    {
        [SerializeField] private LevelModuleSpawnerParameters levelModuleSpawnerParameters;
        
        [SerializeField] private CameraFollow cameraFollow;
        [SerializeField] private BallMono ball;
        
        [Header("UI")]
        [SerializeField] private StartUi startUi;
        [SerializeField] private PointsUi pointsUi;
        
        private LevelModulesManager _modulesManager;
        private int _points;

        private void Start()
        {
            Application.targetFrameRate = 60;
            cameraFollow.Construct(ball.transform);
            _modulesManager = new LevelModulesManager(ball, levelModuleSpawnerParameters);
            _modulesManager.StartSpawn();
            
            startUi.OnStart += OnGameStarted;
            
            ball.OnPointsCollected += OnPointsCollectedHandler;
            ball.OnBallCrashed += OnBallCrashedHandler;
            ball.ReturnToStart();
        }
        
        private void Update()
        {
            _modulesManager.OuterUpdate();
        }

        private void FixedUpdate()
        {
            if (Input.GetKey(KeyCode.Space))
                ball.Jump();
            
            _modulesManager.OuterFixedUpdate();
            cameraFollow.OuterFixedUpdate();
        }

        private void OnGameStarted()
        {
            startUi.ShowWindow(false);
            ball.Launch();
        }

        private void OnPointsCollectedHandler(int pointsCount)
        {
            _points += pointsCount;
            _modulesManager.UpdateWithPoints(_points);
            pointsUi.SetPoints(_points);
        }

        private void OnBallCrashedHandler()
        {
            _points = 0;
            _modulesManager.UpdateWithPoints(_points);
            pointsUi.SetPoints(_points);
            
            ball.ReturnToStart();
            _modulesManager.StartSpawn();
            
            startUi.ShowWindow(true);
        }
    }
}