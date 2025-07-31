using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class QTEManager : MonoBehaviour
{
    public static QTEManager Instance;

    [Header("UI Settings")]
    private TextMeshProUGUI keyDisplay;
    [SerializeField] private GameObject qtePanel;
    [SerializeField] private GameObject qteKeyPrefab;
    [SerializeField] private Canvas canvas;
    private Transform playerTransform;

    [Header("QTE Settings")]
    public List<KeyCode> comboSequence = new List<KeyCode>();
    public int currIndexKey;
    [SerializeField] private bool QTEActive;
    [SerializeField] private bool isDone;
    public QTE_MoopSweep moopSwep;

    public float timerPerKey;
    [SerializeField] private float timer;
    private List<GameObject> activeKeyUIs = new List<GameObject>();
    private List<GameObject> keyUIPool = new List<GameObject>();

    void Awake()
    {
        if (Instance != null) Destroy(gameObject);
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }


        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null) playerTransform = player.transform;
    }

    public void StartQTE(int length)
    {
        if (canvas == null)
        {
            GameObject canvasObj = GameObject.FindGameObjectWithTag("GameCanvas");
            if (canvasObj != null)
            {
                canvas = canvasObj.GetComponent<Canvas>();
            }
            else
            {
                Debug.LogWarning("Tagged Canvas not found.");
            }
        }

        QTE_MoopSweep.Instance.StartQTE();
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

    public void EndQTE()
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
        if (currIndexKey < comboSequence.Count - 1)
        {
            currIndexKey++;
        }
        else
        {
            Debug.LogWarning("Already at last index, won't advance!");
        }
        timer = timerPerKey;
    }

    public bool isActive()
    {
        return QTEActive;
    }
}
