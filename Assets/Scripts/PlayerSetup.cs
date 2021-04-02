using UnityEngine;
using Mirror;

[RequireComponent(typeof(PlayerController))]
public class PlayerSetup : NetworkBehaviour
{
    [SerializeField]
    Behaviour[] componentsToDisable;

    [SerializeField]
    private GameObject playerUIPrefab;
    private GameObject playerUIInstance;

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
    }

    private void OnDisable()
    {
        Destroy(playerUIInstance);

        if(sceneCamera != null)
        {
            sceneCamera.gameObject.SetActive(true);
        }
    }
}
