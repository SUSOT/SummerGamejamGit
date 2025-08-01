using UnityEngine;

public class PatorlOpen : MonoBehaviour
{

    [SerializeField] private GameEventChannelSO patorl;

   public void PatorlOpenEvet()
    {
        patorl.RaiseEvent(PatorlOpenEvents.PatorlEvent);
    }
}
