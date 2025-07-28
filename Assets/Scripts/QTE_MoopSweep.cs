using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QTE_MoopSweep : MonoBehaviour
{
    [Header("QTE details")]
    private float timerPerKey;
    private float timer;
    private bool isQTEActive;
    public bool isClean;
    public float baseScore;
    [SerializeField]private float scoreToAdd;

    public void StartQTE()
    {
        QTEManager.Instance.StartQTE(4);
        scoreToAdd = 0;
        timerPerKey = QTEManager.Instance.timerPerKey;
        timer = timerPerKey;
        isQTEActive = true;
        isClean = false;

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

        if (Input.anyKeyDown)
        {
            if (Input.GetKeyDown(QTEManager.Instance.comboSequence[QTEManager.Instance.currIndexKey]))
            {
                Debug.Log($"Correct: {QTEManager.Instance.comboSequence[QTEManager.Instance.currIndexKey]}");
                scoreToAdd += baseScore;
            }
            else
            {
                Debug.Log("Wrong key!");
            }

            QTEManager.Instance.AdvanceKey();
            timer = timerPerKey;

            if (QTEManager.Instance.currIndexKey < QTEManager.Instance.comboSequence.Count)
            {
                QTEManager.Instance.ShowCurrentKey();
            }
            else
            {
                onQTESuccess();
                return;
            }
        }


        if (timer <= 0 || QTEManager.Instance.currIndexKey >= QTEManager.Instance.comboSequence.Count)
        {
            onQTEFail();
        }
    }

    void onQTESuccess()
    {
        isClean = true;
        isQTEActive = false;

        Player player = GameObject.FindWithTag("Player")?.GetComponent<Player>();
        if (player != null)
        {
            scoreToAdd *= player.multiplierScore;
            player.AddScore(scoreToAdd);
        }

        Debug.Log("QTE SUCCESS!");
    }

    void onQTEFail()
    {
        isQTEActive = false;
        Debug.Log("QTE FAIL!");
    }

    public bool isActive()
    {
        return isQTEActive;
    }
}
