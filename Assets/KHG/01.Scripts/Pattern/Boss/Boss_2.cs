using UnityEngine;

public class Boss_2 : TimeLinePattern
{
    [SerializeField] private Animator _bossAnimator;

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
        _bossAnimator.Play("B2_WARN");
    }
}
