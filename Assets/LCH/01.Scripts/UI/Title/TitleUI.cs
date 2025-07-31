using DG.Tweening;
using EasyTransition;
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
    [SerializeField] private TitleDirection direction;
    [SerializeField] private float inputCooldown = 0.2f;
    [SerializeField] private string loadScene;
    [SerializeField] private Image selectImage;
    private float _inputTime;
    private int _currentIndex = 0;

    private void Awake()
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
                inputSO.EnablePlayerCnt();
                DemoLoadScene.instance.LoadScene(loadScene);
                break;
            case ImageTypeEnum.SETTING:
                SettingManager.Instance.IsOpen = true;
                break;
            case ImageTypeEnum.EXIT:
                Application.Quit();
                break;
        }
    }

    private void HandleMoveSelect(Vector2 value)
    {
        if(SettingManager.Instance.IsOpen == false)
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

            selectImage.gameObject.transform.SetParent(images[_currentIndex].gameObject.transform);
            RectTransform rectTransform = selectImage.rectTransform;
            rectTransform.DOAnchorPos(new Vector2(-90, 0), 0.2f).SetEase(Ease.OutQuad);
            images[_currentIndex].SelectImage();
        }
       
    }

    private void OnDestroy()
    {
        inputSO.OnUINavigation -= HandleMoveSelect;
        inputSO.OnUIOnSubmitPressed -= HandleSubmit;
    }
}
