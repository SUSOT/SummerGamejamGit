using System;
using DG.Tweening;
using GondrLib.ObjectPool.Runtime;
using UnityEngine;

namespace Animation
{
    public class DashCircle : MonoBehaviour, IPoolable
    {
        [SerializeField] private GameObject outCircle;
        [SerializeField] private GameObject inCircle;

        
        [SerializeField] private float outCircleMaxScale = 2.4f;
        [SerializeField] private float time = 0.12f;
        private void OnEnable()
        {
            outCircle.transform.DOScale(Vector2.one * outCircleMaxScale, time)
                .OnComplete(() =>
                {
                    inCircle.SetActive(true);
                    outCircle.transform.DOScale(Vector2.one * outCircleMaxScale, time);
                });
        }

        public PoolingItemSO PoolingType { get; }
        public GameObject GameObject => gameObject;
        public void SetUpPool(Pool pool)
        {
            
        }

        public void ResetItem()
        {
        }
    }
}