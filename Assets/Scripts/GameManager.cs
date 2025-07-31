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


    [Header("Status games")]
    [SerializeField] private float totalScore;
    [SerializeField] private float totalMultiplier;
    [SerializeField] private bool isGameActive;
    [SerializeField] private bool isQTETrigger;
    public float SetValueTotalScoreUI;
    public float SetValueMultiplierScoreUI;

    private bool hasPlayedIntro = false;
    private bool hasPlayedEndDay = false;

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
    }

    private void Start()
    {
        SoundManager.instance.PlaySound2D("Wind");

        if (dialogueTriggers != null && dialogueTriggers.Length > 0 && !hasPlayedIntro)
        {
            dialogueTriggers[0].TriggerDialogue();
            hasPlayedIntro = true;
        }
    }

    private void Update()
    {
        if (totalScore >= 500 && !hasPlayedEndDay)
        {
            dialogueTriggers[1].TriggerDialogue();
            hasPlayedEndDay = true;
        }

        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
        if (LevelManager.Instance.isGameActive)
        {
            GameOn();
        }

        SetTotalScoreForUI();
        SetMultiplierScoreForUI();
    }

    public void SetTotalScoreForUI()
    {
        SetValueTotalScoreUI = totalScore;
        UIManager.Instance.UpdateScoreUI(SetValueTotalScoreUI);
    }

    public void SetTotalScore(float score)
    {
        totalScore += score;
    }

    public void SetMultiplierScoreForUI()
    {
        SetValueMultiplierScoreUI = totalMultiplier;
        UIManager.Instance.UpdateMultiplierUI(SetValueMultiplierScoreUI);
    }

    public void SetMultiplierScore(float Multiplier)
    {
        totalMultiplier += Multiplier;
    }

    public void StartQTE()
    {
        isQTETrigger = true;
        qteManager.StartQTE(combos);
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

    public void GameOn()
    {
        Debug.Log("Beem call by main menu");
        player.SetActive(true);
    }

    public void GameOff()
    {

        player.SetActive(false);
        LevelManager.Instance.isGameActive = false;
    }


    public bool isGameon()
    {
        return isGameActive;
    }
}
