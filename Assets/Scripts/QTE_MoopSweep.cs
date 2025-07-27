using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QTE_MoopSweep : MonoBehaviour
{
    [Header("QTE details")]
    public List<KeyCode> comboKeys = new List<KeyCode>();
    public float timerPerKey;
    private int currIndexKey;
    private float timer;
    private bool isQTEActive;
    public bool isClean;

    public void StartQTE()
    {
        comboKeys = GeneretadComboKeys(4);
        currIndexKey = 0;
        timer = timerPerKey;
        isQTEActive = true;
        isClean = false;

        QTEManager.Instance.StartQTE(comboKeys);
        Debug.Log("QTE Started! Press the correct keys in order!");
        foreach (KeyCode key in comboKeys)
        {
            Debug.Log(key);
        }
    }

    private void Update()
    {
        if (!isQTEActive) return;

        timer -= Time.deltaTime;

        if (Input.GetKeyDown(comboKeys[currIndexKey]))
        {
            Debug.Log($"Correct: {comboKeys[currIndexKey]}");
            currIndexKey++;
            timer = timerPerKey;

            if (currIndexKey >= comboKeys.Count)
            {
                onQTESuccess();
            }
        }
        else if (Input.anyKeyDown)
        {
            Debug.Log("Wrong Keycode!");
        }

        if (timer <= 0)
        {
            onQTEFail();
        }
    }

    void onQTESuccess()
    {
        isClean = true;
        isQTEActive = false;
        Debug.Log("QTE SUCCESS!");
    }

    void onQTEFail()
    {
        isQTEActive = false;
        Debug.Log("QTE FAIL!");
    }

    List<KeyCode> GeneretadComboKeys(int lengt)
    {
        KeyCode[] possibleKey = { KeyCode.A, KeyCode.D, KeyCode.S, KeyCode.W };
        List<KeyCode> combos = new List<KeyCode>();

        for (int i = 0; i < lengt; i++)
        {
            combos.Add(possibleKey[Random.Range(0, possibleKey.Length)]);
        }

        return combos;
    }

    public bool isActive()
    {
        return isQTEActive;
    }
}
