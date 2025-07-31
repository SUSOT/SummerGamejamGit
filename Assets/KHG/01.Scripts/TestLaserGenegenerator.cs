using GondrLib.Dependencies;
using GondrLib.ObjectPool.Runtime;
using KHG.Bullets;
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
        CircleWorm circleWOrm = _poolManager.Pop<CircleWorm>(worm);
        if (circleWOrm == null)
        {
            Debug.LogError("풀에서 ExplodeBullet을 가져오지 못했습니다.");
            yield break;
        }

        circleWOrm.rotationSpeed = 45f;

        yield return new WaitForSeconds(0.5f);
        StartCoroutine(ShootingBullet());
    }
}
