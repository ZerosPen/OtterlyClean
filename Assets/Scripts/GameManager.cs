using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("References")]
    public GameObject player;
    public QTEManager qteManager;
    public int combos;


    [Header("Status games")]
    public float totalScore;
    public float totalMultiplier;
    public bool isGameActive;
    public bool isQTETrigger;

    private void Awake()
    {
        if (Instance != null) Destroy(gameObject);
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        if (player == null) player = GameObject.FindGameObjectWithTag("Player");
        if (qteManager == null) qteManager = QTEManager.Instance;
    }


    private void Update()
    {
        if (totalScore >= 900)
        {
            Debug.Log("The misson for today is done!");
            Debug.Log("You can go home!");
            Debug.Log("Press F to end the day!");
        }
    }

    public void StartQTE()
    {
        isQTETrigger = true;

        qteManager.StartQTE(combos);

        QTE_MoopSweep moopSweep = player.GetComponent<QTE_MoopSweep>();
        if(moopSweep != null)
        {
            moopSweep.StartQTE();
        }
    }

    public void onQTESucces()
    {
        Debug.Log("QTE is Succes!");
        isQTETrigger = false;
    }

    public void onQTEFailed()
    {
        Debug.Log("QTE Failed!");
        isQTETrigger = false;
    }
}
