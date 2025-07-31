using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("References")]
    [SerializeField] private GameObject player;
    [SerializeField] private QTEManager qteManager;
    [SerializeField] private DialogueTrigger[] dialogueTriggers;
    [SerializeField] private int combos;
    [SerializeField] private Sprite[] spritesMoop;
    [SerializeField] private Sprite[] spritesSweep;
    private SpriteRenderer srSweep;
    private SpriteRenderer srMoop;
    public GameObject Moop;
    public GameObject Sweep;
    private UIManager uiManager;


    [Header("Status games")]
    [SerializeField] private float totalScore;
    [SerializeField] private float totalMultiplier;
    [SerializeField] private bool isGameActive;
    [SerializeField] private bool isQTETrigger;
    public float SetValueTotalScoreUI;
    public float SetValueMultiplierScoreUI;
    private bool doneSweep;
    private bool doneMoop;
    private bool doneWashing;
    public bool isSweep;
    public bool isMoop;

    [SerializeField] private bool hasPlayedIntro = false;
    [SerializeField] private bool hasPlayedEndDay = false;

    private void Awake()
    {
        if (Instance != null) Destroy(gameObject);
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        if (player == null) 
        { 
            player = GameObject.FindGameObjectWithTag("Player"); 
        }

        if (qteManager == null) qteManager = QTEManager.Instance;
        uiManager = GameObject.Find("UIManager").GetComponent<UIManager>();
    }

    private void Start()
    {
        SoundManager.instance.PlaySound2D("Wind");

        if (dialogueTriggers != null && dialogueTriggers.Length > 0 && !hasPlayedIntro)
        {
            dialogueTriggers[0].TriggerDialogue();
            hasPlayedIntro = true;
        }

        srMoop = Moop.GetComponent<SpriteRenderer>();
        srSweep = Sweep.GetComponent<SpriteRenderer>();

        if (!doneMoop && !doneSweep)
        {
            srMoop.sprite = spritesMoop[0];
            srSweep.sprite = spritesSweep[0];
        }

        isMoop = isSweep = false;
        totalScore = 0;
        totalMultiplier = 1;
    }

    private void Update()
    {

        if (!hasPlayedEndDay)
        {
            if (!doneSweep)
            {
                Debug.Log("Task Sweep the floor");
            }
            else if (!doneMoop)
            {
                Debug.Log("Task Moop the floor");
            }
            else if (totalScore < 2000)
            {
                Debug.Log("Score is last then the Requirment score");
            }
            else
            {
                dialogueTriggers[1].TriggerDialogue();
                hasPlayedEndDay = true;
            }
        }

        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
        if (LevelManager.Instance.isGameActive)
        {
            GameOn();
        }
    }

    public void SetTotalScore(float score)
    {
        totalScore += score;
        SetValueTotalScoreUI = totalScore;
        Debug.Log("Updating score UI: " + SetValueTotalScoreUI);
        uiManager.UpdateScoreUI(SetValueTotalScoreUI);
    }

    public void SetMultiplierScore(float multiplier)
    {
        totalMultiplier += multiplier;
        SetValueMultiplierScoreUI = totalMultiplier;
        uiManager.UpdateMultiplierUI(SetValueMultiplierScoreUI);
    }

    public void StartQTE()
    {
        isQTETrigger = true;
        qteManager.StartQTE(combos);
    }

    public void onQTESucces()
    {
        if (isMoop)
        {
            doneMoop = true;
            srMoop.sprite = spritesMoop[1];
        }
        else if (isSweep)
        {
            doneSweep = true;
            srSweep.sprite = spritesSweep[1];
        }
        Debug.Log("QTE is Succes!");
        isQTETrigger = false;
    }

    public void onQTEFailed()
    {
        Debug.Log("QTE Failed!");
        isQTETrigger = false;
    }

    public void GameOn()
    {
        Debug.Log("Beem call by main menu");
        player.SetActive(true);
    }

    public void GameOff()
    {

        player.SetActive(false);
        hasPlayedIntro = false;
        hasPlayedEndDay = false;
        LevelManager.Instance.isGameActive = false;
    }


    public bool isGameon()
    {
        return isGameActive;
    }
}
