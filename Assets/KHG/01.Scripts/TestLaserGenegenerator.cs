using GondrLib.Dependencies;
using GondrLib.ObjectPool.Runtime;
using KHG.Bullets;
using System.Collections;
using UnityEngine;

public class TestLaserGenegenerator : MonoBehaviour
{
    [SerializeField] private PoolingItemSO explosion;
    [Inject] private PoolManagerMono _poolManager;

    private void OnEnable()
    {
        Injector.Instance.InjectRuntime(this);
    }
    private void Start()
    {
        StartCoroutine(ShootingBullet());
    }
    private IEnumerator ShootingBullet()
    {
        LaserBullet bullet = _poolManager.Pop<LaserBullet>(explosion);
        if (bullet == null)
        {
            Debug.LogError("풀에서 ExplodeBullet을 가져오지 못했습니다.");
            yield break;
        }

        bullet.rotation = Random.Range(-90f, 90f);

        yield return new WaitForSeconds(0.5f);
        StartCoroutine(ShootingBullet());
    }
}
