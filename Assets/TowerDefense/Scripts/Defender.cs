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

        private float tempo;

        private void Update()
        {
            tempo += Time.deltaTime;

            Enemy alvo = InimigoMaisProximo();
            if (alvo == null) return;

            // o sprite aponta pra cima então up vira a direção do alvo
            transform.up = alvo.transform.position - transform.position;

            if (tempo >= intervaloTiro)
            {
                tempo = 0;
                Bullet bala = Instantiate(prefabBala, transform.position, transform.rotation, transform.parent);
                bala.Iniciar(alvo, dano);
            }
        }

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
