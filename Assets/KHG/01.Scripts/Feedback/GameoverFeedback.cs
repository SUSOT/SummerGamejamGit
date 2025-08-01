using GondrLib.Dependencies;
using GondrLib.ObjectPool.Runtime;
using KHG.Bullets;
using Players;
using Unity.Cinemachine;
using UnityEditor.EditorTools;
using UnityEngine;
using UnityEngine.Rendering;

//[RequireComponent((typeof(Volume)))]
public class GameoverFeedback : Feedback
{
    [SerializeField] private GameEventChannelSO cameraChannel;
    [SerializeField] private Transform player;
    [SerializeField] private Volume gameoverVolume;

    public override void CreateFeedback()
    {
        gameoverVolume.enabled = true;

        if (cameraChannel == null) return;
        CameraFocusEvent evt = CameraEvent.CameraFocusEvent;
        evt.target = player;
        cameraChannel.RaiseEvent(evt);
        Time.timeScale = 0;
    }

    public override void StopFeedback()
    {
        gameoverVolume.enabled = false;
    }
}
