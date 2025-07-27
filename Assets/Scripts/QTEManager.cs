using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QTEManager : MonoBehaviour
{
    public static QTEManager Instance;

    [Header("UI Settings")]
    private TextMeshProUGUI keyDisplay;
    public GameObject qtePanel;
    public GameObject qteKeyPrefab;
    private Canvas canvas;
    private Transform playerTransform;

    [Header("QTE Settings")]
    public List<KeyCode> comboSequence = new List<KeyCode>();
    private int currIndexKey;
    private bool QTEActive;
    public bool isDone;

    [SerializeField] private float timerPerKey = 2f;
    private float timer;
    private List<GameObject> activeKeyUIs = new List<GameObject>();
    private List<GameObject> keyUIPool = new List<GameObject>();

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        canvas = FindObjectOfType<Canvas>();
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null) playerTransform = player.transform;
    }

    public void StartQTE(List<KeyCode> sequence)
    {
        comboSequence = sequence;
        currIndexKey = 0;
        QTEActive = true;
        isDone = false;
        timer = timerPerKey;

        GameObject keyUI = GetOrCreateKeyUI();
        keyUI.SetActive(true);
        activeKeyUIs.Add(keyUI);

        UI_QTE_Follow followScript = keyUI.GetComponent<UI_QTE_Follow>();
        followScript.Initialize(playerTransform, canvas);

        keyDisplay = keyUI.GetComponentInChildren<TextMeshProUGUI>();
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
    private GameObject GetOrCreateKeyUI()
    {
        foreach (GameObject obj in keyUIPool)
        {
            if (!obj.activeInHierarchy)
                return obj;
        }

        GameObject newKeyUI = Instantiate(qteKeyPrefab, canvas.transform);
        keyUIPool.Add(newKeyUI);
        return newKeyUI;
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
        foreach (var obj in activeKeyUIs)
        {
           obj.SetActive(false);
        }

        activeKeyUIs.Clear();
    }


    public bool isActive()
    {
        return QTEActive;
    }
}
