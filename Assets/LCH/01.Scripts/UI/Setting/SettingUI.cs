using DG.Tweening;
using Settings.InputSetting;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SettingUI : MonoBehaviour
{

    [SerializeField] private GameObject backGround;
    [SerializeField] private InputReaderSO input;
    [SerializeField] private GameEventChannelSO sceneCheck;
    [SerializeField] private GameEventChannelSO settingOpne;
    [SerializeField] private GameObject settingUI;
    [SerializeField] private Button mainMenuBnt;

    private void Awake()
    {
        input.OnUIOnCancelPressed += HandleCancelUI;
        backGround.gameObject.transform.DOScale(0, 0.8f);
        sceneCheck.AddListener<SceneChangeCheck>(HandleSceneCheck);
        settingOpne.AddListener<Setting>(HandleSettingOpenCheck);
        mainMenuBnt.gameObject.SetActive(false);
    }

    private void HandleSettingOpenCheck(Setting evt)
    {
        if(evt.Open)
        {
            settingUI.transform.DOScale(1, 0.8f);
        }
    }

    private void HandleSceneCheck(SceneChangeCheck evt)
    {
        if(evt.SceneName != "Title")
        {
            mainMenuBnt.gameObject.SetActive(true);
        }
        else
        {
            mainMenuBnt.gameObject.SetActive(false);
        }
    }

    private void HandleCancelUI()
    {
        backGround.gameObject.transform.DOScale(0,0.8f);
        settingOpne.RaiseEvent(TitleEvents.Setting.Init(false));
    }

    private void OnDestroy()
    {
        input.OnUIOnCancelPressed -= HandleCancelUI;
        sceneCheck.RemoveListener<SceneChangeCheck>(HandleSceneCheck);
        settingOpne.RemoveListener<Setting>(HandleSettingOpenCheck);
    }
}
