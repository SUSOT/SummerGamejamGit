using System.Collections;
using System.Collections.Generic;
using GondrLib.Dependencies;
using GondrLib.ObjectPool.Runtime;
using LCM._01.Scripts.Bullets;
using UnityEngine;

namespace LCM._01.Scripts.Timeline.Infinite
{
    public class LCM_InfinitePattern : InfinitePattern
    {
        [Inject] private PoolManagerMono _poolManager;
        [Inject] private InfiniteScoreManager _scoreManager;

        [SerializeField] private PoolingItemSO crossLaser;
        
        [SerializeField] private float baseSpawnInterval = 2f; 
        [SerializeField] private float minSpawnInterval = 0.5f; 
        [SerializeField] private float spawnPositionRange = 5f; 
                
        private void OnEnable()
        {
            Injector.Instance.InjectRuntime(this);
        }

        private IEnumerator Spawn()
        {
            while (true)
            {
                float currentTime = _scoreManager.GetCurrentTime();
                
                int spawnCount = 1 + Mathf.FloorToInt(currentTime / 30f);
                
                for (int i = 0; i < spawnCount; i++)
                {
                    CrossLaserBullet laserObj = _poolManager.Pop<CrossLaserBullet>(crossLaser);
                    
                    if (laserObj != null)
                    {
                        Vector3 spawnPosition = GetRandomSpawnPosition();
                        laserObj.transform.position = spawnPosition;
                    }
                    
                    yield return new WaitForSeconds(0.1f);
                }
                
                float spawnInterval = Mathf.Max(minSpawnInterval, baseSpawnInterval - (currentTime / 60f));
                yield return new WaitForSeconds(spawnInterval);
            }
        }
        
        private Vector3 GetRandomSpawnPosition()
        {
            float randomX = Random.Range(-spawnPositionRange, spawnPositionRange);
            float randomY = Random.Range(-spawnPositionRange, spawnPositionRange);
            
            return new Vector3(randomX, randomY, 0f);
        }
        public override void Execute(InfinitePatternListSO PatternList, List<InfinitePattern> _activePatterns)
        {
            StartCoroutine(Spawn());
        }
        
        public override void ExecuteNextPattern(InfinitePatternListSO PatternList, List<InfinitePattern> _activePatterns)
        {
            base.ExecuteNextPattern(PatternList, _activePatterns);
        }
    }
}