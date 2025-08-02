using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("Referece")]
    [SerializeField] private TextMeshProUGUI valueScore;
    [SerializeField] private TextMeshProUGUI valueMultiplier;
    [SerializeField] private TextMeshProUGUI DayCount;
    [SerializeField] private TextMeshProUGUI timerText;

    public void UpdateScoreUI(float score)
    {
        valueScore.text = score.ToString();
    }

    public void UpdateMultiplierUI(float multiplier)
    {
        valueMultiplier.text = $"X {multiplier:F1}";
    }

    public void UpdayCount()
    {
        DayCount.text = GameManager.Instance.dayCount.ToString();
    }

    public void UpdateTime(float timeLeft)
    {
        int minutes = Mathf.FloorToInt(timeLeft / 60);
        int seconds = Mathf.FloorToInt(timeLeft % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
