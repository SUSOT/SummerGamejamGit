using GondrLib.ObjectPool.Runtime;
using UnityEngine;

namespace LCM._01.Scripts.Bullets
{
    public class WallBullet : Bullet
    {
        private Rigidbody2D _rigidbody;
        [field: SerializeField] public Vector2 MoveDirection { get; set; }
        [SerializeField] private float moveSpeed;

        private void FixedUpdate()
        {
            _rigidbody.linearVelocity = MoveDirection * moveSpeed;
        }

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
        }

        public override void SetUpPool(Pool pool)
        {
            _rigidbody = GetComponent<Rigidbody2D>();
        }

        public override void ResetItem()
        {
        }
    }
}