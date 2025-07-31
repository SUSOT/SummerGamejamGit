using UnityEngine;

namespace KHG.Bullets
{
    public abstract class AnimEventController : MonoBehaviour
    {
        public abstract void OnDamageStart();
        public abstract void OnDamageEnd();
        public abstract void OnActivated();
        public abstract void DestroySelf();
    }

}