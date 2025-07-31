using DG.Tweening;
using Settings.InputSetting;
using System;
using UnityEngine;
using UnityEngine.Events;

public class SettingUI : MonoBehaviour
{

    [SerializeField] private GameObject backGround;
    [SerializeField] private InputReaderSO input;
    public UnityEvent OnSettingUIClose;

    private void Awake()
    {
        input.OnUIOnCancelPressed += HandleCancelUI;
        backGround.gameObject.transform.DOScale(0, 0.8f);
    }

    private void HandleCancelUI()
    {
        backGround.gameObject.transform.DOScale(0,0.8f);
        OnSettingUIClose?.Invoke(); 
    }

    private void OnDestroy()
    {
        input.OnUIOnCancelPressed -= HandleCancelUI;
    }
}
