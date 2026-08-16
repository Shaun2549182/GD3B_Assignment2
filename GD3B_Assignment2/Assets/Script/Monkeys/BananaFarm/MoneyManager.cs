using UnityEngine;
using TMPro;

public class MoneyManager : MonoBehaviour
{
    public static MoneyManager Instance { get; private set; }

    [SerializeField] private int startingMoney = 650;
    [SerializeField] private TMP_Text moneyText;

    public int Money { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        Money = startingMoney;
        UpdateUI();
    }

    public void AddMoney(int amount)
    {
        Money += amount;
        UpdateUI();
    }

    public bool TrySpendMoney(int amount)
    {
        if (amount > Money)
        {
            return false;
        }

        Money -= amount;
        UpdateUI();
        return true;
    }

    private void UpdateUI()
    {
        if (moneyText != null)
        {
            moneyText.text = $"${Money}";
        }
    }
}