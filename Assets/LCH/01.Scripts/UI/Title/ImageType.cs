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

    private void Awake()
    {
        _myImage = gameObject.GetComponent<Image>();
    }

    public void SelectImage()
    {
        selectImage.color = Color.white;
        _beforColor = _myImage.color;
        _myImage.color = selectColor;
    }

    public void NotSelectImage()
    {
        selectImage.color = selectColor;
        _myImage.color = _beforColor;
    }
}
