using UnityEngine;
using UnityEngine.EventSystems;

namespace TowerDefense
{
    public class DefenderPlacer : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private Camera cameraTD;

        public void OnPointerClick(PointerEventData eventData)
        {
            Slot slot = SlotEm(eventData.position);
            if (slot == null || slot.Torre == null) return;
            if (slot.Torre.Quebrada) return;

            Defender proximo = slot.Torre.ProximoNivel;
            if (proximo != null && TDManager.Instance.Gastar(proximo.Custo))
                slot.Construir(proximo);
        }

        // slot que está debaixo de um ponto da tela (ou null)
        public Slot SlotEm(Vector2 posicaoTela)
        {
            Vector2 posicao = TelaParaMundo(posicaoTela);

            foreach (Slot slot in FindObjectsOfType<Slot>())
            {
                if (Vector2.Distance(slot.transform.position, posicao) < 0.5f)
                    return slot;
            }
            return null;
        }

        // tela > ponto dentro da TD_View > 0 a 1 > mundo do TD
        private Vector3 TelaParaMundo(Vector2 posicaoTela)
        {
            RectTransform area = (RectTransform)transform;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(area, posicaoTela, null, out Vector2 pontoLocal);
            Vector2 proporcao = Rect.PointToNormalized(area.rect, pontoLocal);

            return cameraTD.ViewportToWorldPoint(proporcao);
        }
    }
}