using DG.Tweening;
using EasyTransition;
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
    }

    private bool _sliderCooldown = false;

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
            else
            {
                if (_isSlider)
                {
                    Debug.Log("슬라이더 조작 종료");
                    _slider = null;
                    _isSlider = false;
                    input.OnUINavigation += HandleNaveigation;
                    input.OnUISilder -= HandleSilder;

                    _sliderCooldown = true;
                    Invoke(nameof(ResetSliderCooldown), 0.05f);
                    return;
                }

                if (!_sliderCooldown && !_isSlider)
                {


                    if (selected.TryGetComponent(out Slider slider) && slider != _slider)
                    {
                        Debug.Log("슬라이더 조작 시작");
                        _slider = slider;
                        _isSlider = true;

                        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(_slider.gameObject);

                        input.OnUINavigation -= HandleNaveigation;
                        input.OnUISilder += HandleSilder;
                    }
                }
            }
        }
       
        

    }

    private void ResetSliderCooldown()
    {
        _sliderCooldown = false;
    }
    private void HandleSilder(Vector2 value)
    {
        _sliderInput = value;
    }

    private void HandleNaveigation(Vector2 value)
    {
        if (IsOpen && !_isSlider)
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
        }
    }


    public void GoTitleScene()
    {
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
        if (_isSlider && _slider != null && Mathf.Abs(_sliderInput.x) > 0.1f)
        {
            if (Time.time - _sliderInputTime >= inputCooldown)
            {
                float step = (_slider.maxValue - _slider.minValue) / 10f;

                if (_sliderInput.x < 0)
                {
                    _slider.value = Mathf.Max(_slider.minValue, _slider.value - step);
                }
                else if (_sliderInput.x > 0)
                {
                    _slider.value = Mathf.Min(_slider.maxValue, _slider.value + step);
                }

                if (_slider.name.Contains("Master"))
                    audioSetting.SetMasterVolume();
                else if (_slider.name.Contains("BGM"))
                    audioSetting.SetBgmVolume();
                else if (_slider.name.Contains("SFX"))
                    audioSetting.SetSfxVolume();

                _sliderInputTime = Time.time;
            }
        }
    }



    private void OnDestroy()
    {
        input.OnUIOnCancelPressed -= HandleCancelUI;
        sceneCheck.RemoveListener<SceneChangeCheck>(HandleSceneCheck);
       
    }
}
