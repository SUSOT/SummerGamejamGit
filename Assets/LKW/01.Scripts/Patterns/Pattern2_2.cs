using System.Collections;
using Animation;
using DG.Tweening;
using GondrLib.ObjectPool.Runtime;
using KHG.Bullets;
using UnityEngine;

namespace LKW._01.Scripts.Patterns
{
    public class Pattern2_2 : TimeLinePattern
    {
        [SerializeField] private PoolManagerSO poolManager;
        [SerializeField] private PoolingItemSO bigLaserItem;
        [SerializeField] private SommonerBullet sBullet;

        WaitForSeconds wait = new WaitForSeconds(0.6f);

        public override void Execute()
        {
            sBullet.StopAllCoroutines();
            
            sBullet.transform.DOMoveY(0, 2).OnComplete(() 
                => StartCoroutine(SpawnCoroutine()));

        }

    private IEnumerator SpawnCoroutine()
    {
        sBullet.StartSpawn();
        
        SpawnLaser(new Vector3(-28, 10, 0), true);
        yield return wait;
        SpawnLaser(new Vector3(12, 18, 0), false);
        yield return wait;
        SpawnLaser(new Vector3(28, -6, 0), true);
        yield return wait;
        SpawnLaser(new Vector3(-20, 28, 0), false);
        yield return wait;
        SpawnLaser(new Vector3(-28, 4, 0), true);
        yield return wait;
        SpawnLaser(new Vector3(0, 18, 0), false);
        yield return wait;
        SpawnLaser(new Vector3(28, 8, 0), true);
        yield return wait;
        SpawnLaser(new Vector3(-16, 28, 0), false);
        yield return wait;
        SpawnLaser(new Vector3(-28, -10, 0), true);
        yield return wait;
        SpawnLaser(new Vector3(20, 18, 0), false);
        yield return wait;
        SpawnLaser(new Vector3(28, 0, 0), true);
        yield return wait;
        SpawnLaser(new Vector3(-8, 28, 0), false);
        yield return wait;
        SpawnLaser(new Vector3(-28, 6, 0), true);
        yield return wait;
        SpawnLaser(new Vector3(8, 18, 0), false);
        yield return wait;
        SpawnLaser(new Vector3(28, -4, 0), true);
        yield return wait;
        SpawnLaser(new Vector3(-4, 28, 0), false);
        yield return wait;
        SpawnLaser(new Vector3(-28, -2, 0), true);
        yield return wait;
        SpawnLaser(new Vector3(16, 18, 0), false);
        yield return wait;
        SpawnLaser(new Vector3(28, 10, 0), true);
        yield return wait;
        SpawnLaser(new Vector3(-12, 18, 0), false);
        yield return wait;
        SpawnLaser(new Vector3(-28, 0, 0), true);
        yield return wait;
        SpawnLaser(new Vector3(4, 28, 0), false);
        yield return wait;
        SpawnLaser(new Vector3(28, -8, 0), true);
        yield return wait;
        SpawnLaser(new Vector3(-20, 18, 0), false);
        yield return wait;
        SpawnLaser(new Vector3(-28, 12, 0), true);
        yield return wait;
        SpawnLaser(new Vector3(0, 18, 0), false);
        yield return wait;
        SpawnLaser(new Vector3(28, 6, 0), true);
        yield return wait;
        SpawnLaser(new Vector3(-24, 28, 0), false);
        yield return wait;
        SpawnLaser(new Vector3(-28, -6, 0), true);
        yield return wait;
        SpawnLaser(new Vector3(12, 28, 0), false);
        yield return wait;
        SpawnLaser(new Vector3(28, 2, 0), true);
        yield return wait;
        SpawnLaser(new Vector3(-16, 18, 0), false);
        yield return wait;
        SpawnLaser(new Vector3(-28, 8, 0), true);
        yield return wait;
        SpawnLaser(new Vector3(6, 28, 0), false);
        yield return wait;
        SpawnLaser(new Vector3(28, -12, 0), true);
        yield return wait;
        SpawnLaser(new Vector3(-8, 18, 0), false);
        
        sBullet.StopAllCoroutines();
        sBullet.gameObject.SetActive(false);
    }
        private void SpawnLaser(Vector3 point, bool isHorizontal)
        {
            BigLaser laser = poolManager.Pop(bigLaserItem) as BigLaser;
            laser.transform.position = point;

            if (!isHorizontal)
                laser.rotation = 90f;
        }
    }
}