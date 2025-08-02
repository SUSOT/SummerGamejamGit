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
        _seq.Append(gameObject.transform.DOScale(0.95f, 0.1f).OnComplete(() => gameObject.transform.DOScale(1, 0.1f)));
        _seq.AppendInterval(0.1f);
        _seq.Join(gameObject.transform.DOScale(0.95f, 0.1f).OnComplete(() => gameObject.transform.DOScale(1, 0.1f)));
        _seq.AppendInterval(0.6f);
        _seq.SetLoops(-1);
        _seq.SetEase(Ease.Linear);
    }

    private void OnDestroy()
    {
        setting.RemoveListener<Setting>(HandleSettingOpen);
    }
}
