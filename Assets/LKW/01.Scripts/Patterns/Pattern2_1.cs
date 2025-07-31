using System.Collections;
using GondrLib.Dependencies;
using GondrLib.ObjectPool.Runtime;
using KHG.Bullets;
using UnityEngine;

namespace LKW._01.Scripts.Patterns
{
    public class Pattern2_1 : TimeLinePattern
    {
        [SerializeField] private Transform[] spawnPoints;
        
        [SerializeField] private PoolManagerSO poolManager;
        [SerializeField] private PoolingItemSO laserItem;
        
        public override void Execute()
        {
            StartCoroutine(SpawnCoroutine());
        }

        private IEnumerator SpawnCoroutine()
        {
            int idx = Random.Range(0,2);

            if (idx == 0)
            {
                
            }
            
            LaserBullet laser = poolManager.Pop(laserItem) as LaserBullet;

            laser.transform.position = new Vector3(0, 0, 0);
            yield return new WaitForSeconds(1f);
        }
    }
}