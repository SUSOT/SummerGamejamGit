using LCM._01.Scripts;
using UnityEngine;

namespace KHG.Bullets
{
    public class CircleWorm : Bullet
    {
        [SerializeField] private float moveSpeed = 10;
        public float rotationSpeed { get; set; }

        private Rigidbody2D _rigid;
        private void Awake()
        {
            _rigid = GetComponent<Rigidbody2D>();
        }
        private void Start()
        {
            _rigid.AddTorque(rotationSpeed);
        }
        private void FixedUpdate()
        {
            SetMovement();
        }

        private void SetMovement()
        {
            transform.position += transform.up * moveSpeed * Time.fixedDeltaTime;
        }

        public override void ResetItem()
        {
        }

        public override void SetUpPool(Pool pool)
        {
        }
    }

}