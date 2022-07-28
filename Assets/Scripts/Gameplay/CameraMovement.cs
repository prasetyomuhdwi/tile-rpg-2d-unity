using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private Transform lookAt;
    public float boundX = 0.5f;
    public float boundY = 0.5f;

    private GameObject player;

    private void Start()
    {
        player = GameObject.Find("Player");
        lookAt = player.transform;
    }

    private void LateUpdate()
    {
        Vector3 delta = Vector3.zero;
        
        float deltaX;

        // This is to check if we're inside the bounds on the X axis
        if(lookAt != null)
        {

            deltaX = lookAt.position.x - transform.position.x;
            if(deltaX > boundX || deltaX < -boundX)
            {
                if(transform.position.x < lookAt.position.x)
                {
                    delta.x = deltaX - boundX;
                }
                else
                {
                    delta.x = deltaX+ boundX;
                }

            }

            // This is to check if we're inside the bounds on the Y axis
            float deltaY = lookAt.position.y - transform.position.y;
            if (deltaY > boundY || deltaY < -boundY)
            {
                if (transform.position.y < lookAt.position.y)
                {
                    delta.y = deltaY - boundY;
                }
                else
                {
                    delta.y = deltaY + boundY;
                }

            }
        }


        transform.position += new Vector3(delta.x,delta.y);
    }
}
