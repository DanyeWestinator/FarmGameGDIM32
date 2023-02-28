using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : PlayerInputManager
{
    /// <summary>
    /// The UI item for the pause panel
    /// </summary>
    public GameObject pausePanel;
    /// <summary>
    /// The Text displaying the current score
    /// </summary>
    public TextMeshProUGUI scoreCounter;
    [SerializeField]
    private GameObject PlayerJoinPanel;

    [SerializeField] private List<Color> playerColors = new List<Color>();
    public void OnPlayerJoined(PlayerInput input)
    {
        input.gameObject.name = $"Player {playerCount}";
        PlayerController controller = input.GetComponent<PlayerController>();
        controller.pausePanel = pausePanel;
        controller.scoreCounter = scoreCounter;
        if (playerCount == maxPlayerCount)
        {
            PlayerJoinPanel.SetActive(false);
        }

        input.GetComponentInChildren<SpriteRenderer>().color = playerColors.GetRandom();
    }

    public void OnPlayerLeft(PlayerInput input)
    {
        if (PlayerJoinPanel)
            PlayerJoinPanel.SetActive(true);
    }
}
