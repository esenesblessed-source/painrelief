using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public float walkSpeed = 5f;
    public float runSpeed = 8f;
    public float jumpHeight = 1.5f;
    public float gravity = -9.81f;

    [Header("Sound")]
    public float runningSoundRadius = 8f;
    public float runSoundInterval = 1f; // seconds between running sound pings

    private CharacterController controller;
    private Vector3 velocity;
    private float runSoundTimer = 0f;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // Support mobile input if available
        Vector2 moveInput = Vector2.zero;
        bool isRunningInput = Input.GetKey(KeyCode.LeftShift);
        bool jumpInput = Input.GetButtonDown("Jump");
        if (MobileInput.Instance != null)
        {
            moveInput = MobileInput.Instance.move;
            isRunningInput = MobileInput.Instance.run;
            jumpInput = MobileInput.Instance.jump;
        }

        float speed = isRunningInput ? runSpeed : walkSpeed;
        Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;
        controller.Move(move * speed * Time.deltaTime);

        // emit running sound occasionally while running
        bool isRunning = Input.GetKey(KeyCode.LeftShift) && (Mathf.Abs(Input.GetAxis("Horizontal")) > 0.1f || Mathf.Abs(Input.GetAxis("Vertical")) > 0.1f);
        if (isRunning)
        {
            runSoundTimer += Time.deltaTime;
            if (runSoundTimer >= runSoundInterval)
            {
                if (SoundManager.Instance != null) SoundManager.Instance.MakeSound(transform.position, runningSoundRadius);
                runSoundTimer = 0f;
            }
        }
        else runSoundTimer = runSoundInterval;

        if (controller.isGrounded && velocity.y < 0)
            velocity.y = -2f;

        if (jumpInput && controller.isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            if (SoundManager.Instance != null) SoundManager.Instance.MakeSound(transform.position, 4f);
            // consume mobile jump
            if (MobileInput.Instance != null) MobileInput.Instance.jump = false;
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}
