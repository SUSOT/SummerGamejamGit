using GondrLib.Dependencies;
using GondrLib.ObjectPool.Runtime;
using LCM._01.Scripts.Bullets;
using System.Collections;
using UnityEngine;

namespace KHG.Bullets
{
    public class CircleGenerate : MonoBehaviour
    {
        [SerializeField] private PoolingItemSO normalBullet;

        [SerializeField] private float delayTime;
        [SerializeField] private int bulletCount;

        [SerializeField] private float rotationSpeed = 0;
        [SerializeField] private float generateAngle = 60;
        [Header("Bullet Setting")]
        [SerializeField] private float speed = 15f;
        [SerializeField] private float scale = 1.7f;

        [Inject] private PoolManagerMono _poolManager;

        private void OnEnable()
        {
            Injector.Instance.InjectRuntime(this);
            if (TryGetComponent(out Rigidbody2D rigid)) rigid.AddTorque(rotationSpeed);
        }

        public void GenerateObstacles()
        {
            StartCoroutine(Generate());
        }

        private IEnumerator Generate()
        {
            for (int i = 0; i < bulletCount; i++)
            {
                float currentAngle = generateAngle + (360f / bulletCount) * i;
                Spawn(currentAngle);
                yield return new WaitForSeconds(delayTime);
            }
        }

        private void Spawn(float angle)
        {
            float bulletDirX = Mathf.Cos(angle * Mathf.Deg2Rad);
            float bulletDirY = Mathf.Sin(angle * Mathf.Deg2Rad);
            Vector2 bulletDirection = new Vector2(bulletDirX, bulletDirY).normalized;

            NormalBullet bullet = _poolManager.Pop<NormalBullet>(normalBullet);
            bullet.transform.SetPositionAndRotation(transform.position, Quaternion.identity);
            bullet.transform.localScale = Vector3.one * scale;
            bullet.MoveSpeed = speed;
            bullet.MoveDirection = bulletDirection;
        }
    }
}
