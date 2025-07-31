using Settings.InputSetting;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleUI : MonoBehaviour
{
    [SerializeField] private List<ImageType> images;
    [SerializeField] private InputReaderSO inputSO;
    [SerializeField] private Image settingUI;
    [SerializeField] private float inputCooldown = 0.2f;
    [SerializeField] private string loadScene;
    private float _inputTime;
    private int _currentIndex = 0;

    private void Start()
    {
        inputSO.DisablePlayerCnt();
        inputSO.EnableUICnt();
        images[_currentIndex].SelectImage();
        inputSO.OnUINavigation += HandleMoveSelect;
        inputSO.OnUIOnSubmitPressed += HandleSubmit;
    }

    private void HandleSubmit()
    {
        switch (images[_currentIndex].type)
        {
            case ImageTypeEnum.START:
                SceneManager.LoadScene(loadScene);
                break;
            case ImageTypeEnum.SETTING:
                settingUI.gameObject.SetActive(true);
                break;
            case ImageTypeEnum.EXIT:
                Application.Quit();
                break;
        }
    }

    private void HandleMoveSelect(Vector2 value)
    {
        if (Time.time - _inputTime < inputCooldown || value.y == 0)
            return;

        _inputTime = Time.time;

        images[_currentIndex].NotSelectImage();
        if (value.y < 0)
        {
            _currentIndex = (_currentIndex + 1) % images.Count;
        }
        else if (value.y > 0)
        {
            _currentIndex = (_currentIndex - 1 + images.Count) % images.Count;
        }

        images[_currentIndex].SelectImage();
    }

    private void OnDestroy()
    {
        inputSO.OnUINavigation -= HandleMoveSelect;
        inputSO.OnUIOnSubmitPressed -= HandleSubmit;
    }
}
