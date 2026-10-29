using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DesktopItemUI : MonoBehaviour
{
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private Button button;

    private VirtualItem currentItem;
    private OSManager osManager;
    private FolderWindowUI parentFolderWindow;

    public void Setup(VirtualItem item, OSManager manager, FolderWindowUI folderWindow = null)
    {
        currentItem = item;
        osManager = manager;
        parentFolderWindow = folderWindow;

        if (nameText != null) nameText.text = item.itemName;
        if (iconImage != null && item.icon != null) iconImage.sprite = item.icon;

        if (button != null)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(OnClickItem);
        }
    }

    private void OnClickItem()
    {
        if (currentItem is VirtualFolder subFolder && parentFolderWindow != null)
        {
            parentFolderWindow.OpenSubFolder(subFolder);
        }
        else
        {
            osManager.OpenItem(currentItem);
        }
    }
}