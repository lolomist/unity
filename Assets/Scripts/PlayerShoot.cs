using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Mirror;


public class PlayerShoot : NetworkBehaviour
{

    [SerializeField]
    private Camera cam;

    [SerializeField]
    private NetworkAnimator netAnim;

    [SerializeField]
    private int attackDelayAmount = 4;

    [Header("Attack projectile options")]
    [SerializeField]
    private float projectileSpeed = 0.0001f;
    [SerializeField]
    private float shootingDelay = 0.6f;
    [SerializeField]
    private GameObject projectile;

    [Header("Hands colliders Options")]
    [SerializeField]
    private BoxCollider rightHandColliderComponent;
    [SerializeField]
    private BoxCollider LeftHandboxColliderComponent;

    private bool isAttacking;
    protected float attackTimer;

    void Start()
    {
        if(cam == null)
        {
            Debug.LogError("Pas de caméra renseignée sur le système de tir.");
            this.enabled = false;
        }
        isAttacking = false;
    }

    private void Update()
    {
        attackTimer += Time.deltaTime;
        if (attackTimer >= attackDelayAmount) {
            attackTimer = 0f;
            isAttacking = false; 
            rightHandColliderComponent.enabled = false;
            LeftHandboxColliderComponent.enabled = false;
        }

        if (Input.GetMouseButtonDown(0)) {
            if (isAttacking == false)
                Attack();
        }        
    }

    IEnumerator ExecuteAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
 
        CmdOnHit();
    }

    [Command]
    void CmdOnHit()
    {
        RpcDoHitEffect();
    }

    [ClientRpc]
    void RpcDoHitEffect()
    {
        Vector3 spawnPosition = transform.position + (transform.forward * 4);
        spawnPosition.y += 2f;
        GameObject projectil = Instantiate(projectile, spawnPosition, transform.rotation);
        projectil.GetComponent<Rigidbody>().AddForce(cam.transform.forward * projectileSpeed);
    }

    private void Attack()
    {
        if(!isLocalPlayer) {
            return;
        }

        isAttacking = true;
        rightHandColliderComponent.enabled = true;
        LeftHandboxColliderComponent.enabled = true;
        netAnim.SetTrigger("Attack");
        StartCoroutine(ExecuteAfterTime(shootingDelay)); 
    }
}