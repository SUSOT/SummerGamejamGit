using System.Collections.Generic;
using GondrLib.Dependencies;
using GondrLib.ObjectPool.Runtime;
using LCM._01.Scripts;
using UnityEngine;

public class Test : MonoBehaviour
{
    [Inject] private PoolManagerMono _poolManager;
    [SerializeField] private PoolingItemSO _poolItemSO;
    public List<Bullet> normalBullets = new List<Bullet>();
    private Bullet b;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            b = _poolManager.Pop<Bullet>(_poolItemSO);
            normalBullets.Add(b);
            b.transform.position = Vector2.zero;
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            _poolManager.Push(normalBullets[0]);
            normalBullets.Remove(b);
        }
    }
}
