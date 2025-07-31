using GondrLib.ObjectPool.Runtime;
using UnityEngine;

namespace KHG.Bullets
{
    public abstract class AnimEventController : MonoBehaviour
    {
        public float roationAngle
        {
            get => transform.rotation.z;
            set => transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, value);
        }
        public abstract void OnDamageStart();
        public abstract void OnDamageEnd();
        public abstract void OnActivated();
        public abstract void DestroySelf();
        public abstract void SetUpPool(Pool pool);
        public abstract void ResetItem();
    }

}