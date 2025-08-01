using UnityEngine;

public class BgmChange : MonoBehaviour
{
    [SerializeField] private AudioClip bgmSource;
    [SerializeField] private GameEventChannelSO channel;

    private void Start()
    {
        channel.RaiseEvent(AudioEvents.AudioChangeEvent.Initializer(AudioType.BGM, bgmSource, true));
    }
}
