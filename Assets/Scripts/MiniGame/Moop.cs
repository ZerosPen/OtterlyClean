using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moop : MonoBehaviour
{
    [Header("QTE details")]
    private float timerPerKey;
    private float timer;
    [SerializeField] private bool isQTEActive;
    public bool isClean;
    public float baseScore;
    [SerializeField] private float scoreToAdd;
    [SerializeField] private Animator playerAnimator;

    public static Moop Instance;
    void Awake()
    {
        if (Instance != null) Destroy(gameObject);
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void StartQTE()
    {
        scoreToAdd = 0;
        timerPerKey = QTEManager.Instance.timerPerKey;
        timer = timerPerKey;
        isQTEActive = true;
        Debug.Log($"isQTEActive set to {isQTEActive}");
        isClean = false;

        if (playerAnimator == null)
        {
            playerAnimator = GetComponent<Animator>();
            if (playerAnimator == null)
            {
                Debug.LogWarning("Animator not found on Sweep GameObject.");
            }
        }

        Debug.Log("QTE Started! Press the correct keys in order!");
        foreach (KeyCode key in QTEManager.Instance.comboSequence)
        {
            Debug.Log(key);
        }
        QTEManager.Instance.ShowCurrentKey();
    }

    private void Update()
    {

        if (!isQTEActive) return;
        if (!QTEManager.Instance.isActive()) return;

        timer -= Time.deltaTime;

        if (Input.anyKeyDown && QTEManager.Instance.currIndexKey < QTEManager.Instance.comboSequence.Count)
        {
            bool correct = Input.GetKeyDown(QTEManager.Instance.comboSequence[QTEManager.Instance.currIndexKey]);
            bool isLastKey = QTEManager.Instance.currIndexKey == QTEManager.Instance.comboSequence.Count - 1;

            if (correct)
            { 
                timerPerKey = QTEManager.Instance.timerPerKey;
                scoreToAdd += baseScore;

                QTEManager.Instance.AdvanceKey();
                timer = timerPerKey;

                if (isLastKey)
                {
                    onQTESuccess();
                }
                else
                {
                    QTEManager.Instance.ShowCurrentKey();
                }
            }
            else
            {
                Debug.Log("Wrong key!");
            }
        }
    }

    void onQTESuccess()
    {
        isClean = true;
        isQTEActive = false;

        if (playerAnimator != null)
        {
            playerAnimator.Play("Sweeping");
        }

        Player player = GameObject.FindWithTag("Player")?.GetComponent<Player>();
        if (player != null)
        {
            float multi = 0.5f;
            player.AddMultiplier(multi);
            scoreToAdd = scoreToAdd * player.multiplierScore;
            player.AddScore(scoreToAdd);
        }

        QTEManager.Instance.EndQTE();
        GameManager.Instance.onQTESucces();
        Debug.Log("QTE SUCCESS!");
    }

    void onQTEFail()
    {
        isQTEActive = false;
        QTEManager.Instance.EndQTE();
        GameManager.Instance.onQTEFailed();
        Debug.Log("QTE FAIL!");
    }

    public bool isActive()
    {
        return isQTEActive;
    }
}
