using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TowerDefense
{
    // mostra um número com os sprites de dígito, centralizado e na altura do próprio retângulo
    public class ContadorNumero : MonoBehaviour
    {
        [Tooltip("A arte vai de 1 a 9 e termina no 0")]
        [SerializeField] private Sprite[] sheet = new Sprite[10];
        [SerializeField] private float proporcaoDigito = 10f / 18f;

        private readonly List<Image> imagens = new List<Image>();
        private string texto = "";

        public void Mostrar(int valor)
        {
            texto = Mathf.Max(0, valor).ToString();

            while (imagens.Count < texto.Length)
                imagens.Add(CriarImagem());

            Desenhar();
        }

        // a janela mudou de tamanho: reposiciona os dígitos
        private void OnRectTransformDimensionsChange() => Desenhar();

        private void Desenhar()
        {
            float altura = ((RectTransform)transform).rect.height;
            float largura = altura * proporcaoDigito;

            for (int i = 0; i < imagens.Count; i++)
            {
                bool usado = i < texto.Length;
                imagens[i].gameObject.SetActive(usado);
                if (!usado) continue;

                int numero = texto[i] - '0';
                imagens[i].sprite = sheet[(numero + 9) % 10]; // 1 tá no _0, 0 tá no _9

                RectTransform rt = imagens[i].rectTransform;
                rt.anchorMin = rt.anchorMax = new Vector2(0.5f, 0.5f);
                rt.sizeDelta = new Vector2(largura, altura);
                rt.anchoredPosition = new Vector2((i - (texto.Length - 1) / 2f) * largura, 0f);
            }
        }

        private Image CriarImagem()
        {
            GameObject digito = new GameObject("Digito", typeof(RectTransform), typeof(Image));
            digito.transform.SetParent(transform, false);

            Image imagem = digito.GetComponent<Image>();
            imagem.raycastTarget = false;
            return imagem;
        }
    }
}