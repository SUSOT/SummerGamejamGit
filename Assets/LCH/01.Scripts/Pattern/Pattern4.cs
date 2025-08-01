using GondrLib.Dependencies;
using GondrLib.ObjectPool.Runtime;
using KHG.Bullets;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pattern4 : TimeLinePattern
{

    [SerializeField] private PoolingItemSO laserItem;
    [SerializeField] private List<Vector3> spawnPosandRoatz;
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
        for(int j =0; j < spawnPosandRoatz.Count; j++)
        {
            LaserBullet laser = _poolManager.Pop<LaserBullet>(laserItem);
            laser.transform.position = spawnPosandRoatz[j];
            laser.transform.rotation = Quaternion.Euler(0, 0, spawnPosandRoatz[j].z);
            yield return new WaitForSeconds(0.3f);
        }
    }
}
