using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMotor : MonoBehaviour
{
    public Camera cam;

    [SerializeField]
    private GameObject projectile;

    private Vector3 velocity;
    private Vector3 rotation;
    private Vector3 cameraRotation;
    private Vector3 jumpVelocity;

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Move(Vector3 _velocity)
    {
        velocity = _velocity;
    }

    public void Rotate(Vector3 _rotation)
    {
        rotation = _rotation;
    }

    public void RotateCamera(Vector3 _cameraRotation)
    {
        cameraRotation = _cameraRotation;
    }

    public void ApplyJump(Vector3 _jumpVelocity)
    {
        jumpVelocity = _jumpVelocity;
    }

    private void FixedUpdate()
    {
        PerformMovement();
        PerformRotation();
    }

    private void PerformMovement()
    {
        if(velocity != Vector3.zero)
        {
            rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
        }

        if (jumpVelocity != Vector3.zero)
        {
            rb.AddForce(jumpVelocity * Time.fixedDeltaTime, ForceMode.Acceleration);
        }
    }

    private void PerformRotation()
    {
        rb.MoveRotation(rb.rotation * Quaternion.Euler(rotation));
        cam.transform.Rotate(-cameraRotation);
    }
}
