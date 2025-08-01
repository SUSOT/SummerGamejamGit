using GondrLib.Dependencies;
using GondrLib.ObjectPool.Runtime;
using KHG.Bullets;
using LCM._01.Scripts.Bullets;
using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Pattern5 : TimeLinePattern
{

    [SerializeField] private PoolingItemSO triangle;
    [SerializeField] private PoolingItemSO boomBullet;
    [SerializeField] private int spawnCount;
     private List<Vector2> spawnPos = new List<Vector2>
    {
         new Vector2(-12.4f, 7.8f),
         new Vector2(8.3f, -6.2f),
         new Vector2(-5.7f, 3.1f),
         new Vector2(14.2f, -8.9f),
         new Vector2(-9.8f, 9.5f),
         new Vector2(2.6f, -4.3f),
         new Vector2(-14.1f, 1.7f),
         new Vector2(11.5f, 8.4f),
         new Vector2(-3.2f, -7.6f),
         new Vector2(6.9f, 5.2f),
         new Vector2(-13.7f, -2.8f),
         new Vector2(4.1f, 9.7f),
         new Vector2(-7.5f, -9.1f),
         new Vector2(12.8f, 2.4f),
         new Vector2(-1.6f, 6.3f)
    };
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
        TriangleCannon tc = _poolManager.Pop<TriangleCannon>(triangle);
        tc.transform.position = new Vector3(0, 30, 0);
        tc.FireDuration = 0.4f;
        tc.transform.DOMove(Vector3.zero, 3f).OnComplete(() =>
        {
            tc.StartInfiniteRotation();
        });

        yield return new WaitForSeconds(1.2f);

        for(int i = 0; i < spawnCount; i++)
        {
            ExplodeBullet explodeBullet = _poolManager.Pop<ExplodeBullet>(boomBullet);
            
            explodeBullet.moveable = true;
            explodeBullet.targetPosition = spawnPos[i];
            if(i % 2 == 0)
            {
                explodeBullet.SpawnPosition = new Vector2(0, 20);
            }
            else
            {
                explodeBullet.SpawnPosition = new Vector2(0, -20);
            }

            yield return new WaitForSeconds(1f);
        }
    }
}
