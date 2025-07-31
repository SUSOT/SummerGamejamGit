using UnityEngine;

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
}
