using UnityEngine;
using UnityEngine.AI;

public class ZombieSpawner : MonoBehaviour
{
    public GameObject zombiePrefab;
    public float spawnInterval = 3f;
    public float spawnRadiusMin = 30f;
    public float spawnRadiusMax = 120f;

    private float spawnTimer = 0f;
    private Transform player;

    void Start()
    {
        var p = GameObject.FindGameObjectWithTag("Player");
        if (p != null) player = p.transform;
    }

    void Update()
    {
        if (zombiePrefab == null || player == null) return;

        spawnTimer += Time.deltaTime;
        if (spawnTimer >= spawnInterval)
        {
            spawnTimer = 0f;
            Vector3 spawnPos;
            if (FindSpawnPosition(out spawnPos))
            {
                Instantiate(zombiePrefab, spawnPos, Quaternion.identity);
            }
        }
    }

    bool FindSpawnPosition(out Vector3 result)
    {
        for (int i = 0; i < 12; i++)
        {
            float ang = Random.Range(0f, Mathf.PI * 2f);
            float rad = Random.Range(spawnRadiusMin, spawnRadiusMax);
            Vector3 pos = player.position + new Vector3(Mathf.Cos(ang), 0f, Mathf.Sin(ang)) * rad;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(pos, out hit, 12f, NavMesh.AllAreas))
            {
                // ensure reasonably far from player
                if (Vector3.Distance(hit.position, player.position) > spawnRadiusMin - 2f)
                {
                    result = hit.position;
                    return true;
                }
            }
        }
        result = Vector3.zero;
        return false;
    }
}
