using DG.Tweening;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private GameEventChannelSO audioChangeEvent;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        audioChangeEvent.AddListener<AudioChangeEvent>(HandleAudioChange);
    }

    private void OnDestroy()
    {
        audioChangeEvent.RemoveListener<AudioChangeEvent>(HandleAudioChange);
    }

    private void HandleAudioChange(AudioChangeEvent evt)
    {
        switch (evt.audioType)
        {
            case AudioType.BGM:
                BGMChange(evt.clip);
                break;
            case AudioType.SFX:
                SFXChange(evt.clip,evt.IsLooping,evt.volume);
                break;
        }
    }

    private void BGMChange(AudioClip clip)
    {
        bgmSource.DOPitch(0, 0.8f).OnComplete(()=>
        {
            DOVirtual.DelayedCall(0.3f, () => bgmSource.DOPitch(1, 0.8f));
            
        });
        bgmSource.clip = clip;
        bgmSource.Play();
    } 
    private void SFXChange(AudioClip clip,bool isLoop,float volume)
    {
        sfxSource.loop = isLoop;
        sfxSource.clip = clip;
        sfxSource.volume = volume;
        sfxSource.Play();
    }
}
