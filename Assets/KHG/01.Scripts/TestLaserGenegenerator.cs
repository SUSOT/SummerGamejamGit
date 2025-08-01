using GondrLib.Dependencies;
using GondrLib.ObjectPool.Runtime;
using KHG.Bullets;
using KHG.Obstacles;
using System.Collections;
using UnityEngine;

public class TestLaserGenegenerator : MonoBehaviour
{
    [SerializeField] private PoolingItemSO worm;
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
        WallGen circleWOrm = _poolManager.Pop<WallGen>(worm);
        if (circleWOrm == null)
        {
            Debug.LogError("풀에서 ExplodeBullet을 가져오지 못했습니다.");
            yield break;
        }

        circleWOrm.SpawnVector = transform.position + new Vector3(Random.Range(-17f, 17f), Random.Range(-10f, 10f));

        yield return new WaitForSeconds(0.5f);
        StartCoroutine(ShootingBullet());
    }
}
