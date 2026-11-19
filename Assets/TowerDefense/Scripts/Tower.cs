using UnityEngine;

namespace TowerDefense
{
    public class Tower : MonoBehaviour
    {
        [SerializeField] private int vida = 20;
        [SerializeField] private BarraVida barraVida;

        private int vidaMaxima;

        private void Start()
        {
            vidaMaxima = vida;
            barraVida.Atualizar(vida, vidaMaxima);
        }

        public void TomarDano(int quantidade)
        {
            if (vida <= 0)
                TDManager.Instance.Perder();

            vida -= quantidade;
            barraVida.Atualizar(vida, vidaMaxima);
            Debug.Log($"Torre: {vida} de vida");

            if (vida <= 0)
                Debug.Log("Torre destruída");
        }

        [ContextMenu("Tomar 5 de dano")]
        private void DebugTomarDano() => TomarDano(5);
    }
}
