using UnityEngine;

public class VehicleEnterExit : MonoBehaviour
{
    [Tooltip("Nearby vehicle to enter; if empty, will try to find VehicleController on this object or parent.")]
    public VehicleController vehicle;

    void Reset()
    {
        if (vehicle == null) vehicle = GetComponentInParent<VehicleController>();
    }

    void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        if (vehicle == null) vehicle = GetComponentInParent<VehicleController>();
        if (vehicle == null) return;

        // prompt is handled in-editor / UI; press E to enter
        if (Input.GetKeyDown(KeyCode.E))
        {
            vehicle.EnterVehicle(other.gameObject);
        }
    }
}
