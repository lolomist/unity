using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Mirror;

[RequireComponent(typeof(PlayerMotor))]
[RequireComponent(typeof(ConfigurableJoint))]
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float speed = 3f;

    [SerializeField]
    private float mouseSensitivityX = 3f;

    [SerializeField]
    private float mouseSensitivityY = 3f;

    [SerializeField]
    private Animator animator;

    [SerializeField]
    private NetworkAnimator netAnim;

    [SerializeField]
    private Transform groundCheck;

    [SerializeField]
    private float groundDistance = 0.4f;

    [SerializeField]
    private LayerMask groundMask;

    [SerializeField]
    private float jumpForce = 1000f;

    [Header("Joint Options")]
    [SerializeField]
    private float jointSpring = 20f;
    [SerializeField]
    private float jointMaxForce = 50f;

    private PlayerMotor motor;
    private ConfigurableJoint joint;
    private bool isGrounded;
    private bool isFalling;
    private int deathNumber;

    private void Start()
    {
        motor = GetComponent<PlayerMotor>();
        joint = GetComponent<ConfigurableJoint>();
        SetJointSettings(jointSpring);
        animator.SetBool("Moving", false);
        isFalling = false;
        deathNumber = 0;
    }

    private void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        // Calculer la vélocité (vitesse) du mouvement de notre joueur
        float xMov = Input.GetAxisRaw("Horizontal");
        float zMov = Input.GetAxisRaw("Vertical");

        Vector3 moveHorizontal = transform.right * xMov;
        Vector3 moveVertical = transform.forward * zMov;

        Vector3 velocity = (moveHorizontal + moveVertical).normalized * speed;

        motor.Move(velocity);

        if ((moveHorizontal + moveVertical) == Vector3.zero)
            Idle();
        else if ((moveHorizontal + moveVertical) != Vector3.zero && !Input.GetKey(KeyCode.LeftShift))
            Walk();
        else if ((moveHorizontal + moveVertical) != Vector3.zero && Input.GetKey(KeyCode.LeftShift))
            Run();
        // On calcule la rotation du joueur en un Vector3
        float yRot = Input.GetAxisRaw("Mouse X");

        Vector3 rotation = new Vector3(0, yRot, 0) * mouseSensitivityX;

        motor.Rotate(rotation);

        // On calcule la rotation de la camera en un Vector3
        float xRot = Input.GetAxisRaw("Mouse Y");

        Vector3 cameraRotation = new Vector3(xRot, 0, 0) * mouseSensitivityY;

        motor.RotateCamera(cameraRotation);

        Vector3 jumpVelocity = Vector3.zero;
        if (Input.GetButton("Jump") && isGrounded) {
            jumpVelocity = Vector3.up * jumpForce;
            Jump();
        } else
            SetJointSettings(jointSpring);

        motor.ApplyJump(jumpVelocity);

        if (joint.yDrive.positionSpring == jointSpring && !isGrounded)
            Fall();

        if (joint.yDrive.positionSpring == jointSpring && (isFalling == true && isGrounded))
            Land();

    }

    private void Idle()
    {
        animator.SetBool("Moving", false);
        animator.SetFloat("Velocity", 0);
    }
    
    private void Walk()
    {
        speed = 6f;
        animator.SetBool("Moving", true);
        animator.SetFloat("Velocity", 0.65f);
    }

    private void Run()
    {
        speed = 12f;
        animator.SetBool("Moving", true);
        animator.SetFloat("Velocity", 1f);
    }

    private void Jump()
    {
        //Debug.Log("Jumping");
        animator.SetInteger("Jumping", 1);
        netAnim.SetTrigger("Jump");
        SetJointSettings(0f);
        //motor.ApplyJump(velocity);
        //velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
    }

    private void Fall()
    {
        animator.SetInteger("Jumping", 2);
        netAnim.SetTrigger("Jump");
        isFalling = true;
    }

    private void Land()
    {
        animator.SetInteger("Jumping", 0);
        netAnim.SetTrigger("Jump");
        isFalling = false;
    }

    private void SetJointSettings(float _jointSpring)
    {
        joint.yDrive = new JointDrive { positionSpring = _jointSpring, maximumForce = jointMaxForce };
    }

    public int GetScore()
    {
        return deathNumber;
    }

    public string GetPlayerName()
    {
        return gameObject.name;
    }

    public void addDeath()
    {
        deathNumber++;
    }
}
