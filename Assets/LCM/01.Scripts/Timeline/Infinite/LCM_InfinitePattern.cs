using System.Collections;
using System.Collections.Generic;
using GondrLib.Dependencies;
using GondrLib.ObjectPool.Runtime;
using UnityEngine;

namespace LCM._01.Scripts.Timeline.Infinite
{
    public class LCM_InfinitePattern : InfinitePattern
    {
        [Inject] private PoolManagerMono _poolManager;
        [Inject] private InfiniteScoreManager _scoreManager;

        [SerializeField] private PoolingItemSO crossLaser;
                
        private void OnEnable()
        {
            Injector.Instance.InjectRuntime(this);
        }

        private IEnumerator Spawn()
        {
            _scoreManager.GetCurrentTime()
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