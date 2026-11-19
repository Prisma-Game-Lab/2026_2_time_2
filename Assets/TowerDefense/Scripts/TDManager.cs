using System;
using UnityEngine;

namespace TowerDefense
{
    // regras do minigame: por enquanto, o dinheiro
    public class TDManager : MonoBehaviour
    {
        public static TDManager Instance { get; private set; }

        // a hotbar escuta isso pra escurecer o que não dá pra comprar
        public static event Action<int> DinheiroMudou;

        [SerializeField] private int dinheiroInicial = 100;
        [SerializeField] private ContadorNumero contadorDinheiro;

        public int Dinheiro { get; private set; }

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            MudarDinheiro(dinheiroInicial);
        }

        public bool PodePagar(int valor) => Dinheiro >= valor;

        // tenta pagar; devolve false se não tiver dinheiro
        public bool Gastar(int valor)
        {
            if (!PodePagar(valor)) return false;

            MudarDinheiro(Dinheiro - valor);
            return true;
        }

        public void Ganhar(int valor)
        {
            MudarDinheiro(Dinheiro + valor);
        }

        private void MudarDinheiro(int valor)
        {
            Dinheiro = valor;
            contadorDinheiro.Mostrar(Dinheiro);
            DinheiroMudou?.Invoke(Dinheiro);
        }
    }
}