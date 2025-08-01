using UnityEngine;
using TMPro;

public class InfiniteScoreManager : MonoBehaviour
{
    public TextMeshProUGUI timeText;

    private float startTime;
    private bool isCounting;

    private const string SaveKey = "InfiniteModeBestTime";


    private void Start()
    {
        CountStart();
    }
    void Update()
    {
        if (isCounting)
        {
            float currentElapsedTime = Time.time - startTime;
            UpdateTimeUI(currentElapsedTime);
        }
    }

    public void CountStart()
    {
        startTime = Time.time;
        isCounting = true;
    }

    public void CountEnd()
    {
        if (!isCounting) return;

        float finalTime = Time.time - startTime;
        isCounting = false;

        SaveScore(Mathf.RoundToInt(finalTime));
        UpdateTimeUI(finalTime);
    }

    private void UpdateTimeUI(float time)
    {
        timeText.text = Mathf.FloorToInt(time).ToString("F0");
    }

    public void SaveScore(int score) //임시
    {
        int bestScore = LoadScore();
        if (score > bestScore)
        {
            PlayerPrefs.SetInt(SaveKey, score);
            PlayerPrefs.Save();
        }
    }

    public int LoadScore() //임시
    {
        return PlayerPrefs.GetInt(SaveKey, 0);
    }

    public float GetCurrentTime()
    {
        return isCounting ? Time.time - startTime : 0f;
    }
}
