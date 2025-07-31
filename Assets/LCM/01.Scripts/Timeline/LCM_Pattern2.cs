using System.Collections;
using GondrLib.Dependencies;
using GondrLib.ObjectPool.Runtime;
using KHG.Bullets;
using LCM._01.Scripts.Bullets;
using UnityEngine;

namespace LCM._01.Scripts.Timeline
{
    public class LCM_Pattern2 : TimeLinePattern
    {
        [Inject] private PoolManagerMono _poolManager;

        [SerializeField] private PoolingItemSO wave;
        [SerializeField] private PoolingItemSO cog;
        [SerializeField] private PoolingItemSO dum;
        [SerializeField] private PoolingItemSO laser;
        [SerializeField] private PoolingItemSO triangle;
        
        public LCM_Pattern2(float startTime) : base(startTime)
        {
        }
        
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
            for (int i = 0; i < 10; ++i)
            {
                CircleWorm cw = _poolManager.Pop<CircleWorm>(wave);
            
                if (i < 3)
                {
                    float xOffset = (i - 1) * 15f + 3f;
                    cw.transform.position = new Vector3(xOffset, -20f, 0);
                    cw.transform.rotation = Quaternion.identity;
                }
                else if (i < 5)
                {
                    float yOffset = (i - 3.5f) * 15f - 3f;
                    cw.transform.position = new Vector3(30f, yOffset, 0); 
                    cw.transform.rotation = Quaternion.Euler(0, 0, 90); 
                }
                else if (i < 8)
                {
                    float xOffset = (i - 6) * 15f - 3f;
                    cw.transform.position = new Vector3(xOffset, 20f, 0); 
                    cw.transform.rotation = Quaternion.Euler(0, 0, 180); 
                }
                else
                {
                    float yOffset = (i - 8.5f) * 15f + 3f;
                    cw.transform.position = new Vector3(-30f, yOffset, 0); 
                    cw.transform.rotation = Quaternion.Euler(0, 0, 270); 
                }
                cw.rotationSpeed = Random.Range(-45f, 45f);
                yield return new WaitForSeconds(0.4f);
            }
            
            yield return new WaitForSeconds(4f);
            CogwheelBullet cb = _poolManager.Pop<CogwheelBullet>(cog);
            int rand = Random.Range(0, 2);
            cb.transform.position = new Vector2(rand == 0 ? 45f : -45f, 0);
            cb.MoveDirection = rand == 0 ? Vector2.left : Vector2.right;
            cb.MoveSpeed = 14f;
            cb.RotationSpeed = 6f;
            
            yield return new WaitForSeconds(7f);
            for (int i = 0; i < 5; ++i)
            {
                StartCoroutine(DumbbellBulletCoroutine());
                StartCoroutine(LaserCoroutine());
                yield return new WaitForSeconds(6f);
            }
            
            yield return new WaitForSeconds(3f);
            TriangleCannon tc = _poolManager.Pop<TriangleCannon>(triangle);
            tc.transform.position = new Vector2(0, -50f);
            tc.RotationSpeed = 360;
            tc.RotationDuration = 6f;
            tc.FireDuration = 0.1f;
        }

        private IEnumerator DumbbellBulletCoroutine()
        {
            for (int j = 0; j < 3; ++j)
            {
                DumbbellBullet db = _poolManager.Pop<DumbbellBullet>(dum);
                int rand1 = Random.Range(0, 2);
                db.transform.position = new Vector2(rand1 == 0 ? 28f : -28f, Random.Range(7f,-7f));
                db.MoveDirection = rand1 == 0 ? Vector2.left : Vector2.right;
                db.MoveSpeed = 10f;
                yield return new WaitForSeconds(2f);
            }
        }
        
        private IEnumerator LaserCoroutine()
        {
            for (int j = 0; j < 4; ++j)
            {
                LaserBullet lb = _poolManager.Pop<LaserBullet>(laser);
                lb.transform.position = new Vector2(Random.Range(-5f, 5f), Random.Range(-5f, 5f));
                lb.transform.rotation = Quaternion.Euler(0,0,Random.Range(0,360));
                yield return new WaitForSeconds(0.4f);
            }
        }
    }
}