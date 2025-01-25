using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    public static UnityEngine.Events.UnityAction buttonClicked;

    [SerializeField] private GameObject backgroundPanel;
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject gameOverMenu;

    private GameManager gameManager;

    public enum MyButton
    {
        play,
        quit,
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
        gameManager = GameManager.Instance;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {

        }
    }

    private void OnGameStateChanged(GameManager.GameState newGameState)
    {

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
