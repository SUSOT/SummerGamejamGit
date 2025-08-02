using System;
using System.Collections;
using System.Collections.Generic;
using GondrLib.Dependencies;
using GondrLib.ObjectPool.Runtime;
using KHG.Bullets;
using UnityEngine;
using Random = UnityEngine.Random;

namespace LKW._01.Scripts.InfinityMode
{
    public class LaserPattern : InfinitePattern
    {
        [SerializeField] private Transform[] spawnPoints;
        
        [SerializeField] private PoolManagerSO poolManager;
        [SerializeField] private PoolingItemSO laserItem;
        [SerializeField] private PoolingItemSO explosionBulletItem;

        
        private void OnEnable()
        {
            Injector.Instance.InjectRuntime(this);
        }

        public override void Execute(InfinitePatternListSO PatternList, List<InfinitePattern> _activePatterns)
        {
            StartCoroutine(SpawnCoroutine(PatternList,_activePatterns,1));
            Debug.Log("일반 레이저");
        }
        
        private IEnumerator SpawnCoroutine(InfinitePatternListSO PatternList, List<InfinitePattern> _activePatterns, float time)
        {
            for (int i = 0; i < 40; i++)
            {
                if (i % 5 == 0)
                {
                    
                    for (int j = 0; j < 2; j++)
                    {
                        if (i % 2 == 0)
                        {
                            ExplodeBullet exBullet = poolManager.Pop(explosionBulletItem) as ExplodeBullet;
                            exBullet.targetPosition = spawnPoints[j].position;
                            exBullet.transform.position = spawnPoints[j].position;
                        }
                        else
                        {
                            ExplodeBullet exBullet = poolManager.Pop(explosionBulletItem) as ExplodeBullet;
                            exBullet.targetPosition = spawnPoints[j+2].position;
                            exBullet.transform.position = spawnPoints[j+2].position;
                        }
                    }
                    
                }
                
                LaserBullet laser = poolManager.Pop(laserItem) as LaserBullet;
            
                int idx = Random.Range(0,2);
                Vector3 spawnPos;

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
            
                yield return new WaitForSeconds(0.5f);
            }
            
            ExecuteNextPattern(PatternList, _activePatterns);
        }
        
        public override void ExecuteNextPattern(InfinitePatternListSO PatternList, List<InfinitePattern> _activePatterns)
        {
            base.ExecuteNextPattern(PatternList, _activePatterns);
        }
    }
}