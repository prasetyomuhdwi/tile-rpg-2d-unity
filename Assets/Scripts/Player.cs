using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Movement
{
    public Animator playerAnimator;

    protected override void Start()
    {
        base.Start();
        playerAnimator = GetComponent<Animator>();
    }

    protected override void UpdateMovement(Vector3 input)
    {
        base.UpdateMovement(input);

        bool isRun = moveDelta.magnitude > 0;
        playerAnimator.SetBool("isRun", isRun);
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

        UpdateMovement(new Vector3(moveX, moveY, 0));
    }

}
