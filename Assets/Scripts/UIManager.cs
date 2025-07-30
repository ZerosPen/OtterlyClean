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

    private void Awake()
    {
        if (Instance != null) Destroy(gameObject);
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void UpdateScoreUI(float score)
    {
        valueScore.text = score.ToString();
    }

    public void UpdateMultiplierUI(float multiplier)
    {
        valueMultiplier.text = multiplier.ToString();
    }
}
