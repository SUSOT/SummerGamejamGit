using System.Collections;
using DG.Tweening;
using GondrLib.Dependencies;
using GondrLib.ObjectPool.Runtime;
using KHG.Bullets;
using KHG.Obstacles;
using LCM._01.Scripts.Bullets;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Events;

namespace LCM._01.Scripts.Timeline
{
    public class LCM_BossPattern2 : TimeLinePattern
    {
        [Inject] private PoolManagerMono _poolManager;

        public UnityEvent OnShakeCamera;

        [SerializeField] private GameObject bossPrefab;
        [SerializeField] private GameObject warning;
        [SerializeField] private GameObject bar;
        [SerializeField] private GameObject stick;
        [SerializeField] private GameObject wall;
        [SerializeField] private GameObject wave;
        private GameObject cam;
        private GameObject _boss;
        [SerializeField] private PoolingItemSO wallgen;
        [SerializeField] private PoolingItemSO dumbbell;
        [SerializeField] private PoolingItemSO laser;
        [SerializeField] private PoolingItemSO flagBomb;

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
            var sr = warn.GetComponent<SpriteRenderer>();

            var c = sr.color;
            sr.color = new Color(c.r, c.g, c.b, 0f);

            yield return new DOTweenCYInstruction.WaitForCompletion(
                sr.DOFade(1f, 0.3f)
                    .SetLoops(4, LoopType.Yoyo)
                    .OnComplete(() => Destroy(warn))
            );

            _boss = Instantiate(bossPrefab, new Vector3(35, 0, 0), Quaternion.identity);

            yield return new DOTweenCYInstruction.WaitForCompletion(
                DOTween.Sequence()
                    .Join(_boss.transform.DOMove(new Vector3(-35, 0, 0), 1f).SetEase(Ease.Linear))
                    .Join(_boss.transform.DORotate(new Vector3(0, 0, 540f), 1f, RotateMode.FastBeyond360)
                        .SetEase(Ease.Linear))
            );

            yield return new WaitForSeconds(1.5f);

            _boss.transform.position = new Vector3(0f, 25f, 0f);

            yield return new DOTweenCYInstruction.WaitForCompletion(
                _boss.transform.DOMove(new Vector3(0f, -9f, 0f), 0.5f).SetEase(Ease.InQuad)
            );
            OnShakeCamera?.Invoke();

            for (int x = 7; x <= 27; x += 2)
            {
                var barR = Instantiate(bar, new Vector3(x, -21f, 0f), Quaternion.identity);
                barR.transform.DOMoveY(-9f, 0.2f).SetEase(Ease.OutQuad)
                    .OnComplete(() =>
                        barR.transform.DOMoveY(-21f, 0.2f).SetEase(Ease.InQuad).OnComplete(() => Destroy(barR)));
                var barL = Instantiate(bar, new Vector3(-x, -21f, 0f), Quaternion.identity);
                barL.transform.DOMoveY(-9f, 0.2f).SetEase(Ease.OutQuad)
                    .OnComplete(() =>
                        barL.transform.DOMoveY(-21f, 0.2f).SetEase(Ease.InQuad).OnComplete(() => Destroy(barL)));

                yield return new WaitForSeconds(0.1f);
            }

            cam = FindFirstObjectByType<CinemachineCamera>().gameObject;
            yield return new DOTweenCYInstruction.WaitForCompletion(cam.transform
                .DORotate(new Vector3(0, 0, 180), 3f, RotateMode.FastBeyond360)
                .SetEase(Ease.InOutCubic)
                .SetRelative(true));


            yield return new DOTweenCYInstruction.WaitForCompletion(
                _boss.transform.DOMove(new Vector3(0f, -7f, 0f), 0.4f).SetEase(Ease.InQuad)
            );

            yield return new DOTweenCYInstruction.WaitForCompletion(
                _boss.transform.DOMove(new Vector3(0f, 9f, 0f), 0.3f).SetEase(Ease.InQuad)
            );
            OnShakeCamera?.Invoke();
            for (int x = 7; x <= 27; x += 2)
            {
                var barR = Instantiate(bar, new Vector3(x, 21f, 0f), Quaternion.identity);
                barR.transform.DOMoveY(9f, 0.2f).SetEase(Ease.OutQuad)
                    .OnComplete(() =>
                        barR.transform.DOMoveY(21f, 0.2f).SetEase(Ease.InQuad).OnComplete(() => Destroy(barR)));
                var barL = Instantiate(bar, new Vector3(-x, 21f, 0f), Quaternion.identity);
                barL.transform.DOMoveY(9f, 0.2f).SetEase(Ease.OutQuad)
                    .OnComplete(() =>
                        barL.transform.DOMoveY(21f, 0.2f).SetEase(Ease.InQuad).OnComplete(() => Destroy(barL)));

                yield return new WaitForSeconds(0.1f);
            }

            yield return new DOTweenCYInstruction.WaitForCompletion(cam.transform
                .DORotate(new Vector3(0, 0, 180), 3f, RotateMode.FastBeyond360)
                .SetEase(Ease.InOutSine)
                .SetRelative(true));

            yield return new DOTweenCYInstruction.WaitForCompletion(
                _boss.transform.DOMoveY(30f, 1f).SetEase(Ease.InOutSine)
            );

            float[] ys = { 10f, 0f, -10f };

            foreach (var y in ys)
            {
                var leftStart  = new Vector3(-43f, y, 0f);
                var rightStart = new Vector3( 43f, y, 0f);
    
                var leftStick  = Instantiate(stick, leftStart,  Quaternion.identity);
                var rightStick = Instantiate(stick, rightStart, Quaternion.identity);

                var leftMid  = new Vector3(-40f, y, 0f);
                var rightMid = new Vector3( 40f, y, 0f);
                var leftEnd  = new Vector3(-15f, y, 0f);
                var rightEnd = new Vector3( 15f, y, 0f);

                DOTween.Sequence()
                    .Append(leftStick.transform.DOMove(leftMid, 0.6f).SetEase(Ease.InOutSine))
                    .Join(rightStick.transform.DOMove(rightMid, 0.6f).SetEase(Ease.InOutSine))
                    .AppendInterval(0.7f)
                    .Append(leftStick.transform.DOShakePosition(0.3f, strength: 0.2f, vibrato: 10).SetEase(Ease.Linear))
                    .Join(rightStick.transform.DOShakePosition(0.3f, strength: 0.2f, vibrato: 10).SetEase(Ease.Linear))
                    .AppendInterval(0.2f)
                    .Append(leftStick.transform.DOMove(leftEnd, 0.2f).SetEase(Ease.InOutQuad))
                    .Join(rightStick.transform.DOMove(rightEnd, 0.2f).SetEase(Ease.InOutQuad))
                    .AppendCallback(() => OnShakeCamera?.Invoke())
                    .AppendInterval(1f)
                    .Append(leftStick.transform.DOMove(leftStart, 3f).SetEase(Ease.InOutSine))
                    .Join(rightStick.transform.DOMove(rightStart, 3f).SetEase(Ease.InOutSine))
                    .OnComplete(() => {
                        Destroy(leftStick);
                        Destroy(rightStick);
                    });

                yield return new WaitForSeconds(0.2f);
            }

            yield return new WaitForSeconds(6f);

            for (int i = 0; i < 10; i++)
            {
                WallGen wg = _poolManager.Pop<WallGen>(wallgen);
                wg.transform.position = new Vector2(Random.Range(-24f, 24f), Random.Range(-12f, 12f));
                DOVirtual.DelayedCall(2f, () => wg.DestroyWall());
                yield return new WaitForSeconds(0.2f);
            }

            yield return new WaitForSeconds(3f);
            
            yield return new DOTweenCYInstruction.WaitForCompletion(
                _boss.transform.DOMove(new Vector3(0f, -9f, 0f), 0.5f).SetEase(Ease.InQuad)
            );
            OnShakeCamera?.Invoke();

            for (int x = 7; x <= 27; x += 2)
            {
                var barR = Instantiate(bar, new Vector3(x, -21f, 0f), Quaternion.identity);
                barR.transform.DOMoveY(-9f, 0.2f).SetEase(Ease.OutQuad)
                    .OnComplete(() =>
                        barR.transform.DOMoveY(-21f, 0.2f).SetEase(Ease.InQuad).OnComplete(() => Destroy(barR)));
                var barL = Instantiate(bar, new Vector3(-x, -21f, 0f), Quaternion.identity);
                barL.transform.DOMoveY(-9f, 0.2f).SetEase(Ease.OutQuad)
                    .OnComplete(() =>
                        barL.transform.DOMoveY(-21f, 0.2f).SetEase(Ease.InQuad).OnComplete(() => Destroy(barL)));

                yield return new WaitForSeconds(0.1f);
            }

            yield return new WaitForSeconds(2f);
            
            var spawnedWall = Instantiate(wall, new Vector3(0f, -45f, 0f), Quaternion.identity);

            yield return new DOTweenCYInstruction.WaitForCompletion(
                DOTween.Sequence()
                    .Join(_boss.transform.DOMove(new Vector3(0f, 0f, 0f), 4f).SetEase(Ease.OutQuad))
                    .Join(spawnedWall.transform.DOMove(new Vector3(0f, -36f, 0f), 4f).SetEase(Ease.OutQuad)) // 시작 빠르고 끝 느린 OutQuad
            );

            yield return new DOTweenCYInstruction.WaitForCompletion(_boss.transform.DOMove(new Vector3(0, 30f, 0), 1f)
                .SetEase(Ease.InCirc));


            for (int i = 0; i < 10; ++i)
            {
                DumbbellBullet db = _poolManager.Pop<DumbbellBullet>(dumbbell);
                int rand = Random.Range(0, 2);
                db.transform.position = rand == 0 ? new Vector2(28, 4.5f) : new Vector2(-28,4.5f);
                db.MoveDirection = rand == 0  ? Vector2.left : Vector2.right;
                db.MoveSpeed = 15f;
                
                LaserBullet lb = _poolManager.Pop<LaserBullet>(laser);
                lb.transform.position = new Vector2(Random.Range(-9f, 9f), 7);
                lb.transform.rotation = Quaternion.Euler(0, 0, Random.Range(0f,360f));
                yield return new WaitForSeconds(1.1f);
            }

            yield return new WaitForSeconds(0.5f);
            yield return new DOTweenCYInstruction.WaitForCompletion(
                spawnedWall.transform.DOMove(new Vector3(0f, -45f, 0f), 2f).SetEase(Ease.InOutSine)
            );
            Destroy(spawnedWall);
            
            yield return new DOTweenCYInstruction.WaitForCompletion(
                _boss.transform.DOMove(new Vector3(0f, 0f, 0f), 3f).SetEase(Ease.OutQuad)
            );

            for (int i = 0; i < 4; ++i)
            {


                yield return new DOTweenCYInstruction.WaitForCompletion(
                    _boss.transform.DOScale(17f, 0.2f)
                        .SetEase(Ease.OutBack)
                        .SetLoops(2, LoopType.Yoyo)
                );

                OnShakeCamera?.Invoke();
                var waveInstance = Instantiate(this.wave, _boss.transform.position, _boss.transform.rotation);
                float currentScale = 15f;
                waveInstance.transform.localScale = Vector3.one * currentScale;

                while (currentScale <= 65f)
                {
                    yield return new WaitForSeconds(0.3f);
                    Destroy(waveInstance);
                    currentScale += 3f;
                    waveInstance = Instantiate(this.wave, _boss.transform.position, _boss.transform.rotation);
                    waveInstance.transform.localScale = Vector3.one * currentScale;
                    if (Mathf.Approximately(currentScale, 27f))
                    {
                        for (int j = 0; j < 4; ++j)
                        {
                            ExplodeBullet eb = _poolManager.Pop<ExplodeBullet>(flagBomb);
                            eb.moveable = true;
                            float startX = Random.value < 0.5f 
                                ? Random.Range(-40f, -35f) 
                                : Random.Range(35f, 40f);
                            float startY = Random.value < 0.5f 
                                ? Random.Range(-25f, -20f) 
                                : Random.Range(20f, 25f);
                            eb.transform.position = new Vector2(startX, startY);
                            eb.targetPosition = new Vector2(
                                Random.Range(-20f, 20f),
                                Random.Range(-10f, 10f)
                            );
                        }
                    }
                }
                Destroy(waveInstance);
            }

            yield return new WaitForSeconds(0.5f);
            yield return new DOTweenCYInstruction.WaitForCompletion(_boss.transform.DOMove(new Vector3(0,30f,0), 1f).SetEase(Ease.OutQuad));

            for (int i = 0; i < 10; ++i)
            {
                for (int j = 0; j < 4; ++j)
                {
                    if (j < 2)
                    {
                        LaserBullet lb = _poolManager.Pop<LaserBullet>(laser);
                        float posY = Random.Range(-14.5f, 14.5f);
                        lb.transform.position = new Vector2(0, posY);
                        lb.transform.rotation = Quaternion.identity;
                    }
                    else
                    {
                        LaserBullet lb = _poolManager.Pop<LaserBullet>(laser);
                        float posX = Random.Range(-26f, 26f);
                        lb.transform.position = new Vector2(posX, 0);
                        lb.transform.rotation = Quaternion.Euler(0, 0, 90f);
                    }
                }

                yield return new WaitForSeconds(1f);
            }

            yield return new WaitForSeconds(5f);
            
            yield return new DOTweenCYInstruction.WaitForCompletion(_boss.transform.DOMove(Vector3.zero, 1f).SetEase(Ease.OutQuad));
            
            
            // 이제 보스 최종공격
            
            yield return new DOTweenCYInstruction.WaitForCompletion(
                _boss.transform.DOMove(new Vector3(0f, -9f, 0f), 0.5f).SetEase(Ease.InQuad)
            );
            OnShakeCamera?.Invoke();

            for (int x = 7; x <= 27; x += 2)
            {
                var barR = Instantiate(bar, new Vector3(x, -21f, 0f), Quaternion.identity);
                barR.transform.DOMoveY(-9f, 0.2f).SetEase(Ease.OutQuad)
                    .OnComplete(() =>
                        barR.transform.DOMoveY(-21f, 0.2f).SetEase(Ease.InQuad).OnComplete(() => Destroy(barR)));
                var barL = Instantiate(bar, new Vector3(-x, -21f, 0f), Quaternion.identity);
                barL.transform.DOMoveY(-9f, 0.2f).SetEase(Ease.OutQuad)
                    .OnComplete(() =>
                        barL.transform.DOMoveY(-21f, 0.2f).SetEase(Ease.InQuad).OnComplete(() => Destroy(barL)));

                yield return new WaitForSeconds(0.1f);
            }

            yield return new WaitForSeconds(2f);
            
            var spawnedWall1 = Instantiate(wall, new Vector3(0f, -45f, 0f), Quaternion.identity);

            yield return new DOTweenCYInstruction.WaitForCompletion(
                DOTween.Sequence()
                    .Join(_boss.transform.DOMove(new Vector3(0f, -2f, 0f), 4f).SetEase(Ease.OutQuad))
                    .Join(spawnedWall1.transform.DOMove(new Vector3(0f, -38f, 0f), 4f).SetEase(Ease.OutQuad)) // 시작 빠르고 끝 느린 OutQuad
            );
            
            
            
            
            yield return new DOTweenCYInstruction.WaitForCompletion(
                _boss.transform.DOMove(new Vector3(0f, 9f, 0f), 0.5f).SetEase(Ease.InQuad)
            );
            OnShakeCamera?.Invoke();

            for (int x = 7; x <= 27; x += 2)
            {
                var barR = Instantiate(bar, new Vector3(x, 21f, 0f), Quaternion.identity);
                barR.transform.DOMoveY(9f, 0.2f).SetEase(Ease.OutQuad)
                    .OnComplete(() =>
                        barR.transform.DOMoveY(21f, 0.2f).SetEase(Ease.InQuad).OnComplete(() => Destroy(barR)));
                var barL = Instantiate(bar, new Vector3(-x, 21f, 0f), Quaternion.identity);
                barL.transform.DOMoveY(9f, 0.2f).SetEase(Ease.OutQuad)
                    .OnComplete(() =>
                        barL.transform.DOMoveY(21f, 0.2f).SetEase(Ease.InQuad).OnComplete(() => Destroy(barL)));

                yield return new WaitForSeconds(0.1f);
            }

            yield return new WaitForSeconds(2f);
            
            var spawnedWall2 = Instantiate(wall, new Vector3(0f, 45f, 0f), Quaternion.identity);

            yield return new DOTweenCYInstruction.WaitForCompletion(
                DOTween.Sequence()
                    .Join(_boss.transform.DOMove(new Vector3(0f, 2f, 0f), 4f).SetEase(Ease.OutQuad))
                    .Join(spawnedWall2.transform.DOMove(new Vector3(0f, 38f, 0f), 4f).SetEase(Ease.OutQuad)) // 시작 빠르고 끝 느린 OutQuad
            );
            
            yield return new DOTweenCYInstruction.WaitForCompletion(_boss.transform.DOMove(Vector3.zero, 0.5f).SetEase(Ease.OutQuad));
            
            yield return new DOTweenCYInstruction.WaitForCompletion(_boss.transform.DOMove(new Vector3(1f,0,0), 0.2f).SetEase(Ease.OutQuad));
            yield return new DOTweenCYInstruction.WaitForCompletion(_boss.transform.DOMove(new Vector3(-17.5f,0,0), 0.4f).SetEase(Ease.OutQuad));
            yield return new WaitForSeconds(0.5f);
            yield return new DOTweenCYInstruction.WaitForCompletion(_boss.transform.DOMove(new Vector3(-18.5f,0,0), 0.2f).SetEase(Ease.OutQuad));
            yield return new DOTweenCYInstruction.WaitForCompletion(_boss.transform.DOMove(new Vector3(17.5f,0,0), 0.8f).SetEase(Ease.OutQuad));
            yield return new WaitForSeconds(0.5f);
            yield return new DOTweenCYInstruction.WaitForCompletion(_boss.transform.DOMove(new Vector3(18.5f,0,0), 0.2f).SetEase(Ease.OutQuad));
            yield return new DOTweenCYInstruction.WaitForCompletion(_boss.transform.DOMove(Vector3.zero, 0.4f).SetEase(Ease.OutQuad));
            yield return new WaitForSeconds(0.5f);
            
            yield return new DOTweenCYInstruction.WaitForCompletion(_boss.transform.DOMove(new Vector3(0,-30,0),1f).SetEase(Ease.OutQuad));
            
            float y1 = 0f;

            var leftStart1  = new Vector3(-43f, y1, 0f);
            var rightStart1 = new Vector3( 43f, y1, 0f);

            var leftStick1  = Instantiate(stick, leftStart1,  Quaternion.identity);
            var rightStick1 = Instantiate(stick, rightStart1, Quaternion.identity);

            var leftMid1  = new Vector3(-40f, y1, 0f);
            var rightMid1 = new Vector3( 40f, y1, 0f);
            var leftEnd1  = new Vector3(-15f, y1, 0f);
            var rightEnd1 = new Vector3( 15f, y1, 0f);

            DOTween.Sequence()
                .Append(leftStick1.transform.DOMove(leftMid1, 0.6f).SetEase(Ease.InOutSine))
                .Join(rightStick1.transform.DOMove(rightMid1, 0.6f).SetEase(Ease.InOutSine))
                .AppendInterval(0.7f)
                .Append(leftStick1.transform.DOShakePosition(0.3f, strength: 0.2f, vibrato: 10).SetEase(Ease.Linear))
                .Join(rightStick1.transform.DOShakePosition(0.3f, strength: 0.2f, vibrato: 10).SetEase(Ease.Linear))
                .AppendInterval(0.2f)
                .Append(leftStick1.transform.DOMove(leftEnd1, 0.2f).SetEase(Ease.InOutQuad))
                .Join(rightStick1.transform.DOMove(rightEnd1, 0.2f).SetEase(Ease.InOutQuad))
                .AppendCallback(() => OnShakeCamera?.Invoke())
                .AppendInterval(1f)
                .Append(leftStick1.transform.DOMove(leftStart1, 3f).SetEase(Ease.InOutSine))
                .Join(rightStick1.transform.DOMove(rightStart1, 3f).SetEase(Ease.InOutSine))
                .OnComplete(() => {
                    Destroy(leftStick1);
                    Destroy(rightStick1);
                });

            yield return new WaitForSeconds(6f);

            for (int i = 0; i < 10; i++)
            {
                LaserBullet lb = _poolManager.Pop<LaserBullet>(laser);
                float posX = Mathf.Lerp(-25f, 25f, (float)i / (10 - 1));
                lb.transform.position = new Vector2(posX, 0);
                lb.transform.rotation = Quaternion.Euler(0, 0, 90f);
    
                yield return new WaitForSeconds(0.3f);
            }

            yield return new WaitForSeconds(3f);
            
            yield return new DOTweenCYInstruction.WaitForCompletion(
                DOTween.Sequence()
                    .Join(spawnedWall1.transform.DOMove(new Vector3(0f, -45f, 0f), 2f).SetEase(Ease.InOutSine))
                    .Join(spawnedWall2.transform.DOMove(new Vector3(0f, 45f, 0f), 2f).SetEase(Ease.InOutSine))
                    .OnComplete(() => {
                        Destroy(spawnedWall1);
                        Destroy(spawnedWall2);
                    })
            );
            yield return new WaitForSeconds(2f);
            
            yield return new DOTweenCYInstruction.WaitForCompletion(
                DOTween.Sequence()
                    .Join(_boss.transform.DOMove(Vector3.zero, 2f).SetEase(Ease.InOutSine))
                    .Join(_boss.transform.DORotate(new Vector3(0f, 0f, 1080f), 2f, RotateMode.FastBeyond360).SetEase(Ease.Linear))
            );

            yield return new DOTweenCYInstruction.WaitForCompletion(
                _boss.transform.DOMoveY(-9f, 0.5f).SetEase(Ease.InQuad)
            );
            OnShakeCamera?.Invoke();

            yield return new DOTweenCYInstruction.WaitForCompletion(
                _boss.transform.DOMoveY(-30f, 4f).SetEase(Ease.InOutSine)
            );
        }
    }
}