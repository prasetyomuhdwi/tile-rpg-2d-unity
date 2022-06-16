using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : CollidableObjects
{
    // Damage struct
    public int damagePoint = 1;
    public float weaponPushForce = 2.0f;

    // Upgrade
    public int weaponLevel = 0;
    private SpriteRenderer spriteRenderer;

    //Swing
    private float cooldown = 0.5f;
    private float lastSwing;

    protected override void Start()
    {
        base.Start();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    protected override void Update()
    {
        base.Update();
        if (Input.GetMouseButtonDown(0))
        {
            if(Time.time - lastSwing > cooldown)
            {
                lastSwing = Time.time;
                Swing();
            }
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
                damageAmount = damagePoint,
                origin = transform.position,
                pushForce = weaponPushForce
            };

            collided.SendMessage("ReceiveDamage", dmg);
            
        }
    }

    private void Swing()
    {
        Debug.Log("Swing");
    }
}
