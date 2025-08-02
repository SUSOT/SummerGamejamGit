using System.Collections;
using System.Collections.Generic;
using GondrLib.Dependencies;
using GondrLib.ObjectPool.Runtime;
using KHG.Obstacles;
using LCM._01.Scripts.Bullets;
using UnityEngine;

namespace LCM._01.Scripts.Timeline.Infinite
{
    public class LCM_InfinitePattern2 : InfinitePattern
    {
        [Inject] private PoolManagerMono _poolManager;
        [Inject] private InfiniteScoreManager _scoreManager;

        [SerializeField] private PoolingItemSO wallgen;

        private void OnEnable()
        {
            Injector.Instance.InjectRuntime(this);
        }

        public override void Execute(InfinitePatternListSO PatternList, List<InfinitePattern> _activePatterns)
        {
            StartCoroutine(Spawn(PatternList, _activePatterns));
        }

        private IEnumerator Spawn(InfinitePatternListSO PatternList, List<InfinitePattern> _activePatterns)
        {
            float currentTime = _scoreManager.GetCurrentTime();
            int extraRounds = Mathf.FloorToInt(currentTime / 20f) * 5;
            int totalRounds = 10 + extraRounds;

            for (int i = 0; i < totalRounds; i++)
            {
                WallGen wallObj = _poolManager.Pop<WallGen>(wallgen);
                if (wallObj != null)
                {
                    float randomX = Random.Range(-24f, 24f);
                    float randomY = Random.Range(-12f, 12f);
                    wallObj.transform.position = new Vector3(randomX, randomY, 0f);

                    // 2초 뒤에 DestroyWall 실행
                    StartCoroutine(DestroyWallAfterDelay(wallObj, 2f));
                }

                yield return new WaitForSeconds(0.5f);
            }

            ExecuteNextPattern(PatternList, _activePatterns);
        }

        private IEnumerator DestroyWallAfterDelay(WallGen wall, float delay)
        {
            yield return new WaitForSeconds(delay);
            wall.DestroyWall(); // WallGen 클래스 내 DestroyWall 메서드 호출
        }


        public override void ExecuteNextPattern(InfinitePatternListSO PatternList,
            List<InfinitePattern> _activePatterns)
        {
            base.ExecuteNextPattern(PatternList, _activePatterns);
        }
    }
}