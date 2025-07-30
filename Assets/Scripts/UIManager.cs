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

    
    public void ReassignUIReferences()
    {
        valueScore = GameObject.Find("scoreValue")?.GetComponent<TextMeshProUGUI>();
        valueMultiplier = GameObject.Find("multiplierValue")?.GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        ReassignUIReferences();
    }

    private void Update()
    {
        if (valueScore != null)
            valueScore.text = $"{GameManager.Instance.totalScore:F0}";

        if (valueMultiplier != null)
            valueMultiplier.text = $"X {GameManager.Instance.totalMultiplier:F1}";
    }
}
