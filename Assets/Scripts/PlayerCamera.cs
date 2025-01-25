using System;
using System.Net;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField]
    public GameObject BackgroundContainer; // For parallax
    private Vector3 BackgroundContainerStartPos;

    public GameObject[] Players;
    private GameObject winningPlayer;

    private void OnEnable()
    {
        GameManager.playerWon += PlayerWon;
    }

    private void OnDisable()
    {
        GameManager.playerWon -= PlayerWon;
    }

// Start is called once before the first execution of Update after the MonoBehaviour is created
void Start()
    {
        Players = GameObject.FindGameObjectsWithTag("Player");
        BackgroundContainerStartPos = BackgroundContainer.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        switch (GameManager.Instance.currentGameState)
        {
            case GameManager.GameState.Running:
                FollowPlayers();
                break;
            case GameManager.GameState.Postgame:
                ZoomInOnWinningPlayer();
                break;
        }
    }

    void FollowPlayers()
    {
        // Camera position
        float ySum = 0f;

        foreach (GameObject player in Players)
        {
            ySum += player.transform.position.y;
        }

        float midpoint = ySum / Players.Length;

        transform.SetPositionAndRotation(new Vector3(transform.position.x, midpoint, transform.position.z), transform.rotation);

        // Background parallax
        Vector3 BGsPos = new Vector3(BackgroundContainerStartPos.x, BackgroundContainerStartPos.y + midpoint / 2f, BackgroundContainerStartPos.z);
        BackgroundContainer.transform.position = BGsPos;
    }

    void ZoomInOnWinningPlayer()
    {
        Vector3 newPosition = Vector3.Lerp(transform.position, winningPlayer.transform.position, Time.deltaTime * 2.0f);
        transform.SetPositionAndRotation(new Vector3(newPosition.x, newPosition.y, transform.position.z), transform.rotation);
    }

    void PlayerWon(GameObject player)
    {
        winningPlayer = player;
    }
}
