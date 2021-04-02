using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectileCollision : MonoBehaviour
{

    private PlayerController controller;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<PlayerController>();
    }

    private void OnCollisionEnter (Collision collision)
    {
        if (collision.gameObject.tag == "Projectile")
        {
            //ToggleRagdoll(false);

            //player.gameObject.tag = "Untagged";
            Debug.Log("joueur touché!");
            controller.addDeath();
            // if (winText.activeInHierarchy == true) { 
            //     StartCoroutine(GetBackUp());
            // }
        }
    }
}
