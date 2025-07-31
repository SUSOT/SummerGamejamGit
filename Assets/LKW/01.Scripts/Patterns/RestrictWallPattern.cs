using System;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

namespace LKW._01.Scripts.Patterns
{
    public class RestrictWallPattern : TimeLinePattern
    {
        [SerializeField] private GameObject wall;
        [SerializeField] private GameObject preview;
        [SerializeField] private SpriteRenderer previewRenderer;
        
        [SerializeField] private bool isHorizontal = false;
        [SerializeField] private bool isNegative = false;

        [SerializeField] private float wallHeight;
        [SerializeField] private float wallWidth;

        [SerializeField] private float previewTime;
        
        [SerializeField] private float lifeTime;


        private float spawnTime;
        private bool isActive = false;
        
        public RestrictWallPattern(float startTime) : base(startTime)
        {
        }

        private void OnDestroy()
        {
            DOTween.Kill(wall);
            DOTween.Kill(preview);
        }


        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.Q))
                Execute();
            
            if (Time.time - spawnTime >= lifeTime && isActive == true)
            {
                if (isHorizontal)
                {
                    isActive = false;
                    wall.transform.DOScaleX(0, 1).SetEase(Ease.OutQuad)
                        .OnComplete(() => Destroy(gameObject));
                }
                else
                {
                    isActive = false;
                    wall.transform.DOScaleY(0, 1).SetEase(Ease.OutQuad)
                        .OnComplete(() => Destroy(gameObject));
                }
            }
        }

        public override void Execute()
        {
            

            
            if (isHorizontal)
            {
                wall.transform.localScale = new Vector3(1, wallHeight, 1);
                preview.transform.localScale = new Vector3(1, wallHeight, 1);

                preview.transform.DOScaleX(wallWidth, previewTime);
                previewRenderer.DOColor(new Vector4(1,1,1,0.5f), previewTime / 10).SetEase(Ease.InOutExpo).SetLoops(10, LoopType.Yoyo);
                DOVirtual.DelayedCall(previewTime, () =>
                {
                    preview.SetActive(false);
                    wall.transform.DOScaleX(wallWidth, previewTime / 2).SetEase(Ease.InOutExpo)
                        .OnComplete(() =>
                        {
                            spawnTime = Time.time;
                            isActive = true;
                        });
                });
            }

            else
            {
                wall.transform.localScale = new Vector3(wallHeight,1 , 1);
                preview.transform.localScale = new Vector3(wallHeight,1 , 1);
                
                preview.transform.DOScaleY(wallWidth, previewTime);
                previewRenderer.DOColor(new Vector4(1,1,1,0.5f), previewTime / 10).SetEase(Ease.InOutExpo).SetLoops(10, LoopType.Yoyo);
                DOVirtual.DelayedCall(previewTime, () =>
                {
                    preview.SetActive(false);
                    wall.transform.DOScaleY(wallWidth, previewTime / 2).SetEase(Ease.InOutExpo)
                        .OnComplete(() =>
                        {
                            spawnTime = Time.time;
                            isActive = true;
                        });
                });
            }
        }
    }
}