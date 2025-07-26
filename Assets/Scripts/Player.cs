using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Status")]
    public KeyCode interacButton = KeyCode.E;
    public Transform interact;
    public float interactRadius;
    public LayerMask triggerBox;

    private void Update()
    {
        if (Input.GetKeyDown(interacButton))
        {
            Collider2D[] collids = Physics2D.OverlapCircleAll(interact.position, interactRadius);

            foreach (Collider2D collid in collids)
            {
                if (((1 << collid.gameObject.layer) & triggerBox) == 0) continue;

                Debug.Log("Try interact with objcet!");
            }
        }
    }
}
