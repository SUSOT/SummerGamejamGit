using System;
using System.Collections;
using LKW._01.Scripts.Core;
using UnityEngine;
using UnityEngine.Serialization;

namespace Entities
{
    public class EntityMover : MonoBehaviour, IEntityComponent
    {
        [SerializeField] private float moveSpeed = 5f;
         [SerializeField] private float rotateSpeed = 0.2f;
        private Rigidbody2D _rigidbody;
        private Entity _entity;
        
        public NotifyValue<Vector2> Movement { get; private set; } = new NotifyValue<Vector2>();
        public bool CanManualMove { get; set; }  = false;

        public void Initialize(Entity entity)
        {
            _entity = entity;
            _rigidbody = entity.GetComponent<Rigidbody2D>();
        }
        private void FixedUpdate()
        {
            _rigidbody.linearVelocity = Movement.Value * moveSpeed;
        }

        public void SetMovement(Vector2 movement)
        {
            if (CanManualMove) return;
             Movement.Value = movement;
        }

        public void SetRotation(Vector3 direction)
        {
           float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
           Quaternion targetRotation = Quaternion.Euler(0, 0, targetAngle) * Quaternion.Euler(0, 0, -90f);
            transform.parent.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
        }

        public void StopImmediately()
        {
            Movement.Value = Vector2.zero;
            _rigidbody.linearVelocity = Vector3.zero;
            _rigidbody.angularVelocity = 0;
        }
    }
}