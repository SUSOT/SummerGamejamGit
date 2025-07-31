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
        backGround.gameObject.SetActive(false);
        input.OnUIOnCancelPressed += HandleCancelUI;
    }

    private void HandleCancelUI()
    {
        backGround.gameObject.SetActive(false);
        OnSettingUIClose?.Invoke(); 
    }

    private void OnDestroy()
    {
        input.OnUIOnCancelPressed -= HandleCancelUI;
    }
}
