using System;
using LKW._01.Scripts.Core;
using UnityEngine;

namespace Entities
{
    public class EntityMover : MonoBehaviour, IEntityComponent
    {
        [SerializeField] private float moveSpeed = 5f;
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
            transform.up = direction;
        }

        public void StopImmediately()
        {
            _rigidbody.linearVelocity = Vector3.zero;
        }
    }
}