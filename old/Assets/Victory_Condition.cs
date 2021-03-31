using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Victory_Condition : MonoBehaviour
{
    public GameObject winText;
    bool victory = false;
    // Start is called before the first frame update
    void Start()
    {


        winText.SetActive(false);
       

    }

    // Update is called once per frame
    void Update()
    {
        if (GameObject.FindGameObjectsWithTag("Player").Length > 1)
        {
            victory = true;
        }
        if (GameObject.FindGameObjectsWithTag("Player").Length < 2 && GameObject.FindGameObjectsWithTag("Player").Length > 0 && victory == true)
        {
            // Do something
            winText.SetActive(true);
        }
    }
}
