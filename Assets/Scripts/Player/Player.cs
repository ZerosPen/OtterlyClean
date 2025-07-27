using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Status")]
    public KeyCode interacButton = KeyCode.E;
    public KeyCode WateringButton = KeyCode.F;
    public Transform interact;
    public float interactRadius;
    public LayerMask triggerBox;
    public bool canInteract;

    private QTE_MoopSweep QTEMoopSweep;
    private PlayerMovement playerMovement;
    public PickUpWatering pickUpWatering;

    private void Start()
    {
        QTEMoopSweep = GetComponent<QTE_MoopSweep>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        canInteract = false;

        Collider2D[] collids = Physics2D.OverlapCircleAll(interact.position, interactRadius);

        foreach (Collider2D collid in collids)
        {
            if (collid.gameObject == gameObject) continue;

            if (((1 << collid.gameObject.layer) & triggerBox) == 0)
            {
                continue;
            }

            canInteract = true;

            if (Input.GetKeyDown(interacButton))
            {
                if (collid.name == "Sweep_Moop")
                {
                    if (QTEMoopSweep != null && !QTEMoopSweep.isActive())
                    {
                        StartCoroutine(SweepMoopQTE());
                    }
                }

                if (collid.name == "Wastafel")
                {
                    Debug.Log($"Try Interact with {collid.name}");
                }

                if (collid.name == "BoxWaterCan")
                {
                    pickUpWatering.PickUpWateringCan();
                }

                break;
            }

            if (Input.GetKeyDown(WateringButton))
            {
                if (collid.name == "Plant")
                {
                    PlantWater plant = collid.GetComponent<PlantWater>();
                    if (pickUpWatering.canPickUp)
                    {
                        Debug.Log("Pick Up the watering can!");
                    }
                    else
                    {
                        Debug.Log("Press F to watering the plant!");
                        plant.WateringPlant();
                    }
                }

                break;
            }


            if (canInteract && collid.name != "Plant") Debug.Log($"Press E to Interact {collid.name}");
        }
    }


    IEnumerator SweepMoopQTE()
    {
        QTEMoopSweep.StartQTE();
        playerMovement.StopMovement();
        

        while (QTEMoopSweep.isActive())
        {
            yield return null;
        }

        if (QTEMoopSweep.isClean)
        {
            Debug.Log("Sweep is done!");
        }
        playerMovement.enabled = true;
    }
}
