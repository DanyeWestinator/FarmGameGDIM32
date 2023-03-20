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
    /// The Score manager
    /// </summary>
    public ScoreKeeper scoreKeeper;
    [SerializeField]
    private GameObject PlayerJoinPanel;

    [SerializeField] private List<Color> playerColors = new List<Color>();
    [SerializeField] private Sprite[] playerSprites;
    public void OnPlayerJoined(PlayerInput input)
    {
        input.gameObject.name = $"Player {playerCount}";
        PlayerController controller = input.GetComponent<PlayerController>();
        controller.pausePanel = pausePanel;
        controller.scoreKeeper = scoreKeeper;
        if (playerCount == maxPlayerCount)
        {
            PlayerJoinPanel.SetActive(false);
        }

        //input.GetComponentInChildren<SpriteRenderer>().color = playerColors.GetRandom();
        input.GetComponentInChildren<SpriteRenderer>().sprite = playerSprites[Random.Range(0, playerSprites.Length)];
    }

    public void OnPlayerLeft(PlayerInput input)
    {
        if (PlayerJoinPanel)
            PlayerJoinPanel.SetActive(true);
    }
}
