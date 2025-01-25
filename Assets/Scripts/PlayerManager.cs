using UnityEngine;
using static GameManager;
using static UnityEditor.Experimental.GraphView.GraphView;
using UnityEngine.SceneManagement;

public class NewMonoBehaviourScript : MonoBehaviour
{
    [SerializeField] private GameObject[] players;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneChanged;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneChanged;
    }

    private void OnSceneChanged(Scene newScene, LoadSceneMode unused)
    {
        int index = 0;
        foreach (GameObject player in players)
        {
            if (index < GameManager.Instance.numPlayers)
            {
                player.SetActive(true);
            }
            else
            {
                player.SetActive(false);
            }
            index++;
        }
    }

}
