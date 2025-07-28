using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

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
    public int currIndexKey;
    private bool QTEActive;
    public bool isDone;

    public float timerPerKey;
    [SerializeField]private float timer;
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

    public void StartQTE(int length)
    {
        comboSequence = GenerateComboKeys(length);
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

        if (timer <= 0)
        {
            Debug.Log("Time out!");
            EndQTE();
            return;
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

    public void ShowCurrentKey()
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

    public List<KeyCode> GenerateComboKeys(int length)
    {
        KeyCode[] possibleKeys = { KeyCode.A, KeyCode.S, KeyCode.D, KeyCode.W };
        List<KeyCode> combo = new List<KeyCode>();
        for (int i = 0; i < length; i++)
        {
            combo.Add(possibleKeys[Random.Range(0, possibleKeys.Length)]);
        }
        return combo;
    }

    public void AdvanceKey()
    {
        if (currIndexKey < comboSequence.Count)
        {
            currIndexKey++;
        }
        timer = timerPerKey;
    }

    public bool isActive()
    {
        return QTEActive;
    }
}
