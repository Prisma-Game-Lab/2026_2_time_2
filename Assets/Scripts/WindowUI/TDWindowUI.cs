using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class TDWindowUI : MonoBehaviour
{
    [Header("Configuração da Cena")]
    [SerializeField] private string tdSceneName = "TD_Test";

    [Header("Interface")]
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private Button closeButton;

    private AsyncOperation sceneLoadOperation;

    void Start()
    {
        if (titleText != null) titleText.text = "TD_Test";

        if (closeButton != null)
        {
            closeButton.onClick.RemoveAllListeners();
            closeButton.onClick.AddListener(CloseWindow);
        }

        Scene targetScene = SceneManager.GetSceneByName(tdSceneName);
        if (!targetScene.isLoaded)
        {
            StartCoroutine(LoadTDSceneRoutine());
        }
    }

    private IEnumerator LoadTDSceneRoutine()
    {
        sceneLoadOperation = SceneManager.LoadSceneAsync(tdSceneName, LoadSceneMode.Additive);
        yield return sceneLoadOperation;
    }

    public void CloseWindow()
    {
        Scene targetScene = SceneManager.GetSceneByName(tdSceneName);
        if (targetScene.isLoaded)
        {
            SceneManager.UnloadSceneAsync(tdSceneName);
        }

        Destroy(gameObject);
    }
}