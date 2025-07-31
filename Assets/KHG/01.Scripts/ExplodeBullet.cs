using LCM._01.Scripts;
using System;
using UnityEngine;

namespace KHG.Bullets
{
    public class ExplodeBullet : Bullet
    {
        public event Action OnExplodeEvent;

        private bool _damageable;
        protected override void OnTriggerEnter2D(Collider2D other)
        {
            if(_damageable == true) base.OnTriggerEnter2D(other);
        }

        public void OnDamageStart()
        {
            _damageable = true;
        }
        public void OnDamageEnd()
        {
            _damageable = false;
        }

        public void DestroySelf()
        {
            Destroy(gameObject);
        }

        public void OnExplode()
        {
            OnExplodeEvent?.Invoke();
        }
    }
}
