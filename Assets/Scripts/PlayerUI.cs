using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [SerializeField]
    private Text score;

    private PlayerController controller;

    public void SetController(PlayerController _controller)
    {
        controller = _controller;
    }

    public void Update()
    {
        SetScore(controller.GetScore());
    }

    void SetScore(int _score)
    {
        score.text = "Number of deaths: " + _score;
    }
}
