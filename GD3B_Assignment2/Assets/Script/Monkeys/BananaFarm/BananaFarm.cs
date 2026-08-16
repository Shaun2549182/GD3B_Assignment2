using UnityEngine;

public class BananaFarm : MonoBehaviour
{
    [SerializeField] private GameObject bananaPrefab;
    [SerializeField] private float spawnInterval = 6f;
    [SerializeField] private float spawnRadius = 1.5f; // how far from the farm a banana can land

    private float spawnTimer;

    private void Update()
    {
        spawnTimer -= Time.deltaTime;
        if (spawnTimer <= 0f)
        {
            SpawnBanana();
            spawnTimer = spawnInterval;
        }
    }

    private void SpawnBanana()
    {
        if (bananaPrefab == null)
        {
            Debug.LogWarning($"{name}: no banana prefab assigned.");
            return;
        }

        Vector2 offset = Random.insideUnitCircle * spawnRadius;
        Vector3 spawnPos = transform.position + (Vector3)offset;

        Instantiate(bananaPrefab, spawnPos, Quaternion.identity);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, spawnRadius);
    }
}