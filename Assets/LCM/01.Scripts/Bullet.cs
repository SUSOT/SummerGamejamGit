using GondrLib.Dependencies;
using GondrLib.ObjectPool.Runtime;
using UnityEngine;

namespace LCM._01.Scripts
{
    public abstract class Bullet : MonoBehaviour, IPoolable
    {
        [Inject] protected PoolManagerMono _poolManager;
        [SerializeField] private bool _isPooling = true;

        protected virtual void OnEnable()
        {
            Injector.Instance.InjectRuntime(this);
        }

        protected virtual void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("BulletDestroyZone"))
            {
                if (!_isPooling)
                {
                    Destroy(gameObject);
                }
                else
                {
                    print("push");
                    _poolManager.Push(this);
                }
            }
            ApplyDamage(other);
        }
        public void ApplyDamage(Collider2D targetCol)
        {
            if (targetCol.TryGetComponent(out IDamageable damageable))
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
