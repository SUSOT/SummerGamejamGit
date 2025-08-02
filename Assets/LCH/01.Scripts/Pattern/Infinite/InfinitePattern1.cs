using GondrLib.Dependencies;
using GondrLib.ObjectPool.Runtime;
using KHG.Bullets;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.AI;
using Random = UnityEngine.Random;

public class InfinitePattern1 : InfinitePattern
{
    [SerializeField] private PoolingItemSO wormItem;
    [SerializeField] private PoolingItemSO wallItem;
    [SerializeField] private List<Vector2> spawnPoints;
    [SerializeField] private string patternName;
    [SerializeField] private int currentPatternTime;

    [Inject] private PoolManagerMono _poolManager;

    private void OnEnable()
    {
        Injector.Instance.InjectRuntime(this);
    }
    public override void Execute(InfinitePatternListSO PatternList, List<InfinitePattern> _activePatterns)
    {
        StartCoroutine(Spawn(PatternList,_activePatterns));
    }

    private IEnumerator Spawn(InfinitePatternListSO PatternList, List<InfinitePattern> _activePatterns)
    {
        Wall wall = _poolManager.Pop<Wall>(wallItem);
        wall.Init(new Vector2(0,-20),false,true,55,33,4f);
        wall.SetWall();
        yield return new WaitForSeconds(4f);

        for(int i = 0; i <2; i++)
        {
            for(int j =0; j< 13; j++)
            {
                CircleWorm worm = _poolManager.Pop<CircleWorm>(wormItem);
                float x = Random.Range(-23f, 23f);
                float r = Random.Range(0f, 5f);
                worm.rotationSpeed = r;
                worm.transform.position = new Vector2(x,-7);
                yield return new WaitForSeconds(0.5f);
            }
        }

        _poolManager.Push(wall);
        ExecuteNextPattern(PatternList,_activePatterns);
    }

    public override void ExecuteNextPattern(InfinitePatternListSO PatternList, List<InfinitePattern> _activePatterns)
    {
        base.ExecuteNextPattern(PatternList, _activePatterns);
    }
}
