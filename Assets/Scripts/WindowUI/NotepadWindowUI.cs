using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NotepadWindowUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI contentText;
    [SerializeField] private Button closeButton;

    public void Setup(VirtualTextFile file)
    {
        if (titleText != null) titleText.text = file.itemName;
        if (contentText != null) contentText.text = file.textContent;

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