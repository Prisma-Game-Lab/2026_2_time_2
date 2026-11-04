using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TowerDefense
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField] private float velocidade = 8f;

        private Enemy alvo;
        private int dano;

        public void Iniciar(Enemy novoAlvo, int novoDano)
        {
            alvo = novoAlvo;
            dano = novoDano;
        }

        private void Update()
        {
            // o alvo morreu antes da bala chegar
            if (alvo == null)
            {
                Destroy(gameObject);
                return;
            }

            Vector3 destino = alvo.transform.position;
            transform.up = destino - transform.position;
            transform.position = Vector3.MoveTowards(transform.position, destino, velocidade * Time.deltaTime);

            if (Vector3.Distance(transform.position, destino) < 0.2f)
            {
                alvo.TomarDano(dano);
                Destroy(gameObject);
            }
        }
    }
}