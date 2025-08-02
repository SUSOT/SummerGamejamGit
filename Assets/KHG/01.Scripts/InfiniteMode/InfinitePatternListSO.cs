using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InfinitePatternListSO", menuName = "SO/InfiniteTimeLine/InfinitePatternListSO")]
public class InfinitePatternListSO : ScriptableObject
{
    public List<InfinitePatternSO> patterns = new();
    private List<int> played = new();

    public InfinitePattern GetActiveRandomPattern(List<InfinitePattern> _activePatterns)
    {
        if (_activePatterns == null || _activePatterns.Count == 0)
        {
            Debug.LogError("패턴 리스트가 비어 있습니다.");
            return null;
        }

        if (played.Count >= _activePatterns.Count)
        {
            played.Clear();
        }

        int index = Random.Range(0, _activePatterns.Count);

        int safety = 0; // 무한 루프 방지
        while (played.Contains(index) && safety < 100)
        {
            index = Random.Range(0, _activePatterns.Count);
            safety++;
        }

        played.Add(index);
        return _activePatterns[index];
    }

    public List<InfinitePattern> Init(Transform self, List<InfinitePattern> _activePatterns)
    {
        foreach (var ptn in _activePatterns)
        {
#if UNITY_EDITOR
            Object.DestroyImmediate(ptn?.gameObject);  // 에디터에서는 즉시 파괴
#else
            Object.Destroy(ptn?.gameObject); // 런타임에서는 일반 파괴
#endif
        }

        _activePatterns.Clear();

        foreach (var pattern in patterns)
        {
            if (pattern == null || pattern.Pattern == null)
            {
                Debug.LogWarning("패턴이 null입니다. 건너뜁니다.");
                continue;
            }

            GameObject obj = Instantiate(pattern.Pattern.gameObject, self);
            InfinitePattern ptn = obj.GetComponent<InfinitePattern>();

            if (ptn == null)
            {
                Debug.LogError("패턴 프리팹에 InfinitePattern 컴포넌트가 없습니다.");
                continue;
            }

            _activePatterns.Add(ptn);
        }

        return _activePatterns;
    }
}
