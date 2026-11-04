using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ImageViewerWindowUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private Image displayImage;
    [SerializeField] private Button closeButton;

    public void Setup(VirtualImageFile imageFile)
    {
        if (titleText != null)
            titleText.text = imageFile.itemName;

        if (displayImage != null)
        {
            displayImage.sprite = imageFile.imageContent;
            displayImage.preserveAspect = true;
        }

        if (closeButton != null)
        {
            closeButton.onClick.RemoveAllListeners();
            closeButton.onClick.AddListener(CloseWindow);
        }
    }

    public void CloseWindow()
    {
        Destroy(gameObject);
    }
}