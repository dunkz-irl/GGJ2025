using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button play1Player;
    [SerializeField] private Button play2Player;
    [SerializeField] private Button play3Player;
    [SerializeField] private Button play4Player;

    private void OnEnable()
    {
        play1Player.onClick.AddListener(() => UIManager.Instance.OnButtonClicked(UIManager.MyButton.play));
        play2Player.onClick.AddListener(() => UIManager.Instance.OnButtonClicked(UIManager.MyButton.play));
        play3Player.onClick.AddListener(() => UIManager.Instance.OnButtonClicked(UIManager.MyButton.play));
        play4Player.onClick.AddListener(() => UIManager.Instance.OnButtonClicked(UIManager.MyButton.play));

        play1Player.onClick.AddListener(() => GameManager.Instance.numPlayers = 1);
        play2Player.onClick.AddListener(() => GameManager.Instance.numPlayers = 2);
        play3Player.onClick.AddListener(() => GameManager.Instance.numPlayers = 3);
        play4Player.onClick.AddListener(() => GameManager.Instance.numPlayers = 4);
    }

    private void Start()
    {

    }

    private void OnDisable()
    {
        play1Player.onClick.RemoveAllListeners();
        play2Player.onClick.RemoveAllListeners();
        play3Player.onClick.RemoveAllListeners();
        play4Player.onClick.RemoveAllListeners();
    }
}
