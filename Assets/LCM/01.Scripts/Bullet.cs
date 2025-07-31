using GondrLib.ObjectPool.Runtime;
using UnityEngine;

namespace LCM._01.Scripts
{
    public abstract class Bullet : MonoBehaviour, IPoolable
    {
        protected virtual void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent(out IDamageable damageable))
            {
                damageable.TakeDamage();
            }
        }

        [field: SerializeField] public PoolingItemSO PoolingType { get; private set; }
        public GameObject GameObject => gameObject;
        public abstract void SetUpPool(Pool pool);

        public abstract void ResetItem();
    }
}
