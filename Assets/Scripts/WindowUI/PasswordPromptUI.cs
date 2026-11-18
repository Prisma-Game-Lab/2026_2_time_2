using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Caixa de senha que aparece ao clicar numa pasta bloqueada.
/// É criada pelo OSManager (campo "Password Prompt Prefab").
/// </summary>
public class PasswordPromptUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI titleText;
    [Tooltip("Campo de uma linha onde o jogador digita a senha.")]
    [SerializeField] private TMP_InputField passwordInput;
    [SerializeField] private Button confirmButton;
    [Tooltip("Botão de cancelar ou o \"X\" da janela.")]
    [SerializeField] private Button cancelButton;
    [Tooltip("Texto que aparece quando a senha está errada.")]
    [SerializeField] private TextMeshProUGUI errorText;

    [SerializeField] private string wrongPasswordMessage = "Senha incorreta.";
    [Tooltip("Mostra a senha como ••••• enquanto o jogador digita.")]
    [SerializeField] private bool hideCharacters = true;

    private VirtualFolder folder;
    private Action onUnlocked;

    public void Setup(VirtualFolder lockedFolder, Action onSuccess)
    {
        folder = lockedFolder;
        onUnlocked = onSuccess;

        if (titleText != null) titleText.text = $"\"{folder.itemName}\" está protegida por senha";

        if (errorText != null) errorText.gameObject.SetActive(false);

        if (passwordInput != null)
        {
            if (hideCharacters)
            {
                passwordInput.contentType = TMP_InputField.ContentType.Password;
                passwordInput.ForceLabelUpdate();
            }
            passwordInput.SetTextWithoutNotify("");
            passwordInput.onSubmit.RemoveAllListeners();
            passwordInput.onSubmit.AddListener(_ => TryUnlock());   // Enter confirma
            passwordInput.ActivateInputField();
        }

        if (confirmButton != null)
        {
            confirmButton.onClick.RemoveAllListeners();
            confirmButton.onClick.AddListener(TryUnlock);
        }

        if (cancelButton != null)
        {
            cancelButton.onClick.RemoveAllListeners();
            cancelButton.onClick.AddListener(Close);
        }
    }

    public void TryUnlock()
    {
        if (folder == null || passwordInput == null) return;

        if (folder.CheckPassword(passwordInput.text))
        {
            folder.unlockedThisSession = true;
            Action callback = onUnlocked;
            Destroy(gameObject);
            callback?.Invoke();          // abre a pasta
        }
        else
        {
            if (errorText != null)
            {
                errorText.text = wrongPasswordMessage;
                errorText.gameObject.SetActive(true);
            }
            passwordInput.SetTextWithoutNotify("");
            passwordInput.ActivateInputField();
        }
    }

    public void Close()
    {
        Destroy(gameObject);
    }
}
