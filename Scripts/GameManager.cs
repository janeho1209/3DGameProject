using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Score Tracking")]
    public int totalScore = 0;
    public int customersServed = 0;
    public float averageRating = 0f;

    [Header("UI")]
    public TMP_Text scoreText;
    public TMP_Text ratingText;
    public TMP_Text customersServedText;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddScore(int score)
    {
        totalScore += score;
        customersServed++;

        averageRating = (float)totalScore / customersServed;

        UpdateUI();

        Debug.Log($"📊 Score: {totalScore} | Customers: {customersServed} | Avg Rating: {averageRating:F2}/3.0");
    }

    void UpdateUI()
    {
        if (scoreText != null)
            scoreText.text = $"Score: {totalScore}";

        if (ratingText != null)
            ratingText.text = $"Rating: {averageRating:F2}/3.0 *";

        if (customersServedText != null)
            customersServedText.text = $"Customers Served: {customersServed}";
    }
}