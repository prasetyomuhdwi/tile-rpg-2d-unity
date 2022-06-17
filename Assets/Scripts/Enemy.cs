using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Movement
{
    // animation 
    public Animator enemyAnimator;

    // experience
    public int xpValue = 1;

    // Logic
    public float triggerLenght = 3f;
    public float chaseLenght = 7f;
    public bool chasing;
    private bool collidingWithPlayer;
    private Transform playerTransform;
    private Vector3 startingPosition;

    // Hitbox
    public ContactFilter2D filter;
    private BoxCollider2D hitBox;
    private Collider2D[] hits = new Collider2D[10];

    protected override void Start()
    {
        base.Start();
        enemyAnimator = GetComponent<Animator>();
        playerTransform = GameManager.instance.player.transform;
        startingPosition = transform.position;
        hitBox = transform.GetChild(0).GetComponent<BoxCollider2D>();

    }

    protected override void UpdateMovement(Vector3 input)
    {
        base.UpdateMovement(input);

        bool isRun = moveDelta.magnitude > 0;
        enemyAnimator.SetBool("isRun", isRun);
    }

    protected void FixedUpdate()
    {
        // Is the player in range?
        
        if (Vector3.Distance(playerTransform.position, startingPosition) < chaseLenght)
        {

            if (Vector3.Distance(playerTransform.position, startingPosition) < triggerLenght)
                chasing = true;

            if (chasing)
            {
                if (!collidingWithPlayer)
                {
                    UpdateMovement((playerTransform.position - transform.position).normalized);
                }
            }
            else
            {
                UpdateMovement(startingPosition - transform.position);
            }
        }
        else
        {
            UpdateMovement(startingPosition - transform.position);
            chasing = false;
        }

        // Check for overlaps
        collidingWithPlayer = false;
        hitBox.OverlapCollider(filter, hits);
        for(int i = 0; i < hits.Length; i++)
        {
            if (hits[1] == null)
                continue;

            if (hits[i].tag == "Fighter" && hits[i].name == "Player")
            {
                collidingWithPlayer = true;
            }

            // the array is not cleanedup, so we doit ourself
            hits[i] = null;
        }
    }

    protected override void Death()
    {
        Destroy(gameObject);
        GameManager.instance.experience += xpValue;
        GameManager.instance.ShowText("+ " + xpValue + " xp", 30, Color.magenta, transform.position, Vector3.up * 40, 1.0f);
    }
}
