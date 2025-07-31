using DG.Tweening;
using GondrLib.ObjectPool.Runtime;
using LCM._01.Scripts;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace KHG.Bullets
{
    public class ExplodeBullet : Bullet
    {
        [SerializeField] private bool moveable;
        [SerializeField] private Vector3 targetPosition;
        public UnityEvent ActiveEvent;
        public Vector3 SpawnPosition 
        { 
            get => transform.position;
            set => transform.position = value; 
        }

        private bool _damageable;
        private Pool _explodePool;

        protected override void OnEnable()
        {
            if (moveable) transform.DOMove(targetPosition, 1.5f);
            base.OnEnable();
        }

        protected override void OnTriggerEnter2D(Collider2D other)
        {
            if(_damageable == true) base.OnTriggerEnter2D(other);
        }

        public void DamageStart() => _damageable = true;
        public void DamageEnd() => _damageable = false;
        public void OnActivated() => ActiveEvent?.Invoke();
        public void DestroySelf()
        {
            if (_explodePool != null) _explodePool.Push(this);
        }

        public override void SetUpPool(Pool pool)
        {
            _explodePool = pool;
        }

        public override void ResetItem()
        {
            
        }
    }
}
