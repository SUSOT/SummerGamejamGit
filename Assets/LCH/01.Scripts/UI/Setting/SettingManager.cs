using DG.Tweening;
using EasyTransition;
using GondrLib.ObjectPool.Runtime;
using Settings.InputSetting;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

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
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private AudioMixer audioMixer;

    private bool _isEsc = false;
    private bool _isOpen;
    private bool _isSlider = false;
    private bool isTransitioning = false;
    private int _currentIndex = 0;
    private float _inputTime;
    private Slider _slider;
    private Vector2 _sliderInput;
    private float _sliderInputTime;

    private const float DEFAULT_VOLUME = 0.8f;
    private const string MASTER_VOLUME_KEY = "MasterVolume";
    private const string BGM_VOLUME_KEY = "BGMVolume";
    private const string SFX_VOLUME_KEY = "SFXVolume";

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
                    .OnComplete(() => isTransitioning = false);

                input.OnUINavigation += HandleNaveigation;
                input.OnUIOnSubmitPressed += HandleSubmit;
            }
            else
            {
                _isSlider = false;
                SaveAllSettings();

                backGround.transform.DOScale(0, 0.8f)
                    .SetUpdate(true)
                    .OnComplete(() => isTransitioning = false);

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
            return;
        }

        input.OnUIOnCancelPressed += HandleCancelUI;
        backGround.transform.localScale = Vector3.zero;
        sceneCheck.AddListener<SceneChangeCheck>(HandleSceneCheck);
        mainMenuBnt.gameObject.SetActive(false);
    }

    private void Start()
    {
        masterSlider.onValueChanged.AddListener(value =>
        {
            ApplyVolume("Master", value);
            PlayerPrefs.SetFloat(MASTER_VOLUME_KEY, value);
            PlayerPrefs.Save();
        });

        bgmSlider.onValueChanged.AddListener(value =>
        {
            ApplyVolume("BGM", value);
            PlayerPrefs.SetFloat(BGM_VOLUME_KEY, value);
            PlayerPrefs.Save();
        });

        LoadSavedVolumes();
        UpdateCurrentSlider();
    }

    private void ApplyVolume(string mixerParam, float value)
    {
        float vol = Mathf.Clamp(value, 0.0001f, 1f);
        float dB;

        if (value <= 0.0001f)
        {
            dB = -80f;
        }
        else
        {
            dB = Mathf.Log10(vol) * 20f;
        }

        audioMixer.SetFloat(mixerParam, dB);
    }

    private void LoadSavedVolumes()
    {
        float masterVol = PlayerPrefs.GetFloat(MASTER_VOLUME_KEY, DEFAULT_VOLUME);
        masterSlider.SetValueWithoutNotify(masterVol);
        ApplyVolume("Master", masterVol);

        float bgmVol = PlayerPrefs.GetFloat(BGM_VOLUME_KEY, DEFAULT_VOLUME);
        bgmSlider.SetValueWithoutNotify(bgmVol);
        ApplyVolume("BGM", bgmVol);

        Debug.Log($"볼륨 로드 완료 - Master: {masterVol}, BGM: {bgmVol}");
    }

    private void SaveAllSettings()
    {
        PlayerPrefs.SetFloat(MASTER_VOLUME_KEY, masterSlider.value);
        PlayerPrefs.SetFloat(BGM_VOLUME_KEY, bgmSlider.value);
        PlayerPrefs.Save();

        Debug.Log($"설정 저장 완료 - Master: {masterSlider.value}, BGM: {bgmSlider.value}");
    }

    private void UpdateCurrentSlider()
    {
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
        if (!IsOpen) return;

        if (Time.unscaledTime - _inputTime < inputCooldown || value.y == 0)
            return;

        _inputTime = Time.unscaledTime;

        _currentIndex = value.y < 0
            ? (_currentIndex + 1) % selectSillder.Count
            : (_currentIndex - 1 + selectSillder.Count) % selectSillder.Count;

        selectImage.transform.SetParent(selectSillder[_currentIndex]);
        selectImage.rectTransform.DOAnchorPos(new Vector2(-185, 0), 0.2f).SetUpdate(true).SetEase(Ease.OutQuad);

        UpdateCurrentSlider();
    }

    public void GoTitleScene()
    {
        SaveAllSettings();
        DemoLoadScene.instance.LoadScene("Title");
    }

    private void HandleSceneCheck(SceneChangeCheck evt)
    {
        mainMenuBnt.gameObject.SetActive(evt.SceneName != "Title");
        if (evt.SceneName != "Title")
        {
            _isEsc = true;
            if (!selectSillder.Contains(mainMenuBnt.transform))
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
        IsOpen = _isEsc ? !IsOpen : false;
    }

    private void Update()
    {
        if (!IsOpen || !_isSlider || _slider == null) return;

        _sliderInput = input.sliderDir;

        if (Mathf.Abs(_sliderInput.x) > 0.1f && Time.unscaledTime - _sliderInputTime >= inputCooldown)
        {
            float step = (_slider.maxValue - _slider.minValue) / 20f;

            float newValue = Mathf.Clamp(
                _slider.value + Mathf.Sign(_sliderInput.x) * step,
                _slider.minValue,
                _slider.maxValue
            );

            _slider.value = newValue;

            _sliderInputTime = Time.unscaledTime;
        }
    }

    private void OnDestroy()
    {
        SaveAllSettings();

        input.OnUIOnCancelPressed -= HandleCancelUI;
        sceneCheck.RemoveListener<SceneChangeCheck>(HandleSceneCheck);
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            SaveAllSettings();
        }
    }

    private void OnApplicationFocus(bool hasFocus)
    {
        if (!hasFocus)
        {
            SaveAllSettings();
        }
    }

    [ContextMenu("Test Save Settings")]
    private void TestSaveSettings()
    {
        SaveAllSettings();
    }

    [ContextMenu("Test Load Settings")]
    private void TestLoadSettings()
    {
        LoadSavedVolumes();
    }
}