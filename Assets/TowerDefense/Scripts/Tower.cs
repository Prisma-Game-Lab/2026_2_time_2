using UnityEngine;

namespace TowerDefense
{
    public class Tower : MonoBehaviour
    {
        [SerializeField] private int vida = 20;

        public void TomarDano(int quantidade)
        {
            if (vida <= 0) return;

            vida -= quantidade;
            Debug.Log($"Torre: {vida} de vida");

            if (vida <= 0)
                Debug.Log("Torre destruída");
        }

        [ContextMenu("Tomar 5 de dano")]
        private void DebugTomarDano() => TomarDano(5);
    }
}
