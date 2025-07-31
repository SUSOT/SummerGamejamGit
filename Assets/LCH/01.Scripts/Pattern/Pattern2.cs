using GondrLib.Dependencies;
using GondrLib.ObjectPool.Runtime;
using System.Collections.Generic;
using UnityEngine;

public class Pattern2 : TimeLinePattern
{

    [SerializeField] private PoolingItemSO boomItem;
    [SerializeField] private int SpawnCount;
    [SerializeField] private List<Vector2> spawnPoints;
    [Inject] private PoolManagerMono _poolManager;

    private void OnEnable()
    {
        Injector.Instance.InjectRuntime(this);
    }

    public override void Execute()
    {
        
    }
}
