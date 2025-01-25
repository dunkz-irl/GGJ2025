using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public static UnityEngine.Events.UnityAction<GameState> gameStateChanged;
    public static UnityEngine.Events.UnityAction<GameObject> playerWon;

    public enum GameState
    {
        //Pregame,
        Running,
        Paused,
        Postgame
    }

    public GameState currentGameState { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }

    private void ChangeGameState(GameState newGameState)
    {
        gameStateChanged?.Invoke(newGameState);
        currentGameState = newGameState;
    }

    private void LoadScene(string sceneName)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }

    public IEnumerator StartGame()
    {
        ChangeGameState(GameState.Running);
        LoadScene("Main");
        yield return null;  //Wait for the next frame to ensure the scene has loaded
    }

    public void ReturnToMainMenu()
    {
        //ChangeGameState(GameState.Pregame);
        LoadScene("MainMenu");
    }

    public void PlayerWon(GameObject player)
    {
        ChangeGameState(GameState.Postgame);
        playerWon?.Invoke(player);
    }
}
