using DG.Tweening;
using GondrLib.ObjectPool.Runtime;
using LCM._01.Scripts;
using System.Collections;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.Events;

namespace KHG.Bullets
{
    public class SommonerBullet : Bullet
    {
        [SerializeField] private float moveSpeed = 10;
        [SerializeField] private float repeatDuration = 1f;
        private CircleGenerate _generator;

        public UnityEvent spawnEvent;

        public float ChildSpawnCount => _generator.BulletCount;
        public bool AutoAngle => _generator.AutoAngle;
        public float ChildSpawnAngle => _generator.GenerateAngle;

        public float BulletSpeed => _generator.Speed;
        public float BulletScale => _generator.Scale;
        public float BulletRotateSpeed => _generator.BulletRotation;

        private Rigidbody2D _rigid;
        private Pool _currentPool;
        private Vector3 _originScale;

        protected override void OnEnable()
        {
            base.OnEnable();
            _rigid = GetComponent<Rigidbody2D>();
            _generator = GetComponent<CircleGenerate>();

            _originScale = transform.localScale;
            StartCoroutine(Spawn());
        }
        public override void ResetItem()
        {
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
            transform.position += transform.up * moveSpeed * Time.fixedDeltaTime;
        }

        private IEnumerator Spawn()
        {
            yield return null;
            spawnEvent?.Invoke();
            transform.DOScale(_originScale * 1.5f, 0.1f).OnComplete(() => transform.DOScale(_originScale, 0.1f));
            yield return new WaitForSeconds(repeatDuration);
            StartCoroutine(Spawn());
        }
    }

}