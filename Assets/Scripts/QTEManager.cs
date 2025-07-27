using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QTEManager : MonoBehaviour
{
    public static QTEManager Instance;

    [Header("UI Setttings")]
    public TextMeshProUGUI keyDisplay;
    public GameObject qtePanel;

    [Header("QTE Settings")]
    public List<KeyCode> comboSequence = new List<KeyCode>();
    private int currIndexKey;
    private bool QTEActive;
    public bool isDone;

    [SerializeField] private float timerPerKey = 2f;
    private float timer;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void StartQTE(List<KeyCode> sequence)
    {
        comboSequence = sequence;
        currIndexKey = 0;
        QTEActive = true;
        isDone = false;
        timer = timerPerKey;
        qtePanel.SetActive(true);
        ShowCurrentKey();
    }

    void Update()
    {
        if (!QTEActive) return;

        timer -= Time.deltaTime;

        if (Input.GetKeyDown(comboSequence[currIndexKey]))
        {
            Debug.Log("Correct key: " + comboSequence[currIndexKey]);
            currIndexKey++;
            timer = timerPerKey;

            if (currIndexKey >= comboSequence.Count)
            {
                isDone = true;
                EndQTE();
                return;
            }

            ShowCurrentKey();
        }
        else if (Input.anyKeyDown)
        {
            Debug.Log("Wrong key!");
            EndQTE(); // Optionally fail here
        }

        if (timer <= 0)
        {
            Debug.Log("Time out!");
            EndQTE();
        }
    }

    void ShowCurrentKey()
    {
        if (currIndexKey < comboSequence.Count)
        {
            keyDisplay.text = comboSequence[currIndexKey].ToString();
        }
    }

    void EndQTE()
    {
        Debug.Log("QTE Ended");
        QTEActive = false;
        qtePanel.SetActive(false);
    }

    public bool isActive()
    {
        return QTEActive;
    }
}
