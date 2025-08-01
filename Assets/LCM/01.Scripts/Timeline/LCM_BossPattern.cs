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
        [SerializeField] private GameObject thorn;
        [SerializeField] private GameObject triangleWave;
        private GameObject cam;
        [SerializeField] private PoolingItemSO normal;
        [SerializeField] private PoolingItemSO laser;
        [SerializeField] private PoolingItemSO wallBullet;
        [SerializeField] private PoolingItemSO bomb;
        [SerializeField] private PoolingItemSO cross;
        [SerializeField] private PoolingItemSO triangleCannon;
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
            
            yield return new DOTweenCYInstruction.WaitForCompletion(_boss.transform
                .DORotate(new Vector3(0, 0, -20f), 0.8f, RotateMode.FastBeyond360)
                .SetEase(Ease.InOutSine)
                .SetRelative(true));
            
            _boss.transform.DORotate(new Vector3(0, 0, 2540), 7f, RotateMode.FastBeyond360)
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
            
            List<GameObject> walls = new List<GameObject>();
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
                walls.Add(w);
            
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
                    eb.transform.position = new Vector2(Random.Range(-20f, 20f), Random.Range(-8f, 8f));
                }
            
                yield return new WaitForSeconds(1f);
            
            }
            
            List<Tween> returnTweens = new List<Tween>();
            for (int i = 0; i < walls.Count; i++)
            {
                GameObject w = walls[i];
                Vector2 originalPos;
                Vector2 targetPos = positions[i];
                if (targetPos.x == 0f)
                {
                    float offsetY = targetPos.y + (targetPos.y > 0 ? 3f : -3f);
                    originalPos = new Vector2(targetPos.x, offsetY);
                }
                else
                {
                    float offsetX = targetPos.x + (targetPos.x > 0 ? 4.7f : -4.7f);
                    originalPos = new Vector2(offsetX, targetPos.y);
                }
            
                var rt = w.transform.DOMove(originalPos, 2f)
                    .SetEase(Ease.InBack)
                    .OnComplete(() => Destroy(w));
                returnTweens.Add(rt);
            }
            
            yield return DOTween.Sequence()
                .Join(returnTweens[0])
                .Join(returnTweens[1])
                .Join(returnTweens[2])
                .Join(returnTweens[3])
                .WaitForCompletion();
            
            yield return new DOTweenCYInstruction.WaitForCompletion(
                _boss.transform.DOMove(Vector3.zero, 1f).SetEase(Ease.InOutQuad)
            );
            yield return new DOTweenCYInstruction.WaitForCompletion(
                _boss.transform.DORotate(new Vector3(0, 0, 180f), 0.5f, RotateMode.FastBeyond360).SetEase(Ease.OutBack)
            );
            yield return new DOTweenCYInstruction.WaitForCompletion(
                _boss.transform.DOMove(_boss.transform.position + Vector3.up * 3f, 0.7f).SetEase(Ease.OutSine)
            );
            yield return new DOTweenCYInstruction.WaitForCompletion(
                _boss.transform.DOMove(_boss.transform.position + Vector3.down * 14f, 0.2f).SetEase(Ease.InFlash)
            );
            OnShakeCamera?.Invoke();
            
            Vector2[] thornPositions = { new Vector2(6, -14), new Vector2(-6, -14) };
            List<GameObject> thorns = new List<GameObject>();
            
            foreach (var pos in thornPositions)
            {
                Vector3 startPos = new Vector3(pos.x, -30f, 0f);
                var t = Instantiate(thorn, startPos, Quaternion.identity);
                thorns.Add(t);
                t.transform.DOMove(new Vector3(pos.x, pos.y, 0f), 0.1f).SetEase(Ease.OutFlash);
            }
            
            yield return new WaitForSeconds(0.1f);
            
            int initialCount = thorns.Count;
            for (int i = 0; i < initialCount; i++)
            {
                var prev = thorns[i];
                Vector3 targetPos = prev.transform.position;
                targetPos.x *= 2f;
            
                Vector3 startPos = new Vector3(targetPos.x, -30f, 0f);
                var t2 = Instantiate(thorn, startPos, Quaternion.identity);
                t2.transform.localScale = prev.transform.localScale * 2f;
                thorns.Add(t2);
                t2.transform.DOMove(targetPos, 0.1f).SetEase(Ease.OutFlash);
            }
            
            yield return new WaitForSeconds(0.1f);
            
            for (int i = initialCount; i < initialCount * 2; i++)
            {
                var prev = thorns[i];
                Vector3 targetPos = prev.transform.position;
                targetPos.x *= 2f;
            
                Vector3 startPos = new Vector3(targetPos.x, -30f, 0f);
                var t3 = Instantiate(thorn, startPos, Quaternion.identity);
                t3.transform.localScale = prev.transform.localScale * 2f;
                thorns.Add(t3);
                t3.transform.DOMove(targetPos, 0.1f).SetEase(Ease.OutFlash);
            }
            
            yield return new WaitForSeconds(0.5f);
            
            Vector2[] spawnPositions = { new Vector2(-18, 4), new Vector2(0, 10), new Vector2(18, 4) };
            for (int i = 0; i < 3; ++i)
            {
                CrossLaserBullet cb = _poolManager.Pop<CrossLaserBullet>(cross);
                cb.transform.position = spawnPositions[i];
                cb.RotationSpeed = 0.7f;
            }
            
            yield return new WaitForSeconds(4f);
            
            foreach (var t in thorns)
            {
                t.transform.DOMove(new Vector3(t.transform.position.x, -30f, 0f), 0.5f)
                    .SetEase(Ease.Linear)
                    .OnComplete(() => Destroy(t));
            }
            
            yield return new DOTweenCYInstruction.WaitForCompletion(
                _boss.transform.DOMove(Vector3.zero, 3f).SetEase(Ease.InOutSine)
            );
            yield return new DOTweenCYInstruction.WaitForCompletion(
                _boss.transform.DORotate(Vector3.zero, 0.5f, RotateMode.FastBeyond360)
                    .SetEase(Ease.OutBack)
            );

            yield return new WaitForSeconds(1f);
            
            Vector2[] movePositions = {
                new Vector2(-18, 8),
                new Vector2(18, 8),
                new Vector2(-18, -8),
                new Vector2(18, -8)
            };

            for (int i = 0; i < 4; i++)
            {
                TriangleCannon tc = _poolManager.Pop<TriangleCannon>(triangleCannon);
                tc.transform.position = new Vector2(0, 30);
                tc.FireDuration = 0.4f;
                tc.RotationDuration = 14f;
                tc.transform.DOMove(movePositions[i], 3f).OnComplete(() =>
                {
                    tc.StartInfiniteRotation();
                });
                yield return new WaitForSeconds(0.2f);
            }

            for (int i = 0; i < 5; ++i)
            {
                float randomRotation = Random.Range(10f, 30f) * (Random.Range(0, 2) == 0 ? 1 : -1);

                yield return new DOTweenCYInstruction.WaitForCompletion(
                    _boss.transform.DORotate(new Vector3(0, 0, randomRotation), 0.2f, RotateMode.FastBeyond360)
                        .SetEase(Ease.OutBack)
                        .SetRelative(true)
                );

                yield return new DOTweenCYInstruction.WaitForCompletion(
                    _boss.transform.DOScale(20f, 0.2f)
                        .SetEase(Ease.OutBack)
                        .SetLoops(2, LoopType.Yoyo)
                );

                OnShakeCamera?.Invoke();
                var wave = Instantiate(triangleWave, _boss.transform.position, _boss.transform.rotation);
                wave.transform.localScale = new Vector3(15,15,15);
                wave.transform.DOScale(150f, 3f)
                    .SetEase(Ease.Linear)
                    .OnComplete(() => Destroy(wave));

                yield return new WaitForSeconds(3f);
            }

            yield return new WaitForSeconds(2f);
            
            yield return new DOTweenCYInstruction.WaitForCompletion(
                _boss.transform.DORotate(Vector3.zero, 0.5f, RotateMode.FastBeyond360)
                    .SetEase(Ease.OutBack)
            );

            yield return new DOTweenCYInstruction.WaitForCompletion(
                _boss.transform.DOShakePosition(0.3f, new Vector3(1f, 1f, 0f), 20)
            );
            yield return new WaitForSeconds(0.8f);
            
            yield return new DOTweenCYInstruction.WaitForCompletion(
                _boss.transform.DOShakePosition(0.3f, new Vector3(1f, 1f, 0f), 20)
            );
            yield return new WaitForSeconds(0.8f);

            yield return new DOTweenCYInstruction.WaitForCompletion(
                _boss.transform.DOMove(new Vector3(0f, -11f, 0f), 0.5f).SetEase(Ease.InQuad)
            );
            OnShakeCamera?.Invoke();

            yield return new WaitForSeconds(1f);

            yield return new DOTweenCYInstruction.WaitForCompletion(
                _boss.transform.DOMove(new Vector3(0f, -30f, 0f), 2f).SetEase(Ease.InBack)
                    .OnComplete(() => Destroy(_boss))
            );
            
        }
    }
}