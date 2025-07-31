using GondrLib.ObjectPool.Runtime;
using UnityEngine;

namespace LCM._01.Scripts.Bullets
{
    public class NormalBullet : Bullet
    {
        [field:SerializeField] public Vector2 MoveDirection { get; set; }

        private Rigidbody2D _rigidbody;
        [field:SerializeField] public float moveSpeed { get; set; }
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
    }
}