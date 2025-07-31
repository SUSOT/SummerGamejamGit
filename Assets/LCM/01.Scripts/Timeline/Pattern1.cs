using System.Collections;
using GondrLib.Dependencies;
using GondrLib.ObjectPool.Runtime;
using LCM._01.Scripts.Bullets;
using UnityEngine;

namespace LCM._01.Scripts.Timeline
{
    public class Pattern1 : TimeLinePattern
    {
        [SerializeField] private PoolingItemSO triangleCannon;
        [SerializeField] private PoolingItemSO wall;
        [SerializeField] private PoolingItemSO crossLaser;
        [Inject] private PoolManagerMono _poolManager;
        
        public Pattern1(float startTime) : base(startTime)
        {
            
        }
        
        private void OnEnable()
        {
            Injector.Instance.InjectRuntime(this);
        }

        public override void Execute()
        {
            StartCoroutine(PatternCoroutine());
        }

        private IEnumerator PatternCoroutine()
        {
            for (int i = -1; i < 2; ++i)
            {
                CrossLaserBullet cr = _poolManager.Pop<CrossLaserBullet>(crossLaser);
                cr.transform.position = new Vector3(i * 4, 0, 0);
            }

            yield return new WaitForSeconds(7f);
        }
    }
}
