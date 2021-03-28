using UnityEngine;
using Mirror;

[RequireComponent(typeof(PlayerMotor))]
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

    private PlayerMotor motor;

    private void Start()
    {
        motor = GetComponent<PlayerMotor>();
        animator.SetBool("Moving", false);
    }

    private void Update()
    {
        // Calculer la vélocité (vitesse) du mouvement de notre joueur
        float xMov = Input.GetAxisRaw("Horizontal");
        float zMov = Input.GetAxisRaw("Vertical");

        Vector3 moveHorizontal = transform.right * xMov;
        Vector3 moveVertical = transform.forward * zMov;

        Vector3 velocity = (moveHorizontal + moveVertical).normalized * speed;

        motor.Move(velocity);

        if ((moveHorizontal + moveVertical) == Vector3.zero)
            Idle();
        if ((moveHorizontal + moveVertical) != Vector3.zero && !Input.GetKey(KeyCode.LeftShift))
            Walk();
        // On calcule la rotation du joueur en un Vector3
        float yRot = Input.GetAxisRaw("Mouse X");

        Vector3 rotation = new Vector3(0, yRot, 0) * mouseSensitivityX;

        motor.Rotate(rotation);

        // On calcule la rotation de la camera en un Vector3
        float xRot = Input.GetAxisRaw("Mouse Y");

        Vector3 cameraRotation = new Vector3(xRot, 0, 0) * mouseSensitivityY;

        motor.RotateCamera(cameraRotation);
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
}
