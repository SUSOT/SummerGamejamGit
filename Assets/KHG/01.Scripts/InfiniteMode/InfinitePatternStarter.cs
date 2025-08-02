using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InfinitePatternStarter : MonoBehaviour
{
    [SerializeField] private InfinitePatternListSO listSO;

    private List<InfinitePattern> _activePatterns = new();
    private InfinitePatternListSO copiedSO;
    private void Start()
    {
        copiedSO = Instantiate(listSO);

        _activePatterns = copiedSO.Init(transform, _activePatterns);
        copiedSO.GetActiveRandomPattern(_activePatterns).Execute(listSO, _activePatterns);
    }
}
