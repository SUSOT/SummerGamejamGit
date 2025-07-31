using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Serialization;

namespace LKW._01.Scripts.TimeLine
{
    [CreateAssetMenu(fileName = "PatternDataList", menuName = "SO/TimeLine/PatternDataList", order = 1)]
    public class PatternDataListSO : ScriptableObject
    {
        public List<PatternDataSO> patterns;
    }
}