using GondrLib.ObjectPool.Runtime;
using UnityEngine;

namespace LCM._01.Scripts.Bullets
{
    public class CogwheelBullet : Bullet
    {
        [SerializeField] private Vector2 moveDirection;
        private Rigidbody2D _rigidbody;
        [field:SerializeField] public float MoveSpeed { get; set; }
        [field:SerializeField] public float RotationSpeed{ get; set; }

        private void FixedUpdate()
        {
            _rigidbody.linearVelocity = moveDirection * MoveSpeed;
            transform.Rotate(0f, 0f, RotationSpeed);
        }

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
    }
}