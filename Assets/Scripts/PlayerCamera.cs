using System;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField]
    public GameObject BackgroundContainer; // For parallax
    private Vector3 BackgroundContainerStartPos;

    public GameObject[] Players;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Players = GameObject.FindGameObjectsWithTag("Player");
        BackgroundContainerStartPos = BackgroundContainer.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // Camera position
        float ySum = 0f;

        foreach(GameObject player in Players) 
        {
            ySum += player.transform.position.y;
        }

        float midpoint = ySum / Players.Length;

        transform.SetPositionAndRotation(new Vector3(transform.position.x, midpoint, transform.position.z), transform.rotation);
        
        // Background parallax
        Vector3 BGsPos = new Vector3(BackgroundContainerStartPos.x, BackgroundContainerStartPos.y + midpoint / 2f, BackgroundContainerStartPos.z);
        BackgroundContainer.transform.position = BGsPos;
    }
}
