using System;
using LCM._01.Scripts;
using UnityEngine;

namespace LKW._01.Scripts.InfinityMode
{
    public class Laserbody : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            Debug.Log("충돌");
            ApplyDamage(other);
        }

        public void ApplyDamage(Collider2D targetCol)
        {
            if (targetCol.TryGetComponent(out IDamageable damageable))
            {
                damageable.TakeDamage();
            }
        }
    }
}