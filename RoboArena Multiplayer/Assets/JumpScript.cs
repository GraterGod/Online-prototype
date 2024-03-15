using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Realtime;
using Photon.Pun;
public class JumpScript : MonoBehaviour
{
    public float jumpForce; // The force applied when jumping
    public float gravityScale; // Gravity scale to adjust the gravity effect
    public int maxJumps = 2; // Maximum number of jumps allowed
    private int jumpCount = 0; // Current jump count
    public InputActionReference jumpAction; // Reference to the jump Input Action
    private Rigidbody rb; // Reference to the Rigidbody component

    public bool Grounded;
    public Animator jumpAnim;

    PhotonView view;


    private void Start()
    {
        // Get the Rigidbody component
        rb = GetComponent<Rigidbody>();

        // Enable the Input Action
        jumpAction.action.Enable();

        view = GetComponent<PhotonView>();

    }

    private void OnDisable()
    {
        // Disable the Input Action
        jumpAction.action.Disable();
    }

    private void Update()
    {
        if (view.IsMine)
        {
            // Check if the jump button is pressed
            if (jumpAction.action.triggered)
            {
                Grounded = false;
                jumpAnim.SetBool("Jump", true);
                JumpAction();
            }

            if (Grounded)
            {
                jumpAnim.SetBool("Jump", false);

            }

            if (jumpAction.action.triggered && Grounded == false)
            {
                jumpAnim.SetBool("DoubleJump", true);
            }
            else
            {
                jumpAnim.SetBool("DoubleJump", false);

            }
        }

    }

    private void JumpAction()
    {
        // Check if the player can still jump (hasn't reached the maximum jumps)
        if (jumpCount < maxJumps)
        {
            // Add vertical force to make the player jump
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

            // Increment jump count
            jumpCount++;
        }
    }

    private void FixedUpdate()
    {
        if (view.IsMine)
        {
            // Apply custom gravity scale
            rb.AddForce(Physics.gravity * gravityScale, ForceMode.Acceleration);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Reset jump count when landing on the ground
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("CantThrowGround"))
        {
            jumpCount = 0;
            Grounded = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        // Reset jump count when landing on the ground
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("CantThrowGround"))
        {
            Grounded = false;
        }
    }
}
