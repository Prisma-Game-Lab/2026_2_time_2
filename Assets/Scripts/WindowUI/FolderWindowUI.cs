using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FolderWindowUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private Transform contentContainer;
    [SerializeField] private Button backButton;
    [SerializeField] private Button closeButton;

    private GameObject itemIconPrefab;
    private OSManager osManager;
    private Stack<VirtualFolder> folderHistory = new Stack<VirtualFolder>();
    private VirtualFolder currentFolder;

    public void Setup(VirtualFolder folder, OSManager manager, GameObject iconPrefab)
    {
        osManager = manager;
        itemIconPrefab = iconPrefab;

        if (closeButton != null)
        {
            closeButton.onClick.RemoveAllListeners();
            closeButton.onClick.AddListener(CloseWindow);
        }

        if (backButton != null)
        {
            backButton.onClick.RemoveAllListeners();
            backButton.onClick.AddListener(BackFolder);
        }

        DisplayFolder(folder);
    }

    public void DisplayFolder(VirtualFolder folder)
    {
        currentFolder = folder;
        if (titleText != null) titleText.text = folder.itemName;

        foreach (Transform child in contentContainer)
        {
            Destroy(child.gameObject);
        }

        foreach (VirtualItem item in folder.contents)
        {
            if (item == null) continue;
            GameObject iconObj = Instantiate(itemIconPrefab, contentContainer);

            iconObj.GetComponent<DesktopItemUI>().Setup(item, osManager, this);
        }

        if (backButton != null)
        {
            backButton.interactable = folderHistory.Count > 0;
        }
    }

    public void OpenSubFolder(VirtualFolder subFolder)
    {
        if (currentFolder != null)
        {
            folderHistory.Push(currentFolder);
        }
        DisplayFolder(subFolder);
    }

    public void BackFolder()
    {
        if (folderHistory.Count > 0)
        {
            DisplayFolder(folderHistory.Pop());
        }
    }

    public void CloseWindow()
    {
        Destroy(gameObject);
    }
}