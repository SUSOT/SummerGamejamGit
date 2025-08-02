using GondrLib.Dependencies;
using UnityEngine;

namespace GondrLib.ObjectPool.Runtime
{
    [Provide]
    public class PoolManagerMono : MonoBehaviour, IDependencyProvider
    {
        [SerializeField] public PoolManagerSO poolManager;
        public static PoolManagerMono Instacne;

        private void Awake()
        {
            if(Instacne == null)
            {
                Instacne = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
            poolManager.Initialize(transform);
        }

        public T Pop<T>(PoolingItemSO item) where T : IPoolable
        {
            return (T)poolManager.Pop(item);
        }

        public void Push(IPoolable target)
        {
            poolManager.Push(target);
        }

        public void AllPush()
        {
            poolManager.PushAll();
        }
    }
}