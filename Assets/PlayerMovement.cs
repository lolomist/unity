using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;

    public float speed = 12f;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    Vector3 velocity;
    bool isGrounded;

    private Animator anim;

    private bool isFalling;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();
        anim.SetBool("Moving", false);
        isFalling = false;
    }

    // Update is called once per frame
    void Update()
    {
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

        if (move == Vector3.zero)
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
}
