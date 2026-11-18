using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "OS/Folder")]
public class VirtualFolder : VirtualItem
{
    public List<VirtualItem> contents = new List<VirtualItem>();

    [Header("Bloqueio por senha")]
    [Tooltip("Marque para exigir uma senha antes de abrir esta pasta.")]
    public bool isLocked = false;

    [Tooltip("Senha que o jogador precisa digitar para abrir a pasta.")]
    public string password = "";

    [Tooltip("Se marcado, \"Senha\" e \"senha\" são consideradas diferentes.")]
    public bool caseSensitive = true;

    [Tooltip("Opcional: ícone mostrado enquanto a pasta está bloqueada (ex.: pasta com cadeado).")]
    public Sprite lockedIcon;

    // Não é salvo no asset: vale só enquanto o jogo está rodando.
    [NonSerialized] public bool unlockedThisSession;

    /// <summary>True se a pasta ainda pede senha.</summary>
    public bool RequiresPassword => isLocked && !unlockedThisSession;

    /// <summary>Compara a tentativa com a senha definida no Inspector.</summary>
    public bool CheckPassword(string attempt)
    {
        if (attempt == null) return false;
        StringComparison mode = caseSensitive
            ? StringComparison.Ordinal
            : StringComparison.OrdinalIgnoreCase;
        return string.Equals(attempt, password, mode);
    }
}
