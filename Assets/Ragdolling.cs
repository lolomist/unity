using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ragdolling : MonoBehaviour
{
    [SerializeField] Collider myCollider;
    public float respawnTime = 30f;
    Rigidbody[] rigidbodies;
    bool bIsRagdoll = false;


    // Start is called before the first frame update
    void Start()
    {
        rigidbodies = GetComponentsInChildren<Rigidbody>();
        ToggleRagdoll(true);
    }

    private void OnCollisionEnter (Collision collision)
    {
        if (!bIsRagdoll && collision.gameObject.tag == "Projectile")
        {
            ToggleRagdoll(false);
            StartCoroutine(GetBackUp());
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
        }
    }

    private IEnumerator GetBackUp()
    {
        yield return new WaitForSeconds(respawnTime);
        ToggleRagdoll(true);
    }

    void Animation()
    {
        Animator animator = GetComponent<Animator>();
        animator.SetBool("Moving", false);
        animator.SetFloat("Velocity", 0);
    }
}
