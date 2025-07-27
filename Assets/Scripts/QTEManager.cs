using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QTEManager : MonoBehaviour
{
    public static QTEManager Instance;

    [Header("UI Setttings")]
    public TextMeshProUGUI keyDisplay;
    public GameObject qtePanel;
    public GameObject qteKeyPrefab;
    public Canvas canvas;
    public Transform playerTransform; 

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

        GameObject keyUI = Instantiate(qteKeyPrefab, canvas.transform);
        UI_QTE_Follow followScript = keyUI.GetComponent<UI_QTE_Follow>();

        followScript.playerTransform = playerTransform;
        followScript.canvas = canvas;

        keyDisplay = keyUI.GetComponent<TextMeshProUGUI>();

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

        UI_QTE_Follow[] uiFollowers = FindObjectsOfType<UI_QTE_Follow>();
        foreach (var follower in uiFollowers)
        {
            Destroy(follower.gameObject);
        }
    }

    public bool isActive()
    {
        return QTEActive;
    }
}
