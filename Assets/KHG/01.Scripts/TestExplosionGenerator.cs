using GondrLib.Dependencies;
using GondrLib.ObjectPool.Runtime;
using KHG.Bullets;
using LCM._01.Scripts.Bullets;
using System.Collections;
using UnityEditor.EditorTools;
using UnityEngine;

public class TestExplosionGenerator : MonoBehaviour
{
    [SerializeField] private PoolingItemSO explosion;
    [Inject] private PoolManagerMono _poolManager;

    private void Start()
    {
        StartCoroutine(ShootingBullet());
    }
    private IEnumerator ShootingBullet()
    {
        if (_poolManager == null)
        {
            Debug.LogError("_poolManager가 할당되지 않았습니다.");
            yield break;
        }
        if (explosion == null)
        {
            Debug.LogError("explosion PoolingItemSO가 할당되지 않았습니다.");
            yield break;
        }

        ExplodeBullet bullet = _poolManager.Pop<ExplodeBullet>(explosion);
        if (bullet == null)
        {
            Debug.LogError("풀에서 ExplodeBullet을 가져오지 못했습니다.");
            yield break;
        }

        bullet.SpawnPosition = transform.position + new Vector3(Random.Range(-10f, 10f), Random.Range(-5f, 5f));

        yield return new WaitForSeconds(0.5f);
        StartCoroutine(ShootingBullet());
    }
}
