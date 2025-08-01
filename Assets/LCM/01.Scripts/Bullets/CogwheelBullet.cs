using System;
using System.Collections;
using GondrLib.ObjectPool.Runtime;
using UnityEngine;
using UnityEngine.Events;

namespace LCM._01.Scripts.Bullets
{
    public class CogwheelBullet : Bullet
    {
        [field:SerializeField] public Vector2 MoveDirection { get; set; }
        private Rigidbody2D _rigidbody;
        [field:SerializeField] public float MoveSpeed { get; set; }
        [field:SerializeField] public float RotationSpeed{ get; set; }

        public UnityEvent OnSpawn;

        protected override void OnEnable()
        {
            base.OnEnable();
            StartCoroutine(StartShake());
        }

        private IEnumerator StartShake()
        {
            yield return new WaitForSeconds(0.5f);
            OnSpawn?.Invoke();
        }

        private void OnDisable()
        {
            StopAllCoroutines();
        }

        private void FixedUpdate()
        {
            _rigidbody.linearVelocity = MoveDirection * MoveSpeed;
            transform.Rotate(0f, 0f, -(RotationSpeed * MoveDirection.x));
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