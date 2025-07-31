using GondrLib.Dependencies;
using GondrLib.ObjectPool.Runtime;
using KHG.Bullets;
using System.Collections;
using UnityEngine;

public class LaserPattern_1 : TimeLinePattern
{
    [SerializeField] private PoolingItemSO laserItem;
    [Inject] private PoolManagerMono _poolManager;


    private void OnEnable()
    {
        Injector.Instance.InjectRuntime(this);
    }

    public override void Execute()
    {
        StartCoroutine(Spawn());
    }

    private IEnumerator Spawn()
    {
        for (int i = 0; i < 4; i++)
        {
            int angle = i * 45;
            for (int j = 0; j < 2; j++)
            {
                LaserBullet laser = _poolManager.Pop<LaserBullet>(laserItem);
                laser.transform.position = Vector3.zero;
                laser.rotation = (angle * j) + 45;
            }
            yield return new WaitForSeconds(0.8f);
        }
    }
}
