using System;
using GondrLib.ObjectPool.Runtime;
using UnityEngine;

namespace LCM._01.Scripts.Bullets
{
    public class TileSquareBullet : Bullet
    {
        private bool _isCanAttack = false;
        
        protected override void OnTriggerEnter2D(Collider2D other)
        {
            if (!_isCanAttack) return;
            
            base.OnTriggerEnter2D(other);
        }

        public void CanAttack()
        {
            _isCanAttack = !_isCanAttack;
        }

        public void Dead()
        {
            _poolManager.Push(this);
        }
        public override void SetUpPool(Pool pool)
        {
            
        }

        public override void ResetItem()
        {
            _isCanAttack = false;
        }
    }
}