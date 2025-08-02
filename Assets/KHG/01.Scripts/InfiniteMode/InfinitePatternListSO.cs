using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InfinitePatternListSO", menuName = "SO/InfiniteTimeLine/InfinitePatternListSO")]
public class InfinitePatternListSO : ScriptableObject
{
    public List<InfinitePatternSO> patterns = new();

    private List<int> played = new();
    public InfinitePattern GetActiveRandomPattern(List<InfinitePattern> _activePatterns)
    {
        int index = Random.Range(0, _activePatterns.Count);
        if (_activePatterns == null || _activePatterns.Count < index)
        {
            Debug.LogError($"패턴 리스트 또는 요청한 패턴이 없스빈다");
        };

        while(played.Contains(index))
        {
            if(played.Count >= _activePatterns.Count) played.Clear();
            index = Random.Range(0, _activePatterns.Count);
            Debug.Log($"{played.Count} played patterns");
        }
        return _activePatterns[index];
    }

    public List<InfinitePattern> Init(Transform self, List<InfinitePattern> _activePatterns)
    {
        foreach(var ptn in _activePatterns)
        {
            Destroy(ptn.gameObject);//에러날수도
        }
        _activePatterns.Clear();
        foreach (var pattern in patterns)
        {
            GameObject obj = Instantiate(pattern.Pattern.gameObject, self);
            InfinitePattern ptn = obj.GetComponent<InfinitePattern>();
            _activePatterns.Add(ptn);
        }
        return _activePatterns;
    }
}
