using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Fighter
{
    //Variables
    public float moveSpeed = 4f;
    private BoxCollider2D boxCollider;
    private RaycastHit2D raycastHit;
    public Animator playerAnimator;
    private Vector3 moveDelta;

    private void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        playerAnimator = GetComponent<Animator>();
    }

    //Methods
    private void FixedUpdate()
    {
        Movement(raycastHit);
    }

    private void Movement(RaycastHit2D raycastHit)
    {
        //Get the input X, Y
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        //Cache it in a Vector
        moveDelta = new Vector3(moveX, moveY);

        //Flip the player according to the move direction
        if (moveDelta.x > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (moveDelta.x < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }

        //Collision check
        
        raycastHit = Physics2D.BoxCast(transform.position, boxCollider.size, 0, new Vector3(moveX, 0), Mathf.Abs(moveX * Time.fixedDeltaTime * moveSpeed), LayerMask.GetMask("Enemy", "Block"));
        if (raycastHit.collider)
        {
            // Stop Moveing on the X Axis
            moveDelta.x = 0;
        }

        // Check if we are hitting something in the Y Axis
        raycastHit = Physics2D.BoxCast(transform.position, boxCollider.size, 0, new Vector3(0, moveY), Mathf.Abs(moveY * Time.fixedDeltaTime * moveSpeed), LayerMask.GetMask("Enemy", "Block"));
        

        // Debug.DrawRay(transform.position, new Vector3(moveX,moveY), Color.red, 3.0f);
        
        if (raycastHit.collider)
        {
            // Stop Moveing on the Y Axis
            moveDelta.y = 0; 
        }

        bool isRun = moveDelta.magnitude > 0;
        playerAnimator.SetBool("isRun", isRun);

        transform.Translate(moveDelta * Time.fixedDeltaTime * moveSpeed); 

        //Apply the movement after the previous check


    }
}
