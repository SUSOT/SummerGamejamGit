using TMPro;
using UnityEngine;

namespace LKW._01.Scripts.LeaderBoard
{
    public class RankBox : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private TextMeshProUGUI scoreText;

        public void SetRankBox(string name, int score)
        {
            nameText.text = name;
            scoreText.text = score.ToString();
        }
    }
}