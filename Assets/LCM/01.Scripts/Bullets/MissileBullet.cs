using System.Collections;
using GondrLib.ObjectPool.Runtime;
using UnityEngine;

namespace LCM._01.Scripts.Bullets
{
    public class MissileBullet : Bullet
    {
        [SerializeField] private LayerMask whatIsPlayer;
        [field: SerializeField] public float MoveSpeed { get; set; }
        [field: SerializeField] public float MissileTime { get; set; }
        [field: SerializeField] public float RotationSpeed { get; set; } = 90f; 
        [SerializeField] private GameObject circle;

        private bool _isTargeting = true;
        private Rigidbody2D _rigidbody;
        private Vector2 _direction;

        protected override void OnEnable()
        {
            base.OnEnable();
            StartCoroutine(CountDown());
        }

        private IEnumerator CountDown()
        {
            _isTargeting = true;
            yield return new WaitForSeconds(MissileTime);
            _isTargeting = false;
            circle.GetComponent<SpriteRenderer>().color = Color.black;
        }

        public override void SetUpPool(Pool pool)
        {
            _rigidbody = GetComponent<Rigidbody2D>();
        }

        public override void ResetItem()
        {
            _rigidbody.linearVelocity = Vector2.zero;
            transform.rotation = Quaternion.identity;
            _isTargeting = true;
            circle.GetComponent<SpriteRenderer>().color = Color.white;
        }

        private void FixedUpdate()
        {
            if (_rigidbody == null) return;

            var player = Physics2D.OverlapCircle(transform.position, 70f, whatIsPlayer);

            if (_isTargeting && player != null)
            {
                _direction = (player.transform.position - transform.position).normalized;
                _rigidbody.linearVelocity = _direction * MoveSpeed;
                
                float targetAngle = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg - 225f;
                Quaternion targetRotation = Quaternion.Euler(0f, 0f, targetAngle);
                
                transform.rotation = Quaternion.RotateTowards(
                    transform.rotation, 
                    targetRotation, 
                    RotationSpeed * Time.fixedDeltaTime);
            }
            else
            {
                _rigidbody.linearVelocity = _direction * MoveSpeed;
            }
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, 70f);
            Gizmos.color = Color.white;
        }
#endif
    }
}