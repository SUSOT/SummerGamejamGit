using DG.Tweening;
using GondrLib.ObjectPool.Runtime;
using LCM._01.Scripts;
using UnityEngine;

namespace KHG.Obstacles
{
    public class WallGen : Bullet
    {
        [SerializeField] private SpriteRenderer _warnRenderer;
        public float WarnTime = 1.5f;
        private Vector3 originSize;

        private Pool _wallPool;

        private void Start()
        {
            originSize = transform.localScale;
            transform.localScale = Vector3.zero;
        }

        public void SetWall()
        {
            _warnRenderer.DOFade(0.5f, 0.3f).SetLoops(3,LoopType.Yoyo);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            base.OnTriggerEnter2D(collision.collider);
        }

        public override void SetUpPool(Pool pool)
        {
            _wallPool = pool;
        }

        public override void ResetItem()
        {
            throw new System.NotImplementedException();
        }
    }
}