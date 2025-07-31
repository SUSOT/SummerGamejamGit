using GondrLib.ObjectPool.Runtime;
using UnityEngine;
using UnityEngine.Serialization;

namespace LCM._01.Scripts.Bullets
{
    public class CrossLaserBullet : Bullet
    {
        [field: SerializeField] public float RotationSpeed { get; set; }


        public void Dead()
        {
            _poolManager.Push(this);
        }

        private void FixedUpdate()
        {
            transform.Rotate(0f, 0f, RotationSpeed);
        }

        public override void SetUpPool(Pool pool)
        {
        }

        public override void ResetItem()
        {
            
        }
    }
}