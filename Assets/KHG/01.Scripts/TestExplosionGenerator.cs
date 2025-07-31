using GondrLib.Dependencies;
using GondrLib.ObjectPool.Runtime;
using KHG.Bullets;
using LCM._01.Scripts.Bullets;
using System.Collections;
using UnityEditor.EditorTools;
using UnityEngine;

public class TestExplosionGenerator : MonoBehaviour
{
    [Inject] private PoolManagerMono _poolManager;
    [SerializeField] private PoolingItemSO explosion;

    private void Start()
    {
        StartCoroutine(ShootingBullet());
    }
    private IEnumerator ShootingBullet()
    {
        ExplodeBullet bullet = _poolManager.Pop<ExplodeBullet>(explosion);
        bullet.SpawnPosition = transform.position + new Vector3(Random.Range(-10f,10f),Random.Range(-5f,5f));

        yield return new WaitForSeconds(0.5f);
        StartCoroutine(ShootingBullet());
    }
}
