using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : CollidableObjects
{
    // Damage struct
    public int[] damagePoint = {2,3,4,5,6,7,8,9};
    public float[] weaponPushForce = { 2.0f, 3.0f, 3.5F, 4.0F, 4.5F, 5.0F, 5.5F, 9.5F };

    // Upgrade
    public int weaponLevel = 0;
    private SpriteRenderer spriteRenderer;

    //Swing
    private Animator animator;
    private float cooldown = 0.5f;
    private float lastSwing;

    protected override void Start()
    {
        base.Start();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    protected override void Update()
    {
        base.Update();
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if(Time.time - lastSwing > cooldown)
            {
                lastSwing = Time.time;
                Swing();
            }
        }
        else
        {
            animator.SetBool("isSwing", false);
        }
    }


    protected override void OnCollided(Collider2D collided)
    {
        if(collided.tag == "Fighter")
        {
            if (collided.name == "Player")
                return;
            
            // Create a new damage object, then we`ll send it to the fighter we`ve hit
            Damage dmg = new Damage
            {
                damageAmount = damagePoint[weaponLevel],
                origin = transform.position,
                pushForce = weaponPushForce[weaponLevel]
            };

            collided.SendMessage("ReceiveDamage", dmg);
            
        }
    }

    private void Swing()
    {
        animator.SetBool("isSwing",true);
    }

    public void UpgradeWeapon()
    {
        weaponLevel++;
        spriteRenderer.sprite = GameManager.instance.weaponSprites[weaponLevel];

        // Change stats
    }
}
