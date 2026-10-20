using UnityEngine;

[CreateAssetMenu(menuName = "OS/TextFile")]
public class VirtualTextFile : VirtualItem
{
    [TextArea(5, 10)]
    public string textContent;
}
