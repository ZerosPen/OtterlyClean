using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantWater : MonoBehaviour
{
    [Header("Status Plant")]
    public bool isActive = false;
    public bool isWatered = false;
    public float baseScore;

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

        Player player = GameObject.FindWithTag("Player")?.GetComponent<Player>();
        if(player != null)
        {
            float scoreToAdd = baseScore * player.multiplierScore;
            player.AddScore(scoreToAdd);
        }

        WateringManager.instance.NextPlant();
    }
}
