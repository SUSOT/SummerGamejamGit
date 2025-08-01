
using System.Collections;
using GondrLib.Dependencies;
using GondrLib.ObjectPool.Runtime;
using KHG.Bullets;
using LCM._01.Scripts.Bullets;
using UnityEngine;
using UnityEngine.Serialization;

namespace LKW._01.Scripts.Patterns
{
    public class Pattern2_3 : TimeLinePattern
    {
        [SerializeField] private Transform[] spawnPoints;
        
        [SerializeField] private PoolManagerSO poolManager;
        [SerializeField] private PoolingItemSO laserItem;
         [SerializeField] private PoolingItemSO wheelBulletItem;
        
        WaitForSeconds wait = new WaitForSeconds(1.5f);
        
        public override void Execute()
        {
            StartCoroutine(SpawnCoroutine());
        }

        private IEnumerator SpawnCoroutine()
        {
            for (int i = 0; i <20; i++)
            {
                if (i % 4 == 0)
                {
                    CogwheelBullet wheelBullet = poolManager.Pop(wheelBulletItem) as CogwheelBullet;
                    wheelBullet.transform.position = spawnPoints[2].position;
                    wheelBullet.MoveDirection  = Vector2.down;
                    wheelBullet.MoveSpeed = 5f;
                    wheelBullet.RotationSpeed = 4f;
                }

                for (int j = 0; j < 2; j++)
                {
                    LaserBullet laser = poolManager.Pop(laserItem) as LaserBullet;
                    laser.transform.position =
                        new Vector3(Random.Range(spawnPoints[0].position.x, spawnPoints[1].position.x),0, 0);
                    laser.rotation = 90;
                }
                yield return wait;
            }
        }
    }
}