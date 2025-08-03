using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CountDown : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI CountDownText;
    [SerializeField] public float CountDownTime;
    GameObject otherGameObject;
    CuciPiring CuciPiringScript;
    void Start()
    {
        otherGameObject = GameObject.Find("Player");
        CuciPiringScript = otherGameObject.GetComponent<CuciPiring>();
    }

    void Update()
    {
        if (CountDownTime > 0 && CuciPiringScript.scorePiring < 5)
        {
            CountDownTime -= Time.deltaTime;
        }
        else if (CountDownTime < 0)
        {
            CountDownTime = 0;
            CountDownText.color = Color.red;

            Debug.Log("Banyak piring yang dicuci: " + CuciPiringScript.scorePiring);
        }

        int minutes = Mathf.FloorToInt(CountDownTime / 60);
        int seconds = Mathf.FloorToInt(CountDownTime % 60);
        CountDownText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
