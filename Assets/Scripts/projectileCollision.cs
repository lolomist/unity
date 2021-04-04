using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

[RequireComponent(typeof(PlayerSetup))]
public class projectileCollision : NetworkBehaviour
{

    private PlayerController controller;

    [SerializeField]
    private Behaviour[] disableOnDeath;
    private bool[] wasEnabledOnStart;

    [SerializeField]
    private float respawnTime = 6f;

    [SerializeField]
    private Camera cam;

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
        cam.gameObject.SetActive(false);
        GetComponent<PlayerSetup>().ActivateSceneCamera();

        for (int i = 0; i < disableOnDeath.Length; i++)
            disableOnDeath[i].enabled = false;

        Collider col = GetComponent<Collider>();
        if (col != null)
            col.enabled = false;

        transform.position = new Vector3(2000, 0, 0);

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
        GetComponent<PlayerSetup>().DeactivateSceneCamera();
        cam.gameObject.SetActive(true);

        for (int i = 0; i < disableOnDeath.Length; i++)
            disableOnDeath[i].enabled = wasEnabledOnStart[i];

        Collider col = GetComponent<Collider>();
        if (col != null)
            col.enabled = true;
    }
}
