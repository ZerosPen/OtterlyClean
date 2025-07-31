using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PickUpWatering : MonoBehaviour
{
    [SerializeField] private GameObject wateringCan;        
    [SerializeField] private Transform playerHoldPoint;
    [SerializeField] private Transform WaterCanPoint;
    public bool canPickUp = true;
    public bool isPickUp;
    private bool isFound = false;


    private void Start()
    {
        StartCoroutine(WaitAndFindHoldPoint());
    }

    private void Update()
    {
        if (playerHoldPoint == null)
        {
            FindPlayerHoldPoint();
        }
    }


    private void FindPlayerHoldPoint()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            Transform hold = player.transform.Find("HoldPoint");
            if (hold != null)
            {
                playerHoldPoint = hold;
                Debug.Log("Find the HoldPoint from Player");
            }
            else
            {
                Debug.LogWarning("HoldPoint not found in Player!");
            }
        }
        else
        {
            Debug.LogWarning("Player not found in scene!");
        }
    }

    public void PickUpWateringCan()
    {
        Debug.Log($"[DEBUG] canPickUp: {canPickUp}, wateringCan: {wateringCan}, playerHoldPoint: {playerHoldPoint}");

        if (canPickUp && wateringCan != null && playerHoldPoint != null)
        {
            canPickUp = false;
            isPickUp = true;
            wateringCan.transform.SetParent(playerHoldPoint);
            wateringCan.transform.localPosition = Vector3.zero;
            wateringCan.transform.localRotation = Quaternion.identity;
        }
        if (wateringCan == null)
        {
            Debug.Log("wateringCan is NULL!");
        }
        if (playerHoldPoint == null)
        {
            Debug.Log("playerHoldPoint is missing");
        }
        else
        {
            Debug.Log("Watering is been pickUped");
        }
    }

    public void PutDownWateringCan()
    {
        if (!canPickUp)
        {
            canPickUp = true;
            isPickUp = false;
            wateringCan.transform.SetParent(WaterCanPoint);
            wateringCan.transform.localPosition = Vector3.zero;
            wateringCan.transform.localRotation = Quaternion.identity;
        }
    }

    IEnumerator WaitAndFindHoldPoint()
    {
        yield return new WaitForSeconds(0.1f);
        FindPlayerHoldPoint();
    }
}
