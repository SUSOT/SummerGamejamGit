using System.Collections;
using GondrLib.Dependencies;
using GondrLib.ObjectPool.Runtime;
using KHG.Bullets;
using UnityEngine;

namespace LKW._01.Scripts.Patterns
{
    public class Pattern2_3 : TimeLinePattern
    {
        [SerializeField] private Transform[] spawnPoints;
        
        [SerializeField] private PoolManagerSO poolManager;
        [SerializeField] private PoolingItemSO laserItem;
        
        WaitForSeconds wait = new WaitForSeconds(1.5f);
        
        public override void Execute()
        {
            StartCoroutine(SpawnCoroutine());
        }

        private IEnumerator SpawnCoroutine()
        {
            yield return new WaitForSeconds(6f);
            for (int i = 0; i <16; i++)
            {
                LaserBullet laser = poolManager.Pop(laserItem) as LaserBullet;
                laser.transform.position =
                    new Vector3(Random.Range(spawnPoints[0].position.x, spawnPoints[1].position.x),0, 0);
                laser.rotation = 90;
                yield return wait;
            }
        }
    }
}