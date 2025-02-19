using UnityEngine;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour
{
    [SerializeField] private InputManager inputManager;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Transform playerModel;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpForce = 7f;
    [SerializeField] private float dashForce = 30f; 
    [SerializeField] private float dashDuration = 0.2f;
    [SerializeField] private float dashCooldown = 1f;

    private Rigidbody rb;
    private bool isGrounded;
    private bool canDash = true;
    private bool isDashing = false; // Prevent movement during dash
    private int jumpCount = 0;
    private const int maxJumpCount = 2;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        inputManager.OnMove.AddListener(MovePlayer);
        inputManager.OnJump.AddListener(Jump);
        inputManager.OnDash.AddListener(Dash);
    }

    private void FixedUpdate()
    {
        if (!isGrounded && !isDashing)
        {
            rb.AddForce(Vector3.down * 10f, ForceMode.Acceleration);
        }
    }

    private void MovePlayer(Vector2 input)
    {
        if (isDashing) return; // No movement during dash

        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        forward.y = 0;
        right.y = 0;
        forward.Normalize();
        right.Normalize();

        Vector3 moveDirection = (forward * input.y + right * input.x).normalized;
        rb.linearVelocity = new Vector3(moveDirection.x * speed, rb.linearVelocity.y, moveDirection.z * speed);
    }

    private void Jump()
    {
        if (isGrounded || jumpCount < maxJumpCount)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce, rb.linearVelocity.z);
            jumpCount++;
            isGrounded = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            jumpCount = 0;
        }
    }

    private void Dash()
    {
        if (!canDash) return;

        canDash = false;
        isDashing = true;

        Vector3 dashDirection = cameraTransform.forward;
        dashDirection.y = 0;
        dashDirection.Normalize();

        if (dashDirection.magnitude < 0.1f)
        {
            dashDirection = transform.forward; // Default to player forward if no input
        }

        rb.AddForce(dashDirection * dashForce, ForceMode.Impulse); // Use Impulse for strong effect

        Invoke(nameof(ResetDash), dashDuration);
        Invoke(nameof(EnableDash), dashCooldown);
    }

    private void ResetDash()
    {
        isDashing = false;
        rb.linearVelocity = new Vector3(0, rb.linearVelocity.y, 0); // Stop horizontal movement
    }

    private void EnableDash()
    {
        canDash = true;
    }

    private void Update()
    {
        RotatePlayerModel();
    }

    private void RotatePlayerModel()
    {
        // Get the camera's y-rotation and apply it to the PlayerModel's rotation
        float targetAngle = cameraTransform.eulerAngles.y;
        playerModel.rotation = Quaternion.Euler(0f, targetAngle, 0f);
    }

}
