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

        // 풀링되면 지울코드
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
            _rigidbody.linearVelocity = Vector2.zero;
            _rigidbody.angularVelocity = 0f;
            transform.rotation = Quaternion.identity;
        }

        private void FixedUpdate()
        {
            _rigidbody.linearVelocity = MoveDirection * moveSpeed;
            transform.Rotate(0f, 0f, rotationSpeed);
        }
    }
}