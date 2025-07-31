using System.Collections.Generic;
using LKW._01.Scripts.TimeLine;
using UnityEngine;

public class TimeLineManager : MonoBehaviour
{
    [SerializeField] private PatternDataListSO patternList;
    
    private Queue<PatternDataSO> patternQueue;

    private float currentTime;

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        currentTime = 0f;

        if (patternList == null || patternList.patterns.Count == 0)
        {
            patternQueue = new Queue<PatternDataSO>();
            return;
        }

        patternList.patterns.Sort((a, b) => a.StartTime.CompareTo(b.StartTime));

        patternQueue = new Queue<PatternDataSO>(patternList.patterns);
    }

    private void Update()
    {
        currentTime += Time.deltaTime;
        CheckPatterns();
    }

    private void CheckPatterns()
    {
        while (patternQueue.Count > 0 && currentTime >= patternQueue.Peek().StartTime)
        {
            TimeLinePattern patternObj = patternQueue.Dequeue().pattern;
            Instantiate(patternObj.gameObject).GetComponent<TimeLinePattern>().Execute();

            //TimeLinePattern pattern = patternQueue.Dequeue().pattern;
            //pattern.Execute();
        }
    }
}