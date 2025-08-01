using GondrLib.Dependencies;
using GondrLib.ObjectPool.Runtime;
using LCM._01.Scripts.Bullets;
using System.Collections.Generic;
using UnityEngine;

public class TitleRandomSpawn : MonoBehaviour
{
    [SerializeField] private Vector2 maxSpawnPos;
    [SerializeField] private Vector2 minSpawnPos;
    [SerializeField] private List<PoolingItemSO> poolingItemSO;
    [SerializeField] private float spawnCoolTime = 1f;
    [SerializeField] private int spawnCount = 1;

    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private bool randomDirection = true;

    [Inject] private PoolManagerMono _poolManager;
    private float _currentSpawnTime;

    private void Awake()
    {
        Injector.Instance.InjectRuntime(this);
    }

    private void Update()
    {
        _currentSpawnTime += Time.deltaTime;
        if (_currentSpawnTime >= spawnCoolTime)
        {
            SpawnRandomObjects();
            _currentSpawnTime = 0f;
        }
    }

    private void SpawnRandomObjects()
    {
        if (poolingItemSO == null || poolingItemSO.Count == 0)
        {
            Debug.LogWarning("PoolingItemSO 리스트가 비어있습니다!");
            return;
        }

        for (int i = 0; i < spawnCount; i++)
        {
            int randomIndex = Random.Range(0, poolingItemSO.Count);
            PoolingItemSO selectedItem = poolingItemSO[randomIndex];

            Vector2 randomPos = GetRandomSpawnPosition();

            NormalBullet obj = _poolManager.Pop<NormalBullet>(selectedItem);
            obj.transform.position = randomPos;
            obj.moveSpeed = moveSpeed;
            obj.rotationSpeed = rotationSpeed;

            if (randomDirection)
            {
                Vector2 randomDir = Random.insideUnitCircle.normalized;
                obj.MoveDirection = randomDir;
            }
            else
            {
                obj.MoveDirection = -randomPos.normalized;
            }
        }
    }

    private Vector2 GetRandomSpawnPosition()
    {
        float x = Random.Range(minSpawnPos.x, maxSpawnPos.x);
        float y = Random.Range(minSpawnPos.y, maxSpawnPos.y);
        return new Vector2(x, y);
    }

}