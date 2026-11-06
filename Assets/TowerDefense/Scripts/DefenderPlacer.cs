using UnityEngine;
using UnityEngine.EventSystems;

namespace TowerDefense
{
    // recebe o clique na janela e coloca ou melhora defenders
    public class DefenderPlacer : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private Camera cameraTD;
        [SerializeField] private Transform mundo;
        [SerializeField] private Defender prefabDefender;
        [SerializeField] private float distanciaMinimaTorre = 1.5f;

        private Tower torre;

        private void Start()
        {
            torre = FindObjectOfType<Tower>();
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
                Debug.Log($"mouse {Input.mousePosition}");
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            Vector3 posicao = CliqueParaMundo(eventData.position);

            // alinha no grid de 1 unidade
            posicao.x = Mathf.Round(posicao.x);
            posicao.y = Mathf.Round(posicao.y);
            posicao.z = 0;
            Debug.Log($"tela {eventData.position}: mundo {posicao}");

            Defender existente = DefenderEm(posicao);

            if (existente != null)
            {
                if (existente.ProximoNivel != null)
                {
                    Instantiate(existente.ProximoNivel, posicao, Quaternion.identity, mundo);
                    Destroy(existente.gameObject);
                }
                return;
            }

            if (Vector3.Distance(posicao, torre.transform.position) < distanciaMinimaTorre) return;

            Instantiate(prefabDefender, posicao, Quaternion.identity, mundo);
        }

        // tela > ponto dentro da TD_View > 0 a 1 > mundo do TD
        private Vector3 CliqueParaMundo(Vector2 posicaoTela)
        {
            RectTransform area = (RectTransform)transform;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(area, posicaoTela, null, out Vector2 pontoLocal);
            Vector2 proporcao = Rect.PointToNormalized(area.rect, pontoLocal);

            return cameraTD.ViewportToWorldPoint(proporcao);
        }

        private Defender DefenderEm(Vector3 posicao)
        {
            foreach (Defender defender in FindObjectsOfType<Defender>())
            {
                if (Vector3.Distance(defender.transform.position, posicao) < 0.5f)
                    return defender;
            }
            return null;
        }
    }
}