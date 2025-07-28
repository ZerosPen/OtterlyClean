using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("Referece")]
    public TextMeshProUGUI valueScore;
    public TextMeshProUGUI valueMultiplier;

    private void Awake()
    {
        if (Instance != null) Destroy(gameObject);
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Update()
    {
        valueScore.text = $"{GameManager.Instance.totalScore:F0}";
        valueMultiplier.text = $"X {GameManager.Instance.totalMultiplier:F1}";
    }
}
