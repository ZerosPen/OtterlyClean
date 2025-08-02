using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Status")]
    public KeyCode interacButton = KeyCode.E;
    public KeyCode actionButton = KeyCode.F;
    [SerializeField] private Transform interact;
    [SerializeField] private float interactRadius;
    [SerializeField] private LayerMask triggerBox;
    [SerializeField] private bool canInteract;
    [SerializeField] private Animator animator;
    public bool holdMoop;
    public bool holdSweep;
    public bool holdWatercan;
    private bool canPickUPcan = true;
    private float dirX;
    private float dirY;
    private bool isFacingRight = true;

    [Header("Score")]
    public float score;
    public float multiplierScore;

    private Sweep qteSweep;
    private Moop qteMoop;
    private PlayerMovement playerMovement;
    [SerializeField] private PickUpWatering pickUpWatering;

    private void Awake()
    {
        qteSweep = GetComponent<Sweep>();
        qteMoop = GetComponent<Moop>();
        playerMovement = GetComponent<PlayerMovement>();

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        score = 0f;
        multiplierScore = 1f;
        holdSweep = holdMoop = false;
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

        if (DialogueManager.Instance.isDialogueActive)
        {
            playerMovement.enabled = false;
        }
        else
        {
            playerMovement.enabled = true;
        }

        if (dirX == 0 && dirY == 0)
        {
            if (holdSweep)
            {
                animator.Play("idle-Broom");
                SoundManager.instance.PlaySound2D("Walking");
            }
            else if (holdMoop)
            {
                animator.Play("idle-Moop");
                SoundManager.instance.PlaySound2D("Walking");
            }
            else if (holdWatercan)
            {
                animator.Play("idle-WaterPot");
                SoundManager.instance.PlaySound2D("Walking");
            }
            else
            {
                animator.Play("idle");
            }
        }

        if (dirY <= -1)
        {
            if (holdSweep)
            {
                animator.Play("Walk-Down-Broom");
            }
            else if (holdMoop)
            {
                animator.Play("Walk-Down-Moop");
            }
            else if (holdWatercan)
            {
                animator.Play("Walk-Down-WaterPot");
            }
            else
            {
                animator.Play("Walk-Down");
            }
        }

        else if (dirY >= 1)
        {
            if (holdSweep)
            {
                animator.Play("Walk-Up-Broom");
            }
            else if (holdMoop)
            {
                animator.Play("Walk-Up-Moop");
            }
            else if (holdWatercan)
            {
                animator.Play("Walk-Up-WaterPot");
            }
            else
            {
                animator.Play("Walk-Up");
            } 
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
            if (collid.name == "Sweep")
            {
                if (Input.GetKeyDown(interacButton) && canInteract)
                {
                    if (holdMoop || holdWatercan)
                    {
                        Debug.Log("Must put dont the tools");
                    }
                    else if (!holdSweep)
                    {
                        holdSweep = true;
                        GameManager.Instance.isSweep = holdSweep;
                    }
                    else if (holdSweep)
                    {
                        holdSweep = false;
                        GameManager.Instance.isSweep = holdSweep;
                    }
                }

                if (Input.GetKeyDown(actionButton) && holdSweep && qteSweep != null && !qteSweep.isActive())
                {
                    if (!GameManager.Instance.hasPlayedEndDay)
                    {
                        StartCoroutine(SweepMoopQTE());
                    }
                    else
                    {
                        GameManager.Instance.EndDayDialogue();
                    }

                }
                else
                {
                    if (qteSweep == null)
                    {
                        Debug.LogWarning("QTE Sweep is missing");
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

            if (collid.name == "Moop")
            {
                if (Input.GetKeyDown(interacButton) && canInteract)
                {
                    if (holdSweep || holdWatercan)
                    {
                        Debug.Log("Must put dont the tools");
                    }
                    else if (!holdMoop)
                    {
                        holdMoop = true;
                        GameManager.Instance.isMoop = holdMoop;
                    }
                    else if (holdMoop)
                    {
                        holdMoop = false;
                        GameManager.Instance.isMoop = holdMoop;
                    }

                    
                }
                if (Input.GetKeyDown(actionButton) && holdMoop && qteMoop != null && !qteMoop.isActive())
                {
                    if (!GameManager.Instance.hasPlayedEndDay)
                    {
                        StartCoroutine(SweepMoopQTE());
                    }
                    else
                    {
                        GameManager.Instance.EndDayDialogue();
                    }
                }
                else
                {
                    if (qteMoop == null)
                    {
                        Debug.LogWarning("QTE Sweep is missing");
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

            if (collid.name == "BoxWaterCan" && Input.GetKeyDown(interacButton) && canInteract)
            {
                if (holdSweep || holdMoop)
                {
                    Debug.Log("Must put dont the tools");
                }
                else if (pickUpWatering.canPickUp)
                {
                    holdWatercan = true;
                    pickUpWatering.PickUpWateringCan();
                }
                else if (!pickUpWatering.canPickUp)
                {
                    holdWatercan = false;
                    
                    pickUpWatering.PutDownWateringCan();
                }
                GameManager.Instance.isWatering = holdWatercan;
            }

            if (Input.GetKeyDown(interacButton))
            {
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

                

                break;
            }

            if (Input.GetKeyDown(actionButton))
            {
                if (collid.CompareTag("Plant") && canInteract && !GameManager.Instance.hasPlayedEndDay)
                {
                    PlantWater plant = collid.GetComponent<PlantWater>();
                    if (pickUpWatering.canPickUp && holdWatercan)
                    {
                        Debug.Log("Pick Up the watering can!");
                    }
                    else
                    {
                        Debug.Log("Press F to watering the plant!");
                        plant.WateringPlant();
                    }
                }
                else
                {
                    GameManager.Instance.EndDayDialogue();
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

        if (holdSweep)
        {
            animator.Play("Right-Broom");
        }
        else if (holdMoop)
        {
            animator.Play("Right-Moop");
        }
        else if (holdWatercan)
        {
            animator.Play("Right-WaterPot");
        }
        else
        {
            animator.Play("Right");
        }
        
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

    public void AddMultiplier(float multi)
    {
        multiplierScore += multi;
        GameManager.Instance.SetMultiplierScore(multiplierScore);
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

        if (qteMoop.isClean)
        {
            Debug.Log("Sweep is done!");
        }
        else if (qteMoop.isClean)
        {
            Debug.Log("Moop is done!");
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

    public void Reset()
    {
        score = 0;
        multiplierScore = 1;
        holdMoop = false;
        holdSweep = false;
        holdWatercan = false;
    }

    IEnumerator WaitAndFindpickUpWatering()
    {
        yield return new WaitForSeconds(0.1f);
        FindpickUpWatering();
    }
}
