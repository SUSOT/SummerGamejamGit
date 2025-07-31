using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioSetting : MonoBehaviour
{
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private AudioMixer mixer;
    [SerializeField] private GameEventChannelSO setting;

    private void OnEnable()
    {
        setting.AddListener<Setting>(HandleSetting);
    }

    private void HandleSetting(Setting evt)
    {
        if (evt.Open)
        {
            if (PlayerPrefs.HasKey("BGMVolume"))
            {
                LoadBgmVolume();
            }
            if (PlayerPrefs.HasKey("SFXVolume"))
            {
                LoadSfxVolume();
            }
            if (PlayerPrefs.HasKey("MasterVolume"))
            {
                LoadMasterVolume();
            }
            else
            {
                SetMasterVolume();
                SetBgmVolume();
                SetSfxVolume();
            }
        }
        
    }

    private void LoadBgmVolume()
    {
        bgmSlider.value = PlayerPrefs.GetFloat("BGMVolume");
        SetBgmVolume();
    }

    private void LoadSfxVolume()
    {
        sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume");
        SetSfxVolume();
    }

    private void LoadMasterVolume()
    {
        masterSlider.value = PlayerPrefs.GetFloat("MasterVolume");
        SetMasterVolume();
    }

    public void SetMasterVolume()
    {
        float vol = Mathf.Clamp(masterSlider.value, 0.0001f, 1f);
        float volume = Mathf.Log10(vol) * 20;
        mixer.SetFloat("Master", volume);
        PlayerPrefs.SetFloat("MasterVolume", vol);
    }

    public void SetBgmVolume()
    {
        float vol = Mathf.Clamp(bgmSlider.value, 0.0001f, 1f);
        float volume = Mathf.Log10(vol) * 20;
        mixer.SetFloat("BGM", volume);
        PlayerPrefs.SetFloat("BGMVolume", vol);

    }

    public void SetSfxVolume()
    {
        float vol = Mathf.Clamp(sfxSlider.value, 0.0001f, 1f);
        float volume = Mathf.Log10(vol) * 20;
        mixer.SetFloat("SFX", volume);
        PlayerPrefs.SetFloat("SFXVolume", vol);
    }

    private void OnDestroy()
    {
        setting.RemoveListener<Setting>(HandleSetting);
    }
}
