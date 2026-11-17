using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NotepadWindowUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI titleText;
    [Tooltip("Opcional: campo para o jogador renomear o arquivo. Se ficar vazio, o título é só o titleText.")]
    [SerializeField] private TMP_InputField titleInput;
    [Tooltip("Campo de texto editável (Line Type = Multi Line Newline). Substitui o antigo ConteudoText.")]
    [SerializeField] private TMP_InputField contentInput;
    [SerializeField] private Button saveButton;
    [SerializeField] private Button closeButton;
    [Tooltip("Opcional: mostra \"* não salvo\" ou \"(somente leitura)\".")]
    [SerializeField] private TextMeshProUGUI statusText;

    private VirtualTextFile currentFile;
    private OSManager osManager;
    private bool isNewFile;          // true = arquivo ainda não está na Área de Trabalho
    private bool hasUnsavedChanges;

    public void Setup(VirtualTextFile file, OSManager manager, bool isNew = false)
    {
        currentFile = file;
        osManager = manager;
        isNewFile = isNew;

        bool editable = file.editable;

        if (titleText != null) titleText.text = file.itemName;

        if (titleInput != null)
        {
            titleInput.SetTextWithoutNotify(file.itemName);
            titleInput.readOnly = !editable;
            titleInput.onValueChanged.RemoveAllListeners();
            titleInput.onValueChanged.AddListener(_ => MarkDirty());
        }

        if (contentInput != null)
        {
            contentInput.SetTextWithoutNotify(file.textContent);
            contentInput.readOnly = !editable;
            contentInput.onValueChanged.RemoveAllListeners();
            contentInput.onValueChanged.AddListener(_ => MarkDirty());
        }

        if (saveButton != null)
        {
            saveButton.gameObject.SetActive(editable);
            saveButton.onClick.RemoveAllListeners();
            saveButton.onClick.AddListener(Save);
        }

        if (closeButton != null)
        {
            closeButton.onClick.RemoveAllListeners();
            closeButton.onClick.AddListener(CloseWindow);
        }

        hasUnsavedChanges = false;
        RefreshStatus();

        if (editable && contentInput != null) contentInput.ActivateInputField();
    }

    public void Save()
    {
        if (currentFile == null || !currentFile.editable) return;

        if (titleInput != null && !string.IsNullOrWhiteSpace(titleInput.text))
            currentFile.itemName = titleInput.text.Trim();

        if (contentInput != null)
            currentFile.textContent = contentInput.text;

        if (isNewFile)
        {
            osManager.AddToDesktop(currentFile);   // cria o ícone na Área de Trabalho
            isNewFile = false;
        }
        else
        {
            osManager.RefreshDesktop();            // atualiza o nome no ícone, se mudou
        }

        if (titleText != null) titleText.text = currentFile.itemName;
        hasUnsavedChanges = false;
        RefreshStatus();
    }

    /// <summary>True enquanto o jogador digita nesta janela (útil para bloquear atalhos do jogo).</summary>
    public bool IsTyping =>
        (contentInput != null && contentInput.isFocused) ||
        (titleInput != null && titleInput.isFocused);

    private void Update()
    {
        // Ctrl+S só salva a janela em que o jogador está digitando.
        if (IsTyping && CtrlSPressed()) Save();
    }

    private static bool CtrlSPressed()
    {
#if ENABLE_INPUT_SYSTEM
        var kb = UnityEngine.InputSystem.Keyboard.current;
        return kb != null && kb.ctrlKey.isPressed && kb.sKey.wasPressedThisFrame;
#else
        return (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
               && Input.GetKeyDown(KeyCode.S);
#endif
    }

    private void MarkDirty()
    {
        hasUnsavedChanges = true;
        RefreshStatus();
    }

    private void RefreshStatus()
    {
        if (statusText == null || currentFile == null) return;
        if (!currentFile.editable) statusText.text = "(somente leitura)";
        else statusText.text = hasUnsavedChanges ? "* não salvo" : "";
    }

    public void CloseWindow()
    {
        Destroy(gameObject);
    }
}
