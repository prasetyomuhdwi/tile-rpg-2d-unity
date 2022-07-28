using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fighter : MonoBehaviour
{
    // Public fields
    public float hitPoint = 10f;
    public float maxHitPoint = 10f;
    public float pushRecoverySpeed = 0.2f;

    // Immunity
    protected float immuneTIme = 1.0f;
    protected float lastImmune;

    // Push
    protected Vector3 pushDirection;

    // All fighters can ReceiveDamage / Die
    protected virtual void ReceiveDamage(Damage dmg)
    {
        if(Time.time - lastImmune > immuneTIme)
        {
            lastImmune = Time.time;
            hitPoint -= dmg.damageAmount;
            pushDirection = (transform.position - dmg.origin).normalized * dmg.pushForce;

            GameManager.instance.ShowText("- " + dmg.damageAmount.ToString() + " HP", 35, Color.magenta, transform.position, Vector3.zero, 1f);

            if (hitPoint <= 0)
            {
                hitPoint = 0;
                Death();
            }
        }
    }

    protected virtual void Death()
    {
        Debug.Log("Class Fighter");
    }
}
