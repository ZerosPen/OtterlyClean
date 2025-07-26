using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : Player
{
    [Header("Movement")]
    public float speedMovement;
    public float maxSpeedMovement;
    public float directionX;
    public float directionY;

    [Header("KeyBind")]
    public KeyCode movementLeft = KeyCode.A;
    public KeyCode movementRight = KeyCode.D;
    public KeyCode movementUp = KeyCode.W;
    public KeyCode movementDown = KeyCode.S;

    private Rigidbody2D rb;
    private Vector2 currentVelocity;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        directionX = Input.GetKey(movementLeft) ? -1f :
                     Input.GetKey(movementRight) ? 1f : 0f;
        directionY = Input.GetKey(movementUp) ? 1f :
                     Input.GetKey(movementDown) ? -1f : 0f;
    }

    private void FixedUpdate()
    {
        Vector2 direction = new Vector2(directionX, directionY).normalized;
        Vector2 targetSpeed = direction * maxSpeedMovement;

        rb.velocity = Vector2.SmoothDamp(rb.velocity, targetSpeed, ref currentVelocity, 0.1f);
    }
}
