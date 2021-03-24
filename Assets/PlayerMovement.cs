using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;

    public float speed = 12f;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;
    public int attackDelayAmount = 4;
    public float projectileSpeed = 0.00001f;
    public float shootingDelay = 0.6f;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    public BoxCollider rightHandColliderComponent;
    public BoxCollider LeftHandboxColliderComponent;
    public GameObject projectile;
    public GameObject camera;

    Vector3 velocity;
    bool isGrounded;

    private Animator anim;

    private bool isFalling;
    private bool isAttacking;
    protected float attackTimer;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();
        anim.SetBool("Moving", false);
        isFalling = false;
        isAttacking = false;
    }

    // Update is called once per frame
    void Update()
    {
        attackTimer += Time.deltaTime;

        if (attackTimer >= attackDelayAmount)
        {
            attackTimer = 0f;
            // For every DelayAmount in seconds it will reset the Attack
            isAttacking = false; 
            rightHandColliderComponent.enabled = false;
            LeftHandboxColliderComponent.enabled = false;
        }

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * speed * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            Jump();
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (isAttacking == false) {
                print("attaque");
                isAttacking = true;
                rightHandColliderComponent.enabled = true;
                LeftHandboxColliderComponent.enabled = true;
                anim.SetTrigger("Attack");
                StartCoroutine(ExecuteAfterTime(shootingDelay));
            }
        }
        else if (move == Vector3.zero)
        {
            Idle();
        }
        else if (move != Vector3.zero && !Input.GetKey(KeyCode.LeftShift))
        {
            Walk();
        }
        else if (move != Vector3.zero && Input.GetKey(KeyCode.LeftShift))
        {
            Run();
        }

        velocity.y += gravity * Time.deltaTime;

        if (velocity.y < 0 && !isGrounded)
        {
            anim.SetInteger("Jumping", 2);
            anim.SetTrigger("Jump");
            isFalling = true;
        }

        if (velocity.y < 0 && (isFalling == true && isGrounded))
        {
            anim.SetInteger("Jumping", 0);
            anim.SetTrigger("Jump");
            isFalling = false;
        }

        controller.Move(velocity * Time.deltaTime);
    }

    private void Idle()
    {
        anim.SetBool("Moving", false);
        anim.SetFloat("Velocity", 0);
    }

    private void Walk()
    {
        speed = 6f;
        anim.SetBool("Moving", true);
        anim.SetFloat("Velocity", 0.65f);
    }

    private void Run()
    {
        speed = 12f;
        anim.SetBool("Moving", true);
        anim.SetFloat("Velocity", 1f);
    }

    private void Jump()
    {
        anim.SetInteger("Jumping", 1);
        anim.SetTrigger("Jump");
        velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
    }

    IEnumerator ExecuteAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
 
        // Code to execute after the delay
        Vector3 spawnPosition = this.transform.position + (this.transform.forward * 4);
        spawnPosition.y += 2f;
        GameObject projectil = Instantiate(projectile, spawnPosition, this.transform.rotation) as GameObject;
        projectil.GetComponent<Rigidbody>().AddForce(camera.transform.forward * projectileSpeed);
    }
}
