using GondrLib.Dependencies;
using GondrLib.ObjectPool.Runtime;
using KHG.Bullets;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pattern2 : TimeLinePattern
{

    [SerializeField] private PoolingItemSO boomItem;
    [SerializeField] private PoolingItemSO boomBulletItem;
    [SerializeField] private int SpawnCount;
    private List<Vector2> spawnPoints = new List<Vector2>
{
    new Vector2(-32.4f, 24.7f),
    new Vector2(31.2f, -25.3f),
    new Vector2(-34.1f, 19.8f),
    new Vector2(33.7f, 28.4f),
    new Vector2(-30.5f, -22.6f),
    new Vector2(29.8f, 21.9f),
    new Vector2(-33.2f, -28.1f),
    new Vector2(32.6f, 26.3f),
    new Vector2(-31.7f, 23.5f),
    new Vector2(34.3f, -24.8f),
    new Vector2(-29.3f, -19.7f),
    new Vector2(30.9f, 29.2f),
    new Vector2(-32.8f, 18.6f),
    new Vector2(33.1f, -26.4f),
    new Vector2(-34.5f, 25.1f),
    new Vector2(29.6f, -21.3f),
    new Vector2(-30.2f, 27.8f),
    new Vector2(32.9f, 22.7f),
    new Vector2(-31.4f, -23.9f),
    new Vector2(34.8f, 20.4f),
    new Vector2(-33.6f, -27.2f),
    new Vector2(31.5f, 24.6f),
    new Vector2(-29.7f, 28.9f),
    new Vector2(30.3f, -25.7f),
    new Vector2(-32.1f, 21.2f),
    new Vector2(33.4f, -29.5f),
    new Vector2(-34.2f, 26.8f),
    new Vector2(31.8f, 19.3f),
    new Vector2(-30.6f, -20.4f),
    new Vector2(32.3f, 27.6f)
};
 private  List<Vector2> movePoints = new List<Vector2>
{
    new Vector2(-18.4f, 7.2f),
    new Vector2(15.3f, -1.8f),
    new Vector2(-21.2f, 11.5f),
    new Vector2(19.7f, 4.3f),
    new Vector2(-8.5f, -2.6f),
    new Vector2(12.8f, 9.1f),
    new Vector2(-16.3f, 2.4f),
    new Vector2(21.6f, 8.7f),
    new Vector2(-3.7f, 11.8f),
    new Vector2(7.4f, -0.9f),
    new Vector2(-14.9f, 6.5f),
    new Vector2(20.1f, 10.3f),
    new Vector2(-11.8f, 3.7f),
    new Vector2(5.2f, -2.1f),
    new Vector2(-19.5f, 8.9f),
    new Vector2(13.6f, 1.4f),
    new Vector2(-6.2f, 11.2f),
    new Vector2(17.9f, 5.8f),
    new Vector2(-22.0f, -1.3f),
    new Vector2(9.8f, 7.6f),
    new Vector2(-2.1f, 4.9f),
    new Vector2(21.3f, 12.0f),
    new Vector2(-15.7f, -2.7f),
    new Vector2(4.5f, 9.8f),
    new Vector2(-10.2f, 6.1f),
    new Vector2(18.4f, 2.2f),
    new Vector2(-7.8f, 10.7f),
    new Vector2(14.1f, -1.5f),
    new Vector2(-20.6f, 5.4f),
    new Vector2(11.3f, 8.3f)
};
    [Inject] private PoolManagerMono _poolManager;
    private int _currentSpawnCount;

    private void OnEnable()
    {
        Injector.Instance.InjectRuntime(this);
    }

    public override void Execute()
    {
        StartCoroutine(SpawnBullet());
    }

    private IEnumerator SpawnBullet()
    {

            for (int j = 0; j < SpawnCount; j++)
            {
                ExplodeBullet explode = _poolManager.Pop<ExplodeBullet>(boomItem);
                explode.SpawnPosition = spawnPoints[j];
                explode.targetPosition = movePoints[j];
                explode.moveable = true;
                yield return new WaitForSeconds(0.5f);
            }
            yield return new WaitForSeconds(0.3f);
    }
}
