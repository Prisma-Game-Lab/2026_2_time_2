using UnityEngine;

public class OSManager : MonoBehaviour
{
    [Header("Dados Iniciais")]
    [SerializeField] private VirtualFolder desktopFolder;

    [Header("Área de Trabalho e Janelas")]
    [SerializeField] private Transform desktopIconsContainer;
    [SerializeField] private Transform windowsContainer;
    [SerializeField] private GameObject itemIconPrefab;

    [Header("Prefabs de Janela")]
    [SerializeField] private GameObject notepadWindowPrefab;
    [SerializeField] private GameObject folderWindowPrefab;
    [SerializeField] private GameObject imageViewerWindowPrefab;
    [SerializeField] private GameObject audioPlayerWindowPrefab;

    void Start()
    {
        LoadDesktop();
    }

    public void LoadDesktop()
    {
        foreach (Transform child in desktopIconsContainer)
        {
            Destroy(child.gameObject);
        }

        if (desktopFolder != null)
        {
            foreach (VirtualItem item in desktopFolder.contents)
            {
                if (item == null) continue;
                GameObject iconObj = Instantiate(itemIconPrefab, desktopIconsContainer);
                iconObj.GetComponent<DesktopItemUI>().Setup(item, this);
            }
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
            OpenFolder(folder);
        }
        else if (item is VirtualImageFile imageFile)
        {
            OpenImageFile(imageFile);
        }
        else if (item is VirtualAudioFile audioFile)
        {
            OpenAudioFile(audioFile);
        }
    }

    private void OpenTextFile(VirtualTextFile file)
    {
        GameObject windowObj = Instantiate(notepadWindowPrefab, windowsContainer);
        windowObj.transform.SetAsLastSibling(); 
        windowObj.GetComponent<NotepadWindowUI>().Setup(file);
    }

    private void OpenFolder(VirtualFolder folder)
    {
        GameObject windowObj = Instantiate(folderWindowPrefab, windowsContainer);
        windowObj.transform.SetAsLastSibling(); 
        windowObj.GetComponent<FolderWindowUI>().Setup(folder, this, itemIconPrefab);
    }

    private void OpenImageFile(VirtualImageFile file)
    {
        GameObject windowObj = Instantiate(imageViewerWindowPrefab, windowsContainer);
        windowObj.transform.SetAsLastSibling();
        windowObj.GetComponent<ImageViewerWindowUI>().Setup(file);
    }

    private void OpenAudioFile(VirtualAudioFile file)
    {
        GameObject windowObj = Instantiate(audioPlayerWindowPrefab, windowsContainer);
        windowObj.transform.SetAsLastSibling();
        windowObj.GetComponent<AudioPlayerWindowUI>().Setup(file);
    }
}