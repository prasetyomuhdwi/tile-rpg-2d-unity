using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Movement
{
    // animation 
    public Animator enemyAnimator;

    // experience
    public int xpValue = 1;

    // Score
    public int scoreAfterEnemyDie;
    public string typeOfEnemy;

    // Logic
    public float triggerLenght = 3f;
    public float chaseLenght = 7f;
    public bool chasing;
    private bool collidingWithPlayer;
    private Vector3 startingPosition;

    private Transform player;
    private Vector3 target;
    private float distance;
    private float speed;

    // Hitbox
    public ContactFilter2D filter;
    private BoxCollider2D hitBox;
    private Collider2D[] hits = new Collider2D[10];

    protected override void Start()
    {
        base.Start();
        enemyAnimator = GetComponent<Animator>();
        startingPosition = transform.position;
        hitBox = transform.GetChild(0).GetComponent<BoxCollider2D>();
        
        player = GameManager.instance.player.transform;

    }

    protected void FixedUpdate()
    {
        // Is the player in range?
        distance = Vector2.Distance(transform.position, player.position);
        Vector2 direction = player.position - transform.position;
        direction.Normalize();

        speed = ySpeed * xSpeed;

        if ( distance <= chaseLenght)
        {
            target = player.position;

            if (distance <= triggerLenght)
            {
                chasing = true;
            }

            if (chasing)
            {
                if (!collidingWithPlayer)
                {
                    transform.position = Vector2.MoveTowards(this.transform.position,player.position,speed * Time.deltaTime);
                }
            }
        }
        else
        {
            target = startingPosition;

            transform.position = Vector2.MoveTowards(this.transform.position, startingPosition, speed * Time.deltaTime);

            distance = Vector2.Distance(this.transform.position, startingPosition);

            if (distance < 1)
                chasing = false;
        }

        enemyAnimator.SetBool("isRun", chasing);

        // Check for overlaps
        collidingWithPlayer = false;
        hitBox.OverlapCollider(filter, hits);
        for(int i = 0; i < hits.Length; i++)
        {
            if (hits[1] == null)
                continue;

            if (hits[i].tag == "Fighter")
            {
                if(hits[i].name == "Player")
                {
                    collidingWithPlayer = true;
                }
            }

            // the array is not cleanedup, so we do it ourself
            hits[i] = null;
        }

        // flip enemy
        if((transform.position.x - target.x) < 0f)
        {
            transform.localScale = Vector3.one;
        }else if((transform.position.x - target.x) > 0f)
        {
            transform.localScale = new Vector3(-1f,1f,1f);
        }
    }

    protected override void Death()
    {
        Destroy(gameObject);
        GameManager.instance.GrantXp(xpValue);
        GameManager.instance.GrantScore(typeOfEnemy);
        GameManager.instance.ShowText("+ " + xpValue + " XP", 35, Color.yellow, transform.position, Vector3.up * 40, 1.5f);
    }

}
