using GondrLib.Dependencies;
using GondrLib.ObjectPool.Runtime;
using KHG.Bullets;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.EditorTools;
using UnityEngine;

public class NailPattern : InfinitePattern
{
    [Inject] private InfiniteScoreManager scoreManager;
    [SerializeField] private PoolManagerSO poolManager;
    [SerializeField] private PoolingItemSO poolType;

    private float _curTime;

    private void OnEnable()
    {
        Injector.Instance.InjectRuntime(this);
        _curTime = scoreManager.GetCurrentTime();
    }
    public override void Execute(InfinitePatternListSO PatternList, List<InfinitePattern> _activePatterns)
    {
        StartCoroutine(NailAttack(PatternList, _activePatterns));
    }
    public override void ExecuteNextPattern(InfinitePatternListSO PatternList, List<InfinitePattern> _activePatterns)
    {
        base.ExecuteNextPattern(PatternList, _activePatterns);
    }

    private IEnumerator NailAttack(InfinitePatternListSO PatternList, List<InfinitePattern> _activePatterns)
    {
        for (int i = 0; i < (_curTime / 3) + 2; i++)
        {
            NailBullet bullet = poolManager.Pop(poolType) as NailBullet;
            bullet.SetSpawnValues(transform.position, new Vector3(0, 0, 360 / i),50);
            yield return new WaitForSeconds(1 / _curTime);
        }
        ExecuteNextPattern(PatternList,_activePatterns);
    }
}
