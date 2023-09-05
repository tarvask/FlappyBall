using System;
using UnityEngine;
using UnityEngine.UI;

namespace FlappyBall.Ui
{
    public class StartUi : MonoBehaviour
    {
        [SerializeField] private Button startButton;

        public Action OnStart;

        private void Awake()
        {
            startButton.onClick.AddListener(() => OnStart?.Invoke());
        }

        private void Update()
        {
            if (Input.GetKey(KeyCode.Space))
                OnStart?.Invoke();
        }

        public void ShowWindow(bool isActive)
        {
            gameObject.SetActive(isActive);
        }
    }
}