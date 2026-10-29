using UnityEngine;
using UnityEngine.EventSystems;

public class WindowResize : MonoBehaviour, IDragHandler
{
    [Header("Janela Alvo")]
    [SerializeField] private RectTransform targetWindow;

    [Header("Limites de Tamanho")]
    [SerializeField] private Vector2 minSize = new Vector2(300, 200);
    [SerializeField] private Vector2 maxSize = new Vector2(1200, 800);

    private Canvas canvas;

    void Awake()
    {
        canvas = GetComponentInParent<Canvas>();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (targetWindow == null) return;

        float scale = canvas != null ? canvas.scaleFactor : 1f;
        Vector2 delta = eventData.delta / scale;

        Vector2 currentSize = targetWindow.sizeDelta;

        float newWidth = Mathf.Clamp(currentSize.x + delta.x, minSize.x, maxSize.x);
        float newHeight = Mathf.Clamp(currentSize.y - delta.y, minSize.y, maxSize.y);

        float deltaWidth = newWidth - currentSize.x;
        float deltaHeight = newHeight - currentSize.y;

        targetWindow.sizeDelta = new Vector2(newWidth, newHeight);

        targetWindow.anchoredPosition += new Vector2(
            deltaWidth * targetWindow.pivot.x,
            -deltaHeight * (1f - targetWindow.pivot.y)
        );
    }
}