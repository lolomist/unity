using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class projectileCollision : NetworkBehaviour
{

    private PlayerController controller;

    [SerializeField]
    private Behaviour[] disableOnDeath;
    private bool[] wasEnabledOnStart;

    [SerializeField]
    private float respawnTime = 6f;

    // Start is called before the first frame update
    void Awake()
    {
        controller = GetComponent<PlayerController>();
    }


    public void Setup()
    {
        wasEnabledOnStart = new bool[disableOnDeath.Length];
        for (int i = 0; i < disableOnDeath.Length; i++)
            wasEnabledOnStart[i] = disableOnDeath[i].enabled;
        
        RespawnPlayer();
    }

    private void OnCollisionEnter (Collision collision)
    {
        if (collision.gameObject.tag == "Projectile")
        {
            //ToggleRagdoll(false);

            //player.gameObject.tag = "Untagged";
            Debug.Log("joueur touché!");
            controller.addDeath();
            KillPlayer();
            // if (winText.activeInHierarchy == true) { 
            //     StartCoroutine(GetBackUp());
            // }
        }
    }

    private void KillPlayer()
    {
        for (int i = 0; i < disableOnDeath.Length; i++)
            disableOnDeath[i].enabled = false;

        Collider col = GetComponent<Collider>();
        if (col != null)
            col.enabled = false;

        StartCoroutine(RespawnDelay());
    }

    private IEnumerator RespawnDelay()
    {
        yield return new WaitForSeconds(respawnTime);

        RespawnPlayer();
        Transform spawnPoint = NetworkManager.singleton.GetStartPosition();
        transform.position = spawnPoint.position;
        transform.rotation = spawnPoint.rotation;
    }

    public void RespawnPlayer()
    {
        for (int i = 0; i < disableOnDeath.Length; i++)
            disableOnDeath[i].enabled = wasEnabledOnStart[i];

        Collider col = GetComponent<Collider>();
        if (col != null)
            col.enabled = true;
    }
}
