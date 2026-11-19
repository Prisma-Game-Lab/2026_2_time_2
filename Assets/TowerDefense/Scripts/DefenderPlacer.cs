using UnityEngine;
using UnityEngine.EventSystems;

namespace TowerDefense
{
    // clique: upgrade da torre. segurar em torre quebrada: conserta
    public class DefenderPlacer : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] private Camera cameraTD;
        [SerializeField] private float tempoConserto = 2f;

        private Defender consertando;
        private float progresso;
        private bool ignorarClique;

        public void OnPointerDown(PointerEventData eventData)
        {
            Slot slot = SlotEm(eventData.position);
            if (slot == null || slot.Torre == null || !slot.Torre.Quebrada) return;
            if (!TDManager.Instance.PodePagar(slot.Torre.CustoConserto)) return;

            consertando = slot.Torre;
            progresso = 0f;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            CancelarConserto();
        }

        private void Update()
        {
            if (consertando == null) return;

            // tirou o mouse de cima da torre: cancela
            Slot slot = SlotEm(Input.mousePosition);
            if (slot == null || slot.Torre != consertando)
            {
                CancelarConserto();
                return;
            }

            progresso += Time.deltaTime / tempoConserto;
            consertando.MostrarConserto(progresso);

            if (progresso >= 1f)
            {
                if (TDManager.Instance.Gastar(consertando.CustoConserto))
                    consertando.Consertar();
                else
                    consertando.MostrarConserto(0f);

                consertando = null;
                ignorarClique = true; // o soltar do botão não pode virar upgrade
            }
        }

        private void CancelarConserto()
        {
            if (consertando != null) consertando.MostrarConserto(0f);
            consertando = null;
        }

        public void DefinirCamera(Camera camera) => cameraTD = camera;

        public void OnPointerClick(PointerEventData eventData)
        {
            if (ignorarClique)
            {
                ignorarClique = false;
                return;
            }

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