using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using GondrLib.ObjectPool.Runtime;
using UnityEngine;

namespace LCM._01.Scripts.Bullets
{
    public class TriangleCannon : Bullet
    {
        [SerializeField] private PoolingItemSO normalBullet;
        [field: SerializeField] public float MoveTime { get; set; } = 2.5f;
        [field: SerializeField] public float RotationSpeed { get; set; } = 180f;
        [field: SerializeField] public float RotationDuration { get; set; } = 6f;
        [field: SerializeField] public float FireDuration { get; set; } = 1f;
        
        public List<Transform> muzzles;

        public void StartInfiniteRotation()
        {
            float totalRotationAngle = RotationSpeed * RotationDuration;

            transform.DORotate(
                    new Vector3(0, 0, totalRotationAngle),
                    RotationDuration,
                    RotateMode.FastBeyond360)
                .SetEase(Ease.Linear)
                .SetRelative(true)
                .OnStart(() =>
                    StartCoroutine(ShootingBullet()))
                .OnComplete(() =>
                {
                    StopAllCoroutines();
                    transform.DOMoveY(150f, MoveTime).SetEase(Ease.OutSine);
                });
        }
        

        // ReSharper disable once FunctionRecursiveOnAllPaths
        private IEnumerator ShootingBullet()
        {
            foreach (var bulletTrm in muzzles)
            {
                NormalBullet bullet = _poolManager.Pop<NormalBullet>(normalBullet);
                bullet.transform.SetPositionAndRotation(bulletTrm.position, bulletTrm.rotation);
                bullet.MoveDirection = bulletTrm.up;
                bullet.moveSpeed = 20f;
            }

            yield return new WaitForSeconds(FireDuration);
            StartCoroutine(ShootingBullet());
        }

        public override void SetUpPool(Pool pool)
        {
        }

        public override void ResetItem()
        {
        }
    }
}