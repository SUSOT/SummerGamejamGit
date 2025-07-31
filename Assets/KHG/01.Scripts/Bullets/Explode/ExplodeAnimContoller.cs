using GondrLib.ObjectPool.Runtime;
using KHG.Bullets;
using LCM._01.Scripts;
using UnityEngine;

namespace KHG.Bullets
{
    public class ExplodeAnimContoller : AnimEventController
    {
        [SerializeField] private ExplodeBullet explode;

        public override void OnDamageStart() => explode.DamageStart();
        public override void OnDamageEnd() => explode.DamageEnd();
        public override void OnActivated() => explode.OnActivated();
        public override void DestroySelf() => explode.DestroySelf();

        public override void SetUpPool(Pool pool) => explode.SetUpPool(pool);

        public override void ResetItem() => explode.ResetItem();
    }

}