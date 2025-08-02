using GondrLib.Dependencies;
using GondrLib.ObjectPool.Runtime;
using KHG.Bullets;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.EditorTools;
using UnityEngine;

public class ExplosionPattern : InfinitePattern
{
    [Inject] private InfiniteScoreManager scoreManager;
    [SerializeField] private PoolManagerSO poolManager;
    [SerializeField] private PoolingItemSO poolType;

    private float _curTime;

    private void OnEnable()
    {
        Injector.Instance.InjectRuntime(this);
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
        _curTime = scoreManager.GetCurrentTime();
        int repeatCnt = (int)(_curTime / 2) + 3;
        for (int i = 1; i <= repeatCnt; i++)
        {
            ExplodeBullet bullet = poolManager.Pop(poolType) as ExplodeBullet;
            bullet.transform.position = Vector3.zero;
            bullet.moveable = true;
            bullet.targetPosition = Vector3.zero + new Vector3(Random.Range(-15,15),Random.Range(-7,7));

            float waitTime = _curTime == 0 ? 2 : 1 / _curTime * 2 + 0.5f;
            yield return new WaitForSeconds(waitTime);
        }
        yield return new WaitForSeconds(2f);
        ExecuteNextPattern(PatternList,_activePatterns);
    }
}
