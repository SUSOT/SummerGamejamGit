using UnityEngine;
using DG.Tweening;

public class TitleDirection : MonoBehaviour
{
    private void Start()
    {
        Sequence seq = DOTween.Sequence();
        seq.Append(gameObject.transform
            .DOLocalRotate(new Vector3(0, 0, 10), 0.5f, RotateMode.LocalAxisAdd));
        seq.Join(gameObject.transform.DOScale(0.6f, 1f));
        seq.Append(gameObject.transform
            .DOLocalRotate(new Vector3(0, 0, -10), 0.5f, RotateMode.LocalAxisAdd));
        seq.Join(gameObject.transform.DOScale(1, 1f));
        seq.SetLoops(-1);
        seq.SetEase(Ease.Linear);
    }
}
