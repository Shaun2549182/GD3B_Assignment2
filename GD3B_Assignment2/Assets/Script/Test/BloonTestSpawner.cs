using UnityEngine;

public class BloonTestSpawner : MonoBehaviour
{
    // just a test class that spawns a bloon every n secs
    [SerializeField] private GameObject bloonPrefab;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private float spawnInterval = 0.5f;
    [SerializeField] private bool spawnImmediatelyOnStart = true;

    private float spawnTimer;

    private void Start()
    {
        spawnTimer = spawnImmediatelyOnStart ? 0f : spawnInterval;
    }

    private void Update()
    {
        spawnTimer -= Time.deltaTime;
        if (spawnTimer <= 0f)
        {
            Spawn();
            spawnTimer = spawnInterval;
        }
    }

    private void Spawn()
    {
        if (bloonPrefab == null)
        {
            Debug.LogWarning($"{name}: no bloon prefab assigned.");
            return;
        }

        Vector3 pos = spawnPoint != null ? spawnPoint.position : transform.position;
        Instantiate(bloonPrefab, pos, Quaternion.identity);
    }
}