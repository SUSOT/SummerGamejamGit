using GondrLib.Dependencies;
using GondrLib.ObjectPool.Runtime;
using System.Collections.Generic;
using UnityEngine;

public class Pattern7 : TimeLinePattern
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
        //StartCoroutine(Spawn());
    }
}
