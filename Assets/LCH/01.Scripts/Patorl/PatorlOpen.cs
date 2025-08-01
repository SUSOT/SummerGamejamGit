using GondrLib.ObjectPool.Runtime;
using UnityEngine;

public class PatorlOpen : MonoBehaviour
{

    [SerializeField] private GameEventChannelSO patorl;

   public void PatorlOpenEvet()
    {
        PoolManagerMono.Instacne.AllPush();
        patorl.RaiseEvent(PatorlOpenEvents.PatorlEvent);
    }
}
