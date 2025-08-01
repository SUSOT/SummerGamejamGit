using GondrLib.Dependencies;
using GondrLib.ObjectPool.Runtime;
using KHG.Bullets;
using System.Collections;
using UnityEngine;

public class TestLaserGenerator : MonoBehaviour
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
        ExplodeBullet bullet = _poolManager.Pop<ExplodeBullet>(explosion);
        if (bullet == null)
        {
            Debug.LogError("풀에서 ExplodeBullet을 가져오지 못했습니다.");
            yield break;
        }

        bullet.moveable = true;
        bullet.targetPosition = transform.position + new Vector3(Random.Range(-17f, 17f), Random.Range(-10f, 10f));
        bullet.SpawnPosition = transform.position + new Vector3(Random.Range(-15f, 15f), Random.Range(-9f, 9f));

        yield return new WaitForSeconds(0.01f);
        StartCoroutine(ShootingBullet());
    }
}
