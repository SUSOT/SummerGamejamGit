using System.Collections;
using GondrLib.ObjectPool.Runtime;
using UnityEngine;

namespace LCM._01.Scripts.Bullets
{
    public class MissileBullet : Bullet
    {
        [SerializeField] private LayerMask whatIsPlayer;
        [SerializeField] private float moveSpeed;
        [SerializeField] private float missileTime;
        [SerializeField] private GameObject circle;
        
        private bool _isTargeting = true;
        private Rigidbody2D _rigidbody;
        private Vector2 _direction;

        private IEnumerator Start()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            yield return new WaitForSeconds(missileTime);
            _isTargeting = false;
            circle.GetComponent<SpriteRenderer>().color = Color.black;
        }

        public override void SetUpPool(Pool pool)
        {
            _rigidbody = GetComponent<Rigidbody2D>();
        }

        public override void ResetItem()
        {
            
        }

        private void FixedUpdate()
        {
            if (_isTargeting)
            {
                var player = Physics2D.OverlapCircle(transform.position, 30f, whatIsPlayer);
                _direction= (player.transform.position - transform.position).normalized;
                _rigidbody.linearVelocity = _direction * moveSpeed;
                transform.rotation = Quaternion.Euler(0f, 0f, Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg - 225f);
            }
            else
            {
                _rigidbody.linearVelocity = _direction * moveSpeed;
            }
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, 30f);
            Gizmos.color = Color.white;
        }
#endif
    }
}