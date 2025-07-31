using UnityEngine;
using DG.Tweening;
using System;

public class TitleDirection : MonoBehaviour
{
    [SerializeField] private GameEventChannelSO setting;
    private Sequence _seq;

    private void OnEnable()
    {
        setting.AddListener<Setting>(HandleSettingOpen);
    }

    private void HandleSettingOpen(Setting evt)
    {
        if (evt.Open)
        {
            _seq.Pause();   
        }
        else
        {
            _seq.Play();
        }
    }

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

    private void OnDestroy()
    {
        setting.RemoveListener<Setting>(HandleSettingOpen);
    }
}
