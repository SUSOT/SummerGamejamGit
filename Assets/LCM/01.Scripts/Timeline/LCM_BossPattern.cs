using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using GondrLib.Dependencies;
using GondrLib.ObjectPool.Runtime;
using KHG.Bullets;
using LCM._01.Scripts.Bullets;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Events;

namespace LCM._01.Scripts.Timeline
{
    public class LCM_BossPattern : TimeLinePattern
    {
        [Inject] private PoolManagerMono _poolManager;

        public UnityEvent OnShakeCamera;

        [SerializeField] private GameObject warning;
        [SerializeField] private GameObject bossPrefab;
        [SerializeField] private GameObject warningWall;
        [SerializeField] private GameObject wall;
        private GameObject cam;
        [SerializeField] private PoolingItemSO normal;
        [SerializeField] private PoolingItemSO laser;
        [SerializeField] private PoolingItemSO wallBullet;
        [SerializeField] private PoolingItemSO bomb;
        private GameObject _boss;

        private void OnEnable()
        {
            Injector.Instance.InjectRuntime(this);
        }

        public override void Execute()
        {
            StartCoroutine(PatternCoroutine());
        }

        private IEnumerator PatternCoroutine()
        {
            var warn = Instantiate(warning, Vector3.zero, Quaternion.identity);

            warn.transform.localScale = Vector3.zero;

            yield return new DOTweenCYInstruction.WaitForCompletion(
                warn.transform.DOScale(Vector3.one * 13f, 1f)
                    .SetEase(Ease.OutBack)
                    .SetLoops(4, LoopType.Yoyo)
                    .OnComplete(() => Destroy(warn)));

            _boss = Instantiate(bossPrefab, new Vector3(0, 30, 0), Quaternion.identity);

            yield return new DOTweenCYInstruction.WaitForCompletion(
                _boss.transform.DOMove(Vector3.zero, 3f).SetEase(Ease.InOutQuart));

            yield return new WaitForSeconds(1f);

            _boss.transform.DORotate(new Vector3(0, 0, 2520), 7f, RotateMode.FastBeyond360)
                .SetEase(Ease.InOutSine)
                .SetRelative(true);

            yield return new WaitForSeconds(0.5f);

            for (int i = 0; i < 60; ++i)
            {
                for (int j = 0; j < 3; ++j)
                {
                    NormalBullet nb = _poolManager.Pop<NormalBullet>(normal);
                    nb.transform.position = _boss.transform.position;
                    nb.moveSpeed = 25f;
                    nb.rotationSpeed = 9f;

                    float angle = 120f * j;
                    Vector3 direction = Quaternion.AngleAxis(angle, Vector3.forward) * _boss.transform.up;
                    nb.MoveDirection = direction.normalized;
                }

                if (i % 10 == 0)
                {
                    for (int j = 0; j < 2; ++j)
                    {
                        if (j == 0)
                        {
                            LaserBullet lb = _poolManager.Pop<LaserBullet>(laser);
                            lb.transform.position = new Vector2(0, Random.Range(-12f, 12f));
                            lb.transform.rotation = Quaternion.identity;
                        }
                        else
                        {
                            LaserBullet lb = _poolManager.Pop<LaserBullet>(laser);
                            lb.transform.position = new Vector2(Random.Range(-18f, 18f), 0);
                            lb.transform.rotation = Quaternion.Euler(0, 0, 90f);
                        }
                    }
                }

                yield return new WaitForSeconds(0.1f);
            }

            yield return new DOTweenCYInstruction.WaitForCompletion(
                _boss.transform.DOMove(new Vector3(0, 50, 0), 1f).SetEase(Ease.InBack));

            yield return new WaitForSeconds(0.2f);

            OnShakeCamera?.Invoke();

            yield return new WaitForSeconds(0.5f);

            Vector2[] positions =
            {
                new Vector2(0, 42f),
                new Vector2(0, -42f),
                new Vector2(52f, 0),
                new Vector2(-52f, 0)
            };
            List<Tween> warningTweens = new List<Tween>();
            List<GameObject> warningWalls = new List<GameObject>();
            foreach (var pos in positions)
            {
                var ww = Instantiate(warningWall, pos, Quaternion.identity);
                warningWalls.Add(ww);

                var sr = ww.GetComponent<SpriteRenderer>();
                var c = sr.color;
                sr.color = new Color(c.r, c.g, c.b, 0f);

                var tw = sr.DOFade(1f, 1f)
                    .SetEase(Ease.InOutSine)
                    .SetLoops(4, LoopType.Yoyo);
                warningTweens.Add(tw);
            }

            yield return DOTween.Sequence()
                .Join(warningTweens[0])
                .Join(warningTweens[1])
                .Join(warningTweens[2])
                .Join(warningTweens[3])
                .WaitForCompletion();
            warningWalls.ForEach(w => Destroy(w));

            yield return new WaitForSeconds(1f);

            List<Tween> wallTweens = new List<Tween>();
            foreach (var pos in positions)
            {
                Vector2 spawnPos;
                if (pos.x == 0f)
                {
                    float offsetY = pos.y + (pos.y > 0 ? 3f : -3f);
                    spawnPos = new Vector2(pos.x, offsetY);
                }
                else
                {
                    float offsetX = pos.x + (pos.x > 0 ? 4.7f : -4.7f);
                    spawnPos = new Vector2(offsetX, pos.y);
                }

                var w = Instantiate(wall, spawnPos, Quaternion.identity);

                var tw = w.transform.DOMove(pos, 3f)
                    .SetEase(Ease.InOutQuart);

                wallTweens.Add(tw);
            }

            yield return DOTween.Sequence()
                .Join(wallTweens[0])
                .Join(wallTweens[1])
                .Join(wallTweens[2])
                .Join(wallTweens[3])
                .WaitForCompletion();

            cam = FindFirstObjectByType<CinemachineCamera>().gameObject;
            cam.transform.DORotate(new Vector3(0, 0, 360), 15f, RotateMode.FastBeyond360)
                .SetEase(Ease.InOutSine)
                .SetRelative(true);

            for (int i = 0; i < 14; ++i)
            {
                WallBullet wb = _poolManager.Pop<WallBullet>(wallBullet);
                wb.transform.position = new Vector3(i % 2 == 0 ? -30f : 30f, 0, 0);
                wb.MoveDirection = i % 2 == 0 ? Vector2.right : Vector2.left;
                wb.MoveSpeed = Random.Range(20f, 25f);

                if (i % 2 == 0)
                {
                    ExplodeBullet eb = _poolManager.Pop<ExplodeBullet>(bomb);
                    eb.transform.position = new Vector2(Random.Range(-20f,20f), Random.Range(-8f,8f));
                }
                
                yield return new WaitForSeconds(1f);
            }
        }
    }
}