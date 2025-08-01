using DG.Tweening;
using GondrLib.ObjectPool.Runtime;
using LCM._01.Scripts;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace KHG.Obstacles
{
    public class WallGen : Bullet
    {
        [SerializeField] private GameObject wallObj;
        [SerializeField] private SpriteRenderer _warnRenderer;

        public bool UseAutoSpawn = true;
        public Vector3 SpawnVector { get; set; }

        public UnityEvent OnWallDeployed;
        public float WarnTime = 1.5f;

        private Pool _wallPool;

        protected override void OnEnable()
        {
            base.OnEnable();
            StartCoroutine(StartCo());
        }

        private IEnumerator StartCo()
        {
            transform.position = SpawnVector;
            yield return new WaitForSeconds(0.1f);
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
                wallObj.transform.DOScale(Vector3.one, 0.1f).OnComplete(() => OnWallDeployed?.Invoke());
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
            wallObj.SetActive(false);
            _warnRenderer.gameObject.SetActive(false);
        }
    }
}