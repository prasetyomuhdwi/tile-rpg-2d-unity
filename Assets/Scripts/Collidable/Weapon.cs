using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Weapon : CollidableObjects
{
    // Damage struct
    public int[] damagePoint = {2,3,4,5,6,7,8,9};
    public float[] weaponPushForce = { 2.0f, 3.0f, 3.5F, 4.0F, 4.5F, 5.0F, 5.5F, 9.5F };

    // Upgrade
    public int weaponLevel = 0;
    private SpriteRenderer spriteRenderer;

    //Swing
    public Animator animator;
    public float cooldown = 0.5f;
    public float lastSwing;

    protected override void Start()
    {
        base.Start();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    

    protected override void Update()
    {
        base.Update();
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


    public void UpgradeWeapon()
    {
        weaponLevel++;
        spriteRenderer.sprite = GameManager.instance.weaponSprites[weaponLevel];
    }
}
