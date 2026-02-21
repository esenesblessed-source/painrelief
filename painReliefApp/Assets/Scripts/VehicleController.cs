using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class VehicleController : MonoBehaviour
{
    public float maxTorque = 2000f;
    public float maxSteerAngle = 35f;
    public float brakeForce = 3000f;
    public float maxSpeed = 40f;
    public Transform exitPoint;

    private Rigidbody rb;
    private GameObject driver;
    private bool hasDriver = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb != null) rb.centerOfMass = new Vector3(0f, -0.5f, 0f);
    }

    void Update()
    {
        if (!hasDriver) return;

        // Basic input mapping: WASD / arrows steer and throttle
        float steer = Input.GetAxis("Horizontal") * maxSteerAngle;
        float throttle = Input.GetAxis("Vertical");

        // Simple forward force
        Vector3 forwardForce = transform.forward * throttle * maxTorque * Time.deltaTime;
        rb.AddForce(forwardForce, ForceMode.Acceleration);

        // steering via rotation
        transform.Rotate(0f, steer * Time.deltaTime, 0f);

        // braking
        if (Input.GetKey(KeyCode.Space))
        {
            rb.AddForce(-rb.velocity * brakeForce * Time.deltaTime, ForceMode.Acceleration);
        }

        // limit speed
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        if (flatVel.magnitude > maxSpeed)
        {
            Vector3 limited = flatVel.normalized * maxSpeed;
            rb.velocity = new Vector3(limited.x, rb.velocity.y, limited.z);
        }

        // exit
        if (Input.GetKeyDown(KeyCode.E))
        {
            ExitVehicle();
        }
    }

    public void EnterVehicle(GameObject player)
    {
        if (hasDriver) return;
        driver = player;
        hasDriver = true;
        // hide player and disable its CharacterController if present
        var cc = driver.GetComponent<CharacterController>();
        if (cc != null) cc.enabled = false;
        driver.SetActive(false);

        // attach main camera to vehicle for simple follow
        var cam = Camera.main;
        if (cam != null)
        {
            cam.transform.SetParent(transform);
            cam.transform.localPosition = new Vector3(0f, 1.6f, -5f);
            cam.transform.LookAt(transform.position + Vector3.up * 1.2f);
        }
    }

    public void ExitVehicle()
    {
        if (!hasDriver) return;
        Vector3 spawnPos = transform.position + transform.right * 2f + Vector3.up * 1f;
        if (exitPoint != null) spawnPos = exitPoint.position;

        driver.SetActive(true);
        driver.transform.position = spawnPos;
        var cc = driver.GetComponent<CharacterController>();
        if (cc != null) cc.enabled = true;

        // detach camera
        var cam = Camera.main;
        if (cam != null) cam.transform.SetParent(null);

        driver = null;
        hasDriver = false;
    }
}
