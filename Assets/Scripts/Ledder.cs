using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ledder : CollidableObjects
{
    bool isChange = false;
    protected override void OnCollided(Collider2D collided)
    {
        if(collided.name == "Player")
        {
            // Teleport the Player
            GameManager.instance.SaveState();
            if (!isChange)
            {
                GameManager.instance.ChangeScene();
                isChange = true;    
            }
        }
    }

    
}
