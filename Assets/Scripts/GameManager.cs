using System;
using UnityEngine;
using UnityEngine.SceneManagement;
 
public enum GameState
{
    MainMenu,
    Playing,
    Paused,
    GameOver
}
 

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
 
    // Outros scripts (UI, AudioManager, Player...) se inscrevem aqui
    // em vez de o GameManager ter que conhecer cada um deles.
    public static event Action<GameState> OnGameStateChanged;
 
    [SerializeField] private GameState initialState = GameState.MainMenu;
    [SerializeField] private string mainMenuScene = "MainMenu";
 
    public GameState CurrentState { get; private set; }
 
    private void Awake()
    {
        //se já existe um GameManager, destrói o duplicado
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
 
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
 
    private void Start()
    {
        ChangeState(initialState);
    }
 
    private void Update()
    {
        // Atalho de pausa; pode ser trocado pelo Input System do projeto
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (CurrentState == GameState.Playing) ChangeState(GameState.Paused);
            else if (CurrentState == GameState.Paused) ChangeState(GameState.Playing);
        }
    }
 
    /// <summary>Ponto único por onde o estado do jogo muda.</summary>
    public void ChangeState(GameState newState)
    {
        CurrentState = newState;
 
        switch (newState)
        {
            case GameState.MainMenu:
                Time.timeScale = 1f;
                break;
            case GameState.Playing:
                Time.timeScale = 1f;
                break;
            case GameState.Paused:
                Time.timeScale = 0f; // congela física, animações e Update que usam deltaTime
                break;
            case GameState.GameOver:
                Time.timeScale = 0f;
                break;
        }
 
        OnGameStateChanged?.Invoke(newState);
    }
 
    // ---------- Controle de cenas ----------
 
    public void StartGame(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
        ChangeState(GameState.Playing);
    }
 
    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        ChangeState(GameState.Playing);
    }
 
    public void GoToMainMenu()
    {
        SceneManager.LoadScene(mainMenuScene);
        ChangeState(GameState.MainMenu);
    }
 
    public void TogglePause()
    {
        ChangeState(CurrentState == GameState.Paused ? GameState.Playing : GameState.Paused);
    }
 
    public void GameOver()
    {
        ChangeState(GameState.GameOver);
    }
 
    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}