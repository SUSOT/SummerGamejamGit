using GondrLib.ObjectPool.Runtime;
using UnityEngine;
using UnityEngine.Serialization;

namespace LCM._01.Scripts.Bullets
{
    public class CrossLaserBullet : Bullet
    {
        private bool _isRotate = false;
        [field:SerializeField] public float RotationSpeed { get; set; }

        public void StartRotate()
        {
            _isRotate = true;
        }

        public void StopRotate()
        {
            _isRotate = false;
        }
        
        public void Dead()
        {
            _poolManager.Push(this);
        }

        private void FixedUpdate()
        {
            if(_isRotate)
                transform.Rotate(0f, 0f, RotationSpeed);
        }

        public override void SetUpPool(Pool pool)
        {
        }
        
        public override void ResetItem()
        {
            _isRotate = false;
        }
    }
}