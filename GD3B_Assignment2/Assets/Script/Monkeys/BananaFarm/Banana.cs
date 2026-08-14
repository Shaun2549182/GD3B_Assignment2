using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Banana : MonoBehaviour
{
    [SerializeField] private int value = 20;
    [SerializeField] private float lifeTime = 15f; // seconds before it rots away uncollected

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void OnMouseDown()
    {
        Collect();
    }

    private void Collect()
    {
        if (MoneyManager.Instance != null)
        {
            MoneyManager.Instance.AddMoney(value);
        }
        else
        {
            Debug.LogWarning($"{name}: no MoneyManager in the scene, banana collected but no money was added.");
        }

        Destroy(gameObject);
    }
}