using GondrLib.Dependencies;
using GondrLib.ObjectPool.Runtime;
using KHG.Bullets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonerPattern : InfinitePattern
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
        print("실행");
        StartCoroutine(SummonlAttack(PatternList, _activePatterns));
    }
    public override void ExecuteNextPattern(InfinitePatternListSO PatternList, List<InfinitePattern> _activePatterns)
    {
        base.ExecuteNextPattern(PatternList, _activePatterns);
    }

    private IEnumerator SummonlAttack(InfinitePatternListSO PatternList, List<InfinitePattern> _activePatterns)
    {
        _curTime = scoreManager.GetCurrentTime();
            print("시간:"+_curTime);
        for (int i = 0; i < 3; i++)
        {
            print("생성");
            SommonerBullet bullet = poolManager.Pop(poolType) as SommonerBullet;
            bullet.transform.position = Vector3.zero + new Vector3(Random.Range(-25f,25f), -20, 0);
            bullet.MoveSpeed = 15f;
            bullet.transform.rotation = Quaternion.LookRotation(Vector3.zero);
            bullet.ChildSpawnCount = (int)_curTime / 2 + 2;
            bullet.MoveSpeed = 10;
        }
        yield return new WaitForSeconds(1f);

        ExecuteNextPattern(PatternList, _activePatterns);
    }
}
