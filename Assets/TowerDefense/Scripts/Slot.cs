using UnityEngine;

namespace TowerDefense
{
    public class Slot : MonoBehaviour
    {
        public Defender Torre { get; private set; }

        public void Construir(Defender prefab)
        {
            if (Torre != null)
                Destroy(Torre.gameObject);

            Torre = Instantiate(prefab, transform.position, Quaternion.identity, transform);
        }
    }
}