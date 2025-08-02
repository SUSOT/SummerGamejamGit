using System.Collections;
using System.Collections.Generic;
using GondrLib.Dependencies;
using GondrLib.ObjectPool.Runtime;
using KHG.Bullets;
using KHG.Obstacles;
using UnityEngine;

namespace LCM._01.Scripts.Timeline.Infinite
{
    public class LCM_InfinitePattern3 : InfinitePattern
    {
        [Inject] private PoolManagerMono _poolManager;
        [Inject] private InfiniteScoreManager _scoreManager;

        [SerializeField] private PoolingItemSO frag;

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

            // 처음 3 + 경과 시간 25초마다 1개 증가
            int spawnCount = 3 + Mathf.FloorToInt(currentTime / 25f);

            for (int i = 0; i < spawnCount; i++)
            {
                ExplodeBullet bullet = _poolManager.Pop<ExplodeBullet>(frag);
                if (bullet != null)
                {
                    bullet.transform.position = new Vector3(0f, -30f, 0f);

                    float targetX = Random.Range(-24f, 24f);
                    float targetY = Random.Range(-12f, 12f);
                    bullet.targetPosition = new Vector3(targetX, targetY, 0f);

                    bullet.moveable = true;
                }

                yield return new WaitForSeconds(0.3f);
            }

            ExecuteNextPattern(PatternList, _activePatterns);
        }

        public override void ExecuteNextPattern(InfinitePatternListSO PatternList,
            List<InfinitePattern> _activePatterns)
        {
            base.ExecuteNextPattern(PatternList, _activePatterns);
        }
    }
}