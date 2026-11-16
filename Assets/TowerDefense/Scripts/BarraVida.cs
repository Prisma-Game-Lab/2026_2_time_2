using UnityEngine;

namespace TowerDefense
{
    // barra de vida em cima de um objeto do TD: a moldura fica parada e o preenchimento esvazia pra esquerda
    public class BarraVida : MonoBehaviour
    {
        [SerializeField] private Transform preenchimento;
        [SerializeField] private bool esconderCheia = true;

        // o preenchimento ocupa 58 dos 64 pixels do sprite
        private const float largura = 58f / 64f;

        private Vector3 deslocamento;

        private void Awake()
        {
            deslocamento = transform.position - transform.parent.position;
        }

        public void Atualizar(int vida, int vidaMaxima)
        {
            float proporcao = Mathf.Clamp01((float)vida / vidaMaxima);

            preenchimento.localScale = new Vector3(proporcao, 1f, 1f);
            preenchimento.localPosition = new Vector3(-(1f - proporcao) * largura / 2f, 0f, 0f);

            gameObject.SetActive(!(esconderCheia && proporcao >= 1f));
        }

        // fica sempre em cima e reta, mesmo se o dono girar
        private void LateUpdate()
        {
            transform.SetPositionAndRotation(transform.parent.position + deslocamento, Quaternion.identity);
        }
    }
}