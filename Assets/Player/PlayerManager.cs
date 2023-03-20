using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : PlayerInputManager
{
    /// <summary>
    /// The Score manager
    /// </summary>
    public ScoreKeeper scoreKeeper;
    [SerializeField]
    private GameObject PlayerJoinPanel;

    [SerializeField] private List<Color> playerColors = new List<Color>();
    public static PlayerManager instance;
    public HashSet<PlayerController> players = new HashSet<PlayerController>();

    private void Awake()
    {
        instance = this;
    }

    [SerializeField] private Sprite[] playerSprites;
    public void OnPlayerJoined(PlayerInput input)
    {
        input.gameObject.name = $"Player {playerCount}";
        PlayerController controller = input.GetComponent<PlayerController>();
        controller.scoreKeeper = scoreKeeper;
        if (playerCount == maxPlayerCount)
        {
            PlayerJoinPanel.SetActive(false);
            Destroy(AIPlayerBehavior.aiPlayer.gameObject);
        }

        players.Add(controller);
        //input.GetComponentInChildren<SpriteRenderer>().color = playerColors.GetRandom();
        input.GetComponentInChildren<SpriteRenderer>().sprite = playerSprites[UnityEngine.Random.Range(0, playerSprites.Length)];
    }

    public void OnPlayerLeft(PlayerInput input)
    {
        if (PlayerJoinPanel)
            PlayerJoinPanel.SetActive(true);
    }
}
