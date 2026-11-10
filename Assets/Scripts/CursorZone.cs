using UnityEngine;


public class CursorZone : MonoBehaviour
{
    [Tooltip("Textura do cursor dessa tela")]
    [SerializeField] private Texture2D cursorTexture;

    [Tooltip("Ponto de clique em pixels")]
    [SerializeField] private Vector2 hotspot = Vector2.zero;
    [Range(0.25f, 4f)]
    [SerializeField] private float cursorScale = 1f;

    public Texture2D CursorTexture => cursorTexture;
    public Vector2 Hotspot => hotspot;
    public float CursorScale => cursorScale;
}