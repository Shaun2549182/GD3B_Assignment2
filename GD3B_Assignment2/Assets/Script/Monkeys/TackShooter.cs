using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class TackShooter : MonoBehaviour
{
    [Header("Detection")]
    [SerializeField] private string targetTag = "Bloon";
    [SerializeField] private float range = 23f;

    [Header("Shooting")]
    [SerializeField] private GameObject tackPrefab;
    [SerializeField] private float attackCooldown = 1.12f; // seconds between volleys
    [SerializeField] private int tackCount = 8;
    [Tooltip("This is for art alignment, it doesnt actually rotate")]
    [SerializeField] private float rotationOffset = 0f;

    private CircleCollider2D rangeCollider;

    // we only need to know IF something's in range, not which bloon or where, it just shoots
    private readonly HashSet<Collider2D> collidersInRange = new HashSet<Collider2D>();

    private float fireCooldown;

    private void Awake()
    {
        rangeCollider = GetComponent<CircleCollider2D>();
        rangeCollider.isTrigger = true;
        rangeCollider.radius = range;
    }

    private void Update()
    {
        collidersInRange.RemoveWhere(c => c == null);

        if (collidersInRange.Count == 0)
        {
            return;
        }

        fireCooldown -= Time.deltaTime;
        if (fireCooldown <= 0f)
        {
            FireVolley();
            fireCooldown = attackCooldown;
        }
    }

    private void FireVolley()
    {
        if (tackPrefab == null) return;

        if (tackCount <= 0)
        {
            return;
        }

        float angleStep = 360f / tackCount;

        for (int i = 0; i < tackCount; i++)
        {
            float angle = rotationOffset + (angleStep * i);
            Vector3 direction = Quaternion.Euler(0f, 0f, angle) * Vector3.right;

            GameObject tackGO = Instantiate(tackPrefab, transform.position, Quaternion.identity);
            Dart tack = tackGO.GetComponent<Dart>();
            if (tack != null)
            {
                tack.Fire(direction);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag(targetTag))
        {
            return;
        }

        collidersInRange.Add(other);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag(targetTag))
        {
            return;
        }

        collidersInRange.Remove(other);
    }

    private void OnValidate()
    {
        if (rangeCollider == null)
        {
            rangeCollider = GetComponent<CircleCollider2D>();
        }
        if (rangeCollider != null)
        {
            rangeCollider.radius = range;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}