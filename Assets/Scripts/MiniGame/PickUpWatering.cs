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

    public static PickUpWatering Instance;

    public void PickUpWateringCan()
    {
        if (canPickUp && wateringCan != null && playerHoldPoint != null)
        {
            canPickUp = false;
            isPickUp = true;
            wateringCan.transform.SetParent(playerHoldPoint);
            wateringCan.transform.localPosition = Vector3.zero;
            wateringCan.transform.localRotation = Quaternion.identity;
        }
        else{
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
}
