using UnityEngine;

public class BloonHealthController : MonoBehaviour
{
    public int bloonHealth;

    // Update is called once per frame
    void Update()
    {
        if (bloonHealth == 3)
        {
            SpriteRenderer[] childRenderers = GetComponentsInChildren<SpriteRenderer>();
            foreach (SpriteRenderer sr in childRenderers)
            {
                sr.color = Color.green;
            }
        }

        else if (bloonHealth == 2)
        {
            SpriteRenderer[] childRenderers = GetComponentsInChildren<SpriteRenderer>();
            foreach (SpriteRenderer sr in childRenderers)
            {
                sr.color = Color.blue;

            }
        }

        else if (bloonHealth == 1)
        {
            SpriteRenderer[] childRenderers = GetComponentsInChildren<SpriteRenderer>();
            foreach (SpriteRenderer sr in childRenderers)
            {
                sr.color = Color.red;
            }
        }

        else if (bloonHealth == 0)
        {
            Destroy(gameObject);
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
        if (collision.gameObject.CompareTag("Bullet"))
        {
            bloonHealth -= 1;
        }


        if (collision.gameObject.CompareTag("End"))
        {
            bloonHealth = 0;
        }
    }
    public void SetBloonHealth(int health)
    {
        bloonHealth = health;
    }
    private void Damage()
    {
        bloonHealth -= 1;
    }
}

