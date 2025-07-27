using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantWater : MonoBehaviour
{
    public bool isActive = false;
    public bool isWatered = false;
    public float requiredWater;
    public float currentWater;
    public float waterSpeed;

    public void WateringPlant()
    {
        if (isActive && !isWatered)
        {
            WaterThisPlant();
        }
    }

    public void ActivePlant()
    {
        isActive = true;
    }

    public void DeactivePlant()
    {
        isActive = false;
    }

    void WaterThisPlant()
    {
        Debug.Log($"{gameObject.name} watered!");
        isWatered = true;
        DeactivePlant();
        WateringManager.instance.NextPlant();
    }
}
