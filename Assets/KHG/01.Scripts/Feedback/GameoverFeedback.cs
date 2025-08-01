using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent((typeof(Volume)))]
public class GameoverFeedback : Feedback
{
    [SerializeField] private GameEventChannelSO cameraChannel;
    [SerializeField] private Transform plr;

    private Volume GameoverVolume;

    private void Awake()
    {
        GameoverVolume = GetComponent<Volume>();
    }
    public override void CreateFeedback()
    {
        GameoverVolume.enabled = true;

        if (cameraChannel == null) return;
        cameraChannel.RaiseEvent(CameraEvent.CameraFocusEvent);
        Time.timeScale = 0;
    }

    public override void StopFeedback()
    {
        GameoverVolume.enabled = false;
    }
}
