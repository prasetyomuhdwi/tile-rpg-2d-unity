using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGame : Gate
{
    protected override void OnCollided(Collider2D collided)
    {
        if (enemy == 0)
        {
            if (collided.tag == "Fighter" && collided.name == "Player")
            {
                GameManager.instance.ChangeScene(isBossRoom, isEnd);
            }
        }
    }
}
