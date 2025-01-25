using UnityEngine;

public class WinTrigger : MonoBehaviour
{
    public static UnityEngine.Events.UnityAction<GameObject> playerWon;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameManager.Instance.PlayerWon(collision.gameObject);
    }
}
