using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button playButton;

    private void OnEnable()
    {
        playButton.onClick.AddListener(() => UIManager.Instance.OnButtonClicked(UIManager.MyButton.play));
    }

    private void Start()
    {

    }

    private void OnDisable()
    {
        playButton.onClick.RemoveAllListeners();
    }
}
