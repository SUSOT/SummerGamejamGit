using UnityEngine;

namespace KHG.Object
{
    public class TriggerCaster : DamageableObject
    {
        public override void OnTriggerEnter2D(Collider2D collision)
        {
            base.OnTriggerEnter2D(collision);
        }
    }

}