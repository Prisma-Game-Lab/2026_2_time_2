using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TowerDefense
{
    public class ContadorNumero : MonoBehaviour
    {
        [Tooltip("Elemento N = sprite do dígito N")]
        [SerializeField] private Sprite[] sheet = new Sprite[10];
        [SerializeField] private float proporcaoDigito = 10f / 18f;

        [Header("Moeda (opcional)")]
        [SerializeField] private Sprite[] quadrosMoeda;
        [SerializeField] private float fpsMoeda = 10f;
        [Tooltip("Tamanho da moeda e espaço até os dígitos, em pixels da arte (dígito = 18 de altura)")]
        [SerializeField] private float tamanhoMoeda = 16f;
        [SerializeField] private float espacoMoeda = 2f;

        private readonly List<Image> imagens = new List<Image>();
        private Image moeda;
        private string texto = "";

        private bool TemMoeda => quadrosMoeda != null && quadrosMoeda.Length > 0;

        public void Mostrar(int valor)
        {
            texto = Mathf.Max(0, valor).ToString();

            while (imagens.Count < texto.Length)
                imagens.Add(CriarImagem("Digito"));

            if (TemMoeda && moeda == null)
                moeda = CriarImagem("Moeda");

            Desenhar();
        }

        // todas as moedas usam o mesmo relógio, então giram juntas
        private void Update()
        {
            if (moeda == null) return;

            int quadro = (int)(Time.time * fpsMoeda) % quadrosMoeda.Length;
            moeda.sprite = quadrosMoeda[quadro];
        }

        // a janela mudou de tamanho: reposiciona tudo
        private void OnRectTransformDimensionsChange() => Desenhar();

        private void Desenhar()
        {
            float altura = ((RectTransform)transform).rect.height;
            float pixel = altura / 18f; // um pixel da arte na escala atual
            float largura = altura * proporcaoDigito;

            float larguraMoeda = moeda != null ? tamanhoMoeda * pixel : 0f;
            float espaco = moeda != null ? espacoMoeda * pixel : 0f;
            float total = larguraMoeda + espaco + texto.Length * largura;
            float x = -total / 2f; // borda esquerda

            if (moeda != null)
            {
                Posicionar(moeda.rectTransform, x + larguraMoeda / 2f, larguraMoeda, larguraMoeda);
                x += larguraMoeda + espaco;
            }

            for (int i = 0; i < imagens.Count; i++)
            {
                bool usado = i < texto.Length;
                imagens[i].gameObject.SetActive(usado);
                if (!usado) continue;

                imagens[i].sprite = sheet[texto[i] - '0'];
                Posicionar(imagens[i].rectTransform, x + largura / 2f, largura, altura);
                x += largura;
            }
        }

        private void Posicionar(RectTransform rt, float centroX, float largura, float altura)
        {
            rt.anchorMin = rt.anchorMax = new Vector2(0.5f, 0.5f);
            rt.sizeDelta = new Vector2(largura, altura);
            rt.anchoredPosition = new Vector2(centroX, 0f);
        }

        private Image CriarImagem(string nome)
        {
            GameObject objeto = new GameObject(nome, typeof(RectTransform), typeof(Image));
            objeto.transform.SetParent(transform, false);

            Image imagem = objeto.GetComponent<Image>();
            imagem.raycastTarget = false;
            return imagem;
        }
    }
}