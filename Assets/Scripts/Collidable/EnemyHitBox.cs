using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitBox : CollidableObjects
{
    // Damage
    public int enemyDamage = 1;
    public float enemyPushForce = 4f;

    protected override void OnCollided(Collider2D collided)
    {
        if(collided.tag == "Fighter" && collided.name == "Player")
        {
            // Create a new damage object, before sending it on the fighter we`ve hit
            Damage dmg = new Damage
            {
                damageAmount = enemyDamage,
                origin = transform.position,
                pushForce = enemyPushForce
            };

            collided.SendMessage("ReceiveDamage", dmg);
        }
    }
}
