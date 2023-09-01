using TMPro;
using UnityEngine;

namespace FlappyBall
{
    public class PointsUi : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI pointsText;

        public void SetPoints(int points)
        {
            pointsText.text = $"{points}";
        }
    }
}