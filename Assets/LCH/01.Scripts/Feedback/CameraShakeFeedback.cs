using Unity.Cinemachine;
using UnityEngine;

[RequireComponent((typeof(CinemachineImpulseSource)))]
public class CameraShakeFeedback : Feedback
{
    [SerializeField] private bool onlyPlayPowerAttack = true;
    [SerializeField] private float impulseForce = 0.6f;
    private CinemachineImpulseSource _impulseSource;
    private void Awake()
    {
        _impulseSource = GetComponent<CinemachineImpulseSource>();
    }
    public override void CreateFeedback()
    {
        _impulseSource.GenerateImpulse(impulseForce);
    }

    public override void StopFeedback()
    {
        
    }
}
