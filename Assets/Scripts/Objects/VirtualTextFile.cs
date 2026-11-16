using UnityEngine;

[CreateAssetMenu(menuName = "OS/TextFile")]
public class VirtualTextFile : VirtualItem
{
    [TextArea(5, 10)]
    public string textContent;

    [Tooltip("Se marcado, o jogador pode editar este arquivo durante o jogo. Desmarcado = somente leitura.")]
    public bool editable = false;
}
