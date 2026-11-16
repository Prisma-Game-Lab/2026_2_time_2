using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace TowerDefense
{
    // quadrado da hotbar: arrasta a torre até um slot vazio, se tiver dinheiro
    public class HotbarSlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField] private Defender prefabTorre;
        [SerializeField] private Image fantasma;
        [SerializeField] private DefenderPlacer placer;
        [SerializeField] private TMP_Text textoPreco;
        [SerializeField] private float alphaSemDinheiro = 0.4f;

        private CanvasGroup grupo;
        private bool arrastando;

        private void Awake()
        {
            grupo = GetComponent<CanvasGroup>();
            textoPreco.text = prefabTorre.Custo.ToString();
        }

        private void OnEnable()
        {
            TDManager.DinheiroMudou += AtualizarDisponivel;
        }

        private void OnDisable()
        {
            TDManager.DinheiroMudou -= AtualizarDisponivel;
        }

        private void AtualizarDisponivel(int dinheiro)
        {
            grupo.alpha = dinheiro >= prefabTorre.Custo ? 1f : alphaSemDinheiro;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            arrastando = TDManager.Instance.PodePagar(prefabTorre.Custo);
            if (!arrastando) return;

            fantasma.sprite = prefabTorre.GetComponent<SpriteRenderer>().sprite;
            fantasma.gameObject.SetActive(true);
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!arrastando) return;

            fantasma.transform.position = eventData.position;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (!arrastando) return;

            arrastando = false;
            fantasma.gameObject.SetActive(false);

            Slot slot = placer.SlotEm(eventData.position);
            if (slot != null && slot.Torre == null && TDManager.Instance.Gastar(prefabTorre.Custo))
                slot.Construir(prefabTorre);
        }
    }
}