using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class WindowDrag : MonoBehaviour, IDragHandler
{
    [SerializeField] private RectTransform targetWindow;

    public void OnDrag(PointerEventData eventData)
    {
        if (targetWindow != null)
        {
            targetWindow.anchoredPosition += eventData.delta;
        }
    }
}
