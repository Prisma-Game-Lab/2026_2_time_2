using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TDWindowUI : MonoBehaviour
{
    [Header("Interface")]
    [SerializeField] private string titulo = "Tower Defense";
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private Button closeButton;

    void Start()
    {
        if (titleText != null) titleText.text = titulo;

        if (closeButton != null)
        {
            closeButton.onClick.RemoveAllListeners();
            closeButton.onClick.AddListener(CloseWindow);
        }
    }

    // o mundo do TD é destruído junto
    public void CloseWindow()
    {
        Destroy(gameObject);
    }
}