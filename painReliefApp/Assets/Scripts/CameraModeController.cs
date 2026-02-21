using UnityEngine;

public class CameraModeController : MonoBehaviour
{
    public SimpleCameraController cameraController;
    public KeyCode toggleKey = KeyCode.V;
    public bool startInFirstPerson = true;

    void Start()
    {
        if (cameraController == null)
            cameraController = Camera.main.GetComponent<SimpleCameraController>();

        if (cameraController != null)
            cameraController.SetThirdPerson(!startInFirstPerson);
    }

    void Update()
    {
        if (Input.GetKeyDown(toggleKey))
        {
            if (cameraController != null)
                cameraController.SetThirdPerson(!cameraController.isThirdPerson);
        }
    }
}
