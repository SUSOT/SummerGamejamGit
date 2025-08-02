using GondrLib.Dependencies;
using GondrLib.ObjectPool.Runtime;
using KHG.Bullets;
using LCM._01.Scripts.Bullets;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pattern6 : TimeLinePattern
{

    [SerializeField] private PoolingItemSO cog;
    [SerializeField] private PoolingItemSO laser;
    [SerializeField] private int SpawnCount;
    [SerializeField] private List<Vector3> spawnPoints;
    [SerializeField] private float moveSpeed = 3.5f;
    [Inject] private PoolManagerMono _poolManager;
    private int _currentSpawn;

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
        for (int i = 0; i < SpawnCount; i++)
        {
            CogwheelBullet cogwheel = _poolManager.Pop<CogwheelBullet>(cog);
            cogwheel.transform.position = new Vector2(-40, 0);
            cogwheel.MoveSpeed = moveSpeed;
            cogwheel.RotationSpeed = 10f;
            cogwheel.MoveDirection = Vector2.right;
            yield return new WaitForSeconds(3f);
            for(int j = 0; j <  spawnPoints.Count; j++)
            {
                _currentSpawn = j;
                LaserBullet laserBullet = _poolManager.Pop<LaserBullet>(laser);
                laserBullet.transform.position = spawnPoints[_currentSpawn];
                laserBullet.transform.rotation = Quaternion.Euler(0, 0, spawnPoints[_currentSpawn].z);
                yield return new WaitForSeconds(0.8f);
            }
        }
    }
}
