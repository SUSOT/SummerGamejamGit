using System.Collections.Generic;
using UnityEngine;

public abstract class InfinitePattern : MonoBehaviour
{
    //public InfinitePatternListSO PatternList; 매개변수로 줘볼까
    public abstract void Execute(InfinitePatternListSO PatternList, List<InfinitePattern> _activePatterns);
    public virtual void ExecuteNextPattern(InfinitePatternListSO PatternList, List<InfinitePattern> _activePatterns)
    {
        PatternList.GetActiveRandomPattern(_activePatterns).Execute(PatternList, _activePatterns);
    }
}
