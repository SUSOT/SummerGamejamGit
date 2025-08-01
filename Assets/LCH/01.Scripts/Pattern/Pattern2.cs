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
    [SerializeField] private List<Vector2> movePoints;
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
        for (int i = 0; i < SpawnCount; i++)
        {
            ExplodeBullet explode = _poolManager.Pop<ExplodeBullet>(boomItem);
            explode.moveable = true;
            explode.transform.position = spawnPoints[i];
            explode.targetPosition = movePoints[i];
            yield return new WaitForSeconds(0.4f);
        }
    }
}
