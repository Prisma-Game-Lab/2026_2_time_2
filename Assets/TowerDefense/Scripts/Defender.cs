using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TowerDefense
{
    public class Defender : MonoBehaviour
    {
        [SerializeField] private Bullet prefabBala;
        [SerializeField] private float alcance = 3f;
        [SerializeField] private float intervaloTiro = 1f;
        [SerializeField] private int dano = 1;
        [SerializeField] private Defender proximoNivel;
        public Defender ProximoNivel => proximoNivel;

        [SerializeField] private int vida = 5;
        [SerializeField] private Color corQuebrada = new Color(0.35f, 0.35f, 0.35f, 1f);
        [SerializeField] private int custo = 50;
        
        public int Custo => custo;
        public int CustoConserto => custo / 2;
        private int vidaMaxima;
        private float tempo;
        private BarraVida barraVida;
        private SpriteRenderer sprite;
        private Animator animator;

        public bool Quebrada => vida <= 0;

        private void Awake()
        {
            vidaMaxima = vida;
            barraVida = GetComponentInChildren<BarraVida>(true);
            sprite = GetComponent<SpriteRenderer>();
            animator = GetComponent<Animator>();
        }

        private void Start()
        {
            barraVida.Atualizar(vida, vidaMaxima);
        }

        private void Update()
        {
            if (Quebrada) return;

            tempo += Time.deltaTime;

            Enemy alvo = InimigoMaisProximo();
            if (alvo == null) return;

            // o sprite aponta pra cima então up vira a direção do alvo
            // transform.up = alvo.transform.position - transform.position;

            if (tempo >= intervaloTiro)
            {
                tempo = 0;
                Bullet bala = Instantiate(prefabBala, transform.position, transform.rotation, transform.parent);
                bala.Iniciar(alvo, dano);
            }
        }

        public void TomarDano(int quantidade)
        {
            if (Quebrada) return;

            vida -= quantidade;
            barraVida.Atualizar(vida, vidaMaxima);

            if (Quebrada)
            {
                animator.enabled = false;
                sprite.color = corQuebrada;
            }
        }

        public void Consertar()
        {
            vida = vidaMaxima;
            barraVida.Atualizar(vida, vidaMaxima);

            animator.enabled = true;
            sprite.color = Color.white;
        }

        // 0 = quebrada (cinza), 1 = pronta (cor normal)
        public void MostrarConserto(float progresso)
        {
            sprite.color = Color.Lerp(corQuebrada, Color.white, progresso);
        }

        [ContextMenu("Quebrar")]
        private void DebugQuebrar() => TomarDano(vida);

        [ContextMenu("Consertar")]
        private void DebugConsertar() => Consertar();

        private Enemy InimigoMaisProximo()
        {
            Enemy maisProximo = null;
            float menorDistancia = alcance;

            foreach (Enemy inimigo in FindObjectsOfType<Enemy>())
            {
                float distancia = Vector3.Distance(transform.position, inimigo.transform.position);

                if (distancia <= menorDistancia)
                {
                    menorDistancia = distancia;
                    maisProximo = inimigo;
                }
            }

            return maisProximo;
        }

        // desenha o alcance
        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere(transform.position, alcance);
        }
    }
}