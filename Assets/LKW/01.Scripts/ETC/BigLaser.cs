using GondrLib.Dependencies;
using GondrLib.ObjectPool.Runtime;
using LCM._01.Scripts;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

namespace Animation
{
    
    public class BigLaser : Bullet
    {
        public UnityEvent ShakeEvent;
        
        [Inject] private PoolManagerMono poolManager;
        public float rotation
        {
            get => transform.rotation.z;
            set => transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, value);
        }
        
        protected override void OnTriggerEnter2D(Collider2D other)
        {
            base.OnTriggerEnter2D(other);
        }
        public override void SetUpPool(Pool pool)
        {
        }

        public override void ResetItem()
        {
            transform.rotation = Quaternion.identity;
        }

        public void CamShake()
        {
            ShakeEvent?.Invoke();
        }

        public void GotoPool()
        {
            poolManager.Push(this);
        }
    }
}