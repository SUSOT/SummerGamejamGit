using DG.Tweening;
using UnityEngine;

public class Boss_1 : TimeLinePattern
{
    [SerializeField] private Animator _bossAnimator;

    public override void Execute()
    {

    }
    private void BossSequence()
    {
        Sequence mySequence = DOTween.Sequence();

        mySequence.AppendCallback(()=> _bossAnimator.Play("B1_P1"));
        mySequence.AppendInterval(15);
    }
}
