using DG.Tweening;
using GondrLib.ObjectPool.Runtime;
using LCM._01.Scripts;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class SommonerBullet : Bullet
{
    [SerializeField] private float moveSpeed = 10;
    //[SerializeField] private float rotationSpeed;

    [SerializeField] private float repeatDuration = 1f;

    public UnityEvent spawnEvent;
    private Pool _currentPool;
    private Vector3 _originScale;

    private void Awake()
    {
        _originScale = transform.localScale;
    }
    private void Start()
    {
        StartCoroutine(Spawn());
    }
    public override void ResetItem()
    {
    }

    public override void SetUpPool(Pool pool) => _currentPool = pool;

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);
    }
    private void FixedUpdate()
    {
        SetMovement();
    }

    private void SetMovement()
    {
        transform.position += transform.up * moveSpeed * Time.fixedDeltaTime;
    }

    private IEnumerator Spawn()
    {
        spawnEvent?.Invoke();
        transform.DOScale(_originScale * 1.5f, 0.1f).OnComplete(() => transform.DOScale(_originScale, 0.1f));
        yield return new WaitForSeconds(repeatDuration);
        StartCoroutine(Spawn());
    }
}
