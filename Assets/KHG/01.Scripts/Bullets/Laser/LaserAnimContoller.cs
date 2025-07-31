using GondrLib.ObjectPool.Runtime;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.InputSystem.PlayerInput;

namespace KHG.Bullets
{
    public class LaserAnimContoller : AnimEventController
    {
        [SerializeField] private LaserBullet laser;

        public override void OnDamageStart() => laser.OnDamageStart();
        public override void OnDamageEnd() => laser.OnDamageEnd();
        public override void OnActivated() => laser.OnActivated();
        public override void DestroySelf() => laser.DestroySelf();

        public override void SetUpPool(Pool pool) => laser.SetUpPool(pool);

        public override void ResetItem() => laser.ResetItem();
    }
}
