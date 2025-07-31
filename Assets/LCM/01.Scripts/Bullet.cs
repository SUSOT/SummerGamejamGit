using UnityEngine;

namespace LCM._01.Scripts
{
    public abstract class Bullet : MonoBehaviour
    {
        protected virtual void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent(out IDamageable damageable))
            {
                damageable.TakeDamage();
            }
        }
        
        
    }
}
