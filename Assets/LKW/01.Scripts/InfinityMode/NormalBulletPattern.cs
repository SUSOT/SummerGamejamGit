using System.Collections;
using System.Collections.Generic;
using GondrLib.Dependencies;
using GondrLib.ObjectPool.Runtime;
using LCM._01.Scripts.Bullets;
using UnityEngine;

namespace LKW._01.Scripts.InfinityMode
{
    public class NormalBulletPattern : InfinitePattern
    {
        [Inject] private InfiniteScoreManager scoreManager;
        [SerializeField] private PoolManagerSO poolManager;
        [SerializeField] private PoolingItemSO bulletItem;
        [SerializeField] private Transform[] spawnPoints;

        private void OnEnable()
        {
            Injector.Instance.InjectRuntime(this);
        }
        public override void Execute(InfinitePatternListSO PatternList, List<InfinitePattern> _activePatterns)
        {
            StartCoroutine(WavePattern(PatternList, _activePatterns));
            Debug.Log("normal wave");
        }
        
        
        private IEnumerator WavePattern(InfinitePatternListSO PatternList, List<InfinitePattern> _activePatterns)
        {
            for (int i = 0; i < 40; i++)
            {
                int randIdx = Random.Range(0, 4);

                if (randIdx == 0)
                {
                    NormalBullet bullet = poolManager.Pop(bulletItem) as NormalBullet;

                    bullet.MoveDirection = Vector2.down;

                    bullet.rotationSpeed = 5f;
                    bullet.moveSpeed = 16;
                    
                    float x = Random.Range(spawnPoints[0].position.x, spawnPoints[1].position.x);
                    float y = spawnPoints[1].position.y;
                    
                    bullet.transform.position = new Vector3(x, y, 0);

                }
                else if (randIdx == 1)
                {
                    NormalBullet bullet = poolManager.Pop(bulletItem) as NormalBullet;

                    bullet.MoveDirection = Vector2.up;

                    bullet.rotationSpeed = 5f;
                    bullet.moveSpeed = 16;
                    
                    float x = Random.Range(spawnPoints[0].position.x, spawnPoints[1].position.x);
                    float y = spawnPoints[1].position.y;
                    
                    bullet.transform.position = new Vector3(x, -y, 0);

                }
                else if (randIdx == 2)
                {
                    NormalBullet bullet = poolManager.Pop(bulletItem) as NormalBullet;

                    bullet.MoveDirection = Vector2.left;

                    bullet.rotationSpeed = 5f;
                    bullet.moveSpeed = 16;
                    
                    float x = -spawnPoints[2].position.x;
                    float y = Random.Range(spawnPoints[3].position.y, spawnPoints[2].position.y);
                    
                    bullet.transform.position = new Vector3(x, y, 0);

                }
                else
                {
                    NormalBullet bullet = poolManager.Pop(bulletItem) as NormalBullet;

                    bullet.MoveDirection = Vector2.right;

                    bullet.rotationSpeed = 5f;
                    bullet.moveSpeed = 16;
                    
                    float x = -spawnPoints[2].position.x;
                    float y = Random.Range(spawnPoints[3].position.y, spawnPoints[2].position.y);
                    
                    bullet.transform.position = new Vector3(x, y, 0);

                }

                yield return new WaitForSeconds(0.5f);
            }
            ExecuteNextPattern(PatternList, _activePatterns);
        }
        
        
        public override void ExecuteNextPattern(InfinitePatternListSO PatternList, List<InfinitePattern> _activePatterns)
        {
            base.ExecuteNextPattern(PatternList, _activePatterns);
        }
    }
}