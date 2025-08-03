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
    }

    private void Update()
    {
        if (isWashing)
        {
            HandleWashing();
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
            isWashing = false;
            isFinished = true;
            HideKeyUI(); // Hide UI when done
            Debug.Log("Score Piring: " + scorePiring);
        }
    }

    public void StartDishWashing()
    {
        isWashing = true;
    }
}