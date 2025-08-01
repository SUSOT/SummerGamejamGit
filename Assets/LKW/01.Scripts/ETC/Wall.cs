using DG.Tweening;
using GondrLib.ObjectPool.Runtime;
using UnityEngine;

    public class Wall : MonoBehaviour, IPoolable
    {
        [SerializeField] private GameObject wall;
        [SerializeField] private GameObject preview;
        [SerializeField] private SpriteRenderer previewRenderer;


        private bool isHorizontal;
        private bool isNegative;
        private float wallHeight;
        private float wallWidth;
        private float previewTime;
        private Vector2 initPos;
        
        public void Init(Vector2 startPos, bool isHor, bool isNeg, float height, float width,  float time)
        {
            initPos = startPos;
            isHorizontal = isHor;
            isNegative = isNeg;
            wallHeight = height;
            wallWidth = width;
            previewTime = time;
            
            wall.transform.localScale = Vector3.zero;
            preview.transform.localScale = Vector3.zero;
        }

        public void UnSetWall()
        {
            if (isHorizontal)
            {
                wall.transform.DOScaleX(0, 1).SetEase(Ease.OutQuad)
                    .OnComplete(() => Destroy(gameObject));
            }
            else
            {
                wall.transform.DOScaleY(0, 1).SetEase(Ease.OutQuad)
                    .OnComplete(() => Destroy(gameObject));
            }
        }

        public void SetWall()
        {
            wall.transform.position = initPos;
            preview.transform.position = initPos;

            
            if (isHorizontal)
            {
                wall.transform.localScale = new Vector3(1, wallHeight, 1);
                preview.transform.localScale = new Vector3(1, wallHeight, 1);

                preview.transform.DOScaleX(wallWidth, previewTime);
                previewRenderer.DOColor(new Vector4(1,1,1,0.5f), previewTime / 10)
                    .SetEase(Ease.InOutExpo).SetLoops(10, LoopType.Yoyo);
                DOVirtual.DelayedCall(previewTime* 1.2f, () =>
                {
                    preview.SetActive(false);
                    wall.transform.DOScaleX(wallWidth, previewTime / 2).SetEase(Ease.InOutExpo);
                });
            }

            else
            {
                wall.transform.localScale = new Vector3(wallHeight,1 , 1);
                preview.transform.localScale = new Vector3(wallHeight,1 , 1);
                
                preview.transform.DOScaleY(wallWidth, previewTime);
                previewRenderer.DOColor(new Vector4(1,1,1,0.5f), previewTime / 10)
                    .SetEase(Ease.InOutExpo).SetLoops(10, LoopType.Yoyo);
                DOVirtual.DelayedCall(previewTime * 1.2f, () =>
                {
                    preview.SetActive(false);
                    wall.transform.DOScaleY(wallWidth, previewTime / 2).SetEase(Ease.InOutExpo);
                });
            }

        }
        
        
        [field:SerializeField] public PoolingItemSO PoolingType { get; set; }
        
        public GameObject GameObject  =>gameObject;
        public void SetUpPool(Pool pool)
        {
        }

        public void ResetItem()
        {
        }
    }
