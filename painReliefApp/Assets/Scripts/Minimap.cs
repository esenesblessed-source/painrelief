using UnityEngine;

public class Minimap : MonoBehaviour
{
    public Transform player;
    public Vector3 offset = new Vector3(0f, 50f, 0f);

    void LateUpdate()
    {
        if (player == null)
        {
            var p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) player = p.transform;
        }
        if (player == null) return;
        transform.position = player.position + offset;
        transform.rotation = Quaternion.Euler(90f, player.eulerAngles.y, 0f);
    }
}
