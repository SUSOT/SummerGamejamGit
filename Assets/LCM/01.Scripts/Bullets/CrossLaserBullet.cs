using GondrLib.ObjectPool.Runtime;
using UnityEngine;

namespace LCM._01.Scripts.Bullets
{
    public class CrossLaserBullet : Bullet
    {
        private bool _isRotate = false;
        [SerializeField] private float rotationSpeed;

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
                transform.Rotate(0f, 0f, rotationSpeed);
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