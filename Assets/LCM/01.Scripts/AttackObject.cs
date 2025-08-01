using System;
using UnityEngine;

namespace LCM._01.Scripts
{
    public class AttackObject : MonoBehaviour
    {
        protected virtual void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("BulletDestroyZone"))
            {
                Destroy(gameObject);
            }
            ApplyDamage(other);
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            ApplyDamage(other);
        }

        public void ApplyDamage(Collider2D targetCol)
        {
            if (targetCol.TryGetComponent(out IDamageable damageable))
            {
                damageable.TakeDamage();
            }
        }
        
        public void ApplyDamage(Collision2D targetCol)
        {
            if (targetCol.gameObject.TryGetComponent(out IDamageable damageable))
            {
                damageable.TakeDamage();
            }
        }
    }
}
