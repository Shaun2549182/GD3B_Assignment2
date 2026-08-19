using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerHealthController : MonoBehaviour
{
    [SerializeField] private int health = 3;
    public TextMeshProUGUI healthText;

    private bool isGameOver = false;

    private void Start()
    {
        if (healthText != null)
        {
            healthText.text = health.ToString();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isGameOver) return;

        if (other.CompareTag("Bloon"))
        {
            health -= 1;
            Debug.Log("Bloon Reached End!");

            if (healthText != null)
            {
                healthText.text = health.ToString();
            }

            if (health <= 0)
            {
                isGameOver = true;
                StartCoroutine(HandleGameOver());
            }
        }   
    }

    private IEnumerator HandleGameOver()
    {
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(1.5f);
        Time.timeScale = 1f; // Reset timescale before scene load
        SceneManager.LoadScene("Game Over");
    }
}