using UnityEngine;
using System.Collections.Generic;
using Mirror;

[RequireComponent(typeof(PlayerController))]
public class PlayerSetup : NetworkBehaviour
{
    [SerializeField]
    Behaviour[] componentsToDisable;

    [SerializeField]
    private GameObject playerUIPrefab;
    private GameObject playerUIInstance;

    private Color c;
    [SerializeField]
    private List<Color> TintColors;

    Camera sceneCamera;

    private void Start()
    {
        if (!isLocalPlayer)
        {
            // On va boucler sur les différents composants renseignés et les désactiver si ce joueur n'est pas le notre
            for (int i = 0; i < componentsToDisable.Length; i++)
            {
                componentsToDisable[i].enabled = false;
            }
        }
        else
        {
            sceneCamera = Camera.main;
            if(sceneCamera != null)
            {
                sceneCamera.gameObject.SetActive(false);
                Cursor.lockState = CursorLockMode.Locked;
            }

            // UI du joueur
            playerUIInstance = Instantiate(playerUIPrefab);
            PlayerUI ui = playerUIInstance.GetComponent<PlayerUI>();
            if (ui == null)
                Debug.LogError("Pas de component PlayerUI sur playerUIInstance");
            else
                ui.SetController(GetComponent<PlayerController>());
        }

        GetComponent<projectileCollision>().Setup();
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        GameObject[] objects = GameObject.FindGameObjectsWithTag("Player");

        string netId = objects.Length.ToString();
        PlayerController player = GetComponent<PlayerController>();
        c = TintColors[objects.Length - 1];
        GameManager.RegisterPlayer(netId, player, c);
    }

    private void OnDisable()
    {
        Destroy(playerUIInstance);

        if(sceneCamera != null)
        {
            sceneCamera.gameObject.SetActive(true);
        }

        GameManager.UnregisterPlayer(transform.name);
    }

    public void ActivateSceneCamera()
    {
        if(sceneCamera != null)
        {
            sceneCamera.gameObject.SetActive(true);
        }
    }

    public void DeactivateSceneCamera()
    {
        if(sceneCamera != null)
        {
            sceneCamera.gameObject.SetActive(false);
        }
    }
}
