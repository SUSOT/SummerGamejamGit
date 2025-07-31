using UnityEngine;

public enum AudioType
{
    BGM,
    SFX
}

public static class AudioEvents
{
    public static AudioChangeEvent AudioChangeEvent = new AudioChangeEvent();
}

public class AudioChangeEvent : GameEvent 
{
    public AudioType audioType;
    public AudioClip clip;
    public bool IsLooping;
    public float volume = 1;

    public AudioChangeEvent Initializer(AudioType audioType,AudioClip clip,bool isLooping = false,float volume = 1)
    {
        this.audioType = audioType;
        this.clip = clip;
        IsLooping = isLooping;
        this.volume = volume;
        return this;
    }
}

