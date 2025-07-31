using GondrLib.ObjectPool.Runtime;
using LCM._01.Scripts;
using System;
using UnityEngine;

namespace KHG.Bullets
{
    public class ExplodeBullet : Bullet
    {
        public event Action activeEvent;

        private bool _damageable;
        private Pool _explodePool;
        protected override void OnTriggerEnter2D(Collider2D other)
        {
            if(_damageable == true) base.OnTriggerEnter2D(other);
        }

        public void DamageStart() => _damageable = true;
        public void DamageEnd() => _damageable = false;
        public void OnActivated() => activeEvent?.Invoke();
        public void DestroySelf() => Destroy(gameObject);

        public override void SetUpPool(Pool pool)
        {
            _explodePool = pool;
        }

        public override void ResetItem()
        {
            throw new NotImplementedException();
        }
    }
}
