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
    [SerializeField] private Sprite[] spritesWindowgarden;
    [SerializeField] private SpriteRenderer srSweep;
    [SerializeField] private SpriteRenderer srMoop;
    [SerializeField] private SpriteRenderer srWindowgarden;
    [SerializeField] private Animator bearAnimator;
    public GameObject Moop;
    public GameObject Sweep;
    public GameObject Windowgarden;
    private UIManager uiManager;


    [Header("Status games")]
    [SerializeField] private float totalScore;
    [SerializeField] private float totalMultiplier;
    [SerializeField] private bool isGameActive;
    [SerializeField] private bool isQTETrigger;
    private float SetValueTotalScoreUI;
    private float SetValueMultiplierScoreUI;
    public int dayCount;
    [SerializeField] private bool doneSweep;
    [SerializeField] private bool doneMoop;
    public bool exit;
    public bool doneWatering;
    public bool isSweep;
    public bool isMoop;
    public bool isWatering;
    private bool isFailed;

    [Header("Day Timer")]
    [SerializeField] private float dayTimeLimit = 120f;
    private float currentDayTimer;
    private bool isTimerRunning;

    [SerializeField] private bool hasPlayedIntro = false;
    public bool hasPlayedEndDay = false;

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
        bearAnimator = GameObject.Find("Bear").GetComponent<Animator>();
    }

    private void Start()
    {
        if (bearAnimator != null)
        {
            bearAnimator.Play("Sleep");
        } 

        if (dialogueTriggers != null && dialogueTriggers.Length > 0 && !hasPlayedIntro)
        {
            dialogueTriggers[0].TriggerDialogue();
            hasPlayedIntro = true;
        }

        srMoop = Moop.GetComponent<SpriteRenderer>();
        srSweep = Sweep.GetComponent<SpriteRenderer>();
        

        if (!doneMoop && !doneSweep && !doneWatering)
        {
            srMoop.sprite = spritesMoop[0];
            srSweep.sprite = spritesSweep[0];
            srWindowgarden.sprite = spritesWindowgarden[0];
        }
        TryFindSpriteReferences();
        StartDayTimer();
        isMoop = isSweep = doneSweep = doneMoop = doneWatering = false;
        totalScore = 0;
        totalMultiplier = 1;
    }

    private void TryFindSpriteReferences()
    {
        Moop = GameObject.Find("Moop");
        if (Moop != null)
        {
            srMoop = Moop.GetComponent<SpriteRenderer>();
        }

        Sweep = GameObject.Find("Sweep");
        if (Sweep != null)
        {
            srSweep = Sweep.GetComponent<SpriteRenderer>();
        }

        Windowgarden = GameObject.Find("WindowGarden");
        if (Windowgarden != null)
        {
            srWindowgarden = Windowgarden.GetComponent<SpriteRenderer>();
        }
    }

    private void Update()
    {
        uiManager.UpdateScoreUI(totalScore);
        uiManager.UpdateMultiplierUI(totalMultiplier);

        if (bearAnimator != null)
        {
            bearAnimator.Play("Sleep");
        }

        if (dialogueTriggers != null && dialogueTriggers.Length > 0 && !hasPlayedIntro)
        {
            dialogueTriggers[0].TriggerDialogue();
            hasPlayedIntro = true;
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
        }

        if (Input.GetKeyDown(KeyCode.Space) && hasPlayedIntro)
        {
            if (isFailed)
            {
                FadeManager.Instance.FadeToBlackAndNextDay(resetGame);
            }
            else if (hasPlayedEndDay)
            {
                FadeManager.Instance.FadeToBlackAndNextDay(nextDay);
            }
            else
            {
                dialogueTriggers[3].TriggerDialogue();
            }
        }

        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
        if (LevelManager.Instance.isGameActive)
        {
            if (!exit) GameOn();
        }

        if (isTimerRunning)
        {
            currentDayTimer -= Time.deltaTime;
            uiManager.UpdateTime(currentDayTimer);

            if (currentDayTimer <= 0f)
            {
                isTimerRunning = false;
                EndDayByTimer();
            }
            else if (doneMoop && doneSweep && doneWatering)
            {
                isTimerRunning = false;
                EndDayByTimer();
            }
        }

        if (doneWatering)
        {
            srWindowgarden.sprite = spritesWindowgarden[1];
        }

        uiManager.UpdayCount();
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
        totalMultiplier = multiplier;
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

        resetGame();
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

            // Safely fetch the Bear animator
            GameObject bearObj = GameObject.Find("Bear");
            if (bearObj != null)
            {
                bearAnimator = bearObj.GetComponent<Animator>();
            }
            else
            {
                Debug.LogWarning("Bear GameObject not found in scene.");
                bearAnimator = null;
            }
        }

        resetGame();
    }

    public void EndDayDialogue()
    {
        dialogueTriggers[2].TriggerDialogue();
    }

    private void resetGame()
    {
        totalScore = 0;
        totalMultiplier = 1;
        dayCount = 1;
        doneMoop = false;
        doneSweep = false;
        doneWatering = false;
        hasPlayedIntro = false;
        hasPlayedEndDay = false;
        isFailed = true;
        StartDayTimer();
        TryFindSpriteReferences();

        Player scPlayer = player.GetComponent<Player>();
        scPlayer.Reset();

        Debug.LogWarning("Game has been reset!");
    }

    private void StartDayTimer()
    {
        currentDayTimer = dayTimeLimit;
        isTimerRunning = true;
    }

    private void EndDayByTimer()
    {
        Debug.Log("Time's up for the day!");

        if (doneMoop && doneSweep && doneWatering && totalScore >= 2000)
        {
            Debug.Log("All chores done and score met. Good job!");
            dialogueTriggers[1].TriggerDialogue();
            hasPlayedEndDay = true;
        }
        else
        {
            player.GetComponent<Player>().PlayerFailed();
            isFailed = true;
            Debug.Log("You didn't finish all tasks or meet the score.");
        }

        hasPlayedEndDay = true;
    }

    private void nextDay()
    {
        dayCount++;
        doneMoop = false;
        doneSweep = false;
        doneWatering = false;
        hasPlayedEndDay = false;
        WateringManager.instance.nextDay();
        StartDayTimer();

        if (!doneMoop && !doneSweep)
        {
            srMoop.sprite = spritesMoop[0];
            srSweep.sprite = spritesSweep[0];
        }

        Debug.LogWarning("Game has continue!");
    }
}
