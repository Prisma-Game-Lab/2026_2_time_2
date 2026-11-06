using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TowerDefense
{
    // efeito de impacto: some quando a animação termina
    public class Impact : MonoBehaviour
    {
        [SerializeField] private float duracao = 0.67f;

        private void Start()
        {
            Destroy(gameObject, duracao);
        }
    }
}