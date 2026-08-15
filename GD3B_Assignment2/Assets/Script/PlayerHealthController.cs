using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;

public class PlayerHealthController : MonoBehaviour
{
    int health = 999;
    public TextMeshProUGUI healthText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        healthText.text = "999";
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bloon"))
        {
        health -= 1;
        Debug.Log("Bloon Reached End!");
        healthText.text = $"{health}";
        }   
    }
}
