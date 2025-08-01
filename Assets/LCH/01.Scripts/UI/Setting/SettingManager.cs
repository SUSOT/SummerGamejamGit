using DG.Tweening;
using EasyTransition;
using GondrLib.ObjectPool.Runtime;
using Settings.InputSetting;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SettingManager : MonoBehaviour
{
    public static SettingManager Instance;

    [SerializeField] private GameObject backGround;
    [SerializeField] private InputReaderSO input;
    [SerializeField] private GameEventChannelSO sceneCheck;
    [SerializeField] private float inputCooldown = 0.2f;
    [SerializeField] private List<Transform> selectSillder;
    [SerializeField] private Image selectImage;
    [SerializeField] private Button mainMenuBnt;
    [SerializeField] private AudioSetting audioSetting;
    private bool _isEsc = false;

    private bool _isOpen;
    private bool _isSlider = false;
    private bool isTransitioning = false;
    private int _currentIndex = 0;
    private float _inputTime;
    private Slider _slider;
    private Vector2 _sliderInput;
    private float _sliderInputTime;

    public bool IsOpen
    {
        get => _isOpen;
        set
        {
            if (isTransitioning) return;

            _isOpen = value;
            isTransitioning = true;

            Time.timeScale = _isOpen ? 0 : 1;

            if (_isOpen)
            {
                backGround.transform.DOScale(1, 0.8f)
                    .SetUpdate(true)
                    .OnComplete(() => {
                        isTransitioning = false;
                    });

                input.OnUINavigation += HandleNaveigation;
                input.OnUIOnSubmitPressed += HandleSubmit;
            }
            else
            {

                _isSlider = false;

                backGround.transform.DOScale(0, 0.8f)
                    .SetUpdate(true)
                    .OnComplete(() => {
                        isTransitioning = false;
                    });

                input.OnUINavigation -= HandleNaveigation;
                input.OnUIOnSubmitPressed -= HandleSubmit;
            }
        }
    }



    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        input.OnUIOnCancelPressed += HandleCancelUI;
        backGround.transform.localScale = Vector3.zero;
        sceneCheck.AddListener<SceneChangeCheck>(HandleSceneCheck);
       
        mainMenuBnt.gameObject.SetActive(false);

        if (selectSillder[_currentIndex].TryGetComponent(out Slider slider))
        {
            _slider = slider;
            _isSlider = true;
        }
        else
        {
            _slider = null;
            _isSlider = false;
        }
    }


    private void HandleSubmit()
    {
        if (IsOpen)
        {
            var selected = selectSillder[_currentIndex];

            if (selected.TryGetComponent(out Button button))
            {
                IsOpen = false;
                GoTitleScene();
            }
        }
             

    }

    private void HandleNaveigation(Vector2 value)
    {
        if (IsOpen)
        {
            if (Time.unscaledTime - _inputTime < inputCooldown || value.y == 0)
                return;

            _inputTime = Time.unscaledTime;

            if (value.y < 0)
            {
                _currentIndex = (_currentIndex + 1) % selectSillder.Count;
            }
            else if (value.y > 0)
            {
                _currentIndex = (_currentIndex - 1 + selectSillder.Count) % selectSillder.Count;
            }

            selectImage.transform.SetParent(selectSillder[_currentIndex]);
            RectTransform rectTransform = selectImage.rectTransform;
            rectTransform.DOAnchorPos(new Vector2(-185, 0), 0.2f)
                .SetUpdate(true)
                .SetEase(Ease.OutQuad);

            if (selectSillder[_currentIndex].TryGetComponent(out Slider slider))
            {
                _slider = slider;
                _isSlider = true;
            }
            else
            {
                _slider = null;
                _isSlider = false;
            }

        }
    }


    public void GoTitleScene()
    {
        PoolManagerMono.Instacne.AllPush();
        DemoLoadScene.instance.LoadScene("Title");
    }

    private void HandleSceneCheck(SceneChangeCheck evt)
    {
        mainMenuBnt.gameObject.SetActive(evt.SceneName != "Title");
        if(evt.SceneName != "Title")
        {
            _isEsc = true;
            selectSillder.Add(mainMenuBnt.transform);
        }
        else
        {
            _isEsc = false;
            selectSillder.Remove(mainMenuBnt.transform);
        }
    }

    private void HandleCancelUI()
    {
        if (_isEsc)
        {
            IsOpen = !IsOpen;
        }
        else
        {
            IsOpen = false;
        }
    }

    private void Update()
    {
        if (IsOpen)
        {
            if (!_isSlider) return;

            _sliderInput = input.sliderDir;

            if (_slider != null && Mathf.Abs(_sliderInput.x) > 0.1f)
            {
                if (Time.unscaledTime - _sliderInputTime >= inputCooldown)
                {
                    float step = (_slider.maxValue - _slider.minValue) / 10f;

                    if (_sliderInput.x < 0)
                        _slider.value = Mathf.Max(_slider.minValue, _slider.value - step);
                    else if (_sliderInput.x > 0)
                        _slider.value = Mathf.Min(_slider.maxValue, _slider.value + step);

                    if (_slider.name.Contains("Master"))
                        audioSetting.SetMasterVolume();
                    else if (_slider.name.Contains("BGM"))
                        audioSetting.SetBgmVolume();
                    else if (_slider.name.Contains("SFX"))
                        audioSetting.SetSfxVolume();

                    _sliderInputTime = Time.unscaledTime;
                }
            }
        }
        
    }

    private void OnDestroy()
    {
        input.OnUIOnCancelPressed -= HandleCancelUI;
        sceneCheck.RemoveListener<SceneChangeCheck>(HandleSceneCheck);

    }
}





