using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Mover : Fighter
{
    public BoxCollider2D boxCollider;
    private Vector3 moveDelta;
    private RaycastHit2D hit;
    protected float ySpeed = 2.75f;
    protected float xSpeed = 3.0f;
    
    protected virtual void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
    }

    protected virtual void UpdatedMotor(Vector3 input)
    {

        // Reset MoveDelta
        moveDelta = new Vector3(input.x * xSpeed, input.y * ySpeed,0);

        // // Swap sprite direction, wether going right or left
        // if (moveDelta.x > 0)
        //     transform.localScale = Vector3.one;
        // else if (moveDelta.x < 0)
        //     transform.localScale = new Vector3(-1, 1, 1);

        // Add push vector, if any
        moveDelta += pushDirection;

        // Reduce push force every frame based off recovery speed
        pushDirection = Vector3.Lerp(pushDirection, Vector3.zero, pushRecoverySpeed);


        // Make sure we can move in this direction by casting a box there first, if the box returns null we re free to move
        hit = Physics2D.BoxCast(transform.position, boxCollider.size, 0, new Vector2(0, moveDelta.y), Mathf.Abs(moveDelta.y * Time.deltaTime), LayerMask.GetMask("Actor", "Blocking"));
        if (hit.collider == null)
        {
            // Movement
            transform.Translate(0, moveDelta.y * Time.deltaTime, 0);
        }

        hit = Physics2D.BoxCast(transform.position, boxCollider.size, 0, new Vector2(moveDelta.x, 0), Mathf.Abs(moveDelta.x * Time.deltaTime), LayerMask.GetMask("Actor", "Blocking"));
        if (hit.collider == null)
        {
            // Movement
            transform.Translate(moveDelta.x * Time.deltaTime, 0, 0);
        }
    }

}
