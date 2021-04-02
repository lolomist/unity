using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class WinUI : MonoBehaviour
{
    [SerializeField]
    private Text text;

    private PlayerController controller;


    public void SetController(PlayerController _controller)
    {
        controller = _controller;
    }

    public void DisplayWinner(Dictionary<string, PlayerController> winners)
    {
        if (winners.Count == 1) {
            text.text = "The winner is " + winners.Keys.ElementAt(0) + " with only " + winners.Values.ElementAt(0).GetScore() + " deaths!";
        }
        else {
            text.text = "The winners are ";
            for (int i = 0; i < winners.Count; i++) {
                text.text += winners.Keys.ElementAt(i) + " / ";
            }
            text.text += " with only " + winners.Values.ElementAt(0).GetScore() + " deaths!";
        }
    }
}