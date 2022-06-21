using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Movement
{
    private Animator playerAnimator;
    private SpriteRenderer spriteRenderer;
    public SkinChange skinChange;
   

    protected override void Start()
    {
        base.Start();
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerAnimator = GetComponent<Animator>();   
    }

    protected override void ReceiveDamage(Damage dmg)
    {
        if (Time.time - lastImmune > immuneTIme)
        {
            lastImmune = Time.time;
            hitPoint -= dmg.damageAmount;
            pushDirection = (transform.position - dmg.origin).normalized * dmg.pushForce;

            GameManager.instance.ShowText("- " + dmg.damageAmount.ToString(), 25, Color.red, transform.position, Vector3.zero, 0.5f);

            // playerAnimator.SetBool("isHit", true);

            if (hitPoint <= 0)
            {
                hitPoint = 0;
                Death();
            }
        }
        else
        {
            // playerAnimator.SetBool("isHit",false);
        }

        playerAnimator.SetTrigger("hit");
        GameManager.instance.OnHitPointChange();
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

    // Skin
    public void SwapSprite(int skinId)
    {
        switch(skinId){
            case 0:
                skinChange.KnightMSKin();
                break;
            case 1:
                skinChange.KnightFSKin();
                break;
        }
        // spriteRenderer.sprite = GameManager.instance.playerSprites[skinId];
    }

    

    public void OnLevelUp()
    {
        maxHitPoint++;
        hitPoint = maxHitPoint;
        GameManager.instance.ShowText("Level UP!!", 40, Color.white, transform.position, Vector3.up * 30, 4.0f);
    }

    public void SetLevel(int lvl)
    {
        for (int i = 0; i < lvl; i++)
        {
            OnLevelUp();
        }
    }

    public void Heal(int healingAmount)
    {
        if (hitPoint == maxHitPoint)
            return;

        hitPoint += healingAmount;
        if (hitPoint > maxHitPoint)
            hitPoint = maxHitPoint;
        GameManager.instance.ShowText("+ " + healingAmount.ToString() + " hp", 30, Color.green, transform.position, Vector3.up * 30, 1.0f);
        GameManager.instance.OnHitPointChange();
    }

}
