using System.Collections.Generic;
using UnityEngine;

/*
    this class tells the dart monkey where and in which direction to shoot, it then summons the dart prefab
*/
public class DartMonkey : MonoBehaviour
{
    [Header("Targeting")]
    [SerializeField] private string targetTag = "Bloon";
    [SerializeField] private float range = 32f;

    [Header("Shooting")]
    [SerializeField] private GameObject dartPrefab;
    [SerializeField] private Transform firePoint; // point at which darts spawn (can leave blank to use monke transform)
    [SerializeField] private float attackCooldown = 0.95f;

    [Header("Rotation")]
    [SerializeField] private bool rotateTowardsTarget = true;
    [SerializeField] private float rotationSpeed = 720f;

    private CircleCollider2D rangeCollider;

    private readonly List<GameObject> targetsInRange = new List<GameObject>();

    private float fireCooldown;

    private void Awake()
    {
        rangeCollider = GetComponent<CircleCollider2D>();
        if (rangeCollider != null)
        {
            rangeCollider.isTrigger = true;
            rangeCollider.radius = range;
        }
    }

    private void Update()
    {
        targetsInRange.RemoveAll(b => b == null);

        GameObject target = GetFirstTarget();
        if (target == null)
        {
            return;
        }

        if (rotateTowardsTarget)
        {
            AimAt(target.transform);
        }

        fireCooldown -= Time.deltaTime;
        if (fireCooldown <= 0f)
        {
            Shoot(target);
            fireCooldown = attackCooldown;
        }
    }

    //finds the bloon which has travelled the most amount of distance (the furthest along the path)
    private GameObject GetFirstTarget()
    {
        if (targetsInRange.Count > 0)
        {
            return targetsInRange[0];
        }

        return null;
    }

    private void AimAt(Transform target)
    {
        Vector3 dir = target.position - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        Quaternion targetRot = Quaternion.Euler(0f, 0f, angle);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRot, rotationSpeed * Time.deltaTime);
    }

    private void Shoot(GameObject target)
    {
        if (dartPrefab == null) return;

        Vector3 spawnPos = firePoint != null ? firePoint.position : transform.position;

        Vector3 direction = target.transform.position - spawnPos;

        GameObject dartGO = Instantiate(dartPrefab, spawnPos, Quaternion.identity);
        Dart dart = dartGO.GetComponent<Dart>();
        if (dart != null)
        {
            dart.Fire(direction);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag(targetTag))
        {
            return;
        }

        GameObject bloon = other.gameObject;
        if (bloon != null && !targetsInRange.Contains(bloon))
        {
            targetsInRange.Add(bloon);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag(targetTag))
        {
            return;
        }

        GameObject bloon = other.gameObject;
        if (bloon != null)
        {
            targetsInRange.Remove(bloon);
        }
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
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    } //debug to help find range
}