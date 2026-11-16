using UnityEngine;

namespace TowerDefense
{
    // anda até a torre ativa mais próxima (defender ou a principal), causa dano ao encostar e some
    public class Enemy : MonoBehaviour
    {
        [SerializeField] private int vida = 3;
        [SerializeField] private float velocidade = 1f;
        [SerializeField] private int dano = 1;
        [SerializeField] private float distanciaAtaque = 0.8f;
        [SerializeField] private BarraVida barraVida;

        private Tower torre;
        private int vidaMaxima;

        private void Start()
        {
            torre = FindObjectOfType<Tower>();
            vidaMaxima = vida;
            barraVida.Atualizar(vida, vidaMaxima);
        }

        private void Update()
        {
            Defender defender = DefenderMaisProximo();

            // vai no defender só se ele estiver mais perto que a torre principal
            bool alvoDefender = defender != null &&
                Distancia(defender.transform.position) < Distancia(torre.transform.position);

            Vector3 destino = alvoDefender ? defender.transform.position : torre.transform.position;

            transform.position = Vector3.MoveTowards(transform.position, destino, velocidade * Time.deltaTime);

            if (Distancia(destino) > distanciaAtaque) return;

            if (alvoDefender)
                defender.TomarDano(dano);
            else
                torre.TomarDano(dano);

            Destroy(gameObject);
        }

        private Defender DefenderMaisProximo()
        {
            Defender maisProximo = null;
            float menorDistancia = Mathf.Infinity;

            foreach (Defender defender in FindObjectsOfType<Defender>())
            {
                if (defender.Quebrada) continue;

                float distancia = Distancia(defender.transform.position);
                if (distancia < menorDistancia)
                {
                    menorDistancia = distancia;
                    maisProximo = defender;
                }
            }

            return maisProximo;
        }

        // distância só em x e y
        private float Distancia(Vector3 posicao)
        {
            return Vector2.Distance(transform.position, posicao);
        }

        public void TomarDano(int quantidade)
        {
            vida -= quantidade;
            barraVida.Atualizar(vida, vidaMaxima);

            if (vida <= 0)
                Destroy(gameObject);
        }

        [ContextMenu("Tomar 1 de dano")]
        private void DebugTomarDano() => TomarDano(1);
    }
}