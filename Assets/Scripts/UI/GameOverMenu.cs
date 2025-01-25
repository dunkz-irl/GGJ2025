using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverMenu : MonoBehaviour
{
    [SerializeField] private Button rematchButton;
    [SerializeField] private TMP_Text playerWonText;

    private void OnEnable()
    {
        rematchButton.onClick.AddListener(() => UIManager.Instance.OnButtonClicked(UIManager.MyButton.restart));
    }

    public void PlayerWon(GameObject player)
    {
        playerWonText.text = player.name.ToString() + " Won!";
        playerWonText.color = player.GetComponent<SpriteRenderer>().color;
    }

    private void OnDisable()
    {
        rematchButton.onClick.RemoveAllListeners();
    }
}
