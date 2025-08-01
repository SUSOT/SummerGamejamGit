using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class CameraEvent
{
    public static readonly CameraFocusEvent CameraFocusEvent = new();
}
public class CameraFocusEvent : GameEvent
{
    public Transform target;
    public float targetSize = 10f;
}

public class CameraFocus : MonoBehaviour
{
    [SerializeField] private GameEventChannelSO _cameraChannel;

    [SerializeField] private CinemachineCamera vCam;

    private void Awake()
    {
        _cameraChannel.AddListener<CameraFocusEvent>(OnCameraFocus);
    }

    private void OnCameraFocus(CameraFocusEvent arg)
    {
        SetTarget(arg.target);
        SetLens(arg.targetSize);
    }

    private void SetLens(float targetSize)
    {
        vCam.Lens.OrthographicSize = targetSize;
    }

    private void SetTarget(Transform target)
    {
        vCam.Follow = target;
    }
}
