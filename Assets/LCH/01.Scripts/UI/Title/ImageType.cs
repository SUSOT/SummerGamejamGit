using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public enum ImageTypeEnum
{
    None = 0,
    START,
    SETTING,
    EXIT
}

public class ImageType : MonoBehaviour
{
    [field:SerializeField] public ImageTypeEnum type;
    [SerializeField] private Image selectImage;
    [SerializeField] private Color selectColor;
    private Color _beforColor;
    private Image _myImage;
    private RectTransform _myrect;

    private void Awake()
    {
        _myImage = gameObject.GetComponent<Image>();
        _myrect = _myImage.rectTransform;
    }

    public void SelectImage()
    {
        _myrect.DOSizeDelta(new Vector2(850, 100), 0.3f);
        selectImage.color = Color.white;
        _beforColor = _myImage.color;
        _myImage.color = selectColor;
    }

    public void NotSelectImage()
    {
        _myrect.DOSizeDelta(new Vector2(650, 100), 0.3f);
        selectImage.color = selectColor;
        _myImage.color = _beforColor;
    }
}
