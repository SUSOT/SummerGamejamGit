using UnityEngine;
using DG.Tweening;

public class TitleDirection : MonoBehaviour
{
    private void Start()
    {
        Sequence seq = DOTween.Sequence();
        seq.Append(gameObject.transform.DOScale(0.4f, 1f));
        seq.Join(gameObject.transform.DORotate(new Vector3(0, 0, 5f), 1.2f));
        seq.Append(gameObject.transform.DOScale(1,1f));
        seq.Join(gameObject.transform.DORotate(new Vector3(0,0,-5f),1.2f));
        seq.SetLoops(-1);
        seq.SetEase(Ease.Linear);
    }
}
