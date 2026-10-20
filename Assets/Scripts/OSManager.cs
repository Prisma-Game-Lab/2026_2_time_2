using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class OSManager : MonoBehaviour
{

    [Header("Dados Iniciais")]
    [SerializeField] private VirtualFolder desktopFolder;


    [Header("Referências da Interface")]
    [SerializeField] private Transform desktopIconsContainer;
    [SerializeField] private GameObject itemIconPrefab;


    [Header("Janela de Texto (Bloco de Notas)")]
    [SerializeField] private GameObject notepadWindow;
    [SerializeField] private TextMeshProUGUI notepadTitle;
    [SerializeField] private TextMeshProUGUI notepadContent;

    // Start is called before the first frame update
    void Start()
    {
        LoadDesktop();
    }

    // Update is called once per frame
    public void LoadDesktop()
    {
        foreach (Transform child in desktopIconsContainer)
        {
            Destroy(child.gameObject);
        }

        foreach (VirtualItem item in desktopFolder.contents)
        {
            GameObject iconObj = Instantiate(itemIconPrefab, desktopIconsContainer);
            iconObj.GetComponent<DesktopItemUI>().Setup(item, this);
        }
    }

    public void OpenItem(VirtualItem item)
    {
        if (item is VirtualTextFile textFile)
        {
            OpenTextFile(textFile);
        }
        else if (item is VirtualFolder folder)
        {
            Debug.Log($"Abrindo pasta: {folder.itemName} com {folder.contents.Count} itens");
        }
    }

    private void OpenTextFile(VirtualTextFile file)
    {
        notepadWindow.SetActive(true);
        notepadWindow.transform.SetAsLastSibling();
        notepadTitle.text = file.itemName;
        notepadContent.text = file.textContent;
    }

    public void CloseNotepad()
    {
        notepadWindow.SetActive(false);
    }
}
