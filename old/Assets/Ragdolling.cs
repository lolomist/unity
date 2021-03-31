using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Ragdolling : MonoBehaviour
{
    [SerializeField] Collider myCollider;
    public float respawnTime = 30f;
    Rigidbody[] rigidbodies;
    public GameObject player;
    private GameObject winText;
    bool bIsRagdoll = false;


    // Start is called before the first frame update
    void Start()
    {
        rigidbodies = GetComponentsInChildren<Rigidbody>();
        ToggleRagdoll(true);
        winText = GameObject.Find("WinText");
        // Debug.Log("Here the player tag before collision: " + player.tag);
    }

    private void OnCollisionEnter (Collision collision)
    {
        if (!bIsRagdoll && collision.gameObject.tag == "Projectile")
        {
            ToggleRagdoll(false);

            player.gameObject.tag = "Untagged";
            if (winText.activeInHierarchy == true) { 
                StartCoroutine(GetBackUp());
            }
        }
    }

    private void ToggleRagdoll (bool bisAnimating)
    {
        bIsRagdoll = !bisAnimating;

        myCollider.enabled = bisAnimating;
        foreach (Rigidbody ragdollBone in rigidbodies)
        {
            ragdollBone.isKinematic = bisAnimating;
        }

        GetComponent<Animator>().enabled = bisAnimating;
        if (bisAnimating)
        {
            Animation();

           // Debug.Log("Here the player tag after collision: " + player.tag);
        }
    }

    private IEnumerator GetBackUp()
    {
        yield return new WaitForSeconds(respawnTime);
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        //Application.LoadLevel(0);
        ToggleRagdoll(true);

        player.gameObject.tag = "Player";
    }

    void Animation()
    {
        Animator animator = GetComponent<Animator>();
        animator.SetBool("Moving", false);
        animator.SetFloat("Velocity", 0);
    }
}
