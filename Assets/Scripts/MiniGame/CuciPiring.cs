using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CuciPiring : MonoBehaviour
{
    // public float speed;
    public bool isWashing;
    public bool isFinished;
    public int scorePiring = 0;
    public float amountScore;
    public float baseScore;
    public float CountDownTime;
    public float currTime;
    private int pressCount = 0;
    private Boolean isAPressed = false;
    public GameObject SponsCuci;

    [Header("Interaction Settings")]
    public GameObject keyUIPrefab;
    public Canvas canvas;
    public Transform sinkTransform;

    private GameObject activeKeyUI;
    private TextMeshProUGUI keyText;


    private void Start()
    {
        isWashing = false;
        isFinished = false;
        currTime = CountDownTime;
    }

    private void Update()
    {
        if (!GameManager.Instance.doneWashing)
        {
            isFinished = false;
        }
        if (isWashing && currTime > 0)
        {
            currTime -= Time.deltaTime;
        }

        if (isWashing && currTime <= 0)
        {
            EndMiniGame();
        }

        if (isWashing)
        {
            HandleWashing();
        }

        if (isFinished)
        {
            GameManager.Instance.doneWashing = true;
        }
    }

    private void ShowKeyUI(KeyCode key)
    {
        if (activeKeyUI == null)
        {
            activeKeyUI = Instantiate(keyUIPrefab, canvas.transform);
            activeKeyUI.GetComponent<UI_DishWashing>().Initialize(sinkTransform);
            keyText = activeKeyUI.GetComponentInChildren<TextMeshProUGUI>();
        }

        keyText.text = key.ToString();
    }

    private void HideKeyUI()
    {
        if (activeKeyUI != null)
        {
            Destroy(activeKeyUI);
            activeKeyUI = null;
        }
    }

    void HandleWashing()
    {
        if (canvas == null || sinkTransform == null)
        {
            canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
            sinkTransform = GameObject.Find("Sink").GetComponent<Transform>();
        }

        if (!isAPressed)
        {
            ShowKeyUI(KeyCode.A);
        }
        else
        {
            ShowKeyUI(KeyCode.D);
        }

        if (Input.GetKeyDown(KeyCode.A) && !isAPressed)
        {
            isAPressed = true;
            SponsCuci.transform.position = new Vector3(-5, SponsCuci.transform.position.y, SponsCuci.transform.position.z);
        }

        if (Input.GetKeyDown(KeyCode.D) && isAPressed)
        {
            pressCount++;
            isAPressed = false;
            SponsCuci.transform.position = new Vector3(5, SponsCuci.transform.position.y, SponsCuci.transform.position.z);
        }

        if (pressCount >= 6)
        {
            scorePiring++;
            pressCount = 0;
            EndMiniGame();
            Debug.Log("Score Piring: " + scorePiring);
        }
    }

    private void EndMiniGame()
    {
        if (isFinished) return; // Prevent multiple calls

        isWashing = false;
        isFinished = true;
        HideKeyUI();
        Debug.Log("Mini-game ended! Total cleaned dishes: " + scorePiring);

        float finalScore = 0f;
        Player player = FindObjectOfType<Player>();
        for (int i = 0; i < scorePiring; i++)
        {
            if (currTime >= CountDownTime - 5f) // if game ended with at least 5s left
            {
                finalScore += baseScore * (player.multiplierScore + 0.2f); // 20% bonus
            }
            else
            {
                finalScore += baseScore;
            }
        }

        amountScore = finalScore;

        // Send to player
        
        if (player != null)
        {
            player.AddScore(finalScore);
            Debug.Log("Added score to player: " + finalScore);
        }
        else
        {
            Debug.LogWarning("Player not found in scene!");
        }
    }

    public void StartDishWashing()
    {
        isWashing = true;
        currTime = CountDownTime;
    }
}