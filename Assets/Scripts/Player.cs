using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Movement
{
    private Animator playerAnimator;
    private SpriteRenderer spriteRenderer;

    protected override void Start()
    {
        base.Start();
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerAnimator = GetComponent<Animator>();
        
        DontDestroyOnLoad(gameObject);
    }

    protected override void ReceiveDamage(Damage dmg)
    {
        if (Time.time - lastImmune > immuneTIme)
        {
            lastImmune = Time.time;
            hitPoint -= dmg.damageAmount;
            pushDirection = (transform.position - dmg.origin).normalized * dmg.pushForce;

            GameManager.instance.ShowText("- " + dmg.damageAmount.ToString(), 25, Color.red, transform.position, Vector3.zero, 0.5f);
            
            playerAnimator.SetBool("isHit", true);

            if (hitPoint <= 0)
            {
                hitPoint = 0;
                Death();
            }
        }
        else
        {
            playerAnimator.SetBool("isHit",false);
        }
    }

    private void FixedUpdate()
    {
        //Get the input X, Y
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        if(Mathf.Abs(moveX) > 0 || Mathf.Abs(moveY) > 0)
            playerAnimator.SetBool("isRun", true);
        else
            playerAnimator.SetBool("isRun", false);


        UpdateMovement(new Vector3(moveX, moveY, 0));
    }

    public void SwapSprite(int skinId)
    {
        spriteRenderer.sprite = GameManager.instance.playerSprites[skinId];
    }

    public void OnLevelUp()
    {
        maxHitPoint++;
        hitPoint = maxHitPoint;
    }

    public void SetLevel(int lvl)
    {
        for (int i = 0; i < lvl; i++)
        {
            OnLevelUp();
        }
    }
}
