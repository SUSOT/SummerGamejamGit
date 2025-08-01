using KHG.Object;
using UnityEngine;

public class ColliderCaster : DamageableObject
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnTriggerEnter2D(collision.collider);
    }
}
