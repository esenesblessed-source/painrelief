using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public int width = 10;
    public int depth = 10;
    public float tileSize = 2f;
    public Material floorMaterial;
    public GameObject enemyPrefab;
    public int enemyCount = 5;

    void Start()
    {
        GenerateFloor();
        SpawnEnemies();
    }

    void GenerateFloor()
    {
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < depth; z++)
            {
                GameObject tile = GameObject.CreatePrimitive(PrimitiveType.Cube);
                tile.transform.localScale = new Vector3(tileSize, 0.2f, tileSize);
                tile.transform.position = new Vector3(x * tileSize, -0.1f, z * tileSize);
                var rend = tile.GetComponent<Renderer>();
                if (floorMaterial != null) rend.material = floorMaterial;
                else rend.material.color = ((x + z) % 2 == 0) ? Color.gray : Color.white;
                tile.name = $"Tile_{x}_{z}";
            }
        }
    }

    void SpawnEnemies()
    {
        if (enemyPrefab == null) return;
        for (int i = 0; i < enemyCount; i++)
        {
            float rx = Random.Range(0, width) * tileSize;
            float rz = Random.Range(0, depth) * tileSize;
            Vector3 pos = new Vector3(rx, 0.5f, rz);
            Instantiate(enemyPrefab, pos, Quaternion.identity);
        }
    }

    public Vector3 GetSpawnPosition()
    {
        return new Vector3(tileSize * (width / 2f), 1f, tileSize * (depth / 2f));
    }
}
