using DG.Tweening;
using UnityEngine;

namespace KHG.UI
{
    public class BlockerController : MonoBehaviour
    {
        [SerializeField] private GameEventChannelSO portalChannel;

        [SerializeField] private RectTransform blocker1;
        [SerializeField] private RectTransform blocker2;
        [SerializeField] private GameObject blockerB;

        private void OnEnable()
        {
            blocker1.gameObject.SetActive(true);
            blocker2.gameObject.SetActive(true);
            Sequence _seq = DOTween.Sequence();

            _seq.AppendInterval(2f);
            _seq.Append(blocker1.DOScaleY(1024, 2f));
            _seq.Join(blocker1.DOAnchorPosY(540, 3f)).SetEase(Ease.OutExpo);
            _seq.Join(blocker2.DOScaleY(1024, 2f));
            _seq.Join(blocker2.DOAnchorPosY(-540, 3f)).SetEase(Ease.OutExpo);

            _seq.AppendCallback(()=> blockerB.SetActive(false));

            _seq.Append(blocker1.DOScaleY(0, 1f)).SetEase(Ease.OutQuart);
            _seq.Join(blocker2.DOScaleY(0, 1f)).SetEase(Ease.OutQuart).OnComplete(() =>
            {
            blocker1.gameObject.SetActive(false);
            blocker2.gameObject.SetActive(false);

            PatorlEvent evt = PatorlOpenEvents.PatorlEvent;
            portalChannel.RaiseEvent(evt);
        });
        }
}
}
