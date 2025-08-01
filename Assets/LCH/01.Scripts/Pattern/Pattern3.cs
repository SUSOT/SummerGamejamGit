using GondrLib.Dependencies;
using GondrLib.ObjectPool.Runtime;
using LCM._01.Scripts.Bullets;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pattern3 : TimeLinePattern
{
    [SerializeField] private PoolingItemSO guidedMissileItem;
    [SerializeField] private PoolingItemSO wallItem;
    [SerializeField] private List<Vector2> spawnPos;
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
        for(int i = 0; i < 2; i++)
        {
            WallBullet wallBullet = _poolManager.Pop<WallBullet>(wallItem);
            wallBullet.transform.position = spawnPos[i];
            wallBullet.MoveSpeed += 7f;
            if (wallBullet.transform.position.x > 0)
            {
                wallBullet.MoveDirection = Vector2.left;
                
            }
            else if (wallBullet.transform.position.x < 0)
            {
                wallBullet.MoveDirection = Vector2.right;
            }
            for (int j = i +2; j <= spawnPos.Count; j++)
            {
                MissileBullet missileBullet = _poolManager.Pop<MissileBullet>(guidedMissileItem);
                missileBullet.transform.position = spawnPos[i];
                missileBullet.MoveSpeed += 5f;
                missileBullet.MissileTime = 4f;
                yield return new WaitForSeconds(0.3f);
            }

            yield return new WaitForSeconds(1f);
        }
        
    }
}
