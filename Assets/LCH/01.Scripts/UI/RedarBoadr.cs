using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class RedarBoadr : MonoBehaviour
{
    [SerializeField] private Image readBoadrBackground;

    public void ReadBoarOpne()
    {
        readBoadrBackground.gameObject.transform.DOScale(1, 0.8f);
    }
}
