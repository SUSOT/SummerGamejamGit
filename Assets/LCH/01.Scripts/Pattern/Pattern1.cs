using GondrLib.Dependencies;
using GondrLib.ObjectPool.Runtime;
using LCM._01.Scripts.Bullets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pattern1 : TimeLinePattern
{

    [SerializeField] private PoolingItemSO normalItem;
    [SerializeField] private int SpawnCount;
    [SerializeField] private List<Vector2> spawnPoints;
    [SerializeField] private float moveSpeed = 5f;
    [Inject] private PoolManagerMono _poolManager;

    private void OnEnable()
    {
        Injector.Instance.InjectRuntime(this);
    }

    public override void Execute()
    {
        StartCoroutine(Spawn());
    }

    private IEnumerator Spawn()
    {
        for(int i = 0; i < spawnPoints.Count; i++)
        {
            for (int j = 0; j < SpawnCount; j++)
            {
                NormalBullet obj = _poolManager.Pop<NormalBullet>(normalItem);
                obj.transform.position = spawnPoints[i];
                obj.moveSpeed += moveSpeed;
                obj.MoveDirection = -spawnPoints[i];
                yield return new WaitForSeconds(0.8f);
            }
            yield return new WaitForSeconds(0.5f);
        }
    }
}
