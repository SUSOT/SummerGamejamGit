using System.Collections;
using DG.Tweening;
using GondrLib.Dependencies;
using GondrLib.ObjectPool.Runtime;
using UnityEngine;

namespace LCM._01.Scripts.Timeline
{
    public class LCM_BossPattern : TimeLinePattern
    {
        [Inject] private PoolManagerMono _poolManager;

        [SerializeField] private GameObject warning;
        [SerializeField] private GameObject bossPrefab;
        private GameObject _boss;
        
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
            var warn = Instantiate(warning, Vector3.zero, Quaternion.identity);
            
            warn.transform.localScale = Vector3.zero;
            
            yield return new DOTweenCYInstruction.WaitForCompletion(
                warn.transform.DOScale(Vector3.one * 13f, 1f)
                    .SetEase(Ease.OutBack)
                    .SetLoops(4, LoopType.Yoyo)
                    .OnComplete(() => Destroy(warn)));
            
            _boss = Instantiate(bossPrefab, new Vector3(0,30,0), Quaternion.identity);
            
            yield return new DOTweenCYInstruction.WaitForCompletion(
                _boss.transform.DOMove(Vector3.zero, 3f).SetEase(Ease.InOutQuart));

            yield return new WaitForSeconds(1f);
    
            _boss.transform.DORotate(new Vector3(0, 0, 2520), 7f, RotateMode.FastBeyond360)
                .SetEase(Ease.InOutSine)
                .SetRelative(true);
        }
    }
}