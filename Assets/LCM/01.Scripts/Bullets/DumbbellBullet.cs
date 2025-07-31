using GondrLib.ObjectPool.Runtime;
using UnityEngine;

namespace LCM._01.Scripts.Bullets
{
    public class DumbbellBullet : Bullet
    {
        private Rigidbody2D _rigidbody;
        [field: SerializeField] public Vector2 MoveDirection { get; set; }
        [field: SerializeField] public float MoveSpeed { get; set; }

        private void FixedUpdate()
        {
            if (_rigidbody == null) return;
            _rigidbody.linearVelocity = MoveDirection * MoveSpeed;
        }

        public override void SetUpPool(Pool pool)
        {
            _rigidbody = GetComponent<Rigidbody2D>();
        }

        public override void ResetItem()
        {
            _rigidbody.linearVelocity = Vector2.zero;
        }
    }
}