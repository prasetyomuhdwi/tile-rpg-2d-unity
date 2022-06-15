using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : CollidableObjects
{
    protected bool collected;

    protected override void OnCollided(Collider2D collided)
    {
        if (collided.name == "Player")
            OnCollect();
    }

    protected virtual void OnCollect()
    {
        Debug.Log(name);
    }
}
