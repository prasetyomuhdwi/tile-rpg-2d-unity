using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Movement : Fighter
{
    //Variables
    public float xSpeed = 2.85f;
    public float ySpeed = 3.0f;

    private BoxCollider2D boxCollider;
    private RaycastHit2D raycastHit;
    protected Vector3 moveDelta;

    protected virtual void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
    }

    //Methods
    protected virtual void UpdateMovement(Vector3 input)
    {
      
        //Cache it in a Vector
        moveDelta = new Vector3(input.x * xSpeed,input.y * ySpeed,0);

        //Flip the player according to the move direction right or left
        if (moveDelta.x > 0)
            transform.localScale = Vector3.one;
        else if (moveDelta.x < 0)
            transform.localScale = new Vector3(-1, 1, 1);

        // Add push vector, if any
        moveDelta += pushDirection;

        // Reduce push force every frame, based off recovery speed
        pushDirection = Vector3.Lerp(pushDirection, Vector3.zero,pushRecoverySpeed);

        // Make sure we can move in this direction, by casting a box there first if the box returns null, we`re free to move
        raycastHit = Physics2D.BoxCast(transform.position, boxCollider.size, 0, new Vector2(0,moveDelta.y), Mathf.Abs(moveDelta.y * Time.deltaTime), LayerMask.GetMask("Enemy", "Block"));
        if (raycastHit.collider == null)
        {
            // Make this thing move in y axis!
            transform.Translate(0,moveDelta.y * Time.deltaTime,0);
        }

        // Check if we are hitting something in the Y Axis
        raycastHit = Physics2D.BoxCast(transform.position, boxCollider.size, 0, new Vector2(moveDelta.x, 0), Mathf.Abs(moveDelta.x * Time.deltaTime), LayerMask.GetMask("Enemy", "Block"));
        // Debug.DrawRay(transform.position, new Vector3(moveX,moveY), Color.red, 3.0f);
        if (raycastHit.collider == null)
        {
            // Make this thing move in x axis!
            transform.Translate(moveDelta.x * Time.deltaTime,0, 0);
        }

    }
}
