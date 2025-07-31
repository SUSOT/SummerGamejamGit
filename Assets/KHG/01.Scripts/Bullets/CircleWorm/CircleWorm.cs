using GondrLib.ObjectPool.Runtime;
using LCM._01.Scripts;
using System;
using UnityEngine;

public class CircleWorm : Bullet
{
    [SerializeField] private float moveSpeed = 20;
    [SerializeField] private float rotationSpeed;


    private void FixedUpdate()
    {
        SetMovement();
    }

    private void SetMovement()
    {
        transform.position += transform.up * rotationSpeed * Time.fixedDeltaTime;
        if (rotationSpeed > 0)
        {
            transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, transform.rotation.z + rotationSpeed * Time.fixedDeltaTime);
        }
    }

    public override void ResetItem()
    {
    }

    public override void SetUpPool(Pool pool)
    {
    }
}
