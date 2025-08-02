using DG.Tweening;
using EasyTransition;
using Entities;
using GondrLib.ObjectPool.Runtime;
using Players;
using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Patorl : MonoBehaviour
{
    [SerializeField] private string nextScene;
    [SerializeField] private Vector2 overlapSize;
    [SerializeField] private LayerMask whatIsPlayer;
    [SerializeField] private ContactFilter2D contactFilter;
    [SerializeField] private GameEventChannelSO patorlOpen;
    [SerializeField] private bool IsOpenClose;
    [SerializeField] private bool _Open = false;

    private bool hasTriggeredPortal = false;

    private void Awake()
    {
        patorlOpen.AddListener<PatorlEvent>(HandleOpen);
    }

    private void HandleOpen(PatorlEvent evt)
    {
        _Open = true;
        var sequence = DOTween.Sequence();
        sequence.Append(transform.DOScale(1, 0.5f));
        sequence.Join(transform.DORotate(new Vector3(0, 0, 360), 0.5f, RotateMode.FastBeyond360));
    }

    private void OnDestroy()
    {
        patorlOpen.RemoveListener<PatorlEvent>(HandleOpen);
    }

    private void Start()
    {

        if (IsOpenClose)
        {

            var sequence = DOTween.Sequence();
            sequence.Append(transform.DOScale(4, 0.5f));
            sequence.Join(transform.DORotate(new Vector3(0, 0, 360), 0.5f, RotateMode.FastBeyond360));

            StartCoroutine(AutoClosePortalAtStart());
        }
        StartCoroutine(CheckPlayerOverlap());
        StartCoroutine(CheckPlayerInPotarlOverlap());
    }

    private IEnumerator AutoClosePortalAtStart()
    {
        yield return new WaitForSeconds(2f);

        _Open = false;
        IsOpenClose = false;

        var sequence = DOTween.Sequence();
        sequence.Append(transform.DOScale(0, 0.5f));
        sequence.Join(transform.DORotate(new Vector3(0, 0, -360), 0.5f, RotateMode.FastBeyond360));
    }

    private IEnumerator CheckPlayerInPotarlOverlap()
    {
        while (true)
        {
            if(_Open == true)
            {
                if (hasTriggeredPortal)
                {
                    yield break;
                }

                RaycastHit2D hit = Physics2D.BoxCast(
                    transform.position,
                    new Vector2(4, 4),
                    0,
                    Vector2.zero,
                    0,
                    whatIsPlayer
                );

                if (hit.collider != null)
                {
                    hasTriggeredPortal = true;
                   
                    StartCoroutine(ExecutePortalSequence(hit.collider));
                    yield break;
                }
            }
            yield return new WaitForSeconds(0.3f);

        }
    }

    private IEnumerator ExecutePortalSequence(Collider2D playerCollider)
    {
        playerCollider.transform.SetParent(transform);
        playerCollider.gameObject.transform.position = new Vector2(transform.position.x,transform.position.y);
        EntityMover mover = playerCollider.gameObject.GetComponentInChildren<EntityMover>();
        mover.CanManualMove = false;
        if (nextScene == "Stage3")
        {
            DemoLoadScene.instance.IsNomarlClear = true;
            PlayerPrefs.SetFloat("Clear", 1);
        }
        PoolManagerMono.Instacne.AllPush();
        PoolManagerMono.Instacne.poolManager.Initialize(PoolManagerMono.Instacne.transform);
        DemoLoadScene.instance.LoadScene(nextScene);
        playerCollider.gameObject.transform.position = new Vector2(transform.position.x, transform.position.y);
        
        yield return new WaitForSeconds(0.3f);
        playerCollider.gameObject.transform.position = new Vector2(transform.position.x, transform.position.y);
        var sequence = DOTween.Sequence();
        sequence.Append(transform.DOScale(0, 0.5f));
        sequence.Join(transform.DORotate(new Vector3(0, 0, 360), 0.5f, RotateMode.FastBeyond360));

        yield return sequence.WaitForCompletion();

      
    }

    private IEnumerator CheckPlayerOverlap()
    {
        bool wasOverlapping = false;

        while (true)
        {
            if (_Open)
            {
                if (hasTriggeredPortal)
                {
                    yield break;
                }

                bool isOverlapping = Physics2D.OverlapBox(transform.position, overlapSize, 0, whatIsPlayer);

                if (isOverlapping && !wasOverlapping)
                {
                    transform.DOScale(4f, 0.2f);
                }
                else if (!isOverlapping && wasOverlapping)
                {
                    transform.DOScale(2f, 0.2f);
                }

                wasOverlapping = isOverlapping;
            }

            yield return new WaitForSeconds(0.3f);

        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, overlapSize);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position, new Vector2(4, 4));
    }
#endif
}