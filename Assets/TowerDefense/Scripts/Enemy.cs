using UnityEngine;

namespace TowerDefense
{
    public class Enemy : MonoBehaviour
    {
        [SerializeField] private int vida = 3;
        [SerializeField] private float velocidade = 1f;
        [SerializeField] private int dano = 1;

        private Tower torre;

        private void Start()
        {
            torre = FindObjectOfType<Tower>();
        }

        private void Update()
        {
            transform.position = Vector3.MoveTowards(transform.position, torre.transform.position, velocidade * Time.deltaTime);

            if (Vector3.Distance(transform.position, torre.transform.position) < 1f)
            {
                torre.TomarDano(dano);
                Destroy(gameObject);
            }
        }

        public void TomarDano(int quantidade)
        {
            vida -= quantidade;

            if (vida <= 0)
                Destroy(gameObject);
        }

        [ContextMenu("Tomar 1 de dano")]
        private void DebugTomarDano() => TomarDano(1);
    }
}
