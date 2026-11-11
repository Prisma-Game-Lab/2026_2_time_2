using UnityEngine;
using UnityEngine.UI;

public class PopUpAdUI : MonoBehaviour
{
    [SerializeField] private Image adImage;
    [SerializeField] private Button closeButton;

    public void Setup(Sprite sprite)
    {
        if (adImage != null && sprite != null)
        {
            adImage.sprite = sprite;
        }

        if (closeButton != null)
        {
            closeButton.onClick.RemoveAllListeners();
            closeButton.onClick.AddListener(CloseAd);
        }
    }

    public void CloseAd()
    {
        Destroy(gameObject);
    }
}