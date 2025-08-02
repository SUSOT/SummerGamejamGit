using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPattern : InfinitePattern
{
    [SerializeField] private string patternName;
    [SerializeField] private int currentPatternTime;
    public override void Execute(InfinitePatternListSO PatternList, List<InfinitePattern> _activePatterns)
    {
        StartCoroutine(TestCodeLoop(PatternList,_activePatterns,1, currentPatternTime));
    }
    public override void ExecuteNextPattern(InfinitePatternListSO PatternList, List<InfinitePattern> _activePatterns)
    {
        base.ExecuteNextPattern(PatternList,_activePatterns);
    }

    private IEnumerator TestCodeLoop(InfinitePatternListSO PatternList, List<InfinitePattern> _activePatterns, float time,int cnt)
    {
        for (int i = 0; i < cnt; i++)
        {
            yield return new WaitForSeconds(time);
            print($"{patternName} ½ÇÇà! : {i}");
        }
        ExecuteNextPattern(PatternList, _activePatterns);
    }
}
