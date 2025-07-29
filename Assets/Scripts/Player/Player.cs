using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
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
    public Animator animator;
    private float dirX;
    private float dirY;
    private bool isFacingRight = true;

    [Header("Score")]
    public float score;
    public float multiplierScore;

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
        dirX = playerMovement.directionX;
        dirY = playerMovement.directionY;

        if (dirX == 0 && dirY == 0)
        {
            animator.Play("idle");
        }

        if (dirY <= -1)
        {
            animator.Play("Walk-Down");
        }
        else if (dirY >= 1)
        {
            animator.Play("Walk-Up");
        }

        if (dirX > 0 && !isFacingRight)
        {
            FlipSprite();
        }
        else if (dirX < 0 && isFacingRight)
        {
            FlipSprite();
        }

        GameManager.Instance.totalMultiplier = multiplierScore;
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
                    if (QTEMoopSweep != null && !QTEMoopSweep.isActive() && !pickUpWatering.isPickUp)
                    {
                        StartCoroutine(SweepMoopQTE());
                    }
                    else
                    {
                        Debug.Log("Put down the watering can!");
                    }
                }

                if (collid.name == "Wastafel")
                {
                    if (!pickUpWatering.isPickUp)
                    {
                        Debug.Log($"Try Interact with {collid.name}");
                    }
                    else
                    {
                        Debug.Log("Put down the watering can!");
                    }
                }

                if (collid.name == "BoxWaterCan")
                {
                    if (pickUpWatering.canPickUp)
                    {
                        pickUpWatering.PickUpWateringCan();
                    }
                    else if (!pickUpWatering.canPickUp)
                    {
                        pickUpWatering.PutDownWateringCan();
                    }
                }

                break;
            }

            if (Input.GetKeyDown(WateringButton))
            {
                if (collid.CompareTag("Plant"))
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
        }
    }

    public void FlipSprite()
    {
        isFacingRight = !isFacingRight;
        animator.Play("Right");
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    public void AddScore(float amountScore)
    {
        score += amountScore;
        GameManager.Instance.totalScore = score;
        Debug.Log($"Amount score {score}");
    }

    IEnumerator SweepMoopQTE()
    {
        GameManager.Instance.StartQTE();
        playerMovement.StopMovement();
        playerMovement.enabled = false;

        yield return new WaitForFixedUpdate();

        while (QTEManager.Instance.isActive())
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
