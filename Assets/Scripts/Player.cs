using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Movement
{
    private Animator playerAnimator;
    private SpriteRenderer spriteRenderer;
    public SkinChange skinChange;
    private bool isAlive = true;
   

    protected override void Start()
    {
        base.Start();
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerAnimator = GetComponent<Animator>();   
    }

    protected override void ReceiveDamage(Damage dmg)
    {
        if (!isAlive)
            return;

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
    protected override void Death()
    {
        isAlive = false;
        GameManager.instance.deathAnimator.SetTrigger("show");
    }

    public void Respawn()
    {
        hitPoint = maxHitPoint;
        isAlive = true;
        lastImmune = Time.time;
        pushDirection = Vector3.zero;
    }

    private void FixedUpdate()
    {
        //Get the input X, Y
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        if (isAlive)
        {
            if(Mathf.Abs(moveX) > 0 || Mathf.Abs(moveY) > 0)
                playerAnimator.SetBool("isRun", true);
            else
                playerAnimator.SetBool("isRun", false);

            UpdateMovement(new Vector3(moveX, moveY, 0));
        }
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
    }

    public void OnLevelUp()
    {
        maxHitPoint += GameManager.instance.GetCurrentLevel();
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
