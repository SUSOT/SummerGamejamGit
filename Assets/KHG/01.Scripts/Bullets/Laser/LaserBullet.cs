using GondrLib.ObjectPool.Runtime;
using LCM._01.Scripts;
using System;
using UnityEngine;

public class LaserBullet : Bullet
{
    public event Action activeEvent;
    private bool _damageable;

    private Pool _laserPool;

    public void OnDamageStart() => _damageable = true;
    public void OnDamageEnd() => _damageable = false;
    public void DestroySelf() => Destroy(gameObject);
    public void OnActivated() => activeEvent?.Invoke();

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if(_damageable) base.OnTriggerEnter2D(other);
    }
    public override void ResetItem()
    {
    }

    public override void SetUpPool(Pool pool)
    {
        _laserPool = pool;
    }
}
