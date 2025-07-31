using System;
using DG.Tweening;
using GondrLib.Dependencies;
using GondrLib.ObjectPool.Runtime;
using UnityEngine;

namespace Animation
{
    public class DashCircle : MonoBehaviour, IPoolable
    {
        [SerializeField] private PoolManagerSO poolManager;

        [field: SerializeField] public PoolingItemSO PoolingType { get; private set; }
        public GameObject GameObject => gameObject;

        public void GotoPool()
        {
            poolManager.Push(this);
        }
        
        public void SetUpPool(Pool pool)
        {
            poolManager.Push(this);
        }

        public void ResetItem()
        {
        }
    }
}