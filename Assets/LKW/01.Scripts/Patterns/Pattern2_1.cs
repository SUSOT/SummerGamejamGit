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
            for (int i = 0; i < 40; i++)
            {
                LaserBullet laser = poolManager.Pop(laserItem) as LaserBullet;
            
                int idx = Random.Range(0,2);
                Vector3 spawnPos;

                Debug.Log(idx);
                if (idx == 0)
                {
                    spawnPos = new Vector3(0,Random.Range(spawnPoints[0].position.y, spawnPoints[1].position.y),0);
                }
                else
                {
                    spawnPos = new Vector3(Random.Range(spawnPoints[0].position.x, spawnPoints[1].position.x), 0,0);
                    laser.transform.rotation = Quaternion.Euler(0,0,0);
                    laser.rotation = 90f;
                }
            
                laser.transform.position = spawnPos;
            
                yield return new WaitForSeconds(1);
            }
        }
    }
}