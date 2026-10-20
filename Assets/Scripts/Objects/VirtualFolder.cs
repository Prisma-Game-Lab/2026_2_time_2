using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "OS/Folder")]
public class VirtualFolder : VirtualItem
{
    public List<VirtualItem> contents = new List<VirtualItem>();
}
