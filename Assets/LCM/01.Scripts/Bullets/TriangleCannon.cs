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
        [SerializeField] private float moveTime = 2.5f;
        [SerializeField] private float rotationSpeed = 180f;
        [SerializeField] private float rotationDuration = 6f;
        [SerializeField] private float fireDuration = 1f;

        public List<Transform> muzzles;

        protected override void OnEnable()
        {
            base.OnEnable();
            StartCoroutine(MoveCoroutine());
        }

        private IEnumerator MoveCoroutine()
        {
            yield return new DOTweenCYInstruction.WaitForCompletion(
                transform.DOMove(Vector3.zero, moveTime).SetEase(Ease.OutSine));

            StartInfiniteRotation();
        }

        private void StartInfiniteRotation()
        {
            float totalRotationAngle = rotationSpeed * rotationDuration;

            transform.DORotate(
                    new Vector3(0, 0, totalRotationAngle),
                    rotationDuration,
                    RotateMode.FastBeyond360)
                .SetEase(Ease.Linear)
                .SetRelative(true)
                .OnStart(() =>
                    StartCoroutine(ShootingBullet()))
                .OnComplete(() =>
                {
                    StopAllCoroutines();
                    transform.DOMoveY(30f, moveTime).SetEase(Ease.OutSine);
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
            }

            yield return new WaitForSeconds(fireDuration);
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