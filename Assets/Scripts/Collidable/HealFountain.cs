using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealFountain : CollidableObjects
{
    public float healingAmount = 0.1f;
    public float cooldown = 1.5f;
    public float lastHeal;

    protected override void OnCollided(Collider2D collided)
    {
        if (collided.tag == "Fighter" && collided.name == "Player")
        {
            if (Time.time - lastHeal > cooldown)
            {
                lastHeal = Time.time;
                GameManager.instance.player.Heal(healingAmount);
            }
        }
    }

}
