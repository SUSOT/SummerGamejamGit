using System.Collections;
using GondrLib.Dependencies;
using GondrLib.ObjectPool.Runtime;
using KHG.Bullets;
using LCM._01.Scripts.Bullets;
using UnityEngine;

namespace LCM._01.Scripts.Timeline
{
    public class LCM_Pattern3 : TimeLinePattern
    {
        [Inject] private PoolManagerMono _poolManager;
        [SerializeField] private PoolManagerSO _poolManagerSO;
        
        
        [SerializeField] private PoolingItemSO wallItem;
        [SerializeField] private PoolingItemSO sommoner;
        [SerializeField] private PoolingItemSO tile;
        
        
        
        [SerializeField] private Vector2 initPos;
        [SerializeField] private bool isHorizontal = false;
        [SerializeField] private bool isNegative = false;

        [SerializeField] private float wallHeight;
        [SerializeField] private float wallWidth;

        [SerializeField] private float previewTime;
        
        private Vector2[] spawnedPositions = new Vector2[20];
        
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
            Wall wall = _poolManagerSO.Pop(wallItem) as Wall;
            
            wall.Init(initPos, isHorizontal, isNegative, wallHeight, wallWidth, previewTime);
            
            wall.SetWall();
            yield return new WaitForSeconds(3f);
            for (int i = 0; i < 3; ++i)
            {
                SommonerBullet sb = _poolManager.Pop<SommonerBullet>(sommoner);
    
                float xOffset = (i - 1) * 20f;
                sb.transform.position = new Vector3(xOffset, -20f, 0f);
            }

            yield return new WaitForSeconds(3f);

            for (int i = 0; i < 20; ++i)
            {
                TileSquareBullet tb = _poolManager.Pop<TileSquareBullet>(tile);
                Vector2 newPosition;
                bool validPosition = false;
    
                do 
                {
                    newPosition = new Vector2(
                        Random.Range(-25, 26),
                        Random.Range(-9, 14)
                    );
        
                    validPosition = true;
        
                    for (int j = 0; j < i; j++)
                    {
                        if (Vector2.Distance(newPosition, spawnedPositions[j]) < 2f) 
                        {
                            validPosition = false;
                            break;
                        }
                    }
        
                } while (!validPosition);
    
                tb.transform.position = newPosition;
                spawnedPositions[i] = newPosition;
                yield return new WaitForSeconds(0.4f);
            }
        }
    }
}