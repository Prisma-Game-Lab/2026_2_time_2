using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class DesktopItemUI : MonoBehaviour
{
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private Button button;

    private VirtualItem currentItem;
    private OSManager osManager;

    // Start is called before the first frame update
    public void Setup(VirtualItem item, OSManager manager)
    {
        currentItem = item;
        osManager = manager;

        nameText.text = item.itemName;
        if (item.icon != null) iconImage.sprite = item.icon;

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(OnClickItem);

    }

    private void OnClickItem()
    {
        osManager.OpenItem(currentItem);
    }
}
