using DG.Tweening;
using GondrLib.ObjectPool.Runtime;
using LCM._01.Scripts;
using UnityEngine;
using UnityEngine.Events;

namespace KHG.Obstacles
{
    public class WallGen : Bullet
    {
        [SerializeField] private GameObject wallObj;
        [SerializeField] private SpriteRenderer _warnRenderer;

        public UnityEvent OnWallDeployed;
        public float WarnTime = 1.5f;

        private Pool _wallPool;

        private void Start()
        {
            wallObj.SetActive(false);
            _warnRenderer.gameObject.SetActive(false);
            SetWall();
        }

        public void SetWall()
        {
            _warnRenderer.gameObject.SetActive(true);
            Sequence _seq = DOTween.Sequence();
            for (int i = 0; i < 2; i++)
            {
                _seq.Append(_warnRenderer.DOFade(0.2f, WarnTime / 12));
                _seq.Append(_warnRenderer.DOFade(0, WarnTime / 12));
            }
            _seq.Append(_warnRenderer.DOFade(0.2f, WarnTime / 12));
            _seq.Append(_warnRenderer.DOFade(0, WarnTime / 12)).OnComplete(() =>
            {
                wallObj.SetActive(true);
                _warnRenderer.gameObject.SetActive(false);
                wallObj.transform.DOScale(Vector3.one, 0.2f).OnComplete(() => OnWallDeployed?.Invoke());
            });
        }
        public void DestroyWall()
        {
            if (_wallPool != null) _wallPool.Push(this);
            else Destroy(gameObject);
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