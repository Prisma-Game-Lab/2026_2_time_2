using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace TowerDefense
{
    // quadrado da hotbar
    public class HotbarSlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField] private Defender prefabTorre;
        [SerializeField] private Image fantasma;
        [SerializeField] private DefenderPlacer placer;

        public void OnBeginDrag(PointerEventData eventData)
        {
            fantasma.sprite = prefabTorre.GetComponent<SpriteRenderer>().sprite;
            fantasma.gameObject.SetActive(true);
        }

        public void OnDrag(PointerEventData eventData)
        {
            fantasma.transform.position = eventData.position;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            fantasma.gameObject.SetActive(false);

            Slot slot = placer.SlotEm(eventData.position);
            if (slot != null && slot.Torre == null)
                slot.Construir(prefabTorre);
        }
    }
}