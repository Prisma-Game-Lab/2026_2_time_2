using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

/// <summary>
/// Decide qual cursor mostrar a cada frame:
/// 1) sobre um Selectable interagível (Button, Toggle, Slider...) -> cursor de botão
/// 2) sobre um objeto que tenha (ou cujo pai tenha) um CursorZone -> cursor daquela tela
/// 3) caso contrário -> cursor padrão (null = cursor do sistema)
///
/// O cursor é desenhado pela própria Unity: uma RawImage num Canvas por cima de tudo,
/// que segue o mouse. O cursor do sistema fica escondido. Assim a escala muda
/// o tamanho real do cursor na tela (o cursor de hardware tem tamanho fixo pelo SO).
/// </summary>
public class CursorManager : MonoBehaviour
{
    public static CursorManager Instance { get; private set; }

    [Header("Cursor padrão (fora de qualquer tela)")]
    [SerializeField] private Texture2D defaultCursor;           // pode ficar vazio = cursor do sistema
    [SerializeField] private Vector2 defaultHotspot = Vector2.zero;
    [Range(0.25f, 4f)]
    [SerializeField] private float defaultScale = 1f;

    [Header("Cursor sobre botões interagíveis")]
    [SerializeField] private Texture2D buttonCursor;
    [SerializeField] private Vector2 buttonHotspot = Vector2.zero;
    [Range(0.25f, 4f)]
    [SerializeField] private float buttonScale = 1f;

    [Header("Tamanho global (multiplica todos os cursores)")]
    [Tooltip("Útil para uma opção de acessibilidade no menu de configurações.")]
    [Range(0.25f, 4f)]
    [SerializeField] private float globalScale = 1f;

    [Header("Opções")]
    [Tooltip("Ordem de desenho do Canvas do cursor. Mantenha maior que a de qualquer outro Canvas.")]
    [SerializeField] private int sortingOrder = 32767;
    [SerializeField] private bool persistBetweenScenes = true;

    /// <summary>Altere em tempo de execução, ex.: CursorManager.Instance.GlobalScale = 1.5f;</summary>
    public float GlobalScale
    {
        get => globalScale;
        set => globalScale = Mathf.Clamp(value, 0.25f, 4f);
    }

    private readonly List<RaycastResult> raycastResults = new List<RaycastResult>();
    private PointerEventData pointerData;
    private EventSystem cachedEventSystem;

    // Cursor desenhado pela Unity
    private Canvas cursorCanvas;
    private RectTransform cursorRect;
    private RawImage cursorImage;

    private Texture2D currentTexture;
    private Vector2 currentHotspot;
    private float currentScale;
    private bool hasApplied;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        if (persistBetweenScenes) DontDestroyOnLoad(gameObject);

        CreateCursorCanvas();
    }

    private void CreateCursorCanvas()
    {
        // Canvas próprio, sem GraphicRaycaster: o cursor nunca bloqueia cliques.
        GameObject canvasObj = new GameObject("CursorCanvas", typeof(RectTransform), typeof(Canvas));
        canvasObj.transform.SetParent(transform, false);
        cursorCanvas = canvasObj.GetComponent<Canvas>();
        cursorCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
        cursorCanvas.sortingOrder = sortingOrder;

        GameObject imageObj = new GameObject("Cursor", typeof(RectTransform), typeof(RawImage));
        imageObj.transform.SetParent(canvasObj.transform, false);

        cursorRect = imageObj.GetComponent<RectTransform>();
        cursorRect.anchorMin = Vector2.zero;   // âncora no canto inferior esquerdo,
        cursorRect.anchorMax = Vector2.zero;   // igual à origem da posição do mouse

        cursorImage = imageObj.GetComponent<RawImage>();
        cursorImage.raycastTarget = false;
        cursorImage.enabled = false;
    }

    private void Update()
    {
        Texture2D texture = defaultCursor;
        Vector2 hotspot = defaultHotspot;
        float scale = defaultScale;

        GameObject hovered = GetTopUIObjectUnderPointer();
        if (hovered != null)
        {
            Selectable selectable = hovered.GetComponentInParent<Selectable>();
            if (selectable != null && selectable.IsInteractable() && buttonCursor != null)
            {
                texture = buttonCursor;
                hotspot = buttonHotspot;
                scale = buttonScale;
            }
            else
            {
                CursorZone zone = hovered.GetComponentInParent<CursorZone>();
                if (zone != null)
                {
                    texture = zone.CursorTexture;
                    hotspot = zone.Hotspot;
                    scale = zone.CursorScale;
                }
            }
        }

        Apply(texture, hotspot, scale * globalScale);
        FollowPointer();
    }

    private void FollowPointer()
    {
        Vector2 pos = GetPointerPosition();
        bool insideWindow = pos.x >= 0 && pos.y >= 0 && pos.x <= Screen.width && pos.y <= Screen.height;
        bool hasCustomCursor = currentTexture != null;

        // Esconde a imagem quando o mouse sai da janela do jogo.
        cursorImage.enabled = hasCustomCursor && insideWindow;
        // Esconde o cursor do sistema só quando há um cursor customizado.
        // (Definido a cada frame porque a Unity às vezes o reexibe ao recuperar o foco.)
        Cursor.visible = !hasCustomCursor;

        cursorRect.anchoredPosition = pos / cursorCanvas.scaleFactor;
    }

    private GameObject GetTopUIObjectUnderPointer()
    {
        EventSystem es = EventSystem.current;
        if (es == null) return null;

        // O EventSystem muda quando você troca de cena, então recriamos o PointerEventData.
        if (pointerData == null || cachedEventSystem != es)
        {
            pointerData = new PointerEventData(es);
            cachedEventSystem = es;
        }

        pointerData.position = GetPointerPosition();
        raycastResults.Clear();
        es.RaycastAll(pointerData, raycastResults);

        // O primeiro resultado é o elemento mais "na frente".
        return raycastResults.Count > 0 ? raycastResults[0].gameObject : null;
    }

    private static Vector2 GetPointerPosition()
    {
#if ENABLE_INPUT_SYSTEM
        return Mouse.current != null ? Mouse.current.position.ReadValue() : Vector2.zero;
#else
        return Input.mousePosition;
#endif
    }

    private void Apply(Texture2D texture, Vector2 hotspot, float scale)
    {
        // Só atualiza a imagem quando algo muda.
        if (hasApplied && texture == currentTexture && hotspot == currentHotspot
            && Mathf.Approximately(scale, currentScale)) return;

        currentTexture = texture;
        currentHotspot = hotspot;
        currentScale = scale;
        hasApplied = true;

        if (texture == null) return; // FollowPointer mostra o cursor do sistema

        cursorImage.texture = texture;

        // Tamanho real na tela = tamanho da imagem × escala.
        cursorRect.sizeDelta = new Vector2(texture.width, texture.height) * scale;

        // O hotspot vira o pivot do RectTransform (pivot é 0–1, com Y de baixo para cima).
        cursorRect.pivot = new Vector2(
            hotspot.x / texture.width,
            1f - hotspot.y / texture.height);
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Cursor.visible = true;
            Instance = null;
        }
    }
}