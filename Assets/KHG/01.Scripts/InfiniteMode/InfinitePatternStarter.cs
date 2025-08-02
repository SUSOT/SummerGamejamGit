using System.Collections.Generic;
using UnityEngine;

public class InfinitePatternStarter : MonoBehaviour
{
    [SerializeField] private InfinitePatternListSO listSO;

    private List<InfinitePattern> _activePatterns = new();
    private InfinitePatternListSO copiedSO;

    private void OnEnable()
    {
        if (listSO == null)
        {
            Debug.LogError("listSO가 할당되지 않았습니다.");
            return;
        }

        copiedSO = Instantiate(listSO);

        if (copiedSO == null)
        {
            Debug.LogError("listSO 복사 실패");
            return;
        }

        _activePatterns = copiedSO.Init(transform, _activePatterns);

        if (_activePatterns == null || _activePatterns.Count == 0)
        {
            Debug.LogError("패턴 리스트가 비어있거나 초기화 실패");
            return;
        }

        InfinitePattern selectedPattern = copiedSO.GetActiveRandomPattern(_activePatterns);
        if (selectedPattern == null)
        {
            Debug.LogError("랜덤 패턴 선택 실패");
            return;
        }

        selectedPattern.Execute(copiedSO, _activePatterns);
    }
}
