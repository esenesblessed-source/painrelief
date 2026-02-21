using UnityEngine;

public class SimpleCameraController : MonoBehaviour
{
    public float mouseSensitivity = 2f;
    public Transform player;

    [Tooltip("Enable third-person mode when true.")]
    public bool isThirdPerson = false;
    [Tooltip("Offset from player when in third-person mode.")]
    public Vector3 thirdPersonOffset = new Vector3(0f, 1.8f, -3.5f);
    [Tooltip("Distance smoothing speed when switching modes.")]
    public float smoothSpeed = 8f;

    private float xRotation = 0f;
    private Vector3 currentOffset;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        currentOffset = thirdPersonOffset;
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        if (!isThirdPerson)
        {
            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            if (player != null) player.Rotate(Vector3.up * mouseX);
        }
        else
        {
            // orbit around player
            if (player != null)
            {
                player.Rotate(Vector3.up * mouseX);
                xRotation -= mouseY;
                xRotation = Mathf.Clamp(xRotation, -40f, 60f);

                Quaternion rot = Quaternion.Euler(xRotation, player.eulerAngles.y, 0f);
                currentOffset = Vector3.Lerp(currentOffset, thirdPersonOffset, Time.deltaTime * smoothSpeed);
                Vector3 desiredPos = player.position + rot * currentOffset;
                transform.position = Vector3.Lerp(transform.position, desiredPos, Time.deltaTime * smoothSpeed);
                transform.LookAt(player.position + Vector3.up * 1.2f);
            }
        }
    }

    // Allow external toggling of camera mode
    public void SetThirdPerson(bool third)
    {
        isThirdPerson = third;
        if (!isThirdPerson)
        {
            // snap camera to player's head for first-person
            transform.position = player.position + Vector3.up * 1.6f;
            transform.localRotation = Quaternion.identity;
            xRotation = 0f;
        }
        else
        {
            // position camera behind player
            transform.position = player.position + player.rotation * thirdPersonOffset;
            transform.LookAt(player.position + Vector3.up * 1.2f);
            xRotation = transform.eulerAngles.x;
        }
    }
}
