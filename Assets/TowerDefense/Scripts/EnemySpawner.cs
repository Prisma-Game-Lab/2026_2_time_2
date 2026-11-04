using UnityEngine;

namespace TowerDefense
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private Enemy[] prefabsInimigos;
        [SerializeField] private float intervalo = 2f;
        [SerializeField] private Vector2 tamanhoMapa = new Vector2(16, 9);

        private float tempo;

        private void Update()
        {
            tempo += Time.deltaTime;

            if (tempo >= intervalo)
            {
                tempo = 0;
                SpawnarInimigo();
            }
        }

        private void SpawnarInimigo()
        {
            float metadeLargura = tamanhoMapa.x / 2;
            float metadeAltura = tamanhoMapa.y / 2;

            float x;
            float y;
            int borda = Random.Range(0, 4);

            if (borda == 0) // cima
            {
                x = Random.Range(-metadeLargura, metadeLargura);
                y = metadeAltura;
            }
            else if (borda == 1) // baixo
            {
                x = Random.Range(-metadeLargura, metadeLargura);
                y = -metadeAltura;
            }
            else if (borda == 2) // esquerda
            {
                x = -metadeLargura;
                y = Random.Range(-metadeAltura, metadeAltura);
            }
            else // direita
            {
                x = metadeLargura;
                y = Random.Range(-metadeAltura, metadeAltura);
            }

            Vector3 posicao = transform.position + new Vector3(x, y, 0);
            Enemy prefab = prefabsInimigos[Random.Range(0, prefabsInimigos.Length)];

            Instantiate(prefab, posicao, Quaternion.identity, transform);
        }

        // desenha o retângulo de onde os inimigos nascem
        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireCube(transform.position, tamanhoMapa);
        }
    }
}
