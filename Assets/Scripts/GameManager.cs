using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    [SerializeField] private SpriteRenderer srSweep;
    [SerializeField] private SpriteRenderer srMoop;
    public GameObject Moop;
    public GameObject Sweep;
    private UIManager uiManager;


    [Header("Status games")]
    [SerializeField] private float totalScore;
    [SerializeField] private float totalMultiplier;
    [SerializeField] private bool isGameActive;
    [SerializeField] private bool isQTETrigger;
    private float SetValueTotalScoreUI;
    private float SetValueMultiplierScoreUI;
    private bool doneSweep;
    private bool doneMoop;
    private bool doneWashing;
    public bool isSweep;
    public bool isMoop;
    public bool isWatering;

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
        uiManager.UpdateScoreUI(totalScore);
        if (Moop == null)
        {
            Moop = GameObject.Find("Mop");
        }

        if (Sweep == null)
        {
            Sweep = GameObject.Find("Broom");
            
        }

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
            if (srMoop == null)
            {
                srMoop = Moop.GetComponent<SpriteRenderer>();
            }
            doneMoop = true;
            srMoop.sprite = spritesMoop[1];
        }
        else if (isSweep)
        {
            if (srSweep == null)
            {
                srSweep = Sweep.GetComponent<SpriteRenderer>();
            }

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

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "MainGame")
        {
            uiManager = GameObject.Find("UIManager")?.GetComponent<UIManager>();
            if (uiManager != null)
            {
                uiManager.UpdateScoreUI(totalScore);
                uiManager.UpdateMultiplierUI(totalMultiplier);
            }
        }

        resetGame();
    }

    private void resetGame()
    {
        totalScore = 0;
        totalMultiplier = 1;
        doneMoop = false;
        doneSweep = false;
        doneWashing = false;
        hasPlayedIntro = false;
        hasPlayedEndDay = false;
        Debug.LogWarning("Game has been reset!");
    }
}
