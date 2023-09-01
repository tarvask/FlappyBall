using UnityEngine;

namespace FlappyBall
{
    public class LevelModule : MonoBehaviour
    {
        [SerializeField] private int size;

        public int Size => size;
    }
}