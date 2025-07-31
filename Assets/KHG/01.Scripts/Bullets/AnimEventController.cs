using GondrLib.ObjectPool.Runtime;
using UnityEngine;

namespace KHG.Bullets
{
    public abstract class AnimEventController : MonoBehaviour
    {
        public abstract void OnDamageStart();
        public abstract void OnDamageEnd();
        public abstract void OnActivated();
        public abstract void DestroySelf();
        public abstract void SetUpPool(Pool pool);
        public abstract void ResetItem();
    }

}