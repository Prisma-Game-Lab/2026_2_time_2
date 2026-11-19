using System.Collections;
using UnityEngine;

namespace TowerDefense
{
    // solta as waves montadas no Inspector; quando a última é eliminada, o jogador vence
    public class EnemySpawner : MonoBehaviour
    {
        [System.Serializable]
        public class GrupoInimigos
        {
            public Enemy prefab;
            public int quantidade = 5;
        }

        [System.Serializable]
        public class Wave
        {
            public GrupoInimigos[] grupos;
            public float intervalo = 1.5f; // segundos entre um inimigo e o próximo
        }

        [SerializeField] private Wave[] waves;
        [SerializeField] private float pausaEntreWaves = 5f;
        [SerializeField] private Vector2 tamanhoMapa = new Vector2(11, 11);

        private IEnumerator Start()
        {
            foreach (Wave wave in waves)
            {
                yield return new WaitForSeconds(pausaEntreWaves);

                foreach (GrupoInimigos grupo in wave.grupos)
                {
                    for (int i = 0; i < grupo.quantidade; i++)
                    {
                        if (TDManager.Instance.Terminou) yield break;

                        SpawnarInimigo(grupo.prefab);
                        yield return new WaitForSeconds(wave.intervalo);
                    }
                }

                // os inimigos são filhos do spawner: espera todos morrerem
                while (transform.childCount > 0)
                    yield return null;
            }

            TDManager.Instance.Vencer();
        }

        private void SpawnarInimigo(Enemy prefab)
        {
            float metadeLargura = tamanhoMapa.x / 2;
            float metadeAltura = tamanhoMapa.y / 2;

            float x;
            float y;
            int borda = UnityEngine.Random.Range(0, 4);

            if (borda == 0) // cima
            {
                x = UnityEngine.Random.Range(-metadeLargura, metadeLargura);
                y = metadeAltura;
            }
            else if (borda == 1) // baixo
            {
                x = UnityEngine.Random.Range(-metadeLargura, metadeLargura);
                y = -metadeAltura;
            }
            else if (borda == 2) // esquerda
            {
                x = -metadeLargura;
                y = UnityEngine.Random.Range(-metadeAltura, metadeAltura);
            }
            else // direita
            {
                x = metadeLargura;
                y = UnityEngine.Random.Range(-metadeAltura, metadeAltura);
            }

            Vector3 posicao = transform.position + new Vector3(x, y, 0);
            Instantiate(prefab, posicao, Quaternion.identity, transform);
        }

        // desenha o retângulo de onde os inimigos nascem
        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireCube(transform.position, tamanhoMapa);
        }
    }
}