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

    // 풀 참조
    private Pool pool;

    public void Init(Vector2 startPos, bool isHor, bool isNeg, float height, float width, float time)
    {
        initPos = startPos;
        isHorizontal = isHor;
        isNegative = isNeg;
        wallHeight = height;
        wallWidth = width;
        previewTime = time;

        // 초기 스케일
        wall.transform.localScale = Vector3.one;
        preview.transform.localScale = Vector3.one;
    }

    public void UnSetWall()
    {
        KillAllTweens();

        if (isHorizontal)
        {
            wall.transform.DOScaleX(0, 1)
                .SetEase(Ease.OutQuad)
                .OnComplete(ReturnToPool);
        }
        else
        {
            wall.transform.DOScaleY(0, 1)
                .SetEase(Ease.OutQuad)
                .OnComplete(ReturnToPool);
        }
    }

    public void SetWall()
    {
        KillAllTweens();

        wall.transform.position = initPos;
        preview.transform.position = initPos;

        // 공통 초기화
        preview.SetActive(true);
        previewRenderer.color = new Color(1f, 1, 01f, 0f);
        
        float blinkDuration = previewTime /4 / 2f;

        if (isHorizontal)
        {
            wall.transform.localScale = new Vector3(1, wallHeight, 1);
            preview.transform.localScale = new Vector3(wallWidth, wallHeight, 1);

            previewRenderer.DOColor(new Color(1, 0.4f, 0.4f, 0.3f), blinkDuration)
                .SetLoops(4, LoopType.Yoyo)
                .OnComplete(() =>
                {
                    preview.SetActive(false);
                    wall.transform.DOScaleX(wallWidth, previewTime)
                        .SetEase(Ease.InOutExpo);
                });
        }
        else
        {
            wall.transform.localScale = new Vector3(wallHeight, 1, 1);
            preview.transform.localScale = new Vector3(wallHeight, wallWidth, 1);

            previewRenderer.DOColor(new Color(1, 0.4f, 0.4f, 0.5f), blinkDuration)
                .SetLoops(4, LoopType.Yoyo)
                .OnComplete(() =>
                {
                    preview.SetActive(false);
                    wall.transform.DOScaleY(wallWidth, previewTime)
                        .SetEase(Ease.InOutExpo);
                });
        }
    }

    private void KillAllTweens()
    {
        DOTween.Kill(wall.transform);
        DOTween.Kill(preview.transform);
        DOTween.Kill(previewRenderer);
    }

    private void ReturnToPool()
    {
        KillAllTweens();
        if (pool != null)
            pool.Push(this);
        else
            gameObject.SetActive(false);
    }

    [field: SerializeField] public PoolingItemSO PoolingType { get; set; }

    public GameObject GameObject => gameObject;

    public void SetUpPool(Pool pool)
    {
        this.pool = pool;
    }

    public void ResetItem()
    {
        KillAllTweens();
        wall.transform.localScale = Vector3.one;
        preview.transform.localScale = Vector3.one;
        previewRenderer.color = Color.white;
        preview.SetActive(false);
    }
}
