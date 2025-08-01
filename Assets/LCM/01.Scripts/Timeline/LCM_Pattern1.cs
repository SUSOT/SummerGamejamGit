using System.Collections;
using GondrLib.Dependencies;
using GondrLib.ObjectPool.Runtime;
using KHG.Bullets;
using LCM._01.Scripts.Bullets;
using UnityEngine;

namespace LCM._01.Scripts.Timeline
{
    public class Pattern1 : TimeLinePattern
    {
        [SerializeField] private PoolingItemSO triangleCannon;
        [SerializeField] private PoolingItemSO wall;
        [SerializeField] private PoolingItemSO crossLaser;
        [SerializeField] private PoolingItemSO laser;
        [Inject] private PoolManagerMono _poolManager;
       
        
        private void OnEnable()
        {
            Injector.Instance.InjectRuntime(this);
        }

        public override void Execute()
        {
            StartCoroutine(PatternCoroutine());
        }

        private IEnumerator PatternCoroutine()
        {
            for (int i = -1; i < 2; ++i)
            {
                CrossLaserBullet cr = _poolManager.Pop<CrossLaserBullet>(crossLaser);
                cr.transform.position = new Vector3(i * 20, 0, 0);
                yield return new WaitForSeconds(1f);
            }
            yield return new WaitForSeconds(3f);
            TriangleCannon tc = _poolManager.Pop<TriangleCannon>(triangleCannon);
            tc.transform.position = new Vector3(0, 30, 0);
            tc.MovePosition = Vector2.zero;
            tc.FireDuration = 0.35f;
            for (int i = 0; i < 10; ++i)
            {
                WallBullet wb = _poolManager.Pop<WallBullet>(wall);
                int random = Random.Range(0, 2);
                wb.transform.position = random == 1 ? new Vector3(30f, 0, 0) : new Vector3(-30f, 0, 0);
                wb.MoveDirection = random == 1 ? Vector2.left : Vector2.right;
                wb.MoveSpeed = 8f;
                if (i > 4)
                {
                    LaserBullet lb = _poolManager.Pop<LaserBullet>(laser);
                    lb.transform.position = Vector2.zero;
                    lb.transform.rotation = Quaternion.Euler(0,0,Random.Range(0,360));
                }
                yield return new WaitForSeconds(Random.Range(2f, 3.5f));
            }
            
            for (int i = 0; i < 10; ++i)
            {
                LaserBullet lb = _poolManager.Pop<LaserBullet>(laser);
                lb.transform.position = Vector2.zero;
                lb.transform.rotation = Quaternion.Euler(0,0,Random.Range(0,360));
                yield return new WaitForSeconds(0.1f);
            }
        }
    }
}
