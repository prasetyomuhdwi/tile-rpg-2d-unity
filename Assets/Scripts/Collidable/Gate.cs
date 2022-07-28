using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : CollidableObjects
{
    bool isChange = false;
    protected float enemy;
    public bool isBossRoom = false;
    public bool isEnd = false;

    protected override void Update()
    {
        base.Update();
        enemy = GameObject.FindGameObjectsWithTag("Fighter").Length - 1;
    }

    protected override void OnCollided(Collider2D collided)
    {
        if(enemy == 0) { 
            if(collided.tag == "Fighter" && collided.name == "Player")
            {
                // Teleport the Player
                GameManager.instance.SaveState();

                if (!isChange)
                {
                    GameManager.instance.ChangeScene(isBossRoom, isEnd);
                    isChange = true;    
                }
            }
        }
    }

    
}
