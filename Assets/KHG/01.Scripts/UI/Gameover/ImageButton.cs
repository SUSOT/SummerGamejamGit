using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace KHG.UI
{
    public class ImageButton : MonoBehaviour
    {
        [SerializeField] private Image currentImage;
        [SerializeField] private TextMeshProUGUI textTmp;
        [Header("Setting")]
        [SerializeField] private Color targetColor;
        public void Selected()
        {
            currentImage.color = targetColor;
        }
    }
}
