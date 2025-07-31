using GondrLib.ObjectPool.Runtime;
using UnityEngine;

namespace LCM._01.Scripts.Bullets
{
    public class NormalBullet : Bullet
    {
        [field:SerializeField] public Vector2 MoveDirection { get; set; }

        private Rigidbody2D _rigidbody;
        [SerializeField] private float moveSpeed;
        [SerializeField] private float rotationSpeed;

        public override void SetUpPool(Pool pool)
        {
            _rigidbody = GetComponent<Rigidbody2D>();
        }

        public override void ResetItem()
        {
            _rigidbody.linearVelocity = Vector2.zero;
            _rigidbody.angularVelocity = 0f;
            transform.rotation = Quaternion.identity;
        }

        private void FixedUpdate()
        {
            if (_rigidbody == null) return;
            
            _rigidbody.linearVelocity = MoveDirection * moveSpeed;
            transform.Rotate(0f, 0f, rotationSpeed);
        }
        
        protected override void OnTriggerEnter2D(Collider2D other)
        {
            base.OnTriggerEnter2D(other);
            if (other.gameObject.CompareTag("BulletDestroyZone"))
            {
                Debug.Log(_poolManager);
                _poolManager.Push(this);
            }
        }
    }
}