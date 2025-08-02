using GondrLib.Dependencies;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPattern : InfinitePattern 
{
    [Inject] private InfiniteScoreManager scoreManager;
    [SerializeField] private string patternName;
    [SerializeField] private int currentPatternTime;

    private void OnEnable()
    {
        Injector.Instance.InjectRuntime(this);
    }
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
            float duration = time - scoreManager.GetCurrentTime() / 100;
            yield return new WaitForSeconds(duration);
            print($"{patternName} 실행! : {i},간격:{duration}");
        }
        ExecuteNextPattern(PatternList, _activePatterns);
    }
}
