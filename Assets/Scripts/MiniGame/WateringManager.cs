using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WateringManager : MonoBehaviour
{
    public List<PlantWater> plants;
    public PickUpWatering wateringCan;
    private int currentPlantIndex = 0;

    private List<PlantWater> shufflePlants = new List<PlantWater>();
    public static WateringManager instance;

    private void Awake()
    {
        if(instance == null) instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        shufflePlants = new List<PlantWater>(plants);
        shuffle(shufflePlants);
    }

    private void Update()
    {
        if (wateringCan != null && wateringCan.isPickUp)
        {
            if (shufflePlants.Count > 0 && currentPlantIndex < shufflePlants.Count)
            {
                if (!shufflePlants[currentPlantIndex].isWatered)
                {
                    Debug.Log("Index: " + currentPlantIndex + ", Object: " + shufflePlants[currentPlantIndex].gameObject.name);
                    shufflePlants[currentPlantIndex].ActivePlant();
                }
            }
        }
    }

    public void NextPlant()
    {
        int randomIndex = Random.Range(0, plants.Count);
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

    void shuffle(List<PlantWater> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int randomIndex = Random.Range(i, list.Count);
            PlantWater temp = list[i];
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }
}
