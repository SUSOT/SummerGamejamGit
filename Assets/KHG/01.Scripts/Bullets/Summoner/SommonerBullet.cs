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
        [SerializeField] private float repeatDuration = 1f;
        public float MoveSpeed = 10;

        public UnityEvent OnSpawnEvent;

        public int ChildSpawnCount
        {
            get => _generator.BulletCount;
            set => _generator.BulletCount = value;
        }
        public bool AutoAngle => _generator.AutoAngle;
        public float ChildSpawnAngle => _generator.GenerateAngle;

        public float BulletSpeed => _generator.Speed;
        public float BulletScale => _generator.Scale;
        public float BulletRotateSpeed => _generator.BulletRotation;

        private CircleGenerate _generator;
        private Rigidbody2D _rigid;
        private Pool _currentPool;
        private Vector3 _originScale;
        private bool _isDied;

        protected override void OnEnable()
        {
            base.OnEnable();
            _originScale = transform.localScale;
            StartCoroutine(Spawn());
        }
        public override void ResetItem()
        {
            _rigid = GetComponent<Rigidbody2D>();
            _generator = GetComponent<CircleGenerate>();
            DOTween.KillAll(this);
        }

        public void StartSpawn()
        {
            StartCoroutine(Spawn());
        }

        public override void SetUpPool(Pool pool) => _currentPool = pool;
        
        private void FixedUpdate()
        {
            SetMovement();
        }

        private void SetMovement()
        {
            transform.position += transform.up * MoveSpeed * Time.fixedDeltaTime;
        }
        protected override void OnTriggerEnter2D(Collider2D other)
        {
            base.OnTriggerEnter2D(other);
            if (other.gameObject.CompareTag("BulletDestroyZone"))
            {
                _isDied = true;
            }
        }
        private IEnumerator Spawn()
        {
            if(_isDied) yield break;
            yield return null;
            OnSpawnEvent?.Invoke();
            transform.DOScale(_originScale * 1.5f, 0.1f).OnComplete(() => transform.DOScale(_originScale, 0.1f));
            yield return new WaitForSeconds(repeatDuration);
            if(_isDied) yield break;
            StartCoroutine(Spawn());
        }
    }
}