using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfinitePatternStarter : MonoBehaviour
{
    [SerializeField] private InfinitePatternListSO listSO;

    private List<InfinitePattern> _activePatterns = new();
    private InfinitePatternListSO copiedSO;

    private void OnEnable()
    {
        StartCoroutine(StartExecute());   
    }

    private IEnumerator StartExecute()
    {
        yield return new WaitForSecondsRealtime(0.1f);
        if (listSO == null)
        {
            Debug.LogError("listSO가 할당되지 않았습니다.");
            yield return null;
        }

        copiedSO = Instantiate(listSO);

        if (copiedSO == null)
        {
            Debug.LogError("listSO 복사 실패");
            yield return null;
        }

        _activePatterns = copiedSO.Init(transform, _activePatterns);

        if (_activePatterns == null || _activePatterns.Count == 0)
        {
            Debug.LogError("패턴 리스트가 비어있거나 초기화 실패");
            yield return null;
        }

        InfinitePattern selectedPattern = copiedSO.GetActiveRandomPattern(_activePatterns);
        if (copiedSO == null || _activePatterns == null)
        {
            Debug.LogError("리스트 어ㅗㅄ음");
            yield return null;
        }

        selectedPattern.Execute(copiedSO, _activePatterns);
    }
}
