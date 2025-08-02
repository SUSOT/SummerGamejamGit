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

        [SerializeField] private float spawnPositionRange = 5f;

        private void OnEnable()
        {
            Injector.Instance.InjectRuntime(this);
        }

        private IEnumerator Spawn(InfinitePatternListSO PatternList, List<InfinitePattern> _activePatterns)
        {
            float currentTime = _scoreManager.GetCurrentTime();

            int spawnCount = 1 + Mathf.FloorToInt(currentTime / 30f);

            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < spawnCount; j++)
                {
                    CrossLaserBullet laserObj = _poolManager.Pop<CrossLaserBullet>(crossLaser);

                    if (laserObj != null)
                    {
                        Vector3 spawnPosition = GetRandomSpawnPosition();
                        laserObj.transform.position = spawnPosition;
                    }
                }

                yield return new WaitForSeconds(2f);
            }

            ExecuteNextPattern(PatternList, _activePatterns);
        }

        private Vector3 GetRandomSpawnPosition()
        {
            float randomX = Random.Range(-spawnPositionRange, spawnPositionRange);
            float randomY = Random.Range(-spawnPositionRange, spawnPositionRange);

            return new Vector3(randomX, randomY, 0f);
        }

        public override void Execute(InfinitePatternListSO PatternList, List<InfinitePattern> _activePatterns)
        {
            StartCoroutine(Spawn(PatternList, _activePatterns));
        }

        public override void ExecuteNextPattern(InfinitePatternListSO PatternList,
            List<InfinitePattern> _activePatterns)
        {
            base.ExecuteNextPattern(PatternList, _activePatterns);
        }
    }
}