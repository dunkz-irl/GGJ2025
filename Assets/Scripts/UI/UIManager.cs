using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    public static UnityEngine.Events.UnityAction buttonClicked;

    private GameObject backgroundPanel;
    private GameObject pauseMenu;
    private GameObject optionsMenu;
    private GameObject gameOverMenu;
    private GameManager gameManager;

    public enum MyButton
    {
        play,
        options,
        quit,
        pause,
        resume,
        mainMenu,
        back,
        restart,
        mute,
    }

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        GameManager.gameStateChanged += OnGameStateChanged;
        SceneManager.sceneLoaded += OnSceneChanged;
        GameManager.playerWon += PlayerWon;
    }

    private void OnDisable()
    {
        GameManager.gameStateChanged -= OnGameStateChanged;
        SceneManager.sceneLoaded -= OnSceneChanged;
    }

    private void Start()
    {
        //backgroundPanel = transform.Find("Canvas/BackgroundPanel").gameObject;
        //pauseMenu = transform.Find("Canvas/SafeAreaPanel/PauseMenu").gameObject;
        //optionsMenu = transform.Find("Canvas/SafeAreaPanel/OptionsMenu").gameObject;
        gameOverMenu = transform.Find("Canvas/GameOverMenu").gameObject;
        gameManager = GameManager.Instance;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (optionsMenu.activeInHierarchy)
                OnButtonClicked(MyButton.back);
            else if (pauseMenu.activeInHierarchy)
                OnButtonClicked(MyButton.resume);
            else if (gameManager.currentGameState == GameManager.GameState.Running)
                OnButtonClicked(MyButton.pause);
        }
    }

    private void OnGameStateChanged(GameManager.GameState newGameState)
    {
        //pauseMenu.SetActive(newGameState == GameManager.GameState.Paused);
        //backgroundPanel.SetActive(newGameState == GameManager.GameState.Paused);
    }

    private void OnSceneChanged(Scene newScene, LoadSceneMode unused)
    {

    }

    public void OnButtonClicked(MyButton button)
    {
        buttonClicked?.Invoke();    //If the event has at least one subscriber invoke the event

        switch (button)
        {
            case MyButton.play:
                StartCoroutine(gameManager.StartGame());
                break;
            case MyButton.restart:
                gameOverMenu.SetActive(false);
                StartCoroutine(gameManager.StartGame());
                break;
        }
    }

    void PlayerWon(GameObject player)
    {
        gameOverMenu.SetActive(true);
        gameOverMenu.GetComponent<GameOverMenu>().PlayerWon(player);
    }
}
