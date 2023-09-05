using UnityEngine;

namespace FlappyBall
{
    public class LevelModuleView : MonoBehaviour
    {
        [SerializeField] private int size;

        public int Size => size;
    }
}