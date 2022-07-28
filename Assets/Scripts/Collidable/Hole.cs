using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hole : CollidableObjects
{
    public float enemyDamage = 10f;
    public float enemyPushForce = 0f;

    protected override void Start()
    {
        base.Start();
        enemyDamage = GameManager.instance.player.maxHitPoint;
    }

    protected override void OnCollided(Collider2D collided)
    {
        if (collided.tag == "Fighter" && collided.name == "Player")
        {
            GameManager.instance.deathText.text = "You Fell In The Hole!";

            // Create a new damage object, before sending it on the fighter we`ve hit
            Damage dmg = new Damage
            {
                damageAmount = (int) enemyDamage,
                origin = transform.position,
                pushForce = enemyPushForce
            };

            collided.SendMessage("ReceiveDamage", dmg);
        }
    }
}