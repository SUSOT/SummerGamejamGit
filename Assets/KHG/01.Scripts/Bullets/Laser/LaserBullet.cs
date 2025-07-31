using GondrLib.ObjectPool.Runtime;
using LCM._01.Scripts;
using System;
using UnityEngine;
using UnityEngine.Events;

public class LaserBullet : Bullet
{
    public UnityEvent ActiveEvent;
    public float rotation
    {
        get => transform.rotation.z;
        set => transform.rotation = Quaternion.Euler(transform.rotation.x,transform.rotation.y, value);
    }
    private bool _damageable = false;

    private Pool _laserPool;

    public void OnDamageStart() => _damageable = true;
    public void OnDamageEnd() => _damageable = false;
    public void DestroySelf() => Destroy(gameObject);
    public void OnActivated() => ActiveEvent?.Invoke();

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if(_damageable == true) base.OnTriggerEnter2D(other);
    }
    public override void ResetItem()
    {
    }

    public override void SetUpPool(Pool pool)
    {
        _laserPool = pool;
    }
}
