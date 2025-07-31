using System;
using UnityEngine;

namespace LKW._01.Scripts.TimeLine
{
    [CreateAssetMenu(fileName = "PatternData", menuName = "SO/TimeLine/PatternData", order = 0)]
    public class PatternDataSO : ScriptableObject
    {
        public float StartTime;
        
        public TimeLinePattern pattern;

        private void OnEnable()
        {
        }
    }
}