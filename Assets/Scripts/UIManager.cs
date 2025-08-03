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
    [SerializeField] private GameObject panelDialogue;
    [SerializeField] private TextMeshProUGUI[] task;

    private void Update()
    {
        if (DialogueManager.Instance.isDialogueActive)
        {
            panelDialogue.SetActive(true);
        }
        else
        {
            panelDialogue.SetActive(false);
        }
    }
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
        DayCount.text = $"DAY {GameManager.Instance.dayCount.ToString()}";
    }

    public void UpdateTime(float timeLeft)
    {
        int minutes = Mathf.FloorToInt(timeLeft / 60);
        int seconds = Mathf.FloorToInt(timeLeft % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void UpdateTask(float todayScore)
    {
        if (GameManager.Instance.doneWatering)
        {
            task[0].text = "done watering the plant";
        }
        else
        {
            task[0].text = "watering the plant";
        }

        if (GameManager.Instance.doneSweep)
        {
            task[1].text = "done sweeeping";
        }
        else
        {
            task[1].text = "sweep";
        }

        if (GameManager.Instance.doneMoop)
        {
            task[2].text = "done mop";
        }
        else
        {
            task[2].text = "mop";
        }

        if (GameManager.Instance.doneSweep)
        {
            task[1].text = "done sweeeping";
        }
        else
        {
            task[1].text = "sweeep";
        }
        if (GameManager.Instance.doneWashing)
        {
            task[3].text = "done washing";
        }
        else
        {
            task[3].text = $"wash plater";
        }
        if (GameManager.Instance.totalScore > todayScore)
        {
            task[4].text = "score is pass!";
        }
        else
        {
            task[4].text = $"score today : {todayScore}";
        }
    }
}
