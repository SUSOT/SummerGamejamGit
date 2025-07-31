using UnityEngine;
using DG.Tweening;

public class TitleDirection : MonoBehaviour
{

    private Sequence _seq;

    private void Start()
    {
        _seq = DOTween.Sequence();
        _seq.Append(gameObject.transform
            .DOLocalRotate(new Vector3(0, 0, 10), 0.5f, RotateMode.LocalAxisAdd));
        _seq.Join(gameObject.transform.DOScale(0.6f, 1f));
        _seq.Append(gameObject.transform
            .DOLocalRotate(new Vector3(0, 0, -10), 0.5f, RotateMode.LocalAxisAdd));
        _seq.Join(gameObject.transform.DOScale(1, 1f));
        _seq.SetLoops(-1);
        _seq.SetEase(Ease.Linear);
    }

    public void Pase()
    {
        _seq.Pause();
    }

    public void PlayTween()
    {
        _seq.Play();
    }
}
