using UnityEngine;

public class BloonHealthController : MonoBehaviour
{
    [SerializeField] private int bloonHealth = 3;

    private SpriteRenderer[] childRenderers;

    private void Awake()
    {
        childRenderers = GetComponentsInChildren<SpriteRenderer>();
    }

    private void Start()
    {
        UpdateVisuals();
    }

    public void TakeDamage(int damageAmount)
    {
        bloonHealth -= damageAmount;

        if (bloonHealth <= 0)
        {
            if (MoneyManager.Instance != null)
            {
                MoneyManager.Instance.AddMoney(4);
            }

            Destroy(gameObject);
        }
        else
        {
            UpdateVisuals();
        }
    }

    public void SetBloonHealth(int health)
    {
        bloonHealth = health;
        UpdateVisuals();
    }

    private void UpdateVisuals()
    {
        if (childRenderers == null) return;

        Color targetColor = Color.white;
        if (bloonHealth == 3) targetColor = Color.green;
        else if (bloonHealth == 2) targetColor = Color.blue;
        else if (bloonHealth == 1) targetColor = Color.red;

        foreach (SpriteRenderer sr in childRenderers)
        {
            if (sr != null) sr.color = targetColor;
        }
    }

    private void OnDestroy()
    {
        if (BloonSpawnerScript.Instance != null)
        {
            BloonSpawnerScript.Instance.OnBloonsDestroyed();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            Dart dart = collision.GetComponentInParent<Dart>();
            int damageToTake = (dart != null) ? Mathf.RoundToInt(dart.Damage) : 1;
            TakeDamage(damageToTake);
        }
        else if (collision.CompareTag("End"))
        {
            TakeDamage(bloonHealth);
        }
    }
}