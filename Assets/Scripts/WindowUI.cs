using UnityEngine;
using UnityEngine.EventSystems;

public class WindowUI : MonoBehaviour, IPointerDownHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        transform.SetAsLastSibling();
    }
}
