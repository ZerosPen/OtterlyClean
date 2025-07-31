using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Status")]
    public KeyCode interacButton = KeyCode.E;
    public KeyCode WateringButton = KeyCode.F;
    [SerializeField] private Transform interact;
    [SerializeField] private float interactRadius;
    [SerializeField] private LayerMask triggerBox;
    [SerializeField] private bool canInteract;
    [SerializeField] private Animator animator;
    private bool canPickUPcan = true;
    private float dirX;
    private float dirY;
    private bool isFacingRight = true;

    [Header("Score")]
    public float score;
    public float multiplierScore;

    private QTE_MoopSweep QTEMoopSweep;
    private PlayerMovement playerMovement;
    [SerializeField] private PickUpWatering pickUpWatering;

    private void Awake()
    {
        QTEMoopSweep = GetComponent<QTE_MoopSweep>();
        playerMovement = GetComponent<PlayerMovement>();

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        score = 0f;
        multiplierScore = 1f;
    }

    private void Update()
    {
        if(playerMovement == null)
        {
            Debug.LogWarning("PlayerMovement is NULL");
            return;
        }
        FindpickUpWatering();

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
                if (collid.name == "Sweep_Moop" && canInteract)
                {
                    if (QTEMoopSweep != null && !QTEMoopSweep.isActive())
                    {
                        StartCoroutine(SweepMoopQTE());
                    }
                    else
                    {
                        if (QTEMoopSweep == null)
                        {
                            Debug.LogWarning("QTEMMoopSweep is missing");
                        }
                        else if (pickUpWatering == null)
                        {
                            Debug.LogWarning("pickUpWatering is missing");
                        }
                        else
                        {
                            Debug.Log("Put down the watering can or check references!");
                        }
                    }
                }

                if (collid.name == "Wastafel" && canInteract)
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

                if (collid.name == "BoxWaterCan" && canInteract)
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
                if (collid.CompareTag("Plant") && canInteract)
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

    void FindpickUpWatering()
    {
        if (pickUpWatering == null)
        {
            pickUpWatering = FindObjectOfType<PickUpWatering>();
            if (pickUpWatering == null)
            {
                Debug.LogWarning("PickUpWatering masih NULL setelah scene load!");
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
        GameManager.Instance.SetTotalScore(score) ;
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
    public void showPlayer()
    {
        this.gameObject.SetActive(true);
    }

    public void hidePlayer()
    {
        this.gameObject.SetActive(false);
    }

    IEnumerator WaitAndFindpickUpWatering()
    {
        yield return new WaitForSeconds(0.1f);
        FindpickUpWatering();
    }
}
