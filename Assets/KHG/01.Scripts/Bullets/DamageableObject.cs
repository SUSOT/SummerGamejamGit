using LCM._01.Scripts;
using UnityEngine;

public abstract class DamageableObject : MonoBehaviour
{
    [SerializeField] private Bullet damageApplier;
    public virtual void OnTriggerEnter2D(Collider2D collision)
    {
        damageApplier.ApplyDamage(collision);
    }
}
