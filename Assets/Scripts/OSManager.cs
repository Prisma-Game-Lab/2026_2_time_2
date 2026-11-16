using System.Collections.Generic;
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

    [Header("Novos arquivos de texto")]
    [Tooltip("Ícone dos arquivos criados pelo jogador (ex.: txtPlaceHolder).")]
    [SerializeField] private Sprite newTextFileIcon;
    [SerializeField] private string newTextFileName = "Novo documento";

    // Cópia da árvore de arquivos usada durante o jogo. Os assets originais nunca são alterados.
    private VirtualFolder runtimeDesktop;
    private readonly Dictionary<VirtualItem, VirtualItem> runtimeCopies = new Dictionary<VirtualItem, VirtualItem>();
    private readonly List<VirtualItem> createdAtRuntime = new List<VirtualItem>();

    void Start()
    {
        if (desktopFolder != null)
            runtimeDesktop = (VirtualFolder)GetRuntimeCopy(desktopFolder);

        LoadDesktop();
    }

    /// <summary>
    /// Devolve a cópia de um item (criando-a na primeira vez). Pastas copiam o conteúdo
    /// recursivamente. O dicionário garante que um arquivo que aparece em duas pastas
    /// continue sendo um único arquivo (editar em um lugar reflete no outro).
    /// </summary>
    private VirtualItem GetRuntimeCopy(VirtualItem original)
    {
        if (original == null) return null;
        if (runtimeCopies.TryGetValue(original, out VirtualItem existing)) return existing;

        VirtualItem copy = Instantiate(original);
        copy.name = original.name;            // tira o "(Clone)" do nome
        runtimeCopies[original] = copy;
        createdAtRuntime.Add(copy);

        if (copy is VirtualFolder folderCopy)
        {
            // A lista foi copiada com referências aos assets originais; troca pelas cópias.
            var originalContents = new List<VirtualItem>(folderCopy.contents);
            folderCopy.contents = new List<VirtualItem>();
            foreach (VirtualItem child in originalContents)
            {
                VirtualItem childCopy = GetRuntimeCopy(child);
                if (childCopy != null) folderCopy.contents.Add(childCopy);
            }
        }

        return copy;
    }

    public void LoadDesktop()
    {
        foreach (Transform child in desktopIconsContainer)
        {
            Destroy(child.gameObject);
        }

        if (runtimeDesktop != null)
        {
            foreach (VirtualItem item in runtimeDesktop.contents)
            {
                if (item == null) continue;
                GameObject iconObj = Instantiate(itemIconPrefab, desktopIconsContainer);
                iconObj.GetComponent<DesktopItemUI>().Setup(item, this);
            }
        }
    }

    /// <summary>Recarrega os ícones (ex.: depois que um arquivo foi renomeado).</summary>
    public void RefreshDesktop()
    {
        LoadDesktop();
    }

    /// <summary>Coloca um item novo na Área de Trabalho e mostra o ícone dele.</summary>
    public void AddToDesktop(VirtualItem item)
    {
        if (runtimeDesktop == null || item == null) return;
        if (!runtimeDesktop.contents.Contains(item))
            runtimeDesktop.contents.Add(item);
        LoadDesktop();
    }

    /// <summary>
    /// Abre o bloco de notas com um documento em branco. Ligue no OnClick() do ícone/botão
    /// "Bloco de Notas". O arquivo só aparece na Área de Trabalho quando o jogador salva.
    /// </summary>
    public void CreateNewTextFile()
    {
        VirtualTextFile file = ScriptableObject.CreateInstance<VirtualTextFile>();
        file.name = newTextFileName;
        file.itemName = newTextFileName;
        file.icon = newTextFileIcon;
        file.textContent = "";
        file.editable = true;
        createdAtRuntime.Add(file);

        OpenTextFile(file, isNew: true);
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

    private void OpenTextFile(VirtualTextFile file, bool isNew = false)
    {
        GameObject windowObj = Instantiate(notepadWindowPrefab, windowsContainer);
        windowObj.transform.SetAsLastSibling();
        windowObj.GetComponent<NotepadWindowUI>().Setup(file, this, isNew);
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

    private void OnDestroy()
    {
        // Libera as cópias criadas em tempo de execução.
        foreach (VirtualItem item in createdAtRuntime)
            if (item != null) Destroy(item);
        createdAtRuntime.Clear();
        runtimeCopies.Clear();
    }
}
