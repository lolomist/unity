using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [SerializeField]
    private Text score;

    [SerializeField]
    private Text playerName;

    private PlayerController controller;

    public void SetController(PlayerController _controller)
    {
        controller = _controller;
        DisplayName(controller.GetPlayerName());
    }

    public void Update()
    {
        SetScore(controller.GetScore());
    }

    void SetScore(int _score)
    {
        score.text = "Number of deaths: " + _score;
    }

    void DisplayName(string _playerName)
    {
        playerName.text = "You are " + _playerName;
    }
}
