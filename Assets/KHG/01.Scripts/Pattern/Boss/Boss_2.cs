using UnityEngine;

public class Boss_2 : TimeLinePattern
{
    [SerializeField] private Animator _bossAnimator;
    [SerializeField] private GameEventChannelSO channel;
    [SerializeField] private AudioClip cilp;
    private void Start()
    {
        Execute();
    }
    public override void Execute()
    {
        BossSequence();
    }
    private void BossSequence()
    {
        channel.RaiseEvent(AudioEvents.AudioChangeEvent.Initializer(AudioType.BGM, cilp, true));
        _bossAnimator.Play("B2_WARN");
    }
}
