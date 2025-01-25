using UnityEngine;

public class WinTrigger : MonoBehaviour
{
    public static UnityEngine.Events.UnityAction<GameObject> playerWon;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (GameManager.Instance.currentGameState == GameManager.GameState.Running)
        {
            GameManager.Instance.PlayerWon(collision.gameObject);
        }
    }
}
