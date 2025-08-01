using TMPro;
using UnityEngine;

namespace LKW._01.Scripts.LeaderBoard
{
    public class RankBox : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private TextMeshProUGUI scoreText;
        [SerializeField] private TextMeshProUGUI numberText;

        public void SetRankBox(string name, string time, int number)
        {
            numberText.text = $"{number.ToString()}.";
            nameText.text = name;
            scoreText.text = time;
        }
    }
}