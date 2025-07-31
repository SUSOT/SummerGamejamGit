using DG.Tweening;
using GondrLib.ObjectPool.Runtime;
using LCM._01.Scripts;
using System.Collections;
using UnityEngine;

namespace KHG.Bullets
{
    public class NailBullet : Bullet
    {
        [SerializeField] private GameObject warnningLine;
        [SerializeField] private Transform nail;
        public float MoveSpeed = 10;

        private Vector3 SpawnPosition;

        private SpriteRenderer _warnRenderer;
        private Pool _currentPool;
        private bool _moveable;

        private float _waitTime = 0.5f;
        protected override void OnEnable()
        {
            base.OnEnable();
            _warnRenderer = warnningLine.GetComponent<SpriteRenderer>();
            StartCoroutine(Warnning());
        }
        public override void ResetItem()
        {
            _moveable = false;
            _warnRenderer.color = new Color(_warnRenderer.color.r, _warnRenderer.color.g, _warnRenderer.color.b, 0);
        }
        public void SetSpawnValues(Vector3 pos, Vector3 rotation,float moveSpeed = 10f,float waitTime = .5f)
        {
            SpawnPosition = pos;
            transform.rotation = Quaternion.Euler(rotation);
            MoveSpeed = moveSpeed;
            _waitTime = waitTime;
        }
        public override void SetUpPool(Pool pool) => _currentPool = pool;

        protected override void OnTriggerEnter2D(Collider2D other)
        {
            base.OnTriggerEnter2D(other);
        }
        private void FixedUpdate()
        {
            SetMovement();
        }

        private void SetMovement()
        {
            if (_moveable) nail.transform.position += nail.transform.up * MoveSpeed * Time.fixedDeltaTime;
        }

        private IEnumerator Warnning()
        {
            warnningLine.SetActive(true);
            _warnRenderer.DOFade(1, _waitTime);
            yield return new WaitForSeconds(_waitTime);
            _moveable = true;
            yield return new WaitForSeconds(_waitTime);
            _warnRenderer.DOFade(0, _waitTime / 2);

            if(_currentPool != null) _currentPool.Push(this);
        }
    }
}