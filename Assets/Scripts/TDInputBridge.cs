using UnityEngine;
using UnityEngine.EventSystems;

public class TDInputBridge : MonoBehaviour, IPointerClickHandler, IDragHandler
{
    [SerializeField] private Camera tdCamera;
    private RectTransform rectTransform;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    void Start()
    {
        // Busca a câmera do TD automaticamente se ela não foi setada
        if (tdCamera == null)
        {
            // Procura a câmera que renderiza na TargetTexture do TD
            foreach (Camera cam in Camera.allCameras)
            {
                if (cam.targetTexture != null)
                {
                    tdCamera = cam;
                    break;
                }
            }
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        SendRaycastToTD(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Se seu TD usa arrastar para posicionar torres
        SendRaycastToTD(eventData);
    }

    private void SendRaycastToTD(PointerEventData eventData)
    {
        if (tdCamera == null) return;

        // Converte a posição do mouse na tela para a coordenada local do RawImage
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out Vector2 localPoint))
        {
            // Normaliza as coordenadas para valores entre 0.0 e 1.0 (coordenadas de Viewport)
            Rect rect = rectTransform.rect;
            float normalizedX = (localPoint.x - rect.x) / rect.width;
            float normalizedY = (localPoint.y - rect.y) / rect.height;

            // Gera um raio saindo da câmera do Tower Defense exatamente onde clicou na janela
            Ray ray = tdCamera.ViewportPointToRay(new Vector3(normalizedX, normalizedY, 0));

            // Executa o Raycast na cena do Tower Defense
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                // Aqui dispara a lógica do seu TD (exemplo: selecionar célula de construção)
                // hit.collider.GetComponent<Tile>()?.BuildTower();
                Debug.Log($"Clicou no TD em: {hit.collider.name}");
            }
            // Caso seu Tower Defense seja 2D (Physics2D):
            /*
            RaycastHit2D hit2D = Physics2D.GetRayIntersection(ray);
            if (hit2D.collider != null)
            {
                Debug.Log($"Clicou no TD 2D em: {hit2D.collider.name}");
            }
            */
        }
    }
}