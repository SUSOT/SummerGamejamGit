using GondrLib.Dependencies;
using GondrLib.ObjectPool.Runtime;
using KHG.Bullets;
using LCM._01.Scripts.Bullets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Infinite3 : InfinitePattern
{
    [SerializeField] private PoolingItemSO cog;
    [SerializeField] private PoolingItemSO laser;
    [SerializeField] private PoolingItemSO wallItem;
    [SerializeField] private int SpawnCount = 5;
    [SerializeField] private List<Vector3> spawnPoints;
    [SerializeField] private float moveSpeed = 3.5f;
    [SerializeField] private int currentPatternTime;
    private int _currentSpawn;

    [Inject] private PoolManagerMono _poolManager;

    private void Start()
    {
        Injector.Instance.InjectRuntime(this);
    }
    public override void Execute(InfinitePatternListSO PatternList, List<InfinitePattern> _activePatterns)
    {
        StartCoroutine(Spawn(PatternList, _activePatterns));
    }

    private IEnumerator Spawn(InfinitePatternListSO PatternList, List<InfinitePattern> _activePatterns)
    {
        Wall wall = _poolManager.Pop<Wall>(wallItem);
        wall.Init(new Vector2(-30,0), true, false, 35, 35, 4f);
        wall.SetWall();
        yield return new WaitForSeconds(4f);

        for (int i = 0; i < SpawnCount; i++)
        {
            CogwheelBullet cogwheel = _poolManager.Pop<CogwheelBullet>(cog);
            cogwheel.transform.position = new Vector2(15, 0);
            cogwheel.MoveSpeed = moveSpeed;
            cogwheel.RotationSpeed = 10f;
            cogwheel.MoveDirection = Vector2.left;
            yield return new WaitForSeconds(2.6f);
            for (int j = 0; j < spawnPoints.Count; j++)
            {
                _currentSpawn = j;
                LaserBullet laserBullet = _poolManager.Pop<LaserBullet>(laser);
                laserBullet.transform.position = spawnPoints[_currentSpawn];
                laserBullet.transform.rotation = Quaternion.Euler(0, 0, spawnPoints[_currentSpawn].z);
                yield return new WaitForSeconds(0.8f);
            }
        }

        _poolManager.Push(wall);
        ExecuteNextPattern(PatternList, _activePatterns);
    }

    public override void ExecuteNextPattern(InfinitePatternListSO PatternList, List<InfinitePattern> _activePatterns)
    {
        base.ExecuteNextPattern(PatternList, _activePatterns);
    }
}
