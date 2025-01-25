using System;
using System.Net;
using UnityEngine;
using UnityEngine.Rendering;
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
        float yMin = float.MaxValue;
        float yMax = float.MinValue;

        foreach (GameObject player in Players)
        {
            if (player.transform.position.y < yMin)
            {
                yMin = player.transform.position.y;
            }
            if (player.transform.position.y > yMax)
            {
                yMax = player.transform.position.y;
            }
        }

        float yDiff = yMax - yMin;

        // Zoom out to keep all players in view
        Camera.main.orthographicSize = Math.Clamp(yDiff, 6f, 10f);

        float midpoint = Math.Max(2f, yMin + yDiff / 2);

        transform.SetPositionAndRotation(new Vector3(transform.position.x, midpoint, transform.position.z), transform.rotation);

        // Background parallax
        Vector3 BGsPos = new Vector3(BackgroundContainerStartPos.x, BackgroundContainerStartPos.y + midpoint / 2f, BackgroundContainerStartPos.z);
        BackgroundContainer.transform.position = BGsPos;
    }

    void ZoomInOnWinningPlayer()
    {
        Vector3 newPosition = winningPlayer.transform.position;
        newPosition.z = transform.position.z;
        transform.position = newPosition;
    }

    void PlayerWon(GameObject player)
    {
        winningPlayer = player;
        Camera.main.orthographicSize = 6f;
    }
}
