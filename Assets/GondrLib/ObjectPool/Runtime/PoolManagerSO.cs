using System.Collections.Generic;
using UnityEngine;

namespace GondrLib.ObjectPool.Runtime
{
    [CreateAssetMenu(fileName = "PoolManager", menuName = "SO/Pool/Manager", order = 0)]
    public class PoolManagerSO : ScriptableObject
    {
        public List<PoolingItemSO> itemList = new List<PoolingItemSO>();
        
        //해당 아이템이 존재하는 풀을 만든다.
        private Dictionary<PoolingItemSO, Pool> _pools;
        private Transform _rootTrm;

        // 오류 원인 분석 (Step-by-step pseudocode)
        // 1. PoolManagerMono.Awake()에서 poolManager.Initialize(transform) 호출
        // 2. PoolManagerSO.Initialize(Transform rootTrm) 내부에서 NullReferenceException 발생
        // 3. PoolManagerSO의 private 필드 _pools, _rootTrm, itemList 등 중 하나가 null일 가능성
        // 4. Initialize 메서드에서 _pools를 new로 할당하지 않았거나, itemList가 null이거나, itemList의 요소가 null일 경우
        // 5. ScriptableObject는 생성 시 필드가 null일 수 있으므로, PoolManagerSO의 itemList가 에디터에서 할당되지 않았거나, PoolManagerMono의 poolManager 필드가 에디터에서 할당되지 않았을 가능성

        // 해결 방안
        // 1. PoolManagerMono의 poolManager 필드가 에디터에서 할당되어 있는지 확인
        // 2. PoolManagerSO의 itemList가 null이 아닌지, 요소들이 null이 아닌지 확인
        // 3. PoolManagerSO.Initialize에서 _pools를 new Dictionary로 초기화하는 코드가 있는지 확인

        // 예시: PoolManagerSO.Initialize 내부에 _pools 초기화 코드 추가
        public void Initialize(Transform rootTrm)
        {
            _rootTrm = rootTrm;
            if (_pools == null)
                _pools = new Dictionary<PoolingItemSO, Pool>();

            foreach (var item in itemList)
            {
                if (item == null || item.prefab == null)
                    continue; // null 체크 추가

                if (!_pools.ContainsKey(item))
                {
                    var poolable = item.prefab.GetComponent<IPoolable>();
                    if (poolable == null)
                        continue; // IPoolable 컴포넌트가 없으면 스킵

                    var pool = new Pool(poolable, _rootTrm, item.initCount);
                    _pools.Add(item, pool);
                }
            }
        }

        public IPoolable Pop(PoolingItemSO findItem)
        {
            if (_pools.TryGetValue(findItem, out Pool pool))
            {
                return pool.Pop();
            }

            return default;
        }

        public void Push(IPoolable item)
        {
            if (_pools.TryGetValue(item.PoolingType, out Pool pool))
            {
                pool.Push(item);
            }
        }
        public void PushAll()
        {
            foreach (var pool in _pools.Values)
            {
                pool.PushAll();
            }
        }
    }
}