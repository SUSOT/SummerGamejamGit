using System.Collections;
using UnityEngine;
using Unity.Cinemachine;
using UnityEngine.SceneManagement;
using GondrLib.Dependencies;
using GondrLib.ObjectPool.Runtime;

public class CameraEvent
{
    public static readonly CameraFocusEvent CameraFocusEvent = new();
}
public class CameraFocusEvent : GameEvent
{
    public Transform target;
    public float targetSize = 0f;
}
public class CameraFocus : MonoBehaviour
{
    [SerializeField] private GameEventChannelSO _cameraChannel;
    [SerializeField] private CinemachineCamera vCam;
    [SerializeField] private float transitionDuration = 2f;

    [Inject] private PoolManagerMono poolManagerMono;

    private void OnEnable()
    {
        Injector.Instance.InjectRuntime(this);
    }
    private void Awake()
    {
        _cameraChannel.AddListener<CameraFocusEvent>(OnCameraFocus);
    }

    private void OnDestroy()
    {
        _cameraChannel.RemoveListener<CameraFocusEvent>(OnCameraFocus);
    }

    private void OnCameraFocus(CameraFocusEvent arg)
    {
        StartCoroutine(SmoothFocusTransition(arg.target, arg.targetSize));
    }

    private IEnumerator SmoothFocusTransition(Transform target, float targetSize)
    {
        yield return new WaitForSecondsRealtime(1f);

        Vector3 startPos = vCam.transform.position;
        Vector3 endPos = target.position + new Vector3(0, 0, -10);

        float startSize = vCam.Lens.OrthographicSize;

        float elapsed = 0f;

        while (elapsed < transitionDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = elapsed / transitionDuration;
            t = Mathf.Clamp01(t);

            vCam.transform.position = Vector3.Lerp(startPos, endPos, t);
            vCam.Lens.OrthographicSize = Mathf.Lerp(startSize, targetSize, t);

            yield return null;
        }

        vCam.transform.position = endPos;
        vCam.Lens.OrthographicSize = targetSize;

        PoolManagerMono.Instacne.AllPush();
        Time.timeScale = 1f;
        SceneManager.LoadScene("GameoverScene");
    }
}
