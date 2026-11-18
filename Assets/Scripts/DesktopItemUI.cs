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

        if (iconImage != null)
        {
            Sprite sprite = item.icon;

            // Pasta ainda bloqueada com ícone de cadeado definido: usa esse ícone.
            if (item is VirtualFolder folder && folder.RequiresPassword && folder.lockedIcon != null)
                sprite = folder.lockedIcon;

            if (sprite != null) iconImage.sprite = sprite;
        }

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
            // Subpasta dentro de uma janela de pasta: pede a senha (se precisar) antes de entrar.
            FolderWindowUI window = parentFolderWindow;
            osManager.RequestFolderAccess(subFolder, () =>
            {
                if (window != null) window.OpenSubFolder(subFolder);
            });
        }
        else
        {
            osManager.OpenItem(currentItem);
        }
    }
}
