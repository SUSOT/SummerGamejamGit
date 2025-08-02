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
            bullet.transform.position = transform.position + new Vector3(Random.Range(-7f,7f), Random.Range(-5f, 5f), 0);
            bullet.MoveSpeed = 15f;
            bullet.transform.rotation = Quaternion.Euler(0,0,Random.Range(-180,180));
            bullet.ChildSpawnCount = (int)_curTime / 2 + 2;
            bullet.MoveSpeed = 0;
            yield return new WaitForSeconds(1 / _curTime);
        }

        ExecuteNextPattern(PatternList, _activePatterns);
    }
}
