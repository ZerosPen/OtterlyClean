using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PickUpWatering : MonoBehaviour
{
    public GameObject wateringCan;        
    public Transform playerHoldPoint;     
    private bool canPickUp = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            canPickUp = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            canPickUp = false;
        }
    }

    private void Update()
    {
        if (canPickUp && Input.GetKeyDown(KeyCode.E))
        {
            PickUpWateringCan();
        }
    }

    private void PickUpWateringCan()
    {
        wateringCan.SetActive(true); // In case it's hidden
        wateringCan.transform.SetParent(playerHoldPoint);
        wateringCan.transform.localPosition = Vector3.zero;
        wateringCan.GetComponent<Rigidbody2D>().simulated = false;
        wateringCan.GetComponent<Collider2D>().enabled = false;

        Debug.Log("Watering can picked up!");
        gameObject.SetActive(false); // Disable trigger box after pickup
    }
}
