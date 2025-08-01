using GondrLib.Dependencies;
using GondrLib.ObjectPool.Runtime;
using LCM._01.Scripts.Bullets;
using System.Collections;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

namespace KHG.Bullets
{
    public class CircleGenerate : MonoBehaviour
    {
        [SerializeField] private PoolingItemSO normalBullet;
        [SerializeField] private float rotationSpeed = 0;
        public bool AutoAngle { get; internal set; } = true;

        public float GenerateAngle = 60;
        public float DelayTime;
        public int BulletCount = 5;

        [Header("Bullet Setting")]
        public float Speed = 15f;
        public float Scale = 1.7f;
        public float BulletRotation = 15f;

        [Inject] private PoolManagerMono _poolManager;


        private void OnEnable()
        {
            Injector.Instance.InjectRuntime(this);
            if (TryGetComponent(out Rigidbody2D rigid)) rigid.AddTorque(rotationSpeed);
            CalculateAngle();
        }

        public void GenerateObstacles()
        {
            StartCoroutine(Generate());
        }

        private void CalculateAngle()
        {
            GenerateAngle = AutoAngle ? 360 / (BulletCount - 1) : GenerateAngle;
        }

        private IEnumerator Generate()
        {
            for (int i = 0; i < BulletCount; i++)
            {
                float currentAngle = GenerateAngle + (360f / BulletCount) * i;
                Spawn(currentAngle);
                yield return new WaitForSeconds(DelayTime);
            }
        }

        private void Spawn(float angle)
        {
            float bulletDirX = Mathf.Cos(angle * Mathf.Deg2Rad);
            float bulletDirY = Mathf.Sin(angle * Mathf.Deg2Rad);
            Vector2 bulletDirection = new Vector2(bulletDirX, bulletDirY).normalized;

            NormalBullet bullet = _poolManager.Pop<NormalBullet>(normalBullet);
            bullet.transform.SetPositionAndRotation(transform.position, Quaternion.identity);
            bullet.transform.localScale = Vector3.one * Scale;
            bullet.moveSpeed = Speed;
            bullet.MoveDirection = bulletDirection;
            bullet.rotationSpeed = BulletRotation;
        }
    }
}
