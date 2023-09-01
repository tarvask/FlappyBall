using System;
using UnityEngine;
using UnityEngine.UI;

namespace FlappyBall
{
    public class StartUi : MonoBehaviour
    {
        [SerializeField] private Button startButton;

        public Action OnStart;

        private void Awake()
        {
            startButton.onClick.AddListener(StartGame);
        }

        private void Update()
        {
            if (Input.GetKey(KeyCode.Space))
                StartGame();
        }

        private void StartGame()
        {
            ShowWindow(false);
            OnStart?.Invoke();
        }

        public void ShowWindow(bool isActive)
        {
            gameObject.SetActive(isActive);
        }
    }
}