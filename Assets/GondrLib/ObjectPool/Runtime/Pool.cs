using GondrLib.ObjectPool.Runtime;
using System.Collections.Generic;
using UnityEngine;

public class Pool
{
    private readonly Stack<IPoolable> _pool;
    private readonly List<IPoolable> _activeList = new List<IPoolable>();
    private readonly Transform _parentTrm;
    private readonly GameObject _prefab;

    public Pool(IPoolable poolable, Transform parentTrm, int count)
    {
        _pool = new Stack<IPoolable>(count);
        _parentTrm = parentTrm;
        _prefab = poolable.GameObject;

        for (int i = 0; i < count; i++)
        {
            GameObject gameObj = GameObject.Instantiate(_prefab, _parentTrm);
            gameObj.SetActive(false);
            IPoolable item = gameObj.GetComponent<IPoolable>();
            item.SetUpPool(this);
            _pool.Push(item);
        }
    }

    public IPoolable Pop()
    {
        IPoolable item;
        if (_pool.Count == 0)
        {
            GameObject gameObj = GameObject.Instantiate(_prefab, _parentTrm);
            item = gameObj.GetComponent<IPoolable>();
            item.SetUpPool(this);
        }
        else
        {
            item = _pool.Pop();
        }

        item.GameObject.SetActive(true);
        item.ResetItem();
        _activeList.Add(item);
        return item;
    }

    public void Push(IPoolable item)
    {
        if (_activeList.Contains(item))
        {
            _activeList.Remove(item);
        }

        item.GameObject.SetActive(false);
        _pool.Push(item);
    }

    public void PushAll()
    {
        for (int i = _activeList.Count - 1; i >= 0; i--)
        {
            IPoolable item = _activeList[i];
            item.GameObject.SetActive(false);
            _pool.Push(item);
        }
    }
}
