using DG.Tweening;
using GondrLib.ObjectPool.Runtime;
using UnityEngine;
using UnityEngine.Serialization;

namespace LKW._01.Scripts.Patterns
{
    public class RestrictWallPattern : TimeLinePattern
    {
        [SerializeField] private PoolManagerSO poolManager;
        [SerializeField] private PoolingItemSO wallItem;
        
        [SerializeField] private bool isHorizontal = false;
        [SerializeField] private bool isNegative = false;

        [SerializeField] private float wallHeight;
        [SerializeField] private float wallWidth;

        [SerializeField] private float previewTime;
        
        [SerializeField] private float lifeTime;

        [SerializeField] private Vector2 initPos;

        private Wall wall;

        private float spawnTime;
        private bool isActive = false;
        
        private void Update()
        {
            if (Time.time - spawnTime >= lifeTime && isActive == true)
            {
                isActive = false;
                wall.UnSetWall();
            }
        }

        public override void Execute()
        {
            wall = poolManager.Pop(wallItem) as Wall;
            
            wall.Init(initPos, isHorizontal, isNegative, wallHeight, wallWidth, previewTime);
            
            wall.SetWall();
            
            DOVirtual.DelayedCall(previewTime * 2.5f, () =>
            {
                isActive = true;
                spawnTime = Time.time;
            });
        }
    }
}