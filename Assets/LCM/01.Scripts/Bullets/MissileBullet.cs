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
            StopAllCoroutines();
            _rigidbody.linearVelocity = Vector2.zero;
            transform.rotation = Quaternion.identity;
            _isTargeting = true;
            circle.GetComponent<SpriteRenderer>().color = Color.white;
        }

        private void FixedUpdate()
        {
            if (_rigidbody == null) return;

            Debug.Log(_direction);
            
            if (_isTargeting)
            {
                var player = Physics2D.OverlapCircle(transform.position, 70f, whatIsPlayer);
                _direction= (player.transform.position - transform.position).normalized;
                _rigidbody.linearVelocity = _direction * MoveSpeed;
                transform.rotation = Quaternion.Euler(0f, 0f, Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg - 225f);
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