using UnityEngine;

namespace TowerDefense
{
    // cria o mundo do TD quando a janela abre e destrói quando ela fecha
    public class TDJanela : MonoBehaviour
    {
        [SerializeField] private GameObject prefabMundo;
        [SerializeField] private Vector3 posicaoMundo = new Vector3(1000f, 0f, 0f);
        [SerializeField] private DefenderPlacer placer;

        private GameObject mundo;

        private void Awake()
        {
            // sem pai: fica fora do Canvas, longe do desktop
            mundo = Instantiate(prefabMundo, posicaoMundo, Quaternion.identity);
            placer.DefinirCamera(mundo.GetComponentInChildren<Camera>());
        }

        private void OnDestroy()
        {
            if (mundo != null) Destroy(mundo);
        }
    }
}