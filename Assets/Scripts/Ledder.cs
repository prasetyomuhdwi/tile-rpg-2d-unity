using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Ledder : CollidableObjects
{
    public string[] sceneNames;

    protected override void OnCollided(Collider2D collided)
    {
        if(collided.name == "Player")
        {
            // Teleport the Player
            GameManager.instance.SaveState();
            string sceneName = sceneNames[Random.Range(0, sceneNames.Length)];
            SceneManager.LoadScene(sceneName);
        }
    }
}
