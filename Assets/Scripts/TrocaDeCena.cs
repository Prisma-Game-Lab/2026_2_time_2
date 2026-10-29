using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TrocaDeCena : MonoBehaviour
{
    // o Unity chama isso sozinho quando o jogo começa
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void Criar()
    {
        GameObject objeto = new GameObject("TrocaDeCena");
        objeto.AddComponent<TrocaDeCena>();
        DontDestroyOnLoad(objeto);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            int proxima = (SceneManager.GetActiveScene().buildIndex + 1) % SceneManager.sceneCountInBuildSettings;
            SceneManager.LoadScene(proxima);
        }
    }
}