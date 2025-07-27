using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WateringManager : MonoBehaviour
{
    public List<PlantWater> plants;
    public PickUpWatering wateringCan;
    private int currentPlantIndex = 0;

    public static WateringManager instance;

    private void Awake()
    {
        if(instance == null) instance = this;
        else Destroy(gameObject);
    }

    private void Update()
    {
        if (wateringCan != null && wateringCan.isPickUp)
        {
            if (plants.Count > 0 && currentPlantIndex < plants.Count && !plants[currentPlantIndex].isWatered)
            {
                plants[currentPlantIndex].ActivePlant();
            }
        }
    }

    public void NextPlant()
    {
        currentPlantIndex++;
        if (currentPlantIndex < plants.Count)
        {
            plants[currentPlantIndex].ActivePlant();
        }
        else
        {
            Debug.Log("All plant been watered!");
        }
    }
}
